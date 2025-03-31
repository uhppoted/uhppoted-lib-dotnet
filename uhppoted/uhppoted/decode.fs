namespace uhppoted

open System
open System.Globalization
open System.Net
open System.Net.NetworkInformation
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module internal Decode =
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

    let unpackVersion (slice: byte array) = $"v%x{slice[0]}.%02x{slice[1]}"

    let unpackMAC (slice: byte array) = PhysicalAddress(slice[0..5])

    let unpackDate (slice: byte array) =
        let bcd = $"%02x{slice[0]}%02x{slice[1]}-%02x{slice[2]}-%02x{slice[3]}"

        match DateOnly.TryParseExact(bcd, "yyyy-MM-dd") with
        | true, date -> Nullable date
        | false, _ -> Nullable()

    let unpackDateTime (slice: byte array) : Nullable<DateTime> =
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

    let unpackYYMMDD (slice: byte array) =
        let bcd = $"20%02x{slice[0]}-%02x{slice[1]}-%02x{slice[2]}"

        match DateOnly.TryParseExact(bcd, "yyyy-MM-dd") with
        | true, date -> Nullable date
        | false, _ -> Nullable()

    let unpackHHmm (slice: byte array) =
        let bcd = $"%02x{slice[0]}:%02x{slice[1]}"

        match TimeOnly.TryParseExact(bcd, "HH:mm") with
        | true, time -> Nullable time
        | false, _ -> Nullable()

    let unpackHHmmss (slice: byte array) =
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
                { controller = unpackU32 packet[4..]
                  address = unpackIPv4 packet[8..]
                  netmask = unpackIPv4 packet[12..]
                  gateway = unpackIPv4 packet[16..]
                  MAC = unpackMAC packet[20..25]
                  version = unpackVersion packet[26..]
                  date = unpackDate (packet[28..]) }

    let getListenerResponse (packet: byte array) : Result<GetListenerResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_LISTENER then
            Error("invalid get-listener response")
        else
            let controller = unpackU32 packet[4..]
            let address = unpackIPv4 packet[8..]
            let port = unpackU16 packet[12..]
            let interval = unpackU8 packet[14..]

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
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let getTimeResponse (packet: byte array) : Result<GetTimeResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_TIME then
            Error("invalid get-time response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  datetime = unpackDateTime (packet[8..]) }

    let setTimeResponse (packet: byte array) : Result<SetTimeResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_TIME then
            Error("invalid set-time response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  datetime = unpackDateTime (packet[8..]) }

    let getDoorResponse (packet: byte array) : Result<GetDoorResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_DOOR then
            Error("invalid get-door response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  door = unpackU8 (packet[8..])
                  mode = unpackU8 (packet[9..])
                  delay = unpackU8 (packet[10..]) }

    let setDoorResponse (packet: byte array) : Result<SetDoorResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_DOOR then
            Error("invalid set-door response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  door = unpackU8 (packet[8..])
                  mode = unpackU8 (packet[9..])
                  delay = unpackU8 (packet[10..]) }

    let setDoorPasscodesResponse (packet: byte array) : Result<SetDoorPasscodesResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_DOOR_PASSCODES then
            Error("invalid set-door-passcodes response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let openDoorResponse (packet: byte array) : Result<OpenDoorResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.OPEN_DOOR then
            Error("invalid open-door response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let getStatusResponse (packet: byte array) : Result<GetStatusResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_STATUS then
            Error("invalid get-status response")
        else
            let sysdate = unpackYYMMDD (packet[51..])
            let systime = unpackHHmmss (packet[37..])

            let sysdatetime =
                match sysdate.HasValue, systime.HasValue with
                | true, true -> Nullable(sysdate.Value.ToDateTime(systime.Value))
                | _, _ -> Nullable()

            Ok
                { controller = unpackU32 packet[4..]
                  door1Open = unpackBool packet[28..]
                  door2Open = unpackBool packet[29..]
                  door3Open = unpackBool packet[30..]
                  door4Open = unpackBool packet[31..]
                  door1Button = unpackBool packet[32..]
                  door2Button = unpackBool packet[33..]
                  door3Button = unpackBool packet[34..]
                  door4Button = unpackBool packet[35..]
                  systemError = unpackU8 packet[36..]
                  systemDateTime = sysdatetime
                  sequenceNumber = unpackU32 packet[40..]
                  specialInfo = unpackU8 packet[48..]
                  relays = unpackU8 packet[49..]
                  inputs = unpackU8 packet[50..]
                  evt =
                    {| index = unpackU32 packet[8..]
                       event = unpackU8 packet[12..]
                       granted = unpackBool packet[13..]
                       door = unpackU8 packet[14..]
                       direction = unpackU8 packet[15..]
                       card = unpackU32 packet[16..]
                       timestamp = unpackDateTime (packet[20..])
                       reason = unpackU8 packet[27..] |} }

    let getCardsResponse (packet: byte array) : Result<GetCardsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CARDS then
            Error("invalid get-cards response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  cards = unpackU32 packet[8..] }

    let getCardResponse (packet: byte array) : Result<GetCardResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CARD then
            Error("invalid get-card response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  card = unpackU32 packet[8..]
                  startDate = unpackDate (packet[12..])
                  endDate = unpackDate (packet[16..])
                  door1 = unpackU8 (packet[20..])
                  door2 = unpackU8 (packet[21..])
                  door3 = unpackU8 (packet[22..])
                  door4 = unpackU8 (packet[23..])
                  PIN = unpackU32 (packet[24..]) }

    let getCardAtIndexResponse (packet: byte array) : Result<GetCardAtIndexResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_CARD_AT_INDEX then
            Error("invalid get-card-at-index response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  card = unpackU32 packet[8..]
                  startDate = unpackDate (packet[12..])
                  endDate = unpackDate (packet[16..])
                  door1 = unpackU8 (packet[20..])
                  door2 = unpackU8 (packet[21..])
                  door3 = unpackU8 (packet[22..])
                  door4 = unpackU8 (packet[23..])
                  PIN = unpackU32 (packet[24..]) }

    let putCardResponse (packet: byte array) : Result<PutCardResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.PUT_CARD then
            Error("invalid put-card response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let deleteCardResponse (packet: byte array) : Result<DeleteCardResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.DELETE_CARD then
            Error("invalid delete-card response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let deleteAllCardsResponse (packet: byte array) : Result<DeleteAllCardsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.DELETE_ALL_CARDS then
            Error("invalid delete-all-cards response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let getEventResponse (packet: byte array) : Result<GetEventResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_EVENT then
            Error("invalid get-event response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  index = unpackU32 packet[8..]
                  event = unpackU8 (packet[12..])
                  granted = unpackBool (packet[13..])
                  door = unpackU8 (packet[14..])
                  direction = unpackU8 (packet[15..])
                  card = unpackU32 (packet[16..])
                  timestamp = unpackDateTime (packet[20..])
                  reason = unpackU8 (packet[27..]) }

    let getEventIndexResponse (packet: byte array) : Result<GetEventIndexResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_EVENT_INDEX then
            Error("invalid get-event-index response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  index = unpackU32 packet[8..] }

    let setEventIndexResponse (packet: byte array) : Result<SetEventIndexResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_EVENT_INDEX then
            Error("invalid set-event-index response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let recordSpecialEventsResponse (packet: byte array) : Result<RecordSpecialEventsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.RECORD_SPECIAL_EVENTS then
            Error("invalid record-special-events response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let getTimeProfileResponse (packet: byte array) : Result<GetTimeProfileResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_TIME_PROFILE then
            Error("invalid get-time-profile response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  profile = unpackU8 packet[8..]
                  startDate = unpackDate (packet[9..])
                  endDate = unpackDate (packet[13..])
                  monday = unpackBool (packet[17..])
                  tuesday = unpackBool (packet[18..])
                  wednesday = unpackBool (packet[19..])
                  thursday = unpackBool (packet[20..])
                  friday = unpackBool (packet[21..])
                  saturday = unpackBool (packet[22..])
                  sunday = unpackBool (packet[23..])
                  segment1Start = unpackHHmm (packet[24..])
                  segment1End = unpackHHmm (packet[26..])
                  segment2Start = unpackHHmm (packet[28..])
                  segment2End = unpackHHmm (packet[30..])
                  segment3Start = unpackHHmm (packet[32..])
                  segment3End = unpackHHmm (packet[34..])
                  linkedProfile = unpackU8 packet[36..] }

    let setTimeProfileResponse (packet: byte array) : Result<SetTimeProfileResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_TIME_PROFILE then
            Error("invalid set-time-profile response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let clearTimeProfilesResponse (packet: byte array) : Result<ClearTimeProfilesResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.CLEAR_TIME_PROFILES then
            Error("invalid clear-time-profiles response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let addTaskResponse (packet: byte array) : Result<AddTaskResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.ADD_TASK then
            Error("invalid add-task response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let clearTaskListResponse (packet: byte array) : Result<ClearTaskListResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.CLEAR_TASKLIST then
            Error("invalid clear-tasklist response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let refreshTaskListResponse (packet: byte array) : Result<RefreshTaskListResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.REFRESH_TASKLIST then
            Error("invalid refresh-tasklist response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let setPCControlResponse (packet: byte array) : Result<SetPCControlResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_PC_CONTROL then
            Error("invalid set-pc-control response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let setInterlockResponse (packet: byte array) : Result<SetInterlockResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_INTERLOCK then
            Error("invalid set-interlock response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool (packet[8..]) }

    let activateKeypadsResponse (packet: byte array) : Result<ActivateKeypadsResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.ACTIVATE_KEYPADS then
            Error("invalid activate-keyapds response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let getAntiPassbackResponse (packet: byte array) : Result<GetAntiPassbackResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.GET_ANTIPASSBACK then
            Error("invalid get-antipassback response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  antipassback = unpackU8 packet[8..] }

    let setAntiPassbackResponse (packet: byte array) : Result<SetAntiPassbackResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.SET_ANTIPASSBACK then
            Error("invalid set-antipassback response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let restoreDefaultParametersResponse (packet: byte array) : Result<RestoreDefaultParametersResponse, string> =
        if packet[0] <> messages.SOM then
            Error("invalid controller response")
        else if packet[1] <> messages.RESTORE_DEFAULT_PARAMETERS then
            Error("invalid restore-default-parameters response")
        else
            Ok
                { controller = unpackU32 packet[4..]
                  ok = unpackBool packet[8..] }

    let listenEvent (packet: byte array) : Result<ListenEvent, string> =
        if packet[0] <> messages.SOM && packet[0] <> messages.SOM_v6_62 then
            Error("invalid controller response")
        else if packet[1] <> messages.LISTEN_EVENT then
            Error("invalid listen-event packet")
        else
            let sysdate = unpackYYMMDD (packet[51..])
            let systime = unpackHHmmss (packet[37..])

            let sysdatetime =
                match sysdate.HasValue, systime.HasValue with
                | true, true -> Nullable(sysdate.Value.ToDateTime(systime.Value))
                | _, _ -> Nullable()

            Ok
                { controller = unpackU32 packet[4..]
                  door1Open = unpackBool packet[28..]
                  door2Open = unpackBool packet[29..]
                  door3Open = unpackBool packet[30..]
                  door4Open = unpackBool packet[31..]
                  door1Button = unpackBool packet[32..]
                  door2Button = unpackBool packet[33..]
                  door3Button = unpackBool packet[34..]
                  door4Button = unpackBool packet[35..]
                  systemError = unpackU8 packet[36..]
                  systemDateTime = sysdatetime
                  sequenceNumber = unpackU32 packet[40..]
                  specialInfo = unpackU8 packet[48..]
                  relays = unpackU8 packet[49..]
                  inputs = unpackU8 packet[50..]
                  event =
                    {| index = unpackU32 packet[8..]
                       event = unpackU8 packet[12..]
                       granted = unpackBool packet[13..]
                       door = unpackU8 packet[14..]
                       direction = unpackU8 packet[15..]
                       card = unpackU32 packet[16..]
                       timestamp = unpackDateTime (packet[20..])
                       reason = unpackU8 packet[27..] |} }
