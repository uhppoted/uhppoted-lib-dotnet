module argparse

open System
open System.Net
open System.Collections.Generic
open System.Text.RegularExpressions

open uhppoted

let (|UInt32|_|) (value: string) =
    match UInt32.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|Byte|_|) (value: string) =
    match Byte.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|Boolean|_|) (value: string) =
    match Boolean.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|IPAddress|_|) (value: string) =
    match IPAddress.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|IPEndPoint|_|) (value: string) =
    match IPEndPoint.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|DateTime|_|) (value: string) =
    match DateTime.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|DateOnly|_|) (value: string) =
    match DateOnly.TryParse value with
    | true, v -> Some v
    | _ -> None

let (|DoorMode|_|) (value: string) =
    match value.ToLowerInvariant() with
    | "normally-open" -> Some DoorMode.NormallyOpen
    | "normally-closed" -> Some DoorMode.NormallyClosed
    | "controlled" -> Some DoorMode.Controlled
    | _ -> None

let (|Interlock|_|) (value: string) =
    match value.ToLowerInvariant() with
    | "none" -> Some Interlock.None
    | "1&2" -> Some Interlock.Doors12
    | "3&4" -> Some Interlock.Doors34
    | "1&2,3&4" -> Some Interlock.Doors12And34
    | "1&2&3" -> Some Interlock.Doors123
    | "1&2&3&4" -> Some Interlock.Doors1234
    | _ -> None

let (|AntiPassback|_|) (value: string) =
    match value.ToLowerInvariant() with
    | "disabled" -> Some AntiPassback.Disabled
    | "(1:2);(3:4)" -> Some AntiPassback.Doors12_34
    | "(1,3):(2,4)" -> Some AntiPassback.Doors13_24
    | "1:(2,3)" -> Some AntiPassback.Doors1_23
    | "1:(2,3,4)" -> Some AntiPassback.Doors1_234
    | _ -> None

let (|TaskCode|_|) (value: string) =
    match value.ToLowerInvariant() with
    | "control door" -> Some TaskCode.ControlDoor
    | "unlock door" -> Some TaskCode.UnlockDoor
    | "lock door" -> Some TaskCode.LockDoor
    | "disable time profiles" -> Some TaskCode.DisableTimeProfiles
    | "enable time profiles" -> Some TaskCode.EnableTimeProfiles
    | "enable card + no PIN" -> Some TaskCode.EnableCardNoPIN
    | "enable card + IN PIN" -> Some TaskCode.EnableCardInPIN
    | "enable card + IN/OUT PIN" -> Some TaskCode.EnableCardInOutPIN
    | "enable more cards" -> Some TaskCode.EnableMoreCards
    | "disable more cards" -> Some TaskCode.DisableMoreCards
    | "trigger once" -> Some TaskCode.TriggerOnce
    | "disable pushbutton" -> Some TaskCode.DisablePushbutton
    | "enable pushbutton" -> Some TaskCode.EnablePushbutton
    | _ -> None

let (|Uint32List|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Array.choose (fun v ->
                match UInt32.TryParse(v.Trim()) with
                | true, parsed -> Some parsed
                | _ -> None)

        Some parsed
    with _ ->
        None

let (|Uint8List|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Array.choose (fun v ->
                match Byte.TryParse(v.Trim()) with
                | true, parsed -> Some parsed
                | _ -> None)
            |> Array.toList

        Some parsed
    with _ ->
        None

let (|Uint8Array|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Array.choose (fun v ->
                match Byte.TryParse(v.Trim()) with
                | true, parsed -> Some parsed
                | _ -> None)

        Some parsed
    with _ ->
        None

let (|Permissions|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Seq.choose (fun v ->
                let parts = v.Split(':')

                match parts with
                | [| key; value |] ->
                    match Int32.TryParse(key.Trim()), Byte.TryParse(value.Trim()) with
                    | (true, k), (true, v) -> Some(k, v)
                    | _ -> None
                | [| key |] ->
                    match Int32.TryParse(key.Trim()) with
                    | true, k -> Some(k, 1uy)
                    | _ -> None
                | _ -> None)
            |> Map.ofSeq

        Some parsed
    with _ ->
        None

let (|Weekdays|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Seq.choose (fun v ->
                match v with
                | "Mon" -> Some("monday")
                | "Tue" -> Some("tuesday")
                | "Wed" -> Some("wednesday")
                | "Thu" -> Some("thursday")
                | "Fri" -> Some("friday")
                | "Sat" -> Some("saturday")
                | "Sun" -> Some("sunday")
                | _ -> None)
            |> Set.ofSeq
            |> Set.toList

        Some parsed
    with _ ->
        None

let (|Segments|_|) (value: string) =
    try
        let parsed =
            value.Split(',')
            |> Seq.choose (fun v ->
                let parts = v.Split('-')

                match parts with
                | [| startTime; endTime |] ->
                    match TimeOnly.TryParse(startTime.Trim()), TimeOnly.TryParse(endTime.Trim()) with
                    | (true, p), (true, q) -> Some([ p; q ])
                    | _ -> None
                | _ -> None)
            |> Seq.toList
            |> List.concat
            |> List.toArray

        Some parsed
    with _ ->
        None

let (|Card|_|) (value: string) =
    try
        let tokens = value.ToLower().Split(',', 4) |> Seq.toList

        match tokens with
        | t1 :: t2 :: t3 :: rest ->
            let card =
                match UInt32.TryParse t1 with
                | (true, v) -> v
                | _ -> 0u

            let startDate =
                match DateOnly.TryParse t2 with
                | true, v -> Nullable(v)
                | _ -> Nullable()

            let endDate =
                match DateOnly.TryParse t3 with
                | true, v -> Nullable(v)
                | _ -> Nullable()

            let pattern = @"\[(.*)\](?:,([^,]*)(?:,([ynYN])))?"
            let m = Regex.Match(rest[0], pattern)
            let permissions = m.Groups.[1].Value.Split(',')

            let door d =
                match permissions |> Array.tryFind (fun v -> v = string d) with
                | Some _ -> 1uy
                | None ->
                    match
                        permissions
                        |> Array.tryPick (fun v ->
                            let m = Regex.Match(v, $@"^{d}:(\d+)$")

                            match m.Success, Byte.TryParse m.Groups.[1].Value with
                            | true, (true, v) -> Some(v)
                            | _ -> None)
                    with
                    | Some value -> value
                    | None -> 0uy

            let PIN =
                match UInt32.TryParse m.Groups[2].Value with
                | (true, v) -> v
                | _ -> 0u

            let firstcard =
                match m.Groups[3].Value with
                | "Y"
                | "y" -> true
                | _ -> false

            Some
                { Card = card
                  StartDate = startDate
                  EndDate = endDate
                  Door1 = (door 1)
                  Door2 = (door 2)
                  Door3 = (door 3)
                  Door4 = (door 4)
                  PIN = PIN
                  FirstCard = firstcard }

        | _ -> None

    with _ ->
        None

let (|FirstCard|_|) (value: string) =
    try
        let tokens = value.ToLower().Split(',', 5) |> Seq.toList

        match tokens with
        | t1 :: t2 :: t3 :: t4 :: t5 :: _ ->
            let startTime =
                match TimeOnly.TryParse(t1) with
                | (true, p) -> Nullable(p)
                | _ -> Nullable()

            let endTime =
                match TimeOnly.TryParse(t2) with
                | (true, p) -> Nullable(p)
                | _ -> Nullable()

            let activeMode =
                match t3 with
                | "controlled" -> DoorMode.Controlled
                | "normally open" -> DoorMode.NormallyOpen
                | "normally closed" -> DoorMode.NormallyClosed
                | _ -> DoorMode.Controlled

            let inactiveMode =
                match t4 with
                | "controlled" -> DoorMode.Controlled
                | "normally open" -> DoorMode.NormallyOpen
                | "normally closed" -> DoorMode.NormallyClosed
                | "firstcard only" -> DoorMode.FirstCardOnly
                | _ -> DoorMode.Controlled

            let weekdays = t5.Trim([| '['; ']' |]).Split([| ',' |]) |> Set.ofSeq

            let weekday (day: string) =
                Set.exists (fun (e: string) -> e.StartsWith(day)) weekdays

            let firstcard =
                { StartTime = startTime
                  EndTime = endTime
                  ActiveMode = activeMode
                  InactiveMode = inactiveMode
                  Monday = weekday "mon"
                  Tuesday = weekday "tue"
                  Wednesday = weekday "wed"
                  Thursday = weekday "thu"
                  Friday = weekday "fri"
                  Saturday = weekday "sat"
                  Sunday = weekday "sun" }

            Some firstcard
        | _ -> None

    with _ ->
        None

let rec argparse (args: string list) flag (defval: 'T) : 'T =
    match args with
    | arg :: value :: rest when arg = flag ->
        match box defval, value with
        | :? uint32, UInt32 parsed -> unbox parsed
        | :? byte, Byte parsed -> unbox parsed
        | :? bool, Boolean parsed -> unbox parsed
        | :? IPAddress, IPAddress parsed -> unbox parsed
        | :? IPEndPoint, IPEndPoint parsed -> unbox parsed
        | :? DateTime, DateTime parsed -> unbox parsed
        | :? DateOnly, DateOnly parsed -> unbox parsed
        | :? DoorMode, DoorMode parsed -> unbox parsed
        | :? Interlock, Interlock parsed -> unbox parsed
        | :? AntiPassback, AntiPassback parsed -> unbox parsed
        | :? TaskCode, TaskCode parsed -> unbox parsed
        | :? Card, Card parsed -> unbox parsed
        | :? FirstCard, FirstCard parsed -> unbox parsed

        | _ when typeof<'T> = typeof<UInt32[]> ->
            match value with
            | Uint32List parsed -> unbox parsed
            | _ -> defval

        | _ when typeof<'T> = typeof<byte list> ->
            match value with
            | Uint8List parsed -> unbox parsed
            | _ -> defval

        | _ when typeof<'T> = typeof<Byte[]> ->
            match value with
            | Uint8Array parsed -> unbox parsed
            | _ -> defval

        | _ when typeof<'T> = typeof<Map<Int32, Byte>> ->
            match value with
            | Permissions parsed -> unbox parsed
            | _ -> defval

        | _ when typeof<'T> = typeof<string list> ->
            match value with
            | Weekdays parsed -> unbox parsed
            | _ -> defval

        | _ when typeof<'T> = typeof<TimeOnly array> ->
            match value with
            | Segments parsed -> unbox parsed
            | _ -> defval

        | _ -> defval

    | _ :: rest -> argparse rest flag defval

    | [] -> defval
