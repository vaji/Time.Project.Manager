using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void SetGlobalActive(TPMP_Project proj)
        {
            this.name = proj.name;
            this.type = proj.type;
            this.desc = proj.desc;
            this.time_spent = proj.time_spent;
        }
    }
}
