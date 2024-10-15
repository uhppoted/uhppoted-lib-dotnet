using static System.Console;

using static uhppoted.Uhppoted;

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

