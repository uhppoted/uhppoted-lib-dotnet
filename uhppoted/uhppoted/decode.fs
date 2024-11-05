namespace uhppoted

open System
open System.Globalization
open System.Net
open System.Net.NetworkInformation

module Decode =
    let unpackU8 (slice: byte array) = slice[0]

    let unpackU16 (slice: byte array) =
        let u16 = uint16 0
        let u16 = u16 + uint16 slice[0]
        let u16 = u16 + (uint16 slice[1] <<< 8)
        u16

    let unpackU32 (slice: byte array) =
        let u32 = uint32 0
        let u32 = u32 + uint32 slice[0]
        let u32 = u32 + (uint32 slice[1] <<< 8)
        let u32 = u32 + (uint32 slice[2] <<< 16)
        let u32 = u32 + (uint32 slice[3] <<< 24)
        u32

    let unpackBool (slice: byte array) =
        match slice[0] with
        | 0x01uy -> true
        | _ -> false

    let unpackIPv4 (slice: byte array) = IPAddress(slice[0..3])

    let unpack_version (slice: byte array) = $"v%x{slice[0]}.%02x{slice[1]}"

    let unpackMAC (slice: byte array) = PhysicalAddress(slice[0..5])

    let unpack_date (slice: byte array) =
        try
            let bcd = $"%02x{slice[0]}%02x{slice[1]}-%02x{slice[2]}-%02x{slice[3]}"
            let date = DateOnly.ParseExact(bcd, "yyyy-MM-dd")
            Some(date)
        with _ ->
            None

    let unpack_datetime (slice: byte array) : Nullable<DateTime> =
        let bcd =
            $"%02x{slice[0]}%02x{slice[1]}-%02x{slice[2]}-%02x{slice[3]} %02x{slice[4]}:%02x{slice[5]}:%02x{slice[6]}"

        match
            System.DateTime.TryParseExact(
                bcd,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal
            )
        with
        | true, datetime -> Nullable datetime
        | false, _ -> Nullable()

    let get_controller_response (packet: byte array) : Result<GetControllerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CONTROLLER then
            Error("invalid get-controller response")
        else
            Ok(
                { controller = unpackU32 packet[4..]
                  address = unpackIPv4 packet[8..]
                  netmask = unpackIPv4 packet[12..]
                  gateway = unpackIPv4 packet[16..]
                  MAC = unpackMAC packet[20..25]
                  version = unpack_version packet[26..]
                  date = unpack_date (packet[28..]) }
            )

    let get_listener_response (packet: byte array) : Result<GetListenerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_LISTENER then
            Error("invalid get-listener response")
        else
            let controller = unpackU32 packet[4..]
            let address = unpackIPv4 packet[8..]
            let port = unpackU16 packet[12..]
            let interval = unpackU8 packet[14..]

            Ok(
                { controller = controller
                  endpoint = IPEndPoint(address, int port)
                  interval = interval }
            )

    let set_listener_response (packet: byte array) : Result<SetListenerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_LISTENER then
            Error("invalid set-listener response")
        else
            Ok(
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }
            )

    let get_time_response (packet: byte array) : Result<GetTimeResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_TIME then
            Error("invalid get-time response")
        else
            Ok(
                { controller = unpackU32 packet[4..]
                  datetime = unpack_datetime (packet[8..]) }
            )

    let set_time_response (packet: byte array) : Result<SetTimeResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_TIME then
            Error("invalid set-time response")
        else
            Ok(
                { controller = unpackU32 packet[4..]
                  datetime = unpack_datetime (packet[8..]) }
            )

    let get_door_settings_response (packet: byte array) : Result<GetDoorSettingsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_DOOR then
            Error("invalid get-door-settings response")
        else
            Ok(
                { controller = unpackU32 packet[4..]
                  door = unpackU8 (packet[8..])
                  mode = unpackU8 (packet[9..])
                  delay = unpackU8 (packet[10..]) }
            )
