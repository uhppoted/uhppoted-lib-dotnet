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
        List<byte> keypads = new List<byte>();
        List<string> weekdays = new List<string>();
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

                case TaskCode:
                    switch (args[ix])
                    {
                        case "control door":
                            return (T)(object)TaskCode.ControlDoor;

                        case "unlock door":
                            return (T)(object)TaskCode.UnlockDoor;

                        case "lock door":
                            return (T)(object)TaskCode.LockDoor;

                        case "disable time profiles":
                            return (T)(object)TaskCode.DisableTimeProfiles;

                        case "enable time profiles":
                            return (T)(object)TaskCode.EnableTimeProfiles;

                        case "enable card + no PIN":
                            return (T)(object)TaskCode.EnableCardNoPIN;

                        case "enable card + IN PIN":
                            return (T)(object)TaskCode.EnableCardInPIN;

                        case "enable card + IN/OUT PIN":
                            return (T)(object)TaskCode.EnableCardInOutPIN;

                        case "enable more cards":
                            return (T)(object)TaskCode.EnableMoreCards;

                        case "disable more cards":
                            return (T)(object)TaskCode.DisableMoreCards;

                        case "trigger once":
                            return (T)(object)TaskCode.TriggerOnce;

                        case "disable pushbutton":
                            return (T)(object)TaskCode.DisablePushbutton;

                        case "enable pushbutton":
                            return (T)(object)TaskCode.EnablePushbutton;
                    }
                    break;

                case UInt32[]:
                    foreach (var token in args[ix].Split(','))
                    {
                        if (UInt32.TryParse(token.Trim(), out u32)) passcodes.Add(u32);
                    }

                    return (T)(object)passcodes.ToArray();

                case Byte[]:
                    foreach (var token in args[ix].Split(','))
                    {
                        if (Byte.TryParse(token.Trim(), out u8)) keypads.Add(u8);
                    }

                    return (T)(object)keypads.ToArray();

                case String[]:
                    foreach (var token in args[ix].Split(','))
                    {
                        switch (token)
                        {
                            case "Mon":
                                weekdays.Add("monday");
                                break;

                            case "Tue":
                                weekdays.Add("tuesday");
                                break;

                            case "Wed":
                                weekdays.Add("wednesday");
                                break;

                            case "Thu":
                                weekdays.Add("thursday");
                                break;

                            case "Fri":
                                weekdays.Add("friday");
                                break;

                            case "Sat":
                                weekdays.Add("saturday");
                                break;

                            case "Sun":
                                weekdays.Add("sunday");
                                break;
                        }
                    }

                    return (T)(object)weekdays.ToArray();

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