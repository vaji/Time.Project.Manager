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
    // MAIN VIEW CLASS
    // also is used  to initialize everything
    
    public partial class ShellView : Window
    {

        DispatcherTimer init_timer = new DispatcherTimer();
        DispatcherTimer update_timer = new DispatcherTimer();

        List<UIElement> centerViews = new List<UIElement>();
        Dictionary<UIElement,bool> centerViews_status = new Dictionary<UIElement,bool>();
        Dictionary<Button, UIElement> centerViews_button = new Dictionary<Button, UIElement>();
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

            if (!Initialize_Application()) MessageBox.Show("Something went wrong at start-up!");


            //  proceed with project manager init
            if (!TPMP_ProjectManager.Instance.LoadConfig())
            {
                TPMP_ProjectManager.Instance.Set_New_Root_Path();
            }

            TPMP_ProjectManager.Instance.LoadProjects();
            LocalButtonClick(butHome, null);
            
        }

        public void update_Tick(object sedner, EventArgs e)
        {
            actProj_name.Text = Globals.activeProject.name;
            actProj_type.Text = Globals.activeProject.type;
            if (Globals.currentSession.is_started()) actProj_timer.Text = Globals.currentSession.ElapsedTime();
            else actProj_timer.Text = "";

            
            actProj_lastsession.Text = "Last session: " + Functions.DateTimeToDaysAgo(Globals.activeProject.last_session_date);
            actProj_lastsession_length.Text = Functions.TimeToTextTimeFormat(Globals.activeProject.last_session_duration);

        }

        /// <summary>
        /// 
        /// </summary>

        private void LocalButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            UIElement uielem = centerViews_button[button];

            if (!centerViews_status[uielem])
            {
                CenterViewCloseAll();
                CenterGrid.Children.Add(uielem);
            }
            else
            {
                CenterGrid.Children.Remove(uielem);

            }

            centerViews_status[uielem] = !centerViews_status[uielem];
        }

        public void ForceButtonClick(Button but)
        {
            LocalButtonClick(but, null);
        }
        /* 
             
    }
    public void btnHome(object sender, RoutedEventArgs e)
    {
        if (!centerViews_status[Globals.homeView])
        {
            CenterViewCloseAll();
            CenterGrid.Children.Add(Globals.homeView);
            centerViews_status[Globals.homeView] = !centerViews_status[Globals.homeView];
        }
                  
    }
    public void btnManage(object sender, RoutedEventArgs e)
    {
          
        if (!centerViews_status[Globals.manageView])
        {
            CenterViewCloseAll();
            CenterGrid.Children.Add(Globals.manageView);
        }
        else
        {
            CenterGrid.Children.Remove(Globals.manageView);
                
        }

        centerViews_status[Globals.manageView] = !centerViews_status[Globals.manageView];
    }
        
    public void btnNewProject(object sender, RoutedEventArgs e)
    {
        if (!centerViews_status[Globals.newProjectView])
        {
            CenterViewCloseAll();
            CenterGrid.Children.Add(Globals.newProjectView);
        }
        else
        {
            CenterGrid.Children.Remove(Globals.newProjectView);
                
        }

        centerViews_status[Globals.newProjectView] = !centerViews_status[Globals.newProjectView];
    }

    public void btnSelectProject(object sender, RoutedEventArgs e)
    {
        if (!centerViews_status[Globals.selectProjectView])
        {
            CenterViewCloseAll();
            CenterGrid.Children.Add(Globals.selectProjectView);
        }
        else
        {
            CenterGrid.Children.Remove(Globals.selectProjectView);
               
        }

        centerViews_status[Globals.selectProjectView] = !centerViews_status[Globals.selectProjectView];
    }

    public void btnTasks(object sender, RoutedEventArgs e)
    {
         if (!centerViews_status[Globals.tasksView])
        {
            CenterViewCloseAll();
            CenterGrid.Children.Add(Globals.tasksView);
        }
        else
        {
            CenterGrid.Children.Remove(Globals.tasksView);
               
        }

         centerViews_status[Globals.tasksView] = !centerViews_status[Globals.tasksView];
        
    }
         */

        public void btnMoreProject(object sender, RoutedEventArgs e)
        {
            if (Globals.activeProject.name != "no-name")
            {
                if (!centerViews_status[Globals.moreView])
                {
                    CenterViewCloseAll();
                    CenterGrid.Children.Add(Globals.moreView);
                }
                else
                {
                    CenterGrid.Children.Remove(Globals.moreView);
                }

                centerViews_status[Globals.moreView] = !centerViews_status[Globals.moreView];
            }
            else MessageBox.Show("Please select active project first, sir.");
        }
        public void btnManageView_Log()
        {
            if (!centerViews_status[Globals.logView])
            {
                CenterViewCloseAll();
                CenterGrid.Children.Add(Globals.logView);
                Globals.logView.LoadLog();
            }
            else
            {
                CenterGrid.Children.Remove(Globals.logView);
            }

            centerViews_status[Globals.logView] = !centerViews_status[Globals.logView];
        }


        public void btnTasksView_New()
        {
            if (!centerViews_status[Globals.newTaskView])
            {
                CenterViewCloseAll();
                CenterGrid.Children.Add(Globals.newTaskView);
            }
            else
            {
                CenterGrid.Children.Remove(Globals.newTaskView);
            }

            centerViews_status[Globals.newTaskView] = !centerViews_status[Globals.newTaskView];
        }

        public void btnTasksView_Manage()
        {
            if (!centerViews_status[Globals.taskManageView])
            {
                CenterViewCloseAll();
                CenterGrid.Children.Add(Globals.taskManageView);
            }
            else
            {
                CenterGrid.Children.Remove(Globals.taskManageView);
            }

            centerViews_status[Globals.taskManageView] = !centerViews_status[Globals.taskManageView];
        }
        public void btnStartSession(object sender, EventArgs e)
        {
            if (Globals.activeProject.name != "no-name") {
                if(Globals.currentSession.status == Globals.Session_Status.awaiting)
                {
                    Globals.currentSession.StartNew();
                    panel_PauseStop.Visibility = System.Windows.Visibility.Visible;
                    actProj_timer_start.Visibility = System.Windows.Visibility.Collapsed;
                }
            } else
            {
                MessageBox.Show("Please select active project first, sir.");
            }
        }
        public void btnStopSession(object sender, EventArgs e)
        {
            if (Globals.activeProject.name != "no-name")
            {
                if(Globals.currentSession.status == Globals.Session_Status.started || Globals.currentSession.status == Globals.Session_Status.paused)
                {
                    Globals.currentSession.Stop();
                    Globals.activeProject.SaveSession(); // <---- this saves session to xml, and resets  current session object
                    panel_PauseStop.Visibility = System.Windows.Visibility.Collapsed;
                    actProj_timer_start.Visibility = System.Windows.Visibility.Visible;
                    
                }
            }
        }
        public void btnPauseSession(object sender, EventArgs e)
        {
            if (Globals.activeProject.name != "no-name")
            {
                if (Globals.currentSession.status == Globals.Session_Status.started)
                {
                    Globals.currentSession.Pause();
                }
                else if (Globals.currentSession.status == Globals.Session_Status.paused)
                {
                    Globals.currentSession.Unpause();
                }
            }
        }


        public void CenterViewCloseAll()
        {
            foreach (UIElement value in centerViews)
            {
                if (centerViews_status[value] == true)
                {
                    CenterGrid.Children.Remove(value);
                    centerViews_status[value] = false;
                }
            }
        }
        public void Prompt_Text_Event(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && commandline.Text != null && commandline.Text != "")
            {
                TPMP_Prompt.Instance.Prompt_Text(commandline.Text, false);
            }
        }

        private bool Initialize_Application()
        {
            // create references in Globals namespace for other classes to access them
            // singletons are handled already at this point
            Globals.activeProject = new TPMP_Project();
            Globals.currentSession = new TPMP_Session();

            Globals.mainView = this;
            Globals.homeView = new HomeView();
            Globals.manageView = new ManageView();
            Globals.newProjectView = new NewProjectView();
            Globals.selectProjectView = new SelectProjectView();
            Globals.logView = new LogView();
            Globals.tasksView = new TasksView();
            Globals.newTaskView = new NewTaskView();
            Globals.taskManageView = new TaskManageView();
            Globals.moreView = new MoreView();

            // handle views setup
            centerViews.Add(Globals.manageView); centerViews.Add(Globals.newProjectView); centerViews.Add(Globals.selectProjectView);
            centerViews.Add(Globals.logView); centerViews.Add(Globals.homeView); centerViews.Add(Globals.tasksView); centerViews.Add(Globals.newTaskView);
            centerViews.Add(Globals.taskManageView); centerViews.Add(Globals.moreView);


            centerViews_status[Globals.manageView] = false; centerViews_status[Globals.newProjectView] = false; centerViews_status[Globals.selectProjectView] = false;
            centerViews_status[Globals.logView] = false; centerViews_status[Globals.homeView] = false; centerViews_status[Globals.tasksView] = false; centerViews_status[Globals.newTaskView] = false;
            centerViews_status[Globals.taskManageView] = false; centerViews_status[Globals.moreView] = false;

            centerViews_button[butManage] = Globals.manageView; centerViews_button[butHome] = Globals.homeView; centerViews_button[butNewProject] = Globals.newProjectView;
            centerViews_button[butSelect] = Globals.selectProjectView; centerViews_button[butTasks] = Globals.tasksView; 

            init_timer.Stop();
            // run main timer ******** TO - DO: Replace update_Timer with other solution, as it is VERY much temporary
            update_timer.Start();
            return true;
        }
     
    }
}
