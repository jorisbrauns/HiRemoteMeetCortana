using System.Collections.Generic;

namespace HiRemoteMeetCortana.RemoteCortana.ViewModel
{
    /// <summary>
    /// ViewModelLocator ensures that viewmodels can be instantiated with a common reference to the TripStore. 
    /// This allows for easier decoupling of the store implementation and the view models, and allows for 
    /// less viewmodel specific code in the views.
    /// </summary>
    public class ViewModelLocator
    {

        private Dictionary<string, ViewModelBase> modelSet = new Dictionary<string, ViewModelBase>();

        public ViewModelLocator()
        {
            modelSet.Add("MainViewModel", new MainViewModel());
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return (MainViewModel)modelSet["MainViewModel"];
            }
        }

    }
}
