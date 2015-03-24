using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;


namespace Time.Project.Manager
{
    sealed class  TPMP_Prompt
    {

        // singleton
        private static readonly TPMP_Prompt m_oInstance = new TPMP_Prompt();

        // gui children
        public System.Windows.Controls.TextBlock prompt;
        public System.Windows.Controls.TextBox commandline;


        // ****************
        private List<int> prompt_lines_length = new List<int>();
        private List<string> prompt_lines = new List<string>();

        // ****************
        // *** SETTINGS ***
        // ****************
        private int prompt_lines_max = 11;
        private int prompt_lines_used = 1;
        private List<SolidColorBrush> prompt_lines_color = new List<SolidColorBrush>();

        private TPMP_Prompt()
        {
            prompt = App.Current.MainWindow.FindName("prompt") as System.Windows.Controls.TextBlock;
            commandline = App.Current.MainWindow.FindName("commandline") as System.Windows.Controls.TextBox;
            prompt_lines_length.Add(23);
            prompt_lines_used = 1;
        }

        static TPMP_Prompt()
        {

        }

        public static TPMP_Prompt Instance
        {
            get
            {
                return m_oInstance;
            }
        }

        public void Prompt_Text(string txt, bool ai)
        {
            if (ai) prompt_lines_color.Add(Brushes.DarkSlateBlue);
            else
            {
                if (Prompt_Command(txt)) prompt_lines_color.Add(Brushes.Lime);
                else prompt_lines_color.Add(Brushes.Gray);
            }
            DateTime timeNow = DateTime.Now;
            string formattedTime = timeNow.TimeOfDay.ToString().Substring(0, timeNow.TimeOfDay.ToString().Length - timeNow.TimeOfDay.ToString().LastIndexOf("."));
            prompt_lines.Add(formattedTime + "  :  " + txt);
            prompt_lines_length.Add(txt.Length);

            prompt_lines_used++;

            if (prompt_lines_used >= prompt_lines_max)
            {
                prompt_lines.RemoveAt(0);
                prompt_lines_length.RemoveAt(0);
                prompt_lines_color.RemoveAt(0);
                prompt_lines_used--;
            }

            prompt.Text = null;


            for (var i = 0; i < prompt_lines.Count; i++)
            {
                prompt.Inlines.Add(new Run(prompt_lines[i]) { Foreground = prompt_lines_color[i] });
                prompt.Inlines.Add("\n");
            }

            if (!ai) commandline.Text = "";



        }
        private bool Prompt_Command(string cmd)
        {
            bool is_cmd = false;
            if (cmd == "clear")
            {
                is_cmd = true;
                prompt.Text = null;
                prompt_lines.Clear();
                prompt_lines_length.Clear();
                prompt_lines_used = 0;
                prompt_lines_color.Clear();
            }
            if (cmd.Length >= 3)
            {
                if (cmd.Substring(0, 3) == "new")
                {
                    TPMP_ProjectManager.Instance.Create_New_Project(cmd.Substring(4, cmd.Length - 4));
                    is_cmd = true;
                }
                // set command
                if (cmd.Substring(0, 3) == "set")
                {
                    
                    if (cmd.Length >= 13)
                    { 
                        if(cmd.Substring(4,cmd.Length-4) == "root_path")
                        {
                            is_cmd = true;
                            TPMP_ProjectManager.Instance.Set_New_Root_Path(); // this is fine.
                        
                        }
                    }
                }
            }
            if (cmd.Length >= 6)
            {
                if (cmd.Substring(0, 6) == "select")
                {
                     // TO-DO do this stuff in PROJECT MANAGER god damn it.. 
                    string tmpName = cmd.Substring(7, cmd.Length - 7);
                    bool succed = false;
                    foreach (TPMP_Project value in Globals.listProjects)
                    {
                        if (value.name == tmpName) { Globals.activeProject = value; succed = true; break; }
                    }
                    if (succed) Prompt_Text("Selected " + Globals.activeProject.name + " as active project", true);
                    else Prompt_Text("Project " + tmpName + " not found, sir", true);
                    is_cmd = true;
                }
                if (cmd.Substring(0, 6) == "delete")
                { // this stuff als in project manager!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    string tmpName = cmd.Substring(7, cmd.Length - 7);
                    bool succed = false;
                    foreach (TPMP_Project value in Globals.listProjects)
                    {
                        if (value.name == tmpName) {succed = true; break; }
                    }
                    if (succed)
                    {
                        if (TPMP_ProjectManager.Instance.DeleteProject(tmpName))
                        {
                            Prompt_Text("Deleted " + tmpName + " project", true);
                        }
                    }
                    else Prompt_Text("Project " + tmpName + " not found, sir", true);
                    is_cmd = true;
                }
            }


            return is_cmd;
        }
    }
}
