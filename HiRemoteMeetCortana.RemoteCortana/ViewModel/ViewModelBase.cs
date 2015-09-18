//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HiRemoteMeetCortana.RemoteCortana.ViewModel
{
    /// <summary>
    /// Base class for all view models. Contains the common implementation of 
    /// INotifyPropertyChanged and the notification helper method.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            InitCommands();
        }

        protected abstract void InitCommands();

        /// <summary>
        /// Factory create command 
        /// </summary>
        protected ICommand CreateLightweightCommand(string display, Action execute)
        {
            return CreateLightweightCommand(display, execute, () => true);
        }

        protected ICommand CreateLightweightCommand(string display, Action execute, Func<bool> canExecute)
        {
            var cmd = new RelayCommand(display, () =>
            {
                execute();
            }, canExecute);
            return cmd;
        }
       
        /// <summary>
        /// Notify subscribers of updates to the named property
        /// </summary>
        /// <param name="propertyName">The full, case-sensitive, name of a property.</param>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            var pc = this.PropertyChanged;
            if (pc != null)
            {
                pc(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
