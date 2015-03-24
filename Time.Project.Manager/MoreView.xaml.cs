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
    /// Interaction logic for MoreView.xaml
    /// </summary>
    public partial class MoreView : UserControl
    {
        public MoreView()
        {
            InitializeComponent();
        }

        public void RefreshStats()
        {
            stat_total_hours.Text = Functions.TimeToTextTimeFormat(Globals.activeProject.time_spent, true);
            stat_sessions_count.Text = Globals.activeProject.total_sessions+"";
            stat_longest_session.Text = Functions.TimeToTextTimeFormat(Globals.activeProject.longest_session_duration, true);
            stat_session_duration_average.Text = Functions.TimeToTextTimeFormat(Globals.activeProject.average_session_duration,true);
            stat_last_week.Text = Functions.TimeToTextTimeFormat(Globals.activeProject.last_week, true);
        }
    }
}
