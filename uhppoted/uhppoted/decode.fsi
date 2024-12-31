namespace uhppoted

module internal Decode =
    val unpackU8: slice: byte array -> byte
    val unpackU16: slice: byte array -> uint16
    val unpackU32: slice: byte array -> uint32
    val unpackBool: slice: byte array -> bool
    val unpackIPv4: slice: byte array -> System.Net.IPAddress
    val unpackVersion: slice: byte array -> string
    val unpackMAC: slice: byte array -> System.Net.NetworkInformation.PhysicalAddress
    val unpackDate: slice: byte array -> System.Nullable<System.DateOnly>
    val unpackDateTime: slice: byte array -> System.Nullable<System.DateTime>
    val unpackYYMMDD: slice: byte array -> System.Nullable<System.DateOnly>
    val unpackHHmm: slice: byte array -> System.Nullable<System.TimeOnly>
    val unpackHHmmss: slice: byte array -> System.Nullable<System.TimeOnly>

    val getControllerResponse: packet: byte array -> Result<GetControllerResponse, string>
    val getListenerResponse: packet: byte array -> Result<GetListenerResponse, string>
    val setListenerResponse: packet: byte array -> Result<SetListenerResponse, string>
    val getTimeResponse: packet: byte array -> Result<GetTimeResponse, string>
    val setTimeResponse: packet: byte array -> Result<SetTimeResponse, string>
    val getDoorResponse: packet: byte array -> Result<GetDoorResponse, string>
    val setDoorResponse: packet: byte array -> Result<SetDoorResponse, string>
    val setDoorPasscodesResponse: packet: byte array -> Result<SetDoorPasscodesResponse, string>
    val openDoorResponse: packet: byte array -> Result<OpenDoorResponse, string>
    val getStatusResponse: packet: byte array -> Result<GetStatusResponse, string>
    val getCardsResponse: packet: byte array -> Result<GetCardsResponse, string>
    val getCardResponse: packet: byte array -> Result<GetCardResponse, string>
    val getCardAtIndexResponse: packet: byte array -> Result<GetCardAtIndexResponse, string>
    val putCardResponse: packet: byte array -> Result<PutCardResponse, string>
    val deleteCardResponse: packet: byte array -> Result<DeleteCardResponse, string>
    val deleteAllCardsResponse: packet: byte array -> Result<DeleteAllCardsResponse, string>
    val getEventResponse: packet: byte array -> Result<GetEventResponse, string>
    val getEventIndexResponse: packet: byte array -> Result<GetEventIndexResponse, string>
    val setEventIndexResponse: packet: byte array -> Result<SetEventIndexResponse, string>
    val recordSpecialEventsResponse: packet: byte array -> Result<RecordSpecialEventsResponse, string>
    val getTimeProfileResponse: packet: byte array -> Result<GetTimeProfileResponse, string>
    val setTimeProfileResponse: packet: byte array -> Result<SetTimeProfileResponse, string>
    val clearTimeProfilesResponse: packet: byte array -> Result<ClearTimeProfilesResponse, string>
    val addTaskResponse: packet: byte array -> Result<AddTaskResponse, string>
    val clearTaskListResponse: packet: byte array -> Result<ClearTaskListResponse, string>
    val refreshTaskListResponse: packet: byte array -> Result<RefreshTaskListResponse, string>
    val setPCControlResponse: packet: byte array -> Result<SetPCControlResponse, string>
    val setInterlockResponse: packet: byte array -> Result<SetInterlockResponse, string>
    val activateKeypadsResponse: packet: byte array -> Result<ActivateKeypadsResponse, string>
    val restoreDefaultParametersResponse: packet: byte array -> Result<RestoreDefaultParametersResponse, string>
    val listenEvent: packet: byte array -> Result<ListenEvent, string>
