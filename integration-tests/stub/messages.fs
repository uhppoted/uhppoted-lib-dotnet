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
            request = Requests.getAllControllers
            responses =
              [ Responses.getController405419896
                Responses.getController303986753
                Responses.getController201020304 ] }

          { test = "get-controller"
            request = Requests.getController
            responses = [ Responses.getController ] }

          { test = "set-IPv4"
            request = Requests.setIPv4
            responses = [] }

          { test = "get-listener"
            request = Requests.getListener
            responses = [ Responses.getListener ] }

          { test = "set-listener"
            request = Requests.setListener
            responses = [ Responses.setListener ] }

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
            request = Requests.getStatus
            responses = [ Responses.getStatus ] }

          { test = "get-status-no-event"
            request = Requests.getStatusNoEvent
            responses = [ Responses.getStatusNoEvent ] }

          { test = "get-cards"
            request = Requests.getCards
            responses = [ Responses.getCards ] }

          { test = "get-card"
            request = Requests.getCard
            responses = [ Responses.getCard ] }

          { test = "get-card-not-found"
            request = Requests.getCardNotFound
            responses = [ Responses.getCardNotFound ] }

          { test = "get-card-at-index"
            request = Requests.getCardAtIndex
            responses = [ Responses.getCardAtIndex ] }

          { test = "get-card-at-index-not-found"
            request = Requests.getCardAtIndexNotFound
            responses = [ Responses.getCardAtIndexNotFound ] }

          { test = "get-card-at-index-deleted"
            request = Requests.getCardAtIndexDeleted
            responses = [ Responses.getCardAtIndexDeleted ] }

          { test = "put-card"
            request = Requests.putCard
            responses = [ Responses.putCard ] }

          { test = "delete-card"
            request = Requests.deleteCard
            responses = [ Responses.deleteCard ] }

          { test = "delete-all-cards"
            request = Requests.deleteAllCards
            responses = [ Responses.deleteAllCards ] }

          { test = "get-event"
            request = Requests.getEvent
            responses = [ Responses.getEvent ] }

          { test = "get-event-not-found"
            request = Requests.getEventNotFound
            responses = [ Responses.getEventNotFound ] }

          { test = "getEventOverwritten"
            request = Requests.getEventOverwritten
            responses = [ Responses.getEventOverwritten ] }

          { test = "get-event-index"
            request = Requests.getEventIndex
            responses = [ Responses.getEventIndex ] }

          { test = "set-event-index"
            request = Requests.setEventIndex
            responses = [ Responses.setEventIndex ] }

          { test = "record-special-events"
            request = Requests.recordSpecialEvents
            responses = [ Responses.recordSpecialEvents ] }

          { test = "get-time-profile"
            request = Requests.getTimeProfile
            responses = [ Responses.getTimeProfile ] }

          { test = "get-time-profile-not-found"
            request = Requests.getTimeProfileNotFound
            responses = [ Responses.getTimeProfileNotFound ] }

          { test = "set-time-profile"
            request = Requests.setTimeProfile
            responses = [ Responses.setTimeProfile ] }

          { test = "clear-time-profiles"
            request = Requests.clearTimeProfiles
            responses = [ Responses.clearTimeProfiles ] }

          { test = "add-task"
            request = Requests.addTask
            responses = [ Responses.addTask ] }

          { test = "clear-tasklist"
            request = Requests.clearTaskList
            responses = [ Responses.clearTaskList ] }

          { test = "refresh-tasklist"
            request = Requests.refreshTaskList
            responses = [ Responses.refreshTaskList ] }

          { test = "set-pc-control"
            request = Requests.setPCControl
            responses = [ Responses.setPCControl ] }

          { test = "set-interlock"
            request = Requests.setInterlock
            responses = [ Responses.setInterlock ] }

          { test = "activate-keypads"
            request = Requests.activateKeypads
            responses = [ Responses.activateKeypads ] }

          { test = "restore-default-parameters"
            request = Requests.restoreDefaultParameters
            responses = [ Responses.restoreDefaultParameters ] } ]


    let find request =
        match messages |> Seq.tryFind (fun (msg) -> msg.request.SequenceEqual(request)) with
        | Some(msg) -> Some(msg.responses)
        | _ -> None
