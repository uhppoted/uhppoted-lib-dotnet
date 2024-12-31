namespace uhppoted

module internal internationalisation =
    val translate: e: string -> string
    val TranslateEventType: event: uint8 -> string
    val TranslateEventReason: reason: uint8 -> string
    val TranslateDoorDirection: direction: uint8 -> string
    val TranslateDoorMode: mode: uint8 -> string
    val TranslateDoorInterlock: interlock: uint8 -> string
    val TranslateTaskCode: task: uint8 -> string
    val TranslateRelayState: relay: uint8 -> string
    val TranslateInputState: input: uint8 -> string
