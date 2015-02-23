using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Metro.Core;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.ComponentModel.Composition;

namespace Time.Project.Manager
{
    [Export(typeof(IWindowManager))];
    public class AppWindowManager : MetroWindowManager
    {
        public override MetroWindow CreateCustomWindow(object view, bool windowIsView)
        {
            if (windowIsView)
            {
                return view as MainWindowContainer;
            }

            return new MainWindowContainer
            {
                Content = view
            };
        }
    }
}
