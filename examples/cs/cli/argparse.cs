using static System.Console;

static class ArgParse
{
    public static T Parse<T>(string[] args, string arg, T defval)
    {
        int ix = 0;
        while (ix < args.Length)
        {
            if (args[ix] == arg)
            {
                ix++;
                if (ix < args.Length)
                {
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
            }

            ix++;
        }

        return defval;
    }
}