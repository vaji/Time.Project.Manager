using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Project.Manager;
using System.Xml;

static class Globals
{
    public static TPMP_Session currentSession;
    public static TPMP_Project activeProject;

    public static List<TPMP_Project> listProjects = new List<TPMP_Project>();

    public static HomeView homeView;
    public static ShellView mainView;
    public static ManageView manageView;
    public static NewProjectView newProjectView;
    public static SelectProjectView selectProjectView;
    public static LogView logView;
    public static NewTaskView newTaskView;
    public static TasksView tasksView;
    public static TaskManageView taskManageView;
    public static MoreView moreView;

    public enum Session_Status
    {
        awaiting = 0,
        started,
        finished,
        paused = 3
    }
}


static class Config
{
    public static string appDataPath;
    public const string configPath = "HyperGear/config.xml";
    public static string rootFolderPath;

    public const int max_recent_sessions = 3;
    public const int max_recent_entries = 5;

}

static class Functions
{
    public static string DateTimeToDaysAgo(DateTime data)
    {
        string day = "";
        DateTime tomorrow = DateTime.Now.AddDays(1);
        DateTime yesterday = DateTime.Now.AddDays(-1);

        if (data.Date == DateTime.Now.Date) day = "Today";
        else if (data.Date == tomorrow.Date) day = "Tomorrow";
        else if (data.Date == yesterday.Date) day = "Yesterday";
        else
        {
            TimeSpan dni = data - DateTime.Now;
            int ile = System.Math.Abs(dni.Days);
            day = ile + " days ago";
        }
        return day;
    }

    public static string TimeToTextTimeFormat(TimeSpan time, bool no_seconds = false)
    {
        string format = "";

        if (time.Hours > 0) format += time.Hours + "h ";
        if (time.Minutes > 0) format += time.Minutes + "m ";
        if (time.Seconds > 0 && !no_seconds) format += time.Seconds + "s";
        
        return format;
    }
}