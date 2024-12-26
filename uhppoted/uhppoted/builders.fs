namespace uhppoted

open System
open System.Net

/// Convenience 'C' struct builder implementation for C# and VB.NET.
type CBuilder(controller: uint32) =
    let mutable endpoint: Option<IPEndPoint> = None
    let mutable protocol: Option<string> = None

    /// Sets the optional controller endpoint.
    /// - Parameter `e`: IPv4 controller address:port.
    /// - Returns: The updated builder instance.
    member this.WithEndPoint(e: IPEndPoint) =
        endpoint <- Some(e)
        this

    /// Sets the optional connection protocol.
    /// - Parameter `p`: 'udp' or 'tcp'.
    /// - Returns: The updated builder instance.
    member this.WithProtocol(p: string) =
        protocol <- Some(p)
        this

    /// Builds a `C` struct.
    /// - Returns: A `C` struct initialised with the controller ID and optional endpoint and protocol.
    member this.Build() =
        { controller = controller
          endpoint = endpoint
          protocol = protocol }

/// Convenience Card struct builder implementation for C# and VB.NET.
type CardBuilder(card: uint32) =
    let mutable startDate: Nullable<DateOnly> = Nullable()
    let mutable endDate: Nullable<DateOnly> = Nullable()
    let mutable door1: uint8 = 0uy
    let mutable door2: uint8 = 0uy
    let mutable door3: uint8 = 0uy
    let mutable door4: uint8 = 0uy
    let mutable PIN: uint32 = 0u

    /// Sets the card date from which the card is valid.
    /// - Parameter `date`: Card 'start date'.
    /// - Returns: The updated builder instance.
    member this.WithStartDate(date: DateOnly) =
        startDate <- Nullable(date)
        this

    /// Sets the card date after which the card is no longer valid.
    /// - Parameter `date`: Card 'end date'.
    /// - Returns: The updated builder instance.
    member this.WithEndDate(date: DateOnly) =
        endDate <- Nullable(date)
        this

    /// Sets the card date time profile for door 1:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 1 (defaults to 0).
    /// - Returns: The updated builder instance.
    member this.WithDoor1(profile: uint8) =
        door1 <- profile
        this

    /// Sets the card date time profile for door 2:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 2 (defaults to 0).
    /// - Returns: The updated builder instance.
    member this.WithDoor2(profile: uint8) =
        door2 <- profile
        this

    /// Sets the card date time profile for door 3:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 3 (defaults to 0).
    /// - Returns: The updated builder instance.
    member this.WithDoor3(profile: uint8) =
        door3 <- profile
        this

    /// Sets the card date time profile for door 4:
    /// - 0 is 'no access'
    /// - 1 is 24/7 access
    /// - [2..254] are user defined time profiles
    /// - Parameter `profile`: Time profile to apply for door 4 (defaults to 0).
    /// - Returns: The updated builder instance.
    member this.WithDoor4(profile: uint8) =
        door4 <- profile
        this

    /// Sets the (optional) card PIN code for use with a card reader keypad (0 is 'none').
    /// - Parameter `pin`: PIN code [0..999999]  (defaults to 0).
    /// - Returns: The updated builder instance.
    member this.WithPIN(pin: uint32) =
        PIN <- pin
        this

    /// Builds a Card record.
    /// - Returns: A Card struct initialised with the card number, start and end dates, door
    //             permissions and PIN.
    member this.Build() =
        { Card = card
          StartDate = startDate
          EndDate = endDate
          Door1 = door1
          Door2 = door2
          Door3 = door3
          Door4 = door4
          PIN = PIN }

type TimeProfileBuilder(profile: uint8) =
    let mutable startDate: Nullable<DateOnly> = Nullable()
    let mutable endDate: Nullable<DateOnly> = Nullable()
    let mutable monday: bool = false
    let mutable tuesday: bool = false
    let mutable wednesday: bool = false
    let mutable thursday: bool = false
    let mutable friday: bool = false
    let mutable saturday: bool = false
    let mutable sunday: bool = false
    let mutable segment1Start: Nullable<TimeOnly> = Nullable()
    let mutable segment1End: Nullable<TimeOnly> = Nullable()
    let mutable segment2Start: Nullable<TimeOnly> = Nullable()
    let mutable segment2End: Nullable<TimeOnly> = Nullable()
    let mutable segment3Start: Nullable<TimeOnly> = Nullable()
    let mutable segment3End: Nullable<TimeOnly> = Nullable()
    let mutable linked: uint8 = 0uy

    member this.WithStartDate(v: DateOnly) =
        startDate <- Nullable(v)
        this

    member this.WithEndDate(v: DateOnly) =
        endDate <- Nullable(v)
        this

    member this.WithWeekdays(mon: bool, tue: bool, wed: bool, thurs: bool, fri: bool, sat: bool, sun: bool) =
        monday <- mon
        tuesday <- tue
        wednesday <- wed
        thursday <- thurs
        friday <- fri
        saturday <- sat
        sunday <- sun
        this

    member this.WithSegment1(start_time: TimeOnly, end_time: TimeOnly) =
        segment1Start <- Nullable(start_time)
        segment1End <- Nullable(end_time)
        this

    member this.WithSegment2(start_time: TimeOnly, end_time: TimeOnly) =
        segment2Start <- Nullable(start_time)
        segment2End <- Nullable(end_time)
        this

    member this.WithSegment3(start_time: TimeOnly, end_time: TimeOnly) =
        segment3Start <- Nullable(start_time)
        segment3End <- Nullable(end_time)
        this

    member this.WithLinkedProfile(profile: uint8) =
        linked <- profile
        this

    member this.Build() =
        { Profile = profile
          StartDate = startDate
          EndDate = endDate
          Monday = monday
          Tuesday = tuesday
          Wednesday = wednesday
          Thursday = thursday
          Friday = friday
          Saturday = saturday
          Sunday = sunday
          Segment1Start = segment1Start
          Segment1End = segment1End
          Segment2Start = segment2Start
          Segment2End = segment2End
          Segment3Start = segment3Start
          Segment3End = segment3End
          LinkedProfile = linked }


type TaskBuilder(task: TaskCode, door: uint8) =
    let mutable startDate: Nullable<DateOnly> = Nullable()
    let mutable endDate: Nullable<DateOnly> = Nullable()
    let mutable startTime: Nullable<TimeOnly> = Nullable()
    let mutable monday: bool = false
    let mutable tuesday: bool = false
    let mutable wednesday: bool = false
    let mutable thursday: bool = false
    let mutable friday: bool = false
    let mutable saturday: bool = false
    let mutable sunday: bool = false
    let mutable moreCards: uint8 = 0uy

    member this.WithStartDate(date: DateOnly) =
        startDate <- Nullable(date)
        this

    member this.WithEndDate(date: DateOnly) =
        endDate <- Nullable(date)
        this

    member this.WithStartTime(time: TimeOnly) =
        startTime <- Nullable(time)
        this

    member this.WithWeekdays(mon: bool, tue: bool, wed: bool, thurs: bool, fri: bool, sat: bool, sun: bool) =
        monday <- mon
        tuesday <- tue
        wednesday <- wed
        thursday <- thurs
        friday <- fri
        saturday <- sat
        sunday <- sun
        this

    member this.WithMoreCards(cards: uint8) =
        moreCards <- cards
        this

    member this.Build() =
        { Task = task
          Door = door
          StartDate = startDate
          EndDate = endDate
          StartTime = startTime
          Monday = monday
          Tuesday = tuesday
          Wednesday = wednesday
          Thursday = thursday
          Friday = friday
          Saturday = saturday
          Sunday = sunday
          MoreCards = moreCards }
