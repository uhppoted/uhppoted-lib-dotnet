namespace uhppoted

/// Container class for the network configuration used to connect to an access controller.
type Options =
    { bind: System.Net.IPEndPoint
      broadcast: System.Net.IPEndPoint
      listen: System.Net.IPEndPoint
      timeout: int
      debug: bool }


/// Convenience 'Options' builder implementation for C# and VB.NET.
type OptionsBuilder =
    class
        new: unit -> OptionsBuilder
        member Build: unit -> Options
        member WithBind: endpoint: System.Net.IPEndPoint -> OptionsBuilder
        member WithBroadcast: endpoint: System.Net.IPEndPoint -> OptionsBuilder
        member WithDebug: enable: bool -> OptionsBuilder
        member WithListen: endpoint: System.Net.IPEndPoint -> OptionsBuilder
        member WithTimeout: ms: int -> OptionsBuilder
    end
