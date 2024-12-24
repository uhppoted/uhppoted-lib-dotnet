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
        DateOnly date;
        List<uint> passcodes = new List<uint>();
        Dictionary<int, byte> permissions = new Dictionary<int, byte>();

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

                case DateOnly:
                    if (DateOnly.TryParse(args[ix], out date)) return (T)(object)date;
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

                case Interlock:
                    switch (args[ix])
                    {
                        case "none":
                            return (T)(object)Interlock.None;

                        case "1&2":
                            return (T)(object)Interlock.Doors12;

                        case "3&4":
                            return (T)(object)Interlock.Doors34;

                        case "1&2,3&4":
                            return (T)(object)Interlock.Doors12And34;

                        case "1&2&3":
                            return (T)(object)Interlock.Doors123;

                        case "1&2&3&4":
                            return (T)(object)Interlock.Doors1234;
                    }
                    break;

                case UInt32[]:
                    foreach (var token in args[ix].Split(','))
                    {
                        if (UInt32.TryParse(token.Trim(), out u32)) passcodes.Add(u32);
                    }

                    return (T)(object)passcodes.ToArray();

                case Dictionary<Int32, Byte>:
                    foreach (var token in args[ix].Split(','))
                    {
                        var permission = token.Split(':');
                        int door;
                        byte profile;

                        if (permission.Length > 0)
                        {
                            if (Int32.TryParse(permission[0].Trim(), out door))
                            {
                                if (permission.Length > 1)
                                {
                                    if (Byte.TryParse(permission[1].Trim(), out profile))
                                    {
                                        permissions[door] = profile;
                                    }
                                }
                                else
                                {
                                    permissions[door] = (byte)1;
                                }
                            }
                        }

                    }
                    return (T)(object)permissions;
            }
        }

        return defval;
    }
}