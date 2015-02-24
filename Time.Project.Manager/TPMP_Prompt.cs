using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;

namespace Time.Project.Manager
{
    class TPMP_Prompt
    {
        public TextBlock prompt;
        public TextBox commandline;
        private int prompt_lines_max = 11;
        private int prompt_lines_used = 1;
        private List<int> prompt_lines_length = new List<int>();
        private List<string> prompt_lines = new List<string>();
        private List<SolidColorBrush> prompt_lines_color = new List<SolidColorBrush>();

        public TPMP_Prompt()
        {
            prompt = App.Current.MainWindow.FindName("prompt") as TextBlock;
            commandline = App.Current.MainWindow.FindName("commandline") as TextBox;
            prompt_lines_length.Add(23);
            prompt_lines_used = 1;
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
                    Globals.projectManager.Create_New_Project(cmd.Substring(3, cmd.Length - 3));
                    is_cmd = true;

                }
            }
            if (cmd.Length >= 6)
            {
                if (cmd.Substring(0, 6) == "select")
                {

                    string tmpName = cmd.Substring(6, cmd.Length - 6);
                    bool succed = false;
                    foreach (TPMP_Project value in Globals.listProjects)
                    {
                        if (value.name == tmpName) { Globals.activeProject = value; succed = true; break; }
                    }
                    if (succed) Prompt_Text("Selected " + Globals.activeProject.name + " as active project", true);
                    else Prompt_Text("Project " + tmpName + " not found, sir", true);
                    is_cmd = true;
                }
            }


            return is_cmd;
        }
    }
}
