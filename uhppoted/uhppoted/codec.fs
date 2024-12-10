namespace uhppoted

module internal messages =
    [<Literal>]
    let SOM = 0x17uy

    [<Literal>]
    let GET_STATUS = 0x20uy

    [<Literal>]
    let SET_TIME = 0x30uy

    [<Literal>]
    let GET_TIME = 0x32uy

    [<Literal>]
    let OPEN_DOOR = 0x40uy

    [<Literal>]
    let PUT_CARD = 0x50uy

    [<Literal>]
    let DELETE_CARD = 0x52uy

    [<Literal>]
    let DELETE_ALL_CARDS = 0x54uy

    [<Literal>]
    let GET_CARDS = 0x58uy

    [<Literal>]
    let GET_CARD = 0x5auy

    [<Literal>]
    let GET_CARD_AT_INDEX = 0x5cuy

    [<Literal>]
    let SET_DOOR = 0x80uy

    [<Literal>]
    let GET_DOOR = 0x82uy

    [<Literal>]
    let SET_TIME_PROFILE = 0x88uy

    [<Literal>]
    let CLEAR_TIME_PROFILES = 0x8auy

    [<Literal>]
    let SET_DOOR_PASSCODES = 0x8cuy

    [<Literal>]
    let RECORD_SPECIAL_EVENTS = 0x8euy

    [<Literal>]
    let SET_LISTENER = 0x90uy

    [<Literal>]
    let GET_LISTENER = 0x92uy

    [<Literal>]
    let GET_CONTROLLER = 0x94uy

    [<Literal>]
    let SET_IPv4 = 0x96uy

    [<Literal>]
    let GET_TIME_PROFILE = 0x98uy

    [<Literal>]
    let SET_PC_CONTROL = 0xa0uy

    [<Literal>]
    let SET_INTERLOCK = 0xa2uy

    [<Literal>]
    let ACTIVATE_KEYPADS = 0xa4uy

    [<Literal>]
    let CLEAR_TASKLIST = 0xa6uy

    [<Literal>]
    let ADD_TASK = 0xa8uy

    [<Literal>]
    let SET_FIRST_CARD = 0xaauy

    [<Literal>]
    let REFRESH_TASKLIST = 0xacuy

    [<Literal>]
    let GET_EVENT = 0xb0uy

    [<Literal>]
    let SET_EVENT_INDEX = 0xb2uy

    [<Literal>]
    let GET_EVENT_INDEX = 0xb4uy

    [<Literal>]
    let RESTORE_DEFAULT_PARAMETERS = 0xc8uy

    [<Literal>]
    let LISTEN_EVENT = 0x20uy
