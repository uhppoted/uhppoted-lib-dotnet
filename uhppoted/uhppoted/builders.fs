namespace uhppoted

open System

type TimeProfileBuilder(profile: uint8) =
    let mutable start_date: Nullable<DateOnly> = Nullable()
    let mutable end_date: Nullable<DateOnly> = Nullable()
    let mutable monday: bool = false
    let mutable tuesday: bool = false
    let mutable wednesday: bool = false
    let mutable thursday: bool = false
    let mutable friday: bool = false
    let mutable saturday: bool = false
    let mutable sunday: bool = false
    let mutable segment1_start: Nullable<TimeOnly> = Nullable()
    let mutable segment1_end: Nullable<TimeOnly> = Nullable()
    let mutable segment2_start: Nullable<TimeOnly> = Nullable()
    let mutable segment2_end: Nullable<TimeOnly> = Nullable()
    let mutable segment3_start: Nullable<TimeOnly> = Nullable()
    let mutable segment3_end: Nullable<TimeOnly> = Nullable()
    let mutable linked: uint8 = 0uy

    member this.WithStartDate(v: DateOnly) =
        start_date <- Nullable(v)
        this

    member this.WithEndDate(v: DateOnly) =
        end_date <- Nullable(v)
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
        segment1_start <- Nullable(start_time)
        segment1_end <- Nullable(end_time)
        this

    member this.WithSegment2(start_time: TimeOnly, end_time: TimeOnly) =
        segment2_start <- Nullable(start_time)
        segment2_end <- Nullable(end_time)
        this

    member this.WithSegment3(start_time: TimeOnly, end_time: TimeOnly) =
        segment3_start <- Nullable(start_time)
        segment3_end <- Nullable(end_time)
        this

    member this.WithLinkedProfile(v: uint8) =
        linked <- v
        this

    member this.build() =
        { profile = profile
          start_date = start_date
          end_date = end_date
          monday = monday
          tuesday = tuesday
          wednesday = wednesday
          thursday = thursday
          friday = friday
          saturday = saturday
          sunday = sunday
          segment1_start = segment1_start
          segment1_end = segment1_end
          segment2_start = segment2_start
          segment2_end = segment2_end
          segment3_start = segment3_start
          segment3_end = segment3_end
          linked_profile = linked }
