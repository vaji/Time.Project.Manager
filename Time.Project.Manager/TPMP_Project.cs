using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace Time.Project.Manager
{
    class TPMP_Project
    {
        public string name;
        public string type;
        public string desc;


        public TimeSpan time_spent;
        public int total_sessions;
        public TimeSpan longest_session_duration;
        public TimeSpan average_session_duration;
        public TimeSpan last_week;


        public DateTime last_session_date;
        public TimeSpan last_session_duration;

        public TPMP_Project()
        {
            name = "no-name";
            type = "no-type";
            desc = "no-desc";
            time_spent = new TimeSpan(0,0,0);
        }

        public void LogAddEntry(string entry)
        {
            if (name != "no-name") // TO-DO exceptions
            {
                XmlDocument file = new XmlDocument();
                try
                {
                    file.Load(Config.appDataPath + name + "/log.xml");
                }
                catch (System.IO.FileNotFoundException) { return; }
                

                XmlElement element = file.CreateElement("entry");
                element.InnerText = entry;
                element.SetAttribute("time", DateTime.Now.ToString());

                file.DocumentElement.AppendChild(element);
                file.Save(Config.appDataPath + name + "/log.xml");

                

                // also save entry to recent entries


                XmlDocument file2 = new XmlDocument();
                try
                {
                    file2.Load(Config.appDataPath + "last_entries.xml");
                }
                catch (System.IO.FileNotFoundException) { return;  }
                

                int ile = int.Parse(file2.DocumentElement.GetAttribute("count"));
                if (ile >= Config.max_recent_entries) // 
                {
                    file2.DocumentElement.RemoveChild(file2.DocumentElement.FirstChild);
                    ile--;
                }

                XmlElement element2 = file2.CreateElement("entry");
                element2.InnerText = entry;
                element2.SetAttribute("project", name);
                element2.SetAttribute("time", DateTime.Now.ToString());

                file2.DocumentElement.AppendChild(element2);

                file2.DocumentElement.SetAttribute("count", ile + 1 + "");
                file2.Save(Config.appDataPath + "last_entries.xml");

                // refresh log
                Globals.logView.LoadLog();
            }    
        }
        public void SaveSession()
        {
            XmlDocument file = new XmlDocument();

            try
            {
                file.Load(Config.appDataPath + name + "/sessions.xml");
            }
            catch (System.IO.FileNotFoundException) { return;  }

            int ile_sesji = int.Parse(file.DocumentElement.GetAttribute("count"));
            file.DocumentElement.SetAttribute("count", ile_sesji + 1 + "");

            XmlElement element = file.CreateElement("session");
            element.InnerText = Globals.currentSession.startTime.ToString();
            element.SetAttribute("duration", Globals.currentSession.elapsedTime.ToString());
            file.DocumentElement.AppendChild(element);
            file.Save(Config.appDataPath + name + "/sessions.xml");


            // also save session as recent session

            XmlDocument file2 = new XmlDocument();
            try
            {
                file2.Load(Config.appDataPath + "last_sessions.xml");
            }
            catch (System.IO.FileNotFoundException) { return;  }
           

            int ile = int.Parse(file2.DocumentElement.GetAttribute("count"));
            if(ile >= Config.max_recent_sessions)
            {
                file2.DocumentElement.RemoveChild(file2.DocumentElement.FirstChild);
                ile--;
            }

            XmlElement element2 = file2.CreateElement("session");
            element2.InnerText = Globals.currentSession.startTime.ToString();
            element2.SetAttribute("project", name);
            element2.SetAttribute("duration", Globals.currentSession.elapsedTime.ToString());
            
            file2.DocumentElement.AppendChild(element2);

            file2.DocumentElement.SetAttribute("count", ile + 1 +"");
            file2.Save(Config.appDataPath + "last_sessions.xml");
          

            // reset session
            Globals.currentSession.Clear();
            Globals.currentSession = new TPMP_Session();
        }

        public void LoadLastSession()
        {
            TPMP_Session temp_session = new TPMP_Session();

            XmlReader file = null;
            try
            {
                file = XmlReader.Create(Config.appDataPath + name + "/sessions.xml");
            }
            catch (System.IO.FileNotFoundException) { return; }
            catch (System.IO.DirectoryNotFoundException) { return; }

            int counter = 0;
            int ile_sesji = 0;
            while (file.Read())
            { 
                if(file.IsStartElement() && file.Name == "sessions")
                {
                   ile_sesji = int.Parse(file.GetAttribute("count"));
                }

                if(file.IsStartElement() && file.Name == "session")
                {
                    counter++;
                    if(counter >= ile_sesji)
                    {
                        last_session_duration = TimeSpan.Parse(file.GetAttribute("duration"));
                        file.Read();
                        last_session_date = DateTime.Parse(file.Value.Trim());
                        file.Close();
                        break;
                    }
                }
            }
            file.Close();
            
            

        }
        public bool Load(string name) 
        {
            bool succeed = true;

            XmlReader loader = null;
            try
            {
                loader = XmlReader.Create(Config.appDataPath + name + "/info.xml");
            }
            catch (System.IO.FileNotFoundException) { return false; }
            catch (System.IO.DirectoryNotFoundException) { return false; }

            while (loader.Read())
            {
                if (loader.IsStartElement()) // TO - DO 0.2 wonder if possible to do better xml read
                {
                    switch(loader.Name)
                    {
                        case "name": loader.Read(); this.name = loader.Value.Trim();
                            break;
                        case "type": loader.Read(); this.type = loader.Value.Trim();
                            break;
                        case "desc": loader.Read(); this.desc = loader.Value.Trim();
                            break;
                        case "total_time_spent": loader.Read(); 
                            break;
                    }
                
                }
            }
            TPMP_Prompt.Instance.Prompt_Text("loaded project " + name, true);
            loader.Close();
            return succeed;

        }
        public void SetGlobalActive(TPMP_Project proj)
        {
            this.name = proj.name;
            this.type = proj.type;
            this.desc = proj.desc;
            this.time_spent = proj.time_spent;
            LoadLastSession();
            LoadBasicStatistics();
        }

        public void LoadBasicStatistics()
        {
            // total time, sessions count, entries count,
            // average session duration, longest session, time percent of week

            time_spent = new TimeSpan(0, 0, 0);
            longest_session_duration = new TimeSpan(0, 0, 0);
            total_sessions = 0;
            last_week = new TimeSpan(0, 0, 0);

            XmlReader file = null;
            try
            {
                file = XmlReader.Create(Config.appDataPath + name + "/sessions.xml");
            } catch(System.IO.IOException) { return; }

            while(file.Read())
            {
                if(file.IsStartElement() && file.Name == "session")
                {
                    TimeSpan temp_time = TimeSpan.Parse(file.GetAttribute("duration"));
                    time_spent += temp_time;
                    if (temp_time > longest_session_duration) longest_session_duration = temp_time;
                    total_sessions++;

                    DateTime temp_date = DateTime.Parse(file.ReadElementString());
                    if((DateTime.Now > temp_date) && ((DateTime.Now - temp_date).TotalDays <= 7))
                    {
                        last_week += temp_time;
                    }
                }
            }

            file.Close();

            int temp_avg = 0;
            if(total_sessions != 0) temp_avg = (Convert.ToInt32(time_spent.TotalSeconds) / total_sessions);
            
            average_session_duration = new TimeSpan(0,0,temp_avg);
            Globals.moreView.RefreshStats();
        }
    }
}
