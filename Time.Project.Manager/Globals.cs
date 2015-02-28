using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Project.Manager;
using System.Xml;

static class Globals
{
    public static TPMP_Project activeProject;
    public static TPMP_Prompt prompt;
    public static TPMP_ProjectManager projectManager;
    public static List<TPMP_Project> listProjects = new List<TPMP_Project>();

    public static ManageView manageView;
    public static NewProjectView newProjectView;
    public static SelectProjectView selectProjectView;
    public static LogView logView;
}


static class Config
{
    public static string appDataPath;
    public const string configPath = "HyperGear/config.xml";
    public static string rootFolderPath;

    
}