namespace uhppoted

open System
open System.Globalization
open System.Net
open System.Net.NetworkInformation
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module internal Decode =
    let unpack_u8 (slice: byte array) = slice[0]

    let unpack_u16 (slice: byte array) =
        let u16 = uint16 0
        let u16 = u16 + uint16 slice[0]
        let u16 = u16 + (uint16 slice[1] <<< 8)
        u16

    let unpack_u32 (slice: byte array) =
        let u32 = uint32 0
        let u32 = u32 + uint32 slice[0]
        let u32 = u32 + (uint32 slice[1] <<< 8)
        let u32 = u32 + (uint32 slice[2] <<< 16)
        let u32 = u32 + (uint32 slice[3] <<< 24)
        u32

    let unpack_bool (slice: byte array) =
        match slice[0] with
        | 0x01uy -> true
        | _ -> false

    let unpackIPv4 (slice: byte array) = IPAddress(slice[0..3])

    let unpack_version (slice: byte array) = $"v%x{slice[0]}.%02x{slice[1]}"

    let unpack_MAC (slice: byte array) = PhysicalAddress(slice[0..5])

    let unpack_date (slice: byte array) =
        let bcd = $"%02x{slice[0]}%02x{slice[1]}-%02x{slice[2]}-%02x{slice[3]}"

        match DateOnly.TryParseExact(bcd, "yyyy-MM-dd") with
        | true, date -> Nullable date
        | false, _ -> Nullable()

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

    let unpack_yymmdd (slice: byte array) =
        let bcd = $"20%02x{slice[0]}-%02x{slice[1]}-%02x{slice[2]}"

        match DateOnly.TryParseExact(bcd, "yyyy-MM-dd") with
        | true, date -> Nullable date
        | false, _ -> Nullable()

    let unpack_HHmm (slice: byte array) =
        let bcd = $"%02x{slice[0]}:%02x{slice[1]}"

        match TimeOnly.TryParseExact(bcd, "HH:mm") with
        | true, time -> Nullable time
        | false, _ -> Nullable()

    let unpack_HHmmss (slice: byte array) =
        let bcd = $"%02x{slice[0]}:%02x{slice[1]}:%02x{slice[2]}"

        match TimeOnly.TryParseExact(bcd, "HH:mm:ss") with
        | true, time -> Nullable time
        | false, _ -> Nullable()

    let getControllerResponse (packet: byte array) : Result<GetControllerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CONTROLLER then
            Error("invalid get-controller response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  address = unpackIPv4 packet[8..]
                  netmask = unpackIPv4 packet[12..]
                  gateway = unpackIPv4 packet[16..]
                  MAC = unpack_MAC packet[20..25]
                  version = unpack_version packet[26..]
                  date = unpack_date (packet[28..]) }

    let getListenerResponse (packet: byte array) : Result<GetListenerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_LISTENER then
            Error("invalid get-listener response")
        else
            let controller = unpack_u32 packet[4..]
            let address = unpackIPv4 packet[8..]
            let port = unpack_u16 packet[12..]
            let interval = unpack_u8 packet[14..]

            Ok
                { controller = controller
                  endpoint = IPEndPoint(address, int port)
                  interval = interval }

    let setListenerResponse (packet: byte array) : Result<SetListenerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_LISTENER then
            Error("invalid set-listener response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool packet[8..] }

    let get_time_response (packet: byte array) : Result<GetTimeResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_TIME then
            Error("invalid get-time response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  datetime = unpack_datetime (packet[8..]) }

    let set_time_response (packet: byte array) : Result<SetTimeResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_TIME then
            Error("invalid set-time response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  datetime = unpack_datetime (packet[8..]) }

    let get_door_response (packet: byte array) : Result<GetDoorResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_DOOR then
            Error("invalid get-door response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  door = unpack_u8 (packet[8..])
                  mode = unpack_u8 (packet[9..])
                  delay = unpack_u8 (packet[10..]) }

    let set_door_response (packet: byte array) : Result<SetDoorResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_DOOR then
            Error("invalid set-door response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  door = unpack_u8 (packet[8..])
                  mode = unpack_u8 (packet[9..])
                  delay = unpack_u8 (packet[10..]) }

    let set_door_passcodes_response (packet: byte array) : Result<SetDoorPasscodesResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_DOOR_PASSCODES then
            Error("invalid set-door-passcodes response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let openDoorResponse (packet: byte array) : Result<OpenDoorResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.OPEN_DOOR then
            Error("invalid open-door response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let getStatusResponse (packet: byte array) : Result<GetStatusResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_STATUS then
            Error("invalid get-status response")
        else
            let sysdate = unpack_yymmdd (packet[51..])
            let systime = unpack_HHmmss (packet[37..])

            let sysdatetime =
                match sysdate.HasValue, systime.HasValue with
                | true, true -> Nullable(sysdate.Value.ToDateTime(systime.Value))
                | _, _ -> Nullable()

            Ok
                { controller = unpack_u32 packet[4..]
                  door1Open = unpack_bool packet[28..]
                  door2Open = unpack_bool packet[29..]
                  door3Open = unpack_bool packet[30..]
                  door4Open = unpack_bool packet[31..]
                  door1Button = unpack_bool packet[32..]
                  door2Button = unpack_bool packet[33..]
                  door3Button = unpack_bool packet[34..]
                  door4Button = unpack_bool packet[35..]
                  systemError = unpack_u8 packet[36..]
                  systemDateTime = sysdatetime
                  sequenceNumber = unpack_u32 packet[40..]
                  specialInfo = unpack_u8 packet[48..]
                  relays = unpack_u8 packet[49..]
                  inputs = unpack_u8 packet[50..]
                  evt =
                    {| index = unpack_u32 packet[8..]
                       event = unpack_u8 packet[12..]
                       granted = unpack_bool packet[13..]
                       door = unpack_u8 packet[14..]
                       direction = unpack_u8 packet[15..]
                       card = unpack_u32 packet[16..]
                       timestamp = unpack_datetime (packet[20..])
                       reason = unpack_u8 packet[27..] |} }

    let getCardsResponse (packet: byte array) : Result<GetCardsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CARDS then
            Error("invalid get-cards response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  cards = unpack_u32 packet[8..] }

    let getCardResponse (packet: byte array) : Result<GetCardResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CARD then
            Error("invalid get-card response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  card = unpack_u32 packet[8..]
                  start_date = unpack_date (packet[12..])
                  end_date = unpack_date (packet[16..])
                  door1 = unpack_u8 (packet[20..])
                  door2 = unpack_u8 (packet[21..])
                  door3 = unpack_u8 (packet[22..])
                  door4 = unpack_u8 (packet[23..])
                  PIN = unpack_u32 (packet[24..]) }

    let getCardAtIndexResponse (packet: byte array) : Result<GetCardAtIndexResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CARD_AT_INDEX then
            Error("invalid get-card-at-index response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  card = unpack_u32 packet[8..]
                  start_date = unpack_date (packet[12..])
                  end_date = unpack_date (packet[16..])
                  door1 = unpack_u8 (packet[20..])
                  door2 = unpack_u8 (packet[21..])
                  door3 = unpack_u8 (packet[22..])
                  door4 = unpack_u8 (packet[23..])
                  PIN = unpack_u32 (packet[24..]) }

    let putCardResponse (packet: byte array) : Result<PutCardResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.PUT_CARD then
            Error("invalid put-card response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let deleteCardResponse (packet: byte array) : Result<DeleteCardResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.DELETE_CARD then
            Error("invalid delete-card response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let deleteAllCardsResponse (packet: byte array) : Result<DeleteAllCardsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.DELETE_ALL_CARDS then
            Error("invalid delete-all-cards response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let getEventResponse (packet: byte array) : Result<GetEventResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_EVENT then
            Error("invalid get-event response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  index = unpack_u32 packet[8..]
                  event = unpack_u8 (packet[12..])
                  granted = unpack_bool (packet[13..])
                  door = unpack_u8 (packet[14..])
                  direction = unpack_u8 (packet[15..])
                  card = unpack_u32 (packet[16..])
                  timestamp = unpack_datetime (packet[20..])
                  reason = unpack_u8 (packet[27..]) }

    let getEventIndexResponse (packet: byte array) : Result<GetEventIndexResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_EVENT_INDEX then
            Error("invalid get-event-index response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  index = unpack_u32 packet[8..] }

    let setEventIndexResponse (packet: byte array) : Result<SetEventIndexResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_EVENT_INDEX then
            Error("invalid set-event-index response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool packet[8..] }

    let recordSpecialEventsResponse (packet: byte array) : Result<RecordSpecialEventsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.RECORD_SPECIAL_EVENTS then
            Error("invalid record-special-events response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool packet[8..] }

    let getTimeProfileResponse (packet: byte array) : Result<GetTimeProfileResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_TIME_PROFILE then
            Error("invalid get-time-profile response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  profile = unpack_u8 packet[8..]
                  start_date = unpack_date (packet[9..])
                  end_date = unpack_date (packet[13..])
                  monday = unpack_bool (packet[17..])
                  tuesday = unpack_bool (packet[18..])
                  wednesday = unpack_bool (packet[19..])
                  thursday = unpack_bool (packet[20..])
                  friday = unpack_bool (packet[21..])
                  saturday = unpack_bool (packet[22..])
                  sunday = unpack_bool (packet[23..])
                  segment1_start = unpack_HHmm (packet[24..])
                  segment1_end = unpack_HHmm (packet[26..])
                  segment2_start = unpack_HHmm (packet[28..])
                  segment2_end = unpack_HHmm (packet[30..])
                  segment3_start = unpack_HHmm (packet[32..])
                  segment3_end = unpack_HHmm (packet[34..])
                  linked_profile = unpack_u8 packet[36..] }

    let setTimeProfileResponse (packet: byte array) : Result<SetTimeProfileResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_TIME_PROFILE then
            Error("invalid set-time-profile response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool packet[8..] }

    let clearTimeProfilesResponse (packet: byte array) : Result<ClearTimeProfilesResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.CLEAR_TIME_PROFILES then
            Error("invalid clear-time-profiles response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let addTaskResponse (packet: byte array) : Result<AddTaskResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.ADD_TASK then
            Error("invalid add-task response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let clearTaskListResponse (packet: byte array) : Result<ClearTaskListResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.CLEAR_TASKLIST then
            Error("invalid clear-tasklist response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let refreshTaskListResponse (packet: byte array) : Result<RefreshTaskListResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.REFRESH_TASKLIST then
            Error("invalid refresh-tasklist response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let setPCControlResponse (packet: byte array) : Result<SetPCControlResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_PC_CONTROL then
            Error("invalid set-pc-control response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let setInterlockResponse (packet: byte array) : Result<SetInterlockResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_INTERLOCK then
            Error("invalid set-interlock response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let activateKeypadsResponse (packet: byte array) : Result<ActivateKeypadsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.ACTIVATE_KEYPADS then
            Error("invalid activate-keyapds response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let restoreDefaultParametersResponse (packet: byte array) : Result<RestoreDefaultParametersResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.RESTORE_DEFAULT_PARAMETERS then
            Error("invalid restore-default-parameters response")
        else
            Ok
                { controller = unpack_u32 packet[4..]
                  ok = unpack_bool (packet[8..]) }

    let listenEvent (packet: byte array) : Result<ListenEvent, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.LISTEN_EVENT then
            Error("invalid listen-event packet")
        else
            let sysdate = unpack_yymmdd (packet[51..])
            let systime = unpack_HHmmss (packet[37..])

            let sysdatetime =
                match sysdate.HasValue, systime.HasValue with
                | true, true -> Nullable(sysdate.Value.ToDateTime(systime.Value))
                | _, _ -> Nullable()

            Ok
                { controller = unpack_u32 packet[4..]
                  door1Open = unpack_bool packet[28..]
                  door2Open = unpack_bool packet[29..]
                  door3Open = unpack_bool packet[30..]
                  door4Open = unpack_bool packet[31..]
                  door1Button = unpack_bool packet[32..]
                  door2Button = unpack_bool packet[33..]
                  door3Button = unpack_bool packet[34..]
                  door4Button = unpack_bool packet[35..]
                  systemError = unpack_u8 packet[36..]
                  systemDateTime = sysdatetime
                  sequenceNumber = unpack_u32 packet[40..]
                  specialInfo = unpack_u8 packet[48..]
                  relays = unpack_u8 packet[49..]
                  inputs = unpack_u8 packet[50..]
                  event =
                    {| index = unpack_u32 packet[8..]
                       event = unpack_u8 packet[12..]
                       granted = unpack_bool packet[13..]
                       door = unpack_u8 packet[14..]
                       direction = unpack_u8 packet[15..]
                       card = unpack_u32 packet[16..]
                       timestamp = unpack_datetime (packet[20..])
                       reason = unpack_u8 packet[27..] |} }
