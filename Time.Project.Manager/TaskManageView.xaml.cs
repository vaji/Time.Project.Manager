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
    /// Interaction logic for TaskManageView.xaml
    /// </summary>
    public partial class TaskManageView : UserControl
    {
        public TaskManageView()
        {
            InitializeComponent();
        }

        public void btnLoadTasks(object sender, RoutedEventArgs e)
        {
            tasks_list.Items.Clear();
            TPMP_TaskManager.Instance.LoadTasks(ref tasks_list,-1,true);
        }
        public void btnRemoveTask(object sender, RoutedEventArgs e)
        {
            string selection = tasks_list.SelectedItem.ToString();
            int tmp_pos = selection.IndexOf(":");
            string name = selection.Substring(tmp_pos + 2, selection.Length - tmp_pos - 2).Trim();
           // MessageBox.Show(name);
            TPMP_TaskManager.Instance.RemoveTask(name);
            btnLoadTasks(this, null);
        }

        public void btnCompleteTask(object sender, RoutedEventArgs e)
        {
            string selection = tasks_list.SelectedItem.ToString();
            int tmp_pos = selection.IndexOf(":");
            string name = selection.Substring(tmp_pos + 2, selection.Length - tmp_pos - 2).Trim();
            // MessageBox.Show(name);
            TPMP_TaskManager.Instance.CompleteTask(name);
            btnLoadTasks(this, null);
        }
    }
}
