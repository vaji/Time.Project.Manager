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
using System.Xml;

namespace Time.Project.Manager
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        public void btnAddEntry(object sender, RoutedEventArgs e)
        {
            if(log_add_box.Text != null && log_add_box.Text != "")
                Globals.activeProject.LogAddEntry(log_add_box.Text);

            log_add_box.Clear();
        }

        public void btnLoadLog(object sender, RoutedEventArgs e)
        {
            LoadLog();
        }
        public void LoadLog()
        {
            if (Globals.activeProject.name != "no-name")
            {
                log_name.Text = "Project '" + Globals.activeProject.name + "' log";
                XmlReader file = null;
                try
                {
                    file = XmlReader.Create(Config.appDataPath + Globals.activeProject.name + "/log.xml");
                }
               catch(System.IO.FileNotFoundException) { MessageBox.Show("Could not load log, sir"); return; }
               catch (System.IO.DirectoryNotFoundException) { MessageBox.Show("Could not load log, sir"); return; }

                log_box.Items.Clear();
               while(file.Read())
               { 
                    if(file.NodeType == XmlNodeType.Element && file.Name == "entry")
                    {
                        string temp = file.GetAttribute("time");
                        temp += ": " + file.ReadElementString();
                        log_box.Items.Add(temp);
                    }

               }
               file.Close();
           }
        }
    }
}
