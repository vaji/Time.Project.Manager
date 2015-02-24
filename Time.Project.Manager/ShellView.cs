using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Time.Project.Manager
{

    
    public partial class ShellView : Window
    {

        DispatcherTimer init_timer = new DispatcherTimer();
        DispatcherTimer update_timer = new DispatcherTimer();
        /// <summary>
        ///  constructor
        /// </summary>
        public ShellView()
        {
            InitializeComponent();
           
            init_timer.Interval = TimeSpan.FromMilliseconds(100);
            init_timer.Tick += init_Tick;
            init_timer.Start();

            update_timer.Interval = TimeSpan.FromMilliseconds(100);
            update_timer.Tick += update_Tick;
           
        }
      
        public void init_Tick(object sender, EventArgs e)
        {
            Globals.prompt = new TPMP_Prompt();
            Globals.projectManager = new TPMP_ProjectManager();
            Globals.activeProject = new TPMP_Project();
            init_timer.Stop();
            update_timer.Start();
        }

        public void update_Tick(object sedner, EventArgs e)
        {
            
        }
        public void Prompt_Text_Event(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && commandline.Text != null && commandline.Text != "")
            {
                Globals.prompt.Prompt_Text(commandline.Text, false);

            }
        }
     
    }
}
