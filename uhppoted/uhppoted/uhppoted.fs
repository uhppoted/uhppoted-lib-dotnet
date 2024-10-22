namespace uhppoted

module Uhppoted =
    let get_controllers () : GetControllerResponse list =
        printfn "uhppoted::get-controllers"
        let addr = "192.168.1.255"
        let port = 60000
        let timeout = 1000
        let debug = true

        let request = Encode.get_controller_request 0
        let replies = UDP.broadcast (request, addr, port, timeout, debug)

        replies |> List.map (fun v -> Decode.get_controller_response v)
