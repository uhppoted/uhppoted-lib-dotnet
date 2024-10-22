module Commands

open uhppoted

let get_controllers () =
    let controllers = Uhppoted.get_controllers ()

    printf "get-controllers: %d\n" controllers.Length

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
