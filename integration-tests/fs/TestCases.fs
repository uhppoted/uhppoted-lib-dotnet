namespace FSharp.Tests

open uhppoted

type TestCase =
    | IdCase of uint32
    | ControllerCase of C

module TestCases =

    let controllers =
        Map.ofList
            [ ("uint32", IdCase 405419896u)
              ("controller",
               ControllerCase
                   { controller = 405419896u
                     endpoint = None
                     protocol = None }) ]

    let xxx =
        [| IdCase 405419896u
           ControllerCase
               { controller = 405419896u
                 endpoint = None
                 protocol = None } |]
