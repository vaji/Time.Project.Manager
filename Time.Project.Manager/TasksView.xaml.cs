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

namespace Time.Project.Manager
{
    /// <summary>
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class TasksView : UserControl
    {
        public TasksView()
        {
            InitializeComponent();
        }

        public void btnNewTask (object sender, RoutedEventArgs e)
        {
            Globals.mainView.btnTasksView_New();
        }

        public void btnManageTasks (object sender, RoutedEventArgs e)
        {
            Globals.mainView.btnTasksView_Manage();
            Globals.taskManageView.btnLoadTasks(this, null);
        }
    }
}
