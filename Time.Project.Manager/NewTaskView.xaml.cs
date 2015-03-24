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
    /// Interaction logic for NewTaskView.xaml
    /// </summary>
    public partial class NewTaskView : UserControl
    {
        private DateTime date = DateTime.Now;

        public NewTaskView()
        {
            InitializeComponent();
            newTask_date_box.Text = date.ToString();
        }

        public void btnNewTask(object sender, RoutedEventArgs e)
        {
            if (newTask_name_box.Text != null && newTask_name_box.Text != "")
            {
                TPMP_TaskManager.Instance.AddTask(newTask_name_box.Text, date, newTask_duration_box.Text, 0);
                Globals.mainView.ForceButtonClick(Globals.mainView.butHome);
            }
        }
        public void btnDateBack(object sender, RoutedEventArgs e)
        {
            if (date.Date > DateTime.Now.Date)
            {
                date = date.AddDays(-1);
                newTask_date_box.Text = date.Date.ToString("D");
            }
        }

        public void btnDateForward(object sender, RoutedEventArgs e)
        {
            date = date.AddDays(1);
            newTask_date_box.Text = date.Date.ToString("D");

        }

    }
}
