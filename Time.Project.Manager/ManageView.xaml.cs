﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Time.Project.Manager
{
    /// <summary>
    /// Interaction logic for ManageView.xaml
    /// </summary>
    public partial class ManageView : UserControl
    {
        public ManageView()
        {
            InitializeComponent();
        }

        public void btnLogView(object sender, RoutedEventArgs e)
        {
            Globals.mainView.btnManageView_Log();
        }

        private void butShowLog_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
