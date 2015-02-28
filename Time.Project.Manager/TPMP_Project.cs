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
        public UInt64 time_spent;

        public TPMP_Project()
        {
            name = "no-name";
            type = "no-type";
            desc = "no-desc";
            time_spent = 0;
        }

        public void LogAddEntry(string entry)
        {
            XmlDocument file = new XmlDocument();
            file.LoadXml(Config.appDataPath + name + "/log.xml");

            XmlElement element = file.CreateElement("entry");
            element.SetAttribute("entry",DateTime.Now.ToString());

            file.DocumentElement.AppendChild(element);
            file.Save(Config.appDataPath + name + "/log.xml");

        }
        public bool Load(string name) // TO-DO return bool whether loaded or not
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
                        case "total_time_spent": loader.Read(); this.time_spent = UInt32.Parse(loader.Value.Trim());
                            break;
                    }
                
                }
            }
            Globals.prompt.Prompt_Text("loaded project " + name, true);
            loader.Close();
            return succeed;

        }
        public void SetGlobalActive(TPMP_Project proj)
        {
            this.name = proj.name;
            this.type = proj.type;
            this.desc = proj.desc;
            this.time_spent = proj.time_spent;
        }
    }
}
