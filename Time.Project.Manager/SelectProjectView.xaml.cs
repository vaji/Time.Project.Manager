using System;
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
using System.Windows.Threading;

namespace Time.Project.Manager
{
    /// <summary>
    /// Interaction logic for SelectProjectView.xaml
    /// </summary>
    public partial class SelectProjectView : UserControl
    {
        DispatcherTimer update_timer = new DispatcherTimer();
        DispatcherTimer constraint_timer = new DispatcherTimer();
        List<string> projList_names = new List<string>();

        public SelectProjectView()
        {
            InitializeComponent();

            update_timer.Interval = TimeSpan.FromMilliseconds(2000);
            update_timer.Tick += update_Tick;
            update_timer.Start();
        }

        private void update_Tick(object sender, EventArgs e)
        {
            // TO-DO not tick update finder listbox - use event driven structure to update projects in listbox!
            object selection = findProj_Finder.SelectedItem;

            projList_names.Clear();
            foreach(TPMP_Project value in Globals.listProjects)
            {
                projList_names.Add(value.name);
            }

            
            if(findProj_Label.Text == "" || findProj_Label.Text == null)
            {
                
                findProj_Finder.Items.Clear();
                foreach(string value in projList_names)
                {
                    findProj_Finder.Items.Add(value);
                }
            }
            findProj_Finder.SelectedItem = selection;
            
        }

        private void FinderConstraint(object sender, RoutedEventArgs e)
        {
            object selection = findProj_Finder.SelectedItem;
            string constraint = findProj_Label.Text;

            if (constraint != null && constraint != "")
            {
                findProj_Finder.Items.Clear();
                foreach (string value in projList_names)
                {
                    if (value.IndexOf(constraint) != -1)
                    {
                        findProj_Finder.Items.Add(value);
                    }
                }
            }
            else
            {
                findProj_Finder.Items.Clear();
                foreach (string value in projList_names)
                {
                    findProj_Finder.Items.Add(value);
                }
               
            }
            findProj_Finder.SelectedItem = selection;
        }


        private void btnSelect(object sender, EventArgs e)
        {
            if(findProj_Finder.SelectedItem != null)
            {
                foreach(TPMP_Project value in Globals.listProjects)
                {
                    if(value.name == findProj_Finder.SelectedItem.ToString()) 
                    {
                       Globals.activeProject.SetGlobalActive(value);
                       Globals.mainView.ForceButtonClick(Globals.mainView.butHome);
                       break;
                    }
                }
            }
           

        }
    }
}
