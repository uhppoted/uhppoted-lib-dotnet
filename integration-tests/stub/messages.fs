namespace stub

open System.Collections.Generic
open System.Linq

type Message =
    { test: string
      request: byte array
      responses: byte array list }

module Messages =
    let messages =
        [ { test = "get-all-controllers"
            request = Requests.get_all_controllers
            responses =
              [ Responses.get_controller_405419896
                Responses.get_controller_303986753
                Responses.get_controller_201020304 ] }

          { test = "get-controller"
            request = Requests.get_controller
            responses = [ Responses.get_controller ] }

          { test = "set-IPv4"
            request = Requests.set_IPv4
            responses = [] }

          { test = "get-listener"
            request = Requests.get_listener
            responses = [ Responses.get_listener ] }

          { test = "set-listener"
            request = Requests.set_listener
            responses = [ Responses.set_listener ] }

          { test = "get-time"
            request = Requests.get_time
            responses = [ Responses.get_time ] }

          { test = "set-time"
            request = Requests.set_time
            responses = [ Responses.set_time ] }

          { test = "get-door-settings"
            request = Requests.get_door_settings
            responses = [ Responses.get_door_settings ] } ]

    let find request =
        match messages |> Seq.tryFind (fun (msg) -> msg.request.SequenceEqual(request)) with
        | Some(msg) -> Some(msg.responses)
        | _ -> None
