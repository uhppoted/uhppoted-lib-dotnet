namespace uhppoted

module internal Encode =
    val bcd: v: string -> byte array
    val packU32: packet: byte array -> offset: int -> v: uint32 -> unit
    val packU16: packet: byte array -> offset: int -> v: uint16 -> unit
    val packU8: packet: byte array -> offset: int -> v: uint8 -> unit
    val packBool: packet: byte array -> offset: int -> v: bool -> unit
    val packIPv4: packet: byte array -> offset: int -> v: System.Net.IPAddress -> unit
    val packDateTime: packet: byte array -> offset: int -> v: System.DateTime -> unit
    val packDateOnly: packet: byte array -> offset: int -> v: System.Nullable<System.DateOnly> -> unit
    val packTimeOnly: packet: byte array -> offset: int -> v: System.Nullable<System.TimeOnly> -> unit
    val (|Uint32|_|): v: obj -> uint32 option
    val (|Uint16|_|): v: obj -> uint16 option
    val (|Uint8|_|): v: obj -> uint8 option
    val (|Bool|_|): v: obj -> bool option
    val (|IPv4|_|): v: obj -> System.Net.IPAddress option
    val (|DateTime|_|): v: obj -> System.DateTime option
    val (|DateOnly|_|): v: obj -> System.Nullable<System.DateOnly> option
    val (|TimeOnly|_|): v: obj -> System.Nullable<System.TimeOnly> option
    val pack: packet: byte array -> offset: int -> v: 'T -> unit

    val getControllerRequest: controller: uint32 -> byte array
    val setIPv4Request: controller: uint32 -> address: 'a -> netmask: 'b -> gateway: 'c -> byte array
    val getListenerRequest: controller: uint32 -> byte array

    val setListenerRequest:
        controller: uint32 -> address: System.Net.IPAddress -> port: uint16 -> interval: uint8 -> byte array

    val getTimeRequest: controller: uint32 -> byte array
    val setTimeRequest: controller: uint32 -> datetime: System.DateTime -> byte array
    val getDoorRequest: controller: uint32 -> door: uint8 -> byte array
    val setDoorRequest: controller: uint32 -> door: uint8 -> mode: DoorMode -> delay: uint8 -> byte array

    val setDoorPasscodesRequest:
        controller: uint32 ->
        door: uint8 ->
        passcode1: uint32 ->
        passcode2: uint32 ->
        passcode3: uint32 ->
        passcode4: uint32 ->
            byte array

    val openDoorRequest: controller: uint32 -> door: uint8 -> byte array
    val getStatusRequest: controller: uint32 -> byte array
    val getCardsRequest: controller: uint32 -> byte array
    val getCardRequest: controller: uint32 -> card: uint32 -> byte array
    val getCardAtIndexRequest: controller: uint32 -> index: uint32 -> byte array
    val putCardRequest: controller: uint32 -> card: Card -> byte array
    val deleteCardRequest: controller: uint32 -> card: uint32 -> byte array
    val deleteAllCardsRequest: controller: uint32 -> byte array
    val getEventRequest: controller: uint32 -> index: uint32 -> byte array
    val getEventIndexRequest: controller: uint32 -> byte array
    val setEventIndexRequest: controller: uint32 -> index: uint32 -> byte array
    val recordSpecialEventsRequest: controller: uint32 -> enabled: bool -> byte array
    val getTimeProfileRequest: controller: uint32 -> profile: uint8 -> byte array
    val setTimeProfileRequest: controller: uint32 -> profile: TimeProfile -> byte array
    val clearTimeProfilesRequest: controller: uint32 -> byte array
    val addTaskRequest: controller: uint32 -> task: Task -> byte array
    val clearTaskListRequest: controller: uint32 -> byte array
    val refreshTaskListRequest: controller: uint32 -> byte array
    val setPCControlRequest: controller: uint32 -> enable: bool -> byte array
    val setInterlockRequest: controller: uint32 -> interlock: Interlock -> byte array

    val activateKeypadsRequest:
        controller: uint32 -> reader1: bool -> reader2: bool -> reader3: bool -> reader4: bool -> byte array

    val restoreDefaultParametersRequest: controller: uint32 -> byte array
