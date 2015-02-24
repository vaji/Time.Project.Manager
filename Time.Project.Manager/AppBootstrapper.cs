﻿using System.Windows;
using Caliburn.Micro;

namespace Time.Project.Manager
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
           
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
           
           
        }
    }
}