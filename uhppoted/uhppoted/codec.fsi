namespace uhppoted

module internal messages =
    [<Literal>]
    val SOM: byte = 23uy

    [<Literal>]
    val SOM_v6_62: byte = 25uy

    [<Literal>]
    val GET_STATUS: byte = 32uy

    [<Literal>]
    val SET_TIME: byte = 48uy

    [<Literal>]
    val GET_TIME: byte = 50uy

    [<Literal>]
    val OPEN_DOOR: byte = 64uy

    [<Literal>]
    val PUT_CARD: byte = 80uy

    [<Literal>]
    val DELETE_CARD: byte = 82uy

    [<Literal>]
    val DELETE_ALL_CARDS: byte = 84uy

    [<Literal>]
    val GET_CARDS: byte = 88uy

    [<Literal>]
    val GET_CARD: byte = 90uy

    [<Literal>]
    val GET_CARD_AT_INDEX: byte = 92uy

    [<Literal>]
    val SET_DOOR: byte = 128uy

    [<Literal>]
    val GET_DOOR: byte = 130uy

    [<Literal>]
    val SET_TIME_PROFILE: byte = 136uy

    [<Literal>]
    val CLEAR_TIME_PROFILES: byte = 138uy

    [<Literal>]
    val SET_DOOR_PASSCODES: byte = 140uy

    [<Literal>]
    val RECORD_SPECIAL_EVENTS: byte = 142uy

    [<Literal>]
    val SET_LISTENER: byte = 144uy

    [<Literal>]
    val GET_LISTENER: byte = 146uy

    [<Literal>]
    val GET_CONTROLLER: byte = 148uy

    [<Literal>]
    val SET_IPv4: byte = 150uy

    [<Literal>]
    val GET_TIME_PROFILE: byte = 152uy

    [<Literal>]
    val SET_PC_CONTROL: byte = 160uy

    [<Literal>]
    val SET_INTERLOCK: byte = 162uy

    [<Literal>]
    val ACTIVATE_KEYPADS: byte = 164uy

    [<Literal>]
    val CLEAR_TASKLIST: byte = 166uy

    [<Literal>]
    val ADD_TASK: byte = 168uy

    [<Literal>]
    val SET_FIRST_CARD: byte = 170uy

    [<Literal>]
    val REFRESH_TASKLIST: byte = 172uy

    [<Literal>]
    val GET_EVENT: byte = 176uy

    [<Literal>]
    val SET_EVENT_INDEX: byte = 178uy

    [<Literal>]
    val GET_EVENT_INDEX: byte = 180uy

    [<Literal>]
    val RESTORE_DEFAULT_PARAMETERS: byte = 200uy

    [<Literal>]
    val LISTEN_EVENT: byte = 32uy
