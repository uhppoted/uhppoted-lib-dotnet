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

          { test = "get-door"
            request = Requests.get_door
            responses = [ Responses.get_door ] }

          { test = "get-door-not-found"
            request = Requests.get_door_not_found
            responses = [ Responses.get_door_not_found ] }

          { test = "set-door"
            request = Requests.set_door
            responses = [ Responses.set_door ] }

          { test = "set-door-not-found"
            request = Requests.set_door_not_found
            responses = [ Responses.set_door_not_found ] }

          { test = "set-door-passcodes"
            request = Requests.set_door_passcodes
            responses = [ Responses.set_door_passcodes ] }

          { test = "open-door"
            request = Requests.open_door
            responses = [ Responses.open_door ] }

          { test = "get-status"
            request = Requests.get_status
            responses = [ Responses.get_status ] }

          { test = "get-cards"
            request = Requests.get_cards
            responses = [ Responses.get_cards ] }

          { test = "get-card"
            request = Requests.get_card
            responses = [ Responses.get_card ] }

          { test = "get-card-not-found"
            request = Requests.get_card_not_found
            responses = [ Responses.get_card_not_found ] }

          { test = "get-card-at-index"
            request = Requests.get_card_at_index
            responses = [ Responses.get_card_at_index ] }

          { test = "get-card-at-index-not-found"
            request = Requests.get_card_at_index_not_found
            responses = [ Responses.get_card_at_index_not_found ] }

          { test = "get-card-at-index-deleted"
            request = Requests.get_card_at_index_deleted
            responses = [ Responses.get_card_at_index_deleted ] }

          { test = "put-card"
            request = Requests.put_card
            responses = [ Responses.put_card ] }

          { test = "delete-card"
            request = Requests.delete_card
            responses = [ Responses.delete_card ] }

          { test = "delete-all-cards"
            request = Requests.delete_all_cards
            responses = [ Responses.delete_all_cards ] }

          { test = "get-event"
            request = Requests.get_event
            responses = [ Responses.get_event ] }

          { test = "get-event-not-found"
            request = Requests.get_event_not_found
            responses = [ Responses.get_event_not_found ] }

          { test = "get-event-overwritten"
            request = Requests.get_event_overwritten
            responses = [ Responses.get_event_overwritten ] }

          { test = "get-event-index"
            request = Requests.get_event_index
            responses = [ Responses.get_event_index ] }

          { test = "set-event-index"
            request = Requests.set_event_index
            responses = [ Responses.set_event_index ] }

          { test = "record-special-events"
            request = Requests.record_special_events
            responses = [ Responses.record_special_events ] }

          { test = "get-time-profile"
            request = Requests.get_time_profile
            responses = [ Responses.get_time_profile ] }

          { test = "get-time-profile-not-found"
            request = Requests.get_time_profile_not_found
            responses = [ Responses.get_time_profile_not_found ] }

          { test = "set-time-profile"
            request = Requests.set_time_profile
            responses = [ Responses.set_time_profile ] }

          { test = "clear-time-profiles"
            request = Requests.clear_time_profiles
            responses = [ Responses.clear_time_profiles ] }

          { test = "add-task"
            request = Requests.add_task
            responses = [ Responses.add_task ] }

          { test = "clear-tasklist"
            request = Requests.clearTaskList
            responses = [ Responses.clearTaskList ] } ]


    let find request =
        match messages |> Seq.tryFind (fun (msg) -> msg.request.SequenceEqual(request)) with
        | Some(msg) -> Some(msg.responses)
        | _ -> None
