using System.Net;

using static System.Console;
using uhppoted;

static class ArgParse
{
    public static T Parse<T>(string[] args, string arg, T defval)
    {
        int ix = Array.IndexOf(args, arg);
        uint u32;
        byte u8;
        bool b;
        IPAddress address;
        IPEndPoint endpoint;
        DateTime datetime;

        if (ix >= 0 && ix + 1 < args.Length)
        {
            ix++;
            switch (defval)
            {
                case uint:
                    if (UInt32.TryParse(args[ix], out u32)) return (T)(object)u32;
                    break;

                case byte:
                    if (Byte.TryParse(args[ix], out u8)) return (T)(object)u8;
                    break;

                case bool:
                    if (Boolean.TryParse(args[ix], out b)) return (T)(object)b;
                    break;

                case IPAddress:
#pragma warning disable CS8600
                    if (IPAddress.TryParse(args[ix], out address)) return (T)(object)address;
#pragma warning restore CS8600
                    break;

                case IPEndPoint:
#pragma warning disable CS8600
                    if (IPEndPoint.TryParse(args[ix], out endpoint)) return (T)(object)endpoint;
#pragma warning restore CS8600
                    break;

                case DateTime:
                    if (DateTime.TryParse(args[ix], out datetime)) return (T)(object)datetime;
                    break;

                case DoorMode:
                    switch (args[ix])
                    {
                        case "normally-open":
                            return (T)(object)DoorMode.NormallyOpen;

                        case "normally-closed":
                            return (T)(object)DoorMode.NormallyClosed;

                        case "controlled":
                            return (T)(object)DoorMode.Controlled;
                    }
                    break;
            }
        }

        return defval;
    }
}