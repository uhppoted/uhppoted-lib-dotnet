namespace uhppoted

module internal TCP =
    val sendTo:
        request: byte array * src: System.Net.IPEndPoint * dest: System.Net.IPEndPoint * timeout: int * debug: bool ->
            Result<byte array, string>
