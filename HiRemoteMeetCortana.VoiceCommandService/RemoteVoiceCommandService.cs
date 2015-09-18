using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
namespace HiRemoteMeetCortana.VoiceCommandService
{
    public sealed class RemoteVoiceCommandService : IBackgroundTask
    {
        /// <summary>
        /// the service connection is maintained for the lifetime of a cortana session, once a voice command
        /// has been triggered via Cortana.
        /// </summary>
        VoiceCommandServiceConnection voiceServiceConnection;

        /// <summary>
        /// Lifetime of the background service is controlled via the BackgroundTaskDeferral object, including
        /// registering for cancellation events, signalling end of execution, etc. Cortana may terminate the 
        /// background service task if it loses focus, or the background task takes too long to provide.
        /// 
        /// Background tasks can run for a maximum of 30 seconds.
        /// </summary>
        BackgroundTaskDeferral serviceDeferral;

        /// <summary>
        /// ResourceMap containing localized strings for display in Cortana.
        /// </summary>
        ResourceMap cortanaResourceMap;

        /// <summary>
        /// The context for localized strings.
        /// </summary>
        ResourceContext cortanaContext;

        /// <summary>
        /// Get globalization-aware date formats.
        /// </summary>
        DateTimeFormatInfo dateFormatInfo;

        /// <summary>
        /// Background task entrypoint. Voice Commands using the <VoiceCommandService Target="...">
        /// tag will invoke this when they are recognized by Cortana, passing along details of the 
        /// invocation. 
        /// 
        /// Background tasks must respond to activation by Cortana within 0.5 seconds, and must 
        /// report progress to Cortana every 5 seconds (unless Cortana is waiting for user
        /// input). There is no execution time limit on the background task managed by Cortana,
        /// but developers should use plmdebug (https://msdn.microsoft.com/en-us/library/windows/hardware/jj680085%28v=vs.85%29.aspx)
        /// on the Cortana app package in order to prevent Cortana timing out the task during
        /// debugging.
        /// 
        /// Cortana dismisses its UI if it loses focus. This will cause it to terminate the background
        /// task, even if the background task is being debugged. Use of Remote Debugging is recommended
        /// in order to debug background task behaviors. In order to debug background tasks, open the
        /// project properties for the app package (not the background task project), and enable
        /// Debug -> "Do not launch, but debug my code when it starts". Alternatively, add a long
        /// initial progress screen, and attach to the background task process while it executes.
        /// </summary>
        /// <param name="taskInstance">Connection to the hosting background service process.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();

            // Register to receive an event if Cortana dismisses the background task. This will
            // occur if the task takes too long to respond, or if Cortana's UI is dismissed.
            // Any pending operations should be cancelled or waited on to clean up where possible.
            taskInstance.Canceled += OnTaskCanceled;

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            // Load localized resources for strings sent to Cortana to be displayed to the user.
            cortanaResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

            // Select the system language, which is what Cortana should be running as.
            cortanaContext = ResourceContext.GetForViewIndependentUse();

            // Get the currently used system date format
            dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

            // This should match the uap:AppService and VoiceCommandService references from the 
            // package manifest and VCD files, respectively. Make sure we've been launched by
            // a Cortana Voice Command.
            if (triggerDetails != null && triggerDetails.Name == "RemoteVoiceCommandService")
            {
                try
                {
                    voiceServiceConnection =
                        VoiceCommandServiceConnection.FromAppServiceTriggerDetails(
                            triggerDetails);

                    voiceServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;

                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();

                    // Depending on the operation (defined in AdventureWorks:AdventureWorksCommands.xml)
                    // perform the appropriate command.
                    switch (voiceCommand.CommandName)
                    {
                        case "turnOnLights":
                            var destination = voiceCommand.Properties["output"][0];
                            await TurnLightsOnCommand(destination);
                            break;
                        default:
                            LaunchAppInForeground();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Handling Voice Command failed " + ex.ToString());
                }
            }
        }

        private async Task TurnLightsOnCommand(string output)
        {

            VoiceCommandResponse response;
            await ShowProgressScreen("Looking for lights in " + output);

            string confirmuserPrompt = string.Format("Turn on {0} lights?", output);
            var userPrompt = new VoiceCommandUserMessage
            {
                DisplayMessage = confirmuserPrompt,
                SpokenMessage = confirmuserPrompt
            };

            string confirm = string.Format("Did you want to turn on {0} light?", output);
            var userReprompt = new VoiceCommandUserMessage
            {
                DisplayMessage = confirm,
                SpokenMessage = confirm
            };

            response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

            var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);

            // If RequestConfirmationAsync returns null, Cortana's UI has likely been dismissed.
            if (voiceCommandConfirmation != null)
            {
                if (voiceCommandConfirmation.Confirmed == true)
                {
                    await ShowProgressScreen("Turning on lights");

                    var userMessage = new VoiceCommandUserMessage
                    {
                        DisplayMessage = "Turned on lights in " + output,
                        SpokenMessage = "Turned on lights in " + output
                    };
                    
                    response = VoiceCommandResponse.CreateResponse(userMessage);
                    await voiceServiceConnection.ReportSuccessAsync(response);
                }
                else
                {
                    var userMessage = new VoiceCommandUserMessage
                    {
                        DisplayMessage = "Okay, keeping those lights turned off!",
                        SpokenMessage = "Okay, keeping those lights turned off!"
                    };                    

                    response = VoiceCommandResponse.CreateResponse(userMessage);
                    await voiceServiceConnection.ReportSuccessAsync(response);
                }
            }

        }

        /// <summary>
        /// Show a progress screen. These should be posted at least every 5 seconds for a 
        /// long-running operation, such as accessing network resources over a mobile 
        /// carrier network.
        /// </summary>
        /// <param name="message">The message to display, relating to the task being performed.</param>
        /// <returns></returns>
        private async Task ShowProgressScreen(string message)
        {
            var userProgressMessage = new VoiceCommandUserMessage();
            userProgressMessage.DisplayMessage = userProgressMessage.SpokenMessage = message;

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userProgressMessage);
            await voiceServiceConnection.ReportProgressAsync(response);
        }

        //private async Task SearchingLightsCommand(string output)
        //{

        //    await ShowProgressScreen("Loading 1234");

        //    var userMessage = new VoiceCommandUserMessage();

        //    userMessage.DisplayMessage = "Voice Command User Message";
        //    userMessage.SpokenMessage = "Voice Command User Message";

        //    //Tiles
        //    var destinationTile = new VoiceCommandContentTile
        //    {
        //        Title = "Title test",
        //        ContentTileType = VoiceCommandContentTileType.TitleWith68x68IconAndText,
        //        Image = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///HiRemoteMeetCortana.VoiceCommands/Images/GreyTile.png")),
        //        AppLaunchArgument = "argument doorgegeven",
        //        TextLine1 = "TextLine1 1234",
        //        TextLine2 = "TextLine2 1234",
        //        TextLine3 = "TextLine3 1235",
        //    };

        //    var destinationsContentTiles = new List<VoiceCommandContentTile>();
        //    destinationsContentTiles.Add(destinationTile);

        //    var response = VoiceCommandResponse.CreateResponse(userMessage, destinationsContentTiles);
        //    response.AppLaunchArgument = output;

        //    await voiceServiceConnection.ReportSuccessAsync(response);
        //}

        private async void LaunchAppInForeground()
        {
            var userMessage = new VoiceCommandUserMessage();
            userMessage.SpokenMessage = cortanaResourceMap.GetValue("LaunchingHiRemoteMeetCortana", cortanaContext).ValueAsString;

            var response = VoiceCommandResponse.CreateResponse(userMessage);

            response.AppLaunchArgument = "";

            await voiceServiceConnection.RequestAppLaunchAsync(response);
        }

        private void OnVoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            if (this.serviceDeferral != null)
            {
                this.serviceDeferral.Complete();
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            System.Diagnostics.Debug.WriteLine("Task cancelled, clean up");
            if (this.serviceDeferral != null)
            {
                this.serviceDeferral.Complete();
            }
        }
    }
}
