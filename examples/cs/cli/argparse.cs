using static System.Console;

static class ArgParse
{
    public static T Parse<T>(string[] args, string arg, T defval)
    {
        int ix = Array.IndexOf(args, arg);

        if (ix >= 0 && ix + 1 < args.Length)
        {
            ix++;
            switch (defval)
            {
                case uint:
                    uint v;
                    if (UInt32.TryParse(args[ix], out v))
                    {
                        return (T)(object)v;
                    }
                    break;
            }
        }

        return defval;
    }
}