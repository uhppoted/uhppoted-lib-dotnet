module Commands

open uhppoted

let get_controllers () =
    let controllers = Uhppoted.get_all_controllers ()

    printf "get-all-controllers: %d\n" controllers.Length

    controllers
    |> List.iter (fun controller ->
        printf "  controller %u\n" controller.controller
        printf "    address  %A\n" controller.address
        printf "    netmask  %A\n" controller.netmask
        printf "    gateway  %A\n" controller.gateway
        printf "    MAC      %A\n" controller.MAC
        printf "    version  %s\n" controller.version
        printf "    date     %A\n" controller.date
        printf "\n")

let get_controller () =
    match Uhppoted.get_controller 405419896u with
    | Ok response ->
        printf "get-controller\n"
        printf "  controller %u\n" response.controller
        printf "    address  %A\n" response.address
        printf "    netmask  %A\n" response.netmask
        printf "    gateway  %A\n" response.gateway
        printf "    MAC      %A\n" response.MAC
        printf "    version  %s\n" response.version
        printf "    date     %A\n" response.date
        printf "\n"

    | Error err ->
        printf "  ** ERROR %A\n" err

