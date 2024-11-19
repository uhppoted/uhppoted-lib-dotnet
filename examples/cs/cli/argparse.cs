using System.Net;

using static System.Console;

static class ArgParse
{
    public static T Parse<T>(string[] args, string arg, T defval)
    {
        int ix = Array.IndexOf(args, arg);
        uint u32;
        IPAddress addr;

        if (ix >= 0 && ix + 1 < args.Length)
        {
            ix++;
            switch (defval)
            {
                case uint:
                    if (UInt32.TryParse(args[ix], out u32)) return (T)(object)u32;
                    break;

                case IPAddress:
                    if (IPAddress.TryParse(args[ix], out addr)) return (T)(object)addr;
                    break;
            }
        }

        return defval;
    }
}