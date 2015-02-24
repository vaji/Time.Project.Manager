using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time.Project.Manager
{
    class TPMP_ProjectManager
    {
        public void Create_New_Project(string name, string type = "no-type", string desc = "no-desc")
        {
            TPMP_Project newProj = new TPMP_Project();
            newProj.name = name;
            newProj.type = type;
            newProj.desc = desc;
            newProj.time_spent = 0;
            Globals.listProjects.Add(newProj);
            Globals.prompt.Prompt_Text("Created new project called " + name + " of type " + type, true);
        }
    }
}
