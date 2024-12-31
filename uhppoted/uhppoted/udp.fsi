namespace uhppoted

module internal UDP =
    val broadcast:
        request: byte array *
        bind: System.Net.IPEndPoint *
        broadcast: System.Net.IPEndPoint *
        timeout: int *
        debug: bool ->
            Result<byte array list, string>

    val broadcastTo:
        request: byte array *
        bind: System.Net.IPEndPoint *
        broadcast: System.Net.IPEndPoint *
        timeout: int *
        debug: bool ->
            Result<byte array, string>

    val sendTo:
        request: byte array * src: System.Net.IPEndPoint * dest: System.Net.IPEndPoint * timeout: int * debug: bool ->
            Result<byte array, string>

    val listen:
        bind: System.Net.IPEndPoint ->
        callback: (byte array -> unit) ->
        token: System.Threading.CancellationToken ->
        debug: bool ->
            Result<unit, string>
