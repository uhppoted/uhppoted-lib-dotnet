using static System.Console;

using static uhppoted_lib_dotnet.Uhppoted;

class Commands
{

    public static void GetControllers()
    {
        try
        {
            get_controllers();
        }
        catch (Exception err)
        {
            WriteLine("Exception caught: " + err.Message);
            WriteLine("StackTrace: " + err.StackTrace);
        }
    }
}

