using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Time.Project.Manager
{
    sealed class TPMP_TaskManager
    {
        // singleton
        private static readonly TPMP_TaskManager m_oInstance = new TPMP_TaskManager();

        static TPMP_TaskManager()
        {

        }
        private TPMP_TaskManager()
        {

        }

        public static TPMP_TaskManager Instance
        {
            get
            {
                return m_oInstance;
            }
        }

        public void AddTask(string desc, DateTime date, string duration, int important) // add new task and save it to file
        {
            // first read postion in xml file, this is to keep tasks date ascending, makes it easier during load - no need to sort it then
            XmlReader file = null; 
            try
            {
                file = XmlReader.Create(Config.appDataPath + "tasks.xml");
            }
            catch (System.IO.FileNotFoundException) { return;  }
            catch (System.IO.IOException) { return; }
         
          
            int line = 0;
            while(file.Read())
            {
                if(file.IsStartElement() && file.Name == "task")
                {
                    DateTime tmp = DateTime.Parse(file.GetAttribute("date"));
                    int tmp_int = DateTime.Compare(date, tmp);
                    if (tmp_int > 0) line++;
                }
            }
            file.Close();
            
            XmlDocument file2 = new XmlDocument();
            file2.Load(Config.appDataPath + "tasks.xml");

            XmlElement element = file2.CreateElement("task");
            element.SetAttribute("date", date.Date.ToString());
            element.SetAttribute("important", important+"");
            element.InnerText = desc;
          
            XmlNode temp_node = file2.DocumentElement.ChildNodes[line-1];
            file2.DocumentElement.InsertAfter(element, temp_node);
            file2.Save(Config.appDataPath + "tasks.xml");
        }

        public void RemoveTask(string name)
        {
            XmlReader file = null;

            try
            {
                file = XmlReader.Create(Config.appDataPath + "tasks.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.IO.IOException) { return; }

            int line = 0;
            while(file.Read())
            {
                if (file.IsStartElement() && file.Name == "task")
                {
                    if (file.ReadElementString() != name) line++;
                    else break;
                }
            }
            file.Close();

            XmlDocument file2 = new XmlDocument();

            try
            {
                file2.Load(Config.appDataPath + "tasks.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.IO.IOException) { return; }
         
            XmlNode node = file2.DocumentElement.ChildNodes[line];
            file2.DocumentElement.RemoveChild(node);
            file2.Save(Config.appDataPath + "tasks.xml");

        }

        public void LoadTasks(ref System.Windows.Controls.ListBox listbox, int days = -1, bool with_completed = false) // load all tasks (tasks in following days) in file to passed listbox
        {
            // for days = -1, load all tasks to listbox,
            // for days = 0, load today tasks
            // for days >= 1, load tasks for today + for following n-days

            XmlReader file = null;
            try 
            {
                file = XmlReader.Create(Config.appDataPath + "tasks.xml");
            }
            catch(System.IO.FileNotFoundException) { return; }
            catch(System.IO.IOException) { return; }

            while(file.Read())
            {
                if(file.IsStartElement() && file.Name == "task")
                {
                    DateTime date = DateTime.Parse(file.GetAttribute("date"));
                    // TO-DO  duration loading, timespan
                    int important = int.Parse(file.GetAttribute("important"));
                    string is_completed = file.GetAttribute("completed");
                    bool bIs_completed = false;
                    string desc = file.ReadElementString();

                    if (is_completed == "yes") bIs_completed = true;
                    else bIs_completed = false;

                    if(days == -1)
                    { 
                         if(with_completed)
                         {
                             listbox.Items.Add(is_completed + " " + date.Date.ToString("D") + ": " + desc);
                         }
                         else
                         {
                             if (!bIs_completed) listbox.Items.Add(is_completed + " " + date.Date.ToString("D") + ": " + desc);
                         }
                    }
                    else if(days == 0)
                    {
                        if(date.Date == DateTime.Now.Date)
                        {
                            if (with_completed)
                            {
                                listbox.Items.Add(is_completed + " " + date.Date.ToString("D") + ": " + desc);
                            }
                            else
                            {
                                if (!bIs_completed) listbox.Items.Add(is_completed + " " + date.Date.ToString("D") + ": " + desc);
                            }
                        }
                    }
                    else if(days > 0)
                    {
                        if((DateTime.Now - date).TotalDays <= days && (DateTime.Now < date))
                        {

                            if (with_completed)
                            {
                                listbox.Items.Add(is_completed + " " + date.Date.ToString("D") + ": " + desc);
                            }
                            else
                            {
                                if (!bIs_completed) listbox.Items.Add(is_completed + " " + date.Date.ToString("D") + ": " + desc);
                            }

                        }
                    }
                }
            }
            file.Close();

        }

        public void CompleteTask(string name)
        {
            XmlReader file = null;

            int line = 0;
            try
            {
                file = XmlReader.Create(Config.appDataPath + "tasks.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.IO.IOException) { return; }

            while(file.Read())
            {
                if(file.IsStartElement() && file.Name == "task")
                {
                    if (file.ReadElementString() != name)
                    {
                        line++;
                    }
                    else break;
                }
            }

            file.Close();

            XmlDocument file2 = new XmlDocument();

            try
            {
                file2.Load(Config.appDataPath + "tasks.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.IO.IOException) { return; }

            XmlElement node = file2.DocumentElement.ChildNodes[line] as XmlElement;
            node.SetAttribute("completed", "yes");
            file2.Save(Config.appDataPath + "tasks.xml");

        }
    }
}
