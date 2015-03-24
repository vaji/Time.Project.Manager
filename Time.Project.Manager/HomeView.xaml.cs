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
using System.Xml;
namespace Time.Project.Manager
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            DispatcherTimer update_tick = new DispatcherTimer();
            update_tick.Interval = TimeSpan.FromSeconds(1);
            update_tick.Tick += update_Tick;
            update_tick.Start();
           
           
        }

        public void update_Tick(object sender, EventArgs e)
        {

            // LOAD TASKS
            tasks_today.Items.Clear();
            tasks_next.Items.Clear();
            TPMP_TaskManager.Instance.LoadTasks(ref tasks_today, 0);
            TPMP_TaskManager.Instance.LoadTasks(ref tasks_next, 7);

           
            XmlReader file = null;
            // LOAD LAST SESSIONS
            // also refresh last session of active project
            Globals.activeProject.LoadLastSession();

            try
            {
                file = XmlReader.Create(Config.appDataPath + "last_sessions.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.Security.SecurityException) { return; }

            recent_sessions.Items.Clear();
            while(file.Read())
            {
                if(file.IsStartElement())
                {
                    if(file.Name == "session")
                    {
                        string proj_name = file.GetAttribute("project");
                        string duration = file.GetAttribute("duration");
                        string temp_item = file.ReadElementString();

                        DateTime date = DateTime.Parse(temp_item);
                        temp_item =  Functions.DateTimeToDaysAgo(date);

                        TimeSpan dur = TimeSpan.Parse(duration);
                        duration = Functions.TimeToTextTimeFormat(dur);

                        recent_sessions.Items.Add(temp_item +" - " + proj_name + "  :  " + duration);
                    }
                }
            }
            file.Close();

            // LOAD LAST ENTRIES
            try
            {
                file = XmlReader.Create(Config.appDataPath + "last_entries.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.Security.SecurityException) { return; }

            recent_entries.Items.Clear();
            while (file.Read())
            {
                if (file.IsStartElement())
                {
                    if (file.Name == "entry")
                    {
                        string proj_name = file.GetAttribute("project");
                        string time = file.GetAttribute("time");
                        string txt = file.ReadElementString();

                        DateTime date = DateTime.Parse(time);
                        time = Functions.DateTimeToDaysAgo(date);

                        recent_entries.Items.Add(time + " - " + proj_name + "  :  " + txt);
                    }
                }
            }
            file.Close();
        }
    }
}
