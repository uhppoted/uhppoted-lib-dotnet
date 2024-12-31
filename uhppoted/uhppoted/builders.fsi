namespace uhppoted

/// Convenience 'C' struct builder implementation for C# and VB.NET.
type CBuilder =
    new: controller: uint32 -> CBuilder
    member Build: unit -> C
    member WithEndPoint: e: System.Net.IPEndPoint -> CBuilder
    member WithProtocol: p: string -> CBuilder

/// Convenience Card struct builder implementation for C# and VB.NET.
type CardBuilder =
    new: card: uint32 -> CardBuilder
    member Build: unit -> Card
    member WithDoor1: profile: uint8 -> CardBuilder
    member WithDoor2: profile: uint8 -> CardBuilder
    member WithDoor3: profile: uint8 -> CardBuilder
    member WithDoor4: profile: uint8 -> CardBuilder
    member WithEndDate: date: System.DateOnly -> CardBuilder
    member WithPIN: pin: uint32 -> CardBuilder
    member WithStartDate: date: System.DateOnly -> CardBuilder

type TimeProfileBuilder =
    new: profile: uint8 -> TimeProfileBuilder
    member Build: unit -> TimeProfile
    member WithEndDate: v: System.DateOnly -> TimeProfileBuilder
    member WithLinkedProfile: profile: uint8 -> TimeProfileBuilder
    member WithSegment1: start_time: System.TimeOnly * end_time: System.TimeOnly -> TimeProfileBuilder
    member WithSegment2: start_time: System.TimeOnly * end_time: System.TimeOnly -> TimeProfileBuilder
    member WithSegment3: start_time: System.TimeOnly * end_time: System.TimeOnly -> TimeProfileBuilder
    member WithStartDate: v: System.DateOnly -> TimeProfileBuilder

    member WithWeekdays:
        mon: bool * tue: bool * wed: bool * thurs: bool * fri: bool * sat: bool * sun: bool -> TimeProfileBuilder

type TaskBuilder =
    new: task: TaskCode * door: uint8 -> TaskBuilder
    member Build: unit -> Task
    member WithEndDate: date: System.DateOnly -> TaskBuilder
    member WithMoreCards: cards: uint8 -> TaskBuilder
    member WithStartDate: date: System.DateOnly -> TaskBuilder
    member WithStartTime: time: System.TimeOnly -> TaskBuilder

    member WithWeekdays:
        mon: bool * tue: bool * wed: bool * thurs: bool * fri: bool * sat: bool * sun: bool -> TaskBuilder
