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

namespace Time.Project.Manager
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();
        }

        private void btnNewProject(object sender, EventArgs e)
        {
            if(newProj_name_box.Text != null && newProj_name_box.Text != "")
            {
                Globals.projectManager.Create_New_Project(newProj_name_box.Text,newProj_type_box.Text,newProj_desc_box.Text);
            } else
            {
                MessageBox.Show("Project name cannot be empty, sir!");
            }
            
        }
    }
}
