# HiRemoteMeetCortana
Stop dreaming!
No, really... it’s about time to wake up now. So, why is it so hard?

Get up and maximize your energy with our Cortana (by Windows 10) and Raspberry Pi light-hack!

It’s so easy: tell Cortana at what time you want to wake up and she will gently light up your room with whatever bulb you want.

Easy to implement explained top to bottom. All code contains comment to get a good understanding how things are working.
If something is not clear, you can find more information on my blog: http://blog.jorisbrauns.be

#HiRemoteMeetCortana.RemoteCortana
Is the remote controller to turn on the light bulb alarm by using Cortana our the user interface. Build as a Universal Windows Platform application for Windows 10 Family. Cortana is an intelligent personal assistant created by Microsoft which is a voice control.

#HiRemoteMeetCortana.VoiceCommandService
The voice command service is used in the previous project as a background service. By feeding this background service, Cortana is able to interact with the Universal Application Platform even when it's not active.

#HiRemoteMeetCortana.WebService
This project is our middleware and communication layer between Rasbperry Pi and our Universal Windows Platform (UWP) which is hosted on Azure. (Microsoft cloud hosting)
When we have sent a command from UWP to our middleware it will pass on a message by using SignalR to our Raspberry Pi.
SignalR is a library for ASP.NET developers to add real-time web functionality to applications.

#HiRemoteMeetCortana.RaspberryPi
All the logic of turning on the light bulb is handled in this UWP. It is receiving his information from the middleware (azure) and will make sure the light goes on when needed.

