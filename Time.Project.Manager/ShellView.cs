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

        List<UIElement> centerViews = new List<UIElement>();
        Dictionary<UIElement,bool> centerViews_status = new Dictionary<UIElement,bool>();

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

            Globals.manageView = new ManageView();
            Globals.newProjectView = new NewProjectView();
            Globals.selectProjectView = new SelectProjectView();
            Globals.logView = new LogView();

            centerViews.Add(Globals.manageView); centerViews.Add(Globals.newProjectView); centerViews.Add(Globals.selectProjectView);
            centerViews.Add(Globals.logView);

            centerViews_status[Globals.manageView] = false; centerViews_status[Globals.newProjectView] = false; centerViews_status[Globals.selectProjectView] = false;
            centerViews_status[Globals.logView] = false;
            
            init_timer.Stop();
            update_timer.Start();

            
            
            if (!Globals.projectManager.LoadConfig())
            {
                Globals.projectManager.Set_New_Root_Path();
            }

            Globals.projectManager.LoadProjects();
        }

        public void update_Tick(object sedner, EventArgs e)
        {
            actProj_name.Text = Globals.activeProject.name;
            actProj_type.Text = Globals.activeProject.type;
        }

        /// <summary>
        /// 
        /// </summary>

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
                Globals.prompt.Prompt_Text(commandline.Text, false);

            }
        }
     
    }
}
