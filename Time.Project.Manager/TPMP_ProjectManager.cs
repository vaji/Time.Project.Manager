using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Time.Project.Manager
{
    sealed class TPMP_ProjectManager
    {
        // singleton
        private static readonly TPMP_ProjectManager m_oInstance = new TPMP_ProjectManager();

        static TPMP_ProjectManager()
        { 
        
        }
        private TPMP_ProjectManager()
        {

        }

        public static TPMP_ProjectManager Instance
        {
            get
            {
                return m_oInstance;
            }
        }

        public void Create_New_Project(string name, string type = "no-type", string desc = "no-desc")
        {
            if (Can_Create_Project(name)) 
            {
                TPMP_Project newProj = new TPMP_Project();
                newProj.name = name;
                newProj.type = type;
                newProj.desc = desc;
                newProj.time_spent = new TimeSpan(0,0,0);
                Globals.listProjects.Add(newProj);

                Directory.CreateDirectory(Config.appDataPath + name);

                XmlWriter file = XmlWriter.Create(Config.appDataPath + name + "/info.xml");

                file.WriteStartDocument(true);
                file.WriteStartElement("general");

                file.WriteElementString("name", name);
                file.WriteElementString("type", type);
                file.WriteElementString("desc", desc);
                file.WriteElementString("total_time_spent", "0");

                file.WriteEndElement();
                file.WriteEndDocument();
                file.Close();

                file = XmlWriter.Create(Config.appDataPath + name + "/sessions.xml");
                file.WriteStartElement("sessions");
                file.WriteAttributeString("count", 0+"");
                file.WriteString("");
                file.WriteEndDocument();
                file.Close();

                file = XmlWriter.Create(Config.appDataPath + name + "/log.xml");
                file.WriteStartElement("log");

                file.WriteStartElement("entry");
                file.WriteAttributeString("time", DateTime.Now.ToString());
                file.WriteString("Created project");
                file.WriteEndElement();

                file.WriteString("");
                file.WriteEndDocument();
                file.Close();



                StreamWriter file2 = File.AppendText(Config.appDataPath + "projects");
                file2.WriteLine(name);
                file2.Close();

                TPMP_Prompt.Instance.Prompt_Text("Created new project called " + name + " of type " + type, true);
            }
            else
            {
                TPMP_Prompt.Instance.Prompt_Text("My apologies, sir, but there already exists project named " + name, true);
            }
        }

        public bool Can_Create_Project(string name)
        {
            bool can = true;

            foreach (TPMP_Project value in Globals.listProjects)
            {
                if (name == value.name) { can = false; break; }
            }
            return can;
        }

        public void LoadProjects() // TO-DO return bool value 
        {
            List<string> projectList = new List<string>();
            StreamReader file = new StreamReader(Config.appDataPath + "projects");

            while (!file.EndOfStream)
            {
                projectList.Add(file.ReadLine());
            }
            file.Close();

            foreach (string value in projectList)
            {
                TPMP_Project tmp = new TPMP_Project();
                if (tmp.Load(value))
                {
                    Globals.listProjects.Add(tmp);
                }
                else MessageBox.Show("There was a problem loading " + value + " project.");
              
            }
        
        }

        public bool DeleteProject(string name)
        {
            bool exists = false;
            TPMP_Project tmp;
            foreach (TPMP_Project value in Globals.listProjects)
            {
                if (value.name == name) 
                { 
                    exists = true;
                    tmp = value;
                    Globals.listProjects.Remove(value); 
                    tmp = null; 
                    break; 
                }
            }
            if (exists)
            {

                try 
                {
                   Directory.Delete(Config.appDataPath+name,true);
                    // delete project name from project list in projects file in app data folder
                   string[] lines = File.ReadAllLines(Config.appDataPath+"projects");
                   string[] new_lines = new string[lines.Length-1];
                   int p = 0;
                   for(int i = 0;i<lines.Length;i++)
                   {
                       if (lines[i] != name)
                       {
                           new_lines[p] = lines[i];
                           p++;
                       }
                   }
                   File.WriteAllLines(Config.appDataPath + "projects", new_lines);
                 
                }
                catch(DirectoryNotFoundException) { MessageBox.Show("Project files corrupted"); return true; }
            }
            else
            {
                TPMP_Prompt.Instance.Prompt_Text("There is no such project named " + name + ", sir", true);
                return false;
            }
            return true;
        }
        public void Set_New_Root_Path()
        {
            FolderBrowserDialog newRootPathDialog = new FolderBrowserDialog();
            newRootPathDialog.Description = "Please select new root folder for your projects, Sir.";
            if (newRootPathDialog.ShowDialog() == DialogResult.OK)
            {
                Config.rootFolderPath = newRootPathDialog.SelectedPath;
                TPMP_Prompt.Instance.Prompt_Text("New path of root folder is: " + Config.rootFolderPath, true);
                SaveConfig();
            }
        }

        public void SaveConfig()
        {
            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Config.configPath);
            XmlWriter saver = XmlWriter.Create(configFilePath);

            saver.WriteStartDocument(true);
            saver.WriteElementString("rootPath", Config.rootFolderPath);
            saver.WriteEndDocument();
            saver.Close();
        }

        public bool LoadConfig()
        {
            string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Config.configPath);
            XmlReader loader = null;

            try
            {
                loader = XmlReader.Create(configFilePath);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"HyperGear/"));
            }
            catch (System.IO.FileNotFoundException)
            {
                XmlWriter createConfig = XmlWriter.Create(configFilePath);
                createConfig.Close();

                XmlWriter createProjFile = XmlWriter.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"HyperGear/projects.xml"));
                createProjFile.Close();

                XmlWriter createLastSessionFile = XmlWriter.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HyperGear/last_sessions.xml"));
                createLastSessionFile.WriteStartDocument();
                createLastSessionFile.WriteStartElement("last_sessions");
                createLastSessionFile.WriteAttributeString("count", 0 + "");
                createLastSessionFile.WriteEndDocument();
                createLastSessionFile.Close();

                XmlWriter createLastEntriesFile = XmlWriter.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HyperGear/last_entries.xml"));
                createLastSessionFile.WriteStartDocument();
                createLastSessionFile.WriteStartElement("last_entries");
                createLastSessionFile.WriteAttributeString("count", 0 + "");
                createLastSessionFile.WriteEndDocument();
                createLastEntriesFile.Close();

                XmlWriter createTasksFile = XmlWriter.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HyperGear/tasks.xml"));
                createLastSessionFile.WriteStartDocument();
                createLastSessionFile.WriteStartElement("tasks");
                createLastSessionFile.WriteEndDocument();
                createLastEntriesFile.Close();

                MessageBox.Show("First time with HyperGear, Sir?", "Nice to meet you :)");
                return false;
            }

            try 
            {
                loader = XmlReader.Create(configFilePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                XmlWriter createConfig = XmlWriter.Create(configFilePath);
                createConfig.Close();
                MessageBox.Show("First time with HyperGear, Sir?", "Nice to meet you :)");
                return false;
            }

            loader = XmlReader.Create(configFilePath);
            bool succeed = false;
            while (loader.Read())
            {
                if ((loader.NodeType == XmlNodeType.Element) && (loader.Name == "rootPath"))
                {

                    Config.rootFolderPath = loader.ReadElementString();
                    TPMP_Prompt.Instance.Prompt_Text("found path in config: " + Config.rootFolderPath, true);
                    succeed = true; loader.Close();  break;
                }

            }
            loader.Close();

            Config.appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HyperGear/");
            return succeed;
        }
    }
}
