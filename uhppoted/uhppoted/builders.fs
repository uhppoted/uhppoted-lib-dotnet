namespace uhppoted

open System
open System.Net

/// Convenience 'C' builder implementation for C# and VB.NET.
type ControllerBuilder(controller: uint32) =
    let mutable endpoint: Option<IPEndPoint> = None
    let mutable protocol: Option<string> = None

    /// Sets the optional controller endpoint.
    /// - Parameter `e`: IPv4 controller address:port.
    /// - Returns: The updated builder instance.
    member this.WithEndpoint(e: IPEndPoint) =
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
        { Controller = controller
          Endpoint = endpoint
          Protocol = protocol }

type CardBuilder(card: uint32) =
    let mutable startDate: Nullable<DateOnly> = Nullable()
    let mutable endDate: Nullable<DateOnly> = Nullable()
    let mutable door1: uint8 = 0uy
    let mutable door2: uint8 = 0uy
    let mutable door3: uint8 = 0uy
    let mutable door4: uint8 = 0uy
    let mutable PIN: uint32 = 0u

    member this.WithStartDate(v: DateOnly) =
        startDate <- Nullable(v)
        this

    member this.WithEndDate(v: DateOnly) =
        endDate <- Nullable(v)
        this

    member this.WithDoor1(profile: uint8) =
        door1 <- profile
        this

    member this.WithDoor2(profile: uint8) =
        door2 <- profile
        this

    member this.WithDoor3(profile: uint8) =
        door3 <- profile
        this

    member this.WithDoor4(profile: uint8) =
        door4 <- profile
        this

    member this.WithPIN(v: uint32) =
        PIN <- v
        this

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


type TaskBuilder(task: uint8, door: uint8) =
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
