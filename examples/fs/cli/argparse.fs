module argparse
    open System
    open System.Net

    let (|UInt32|_|) (value: string) =
        match UInt32.TryParse value with
        | true, v -> Some(v)
        | _ -> None

    let (|IPAddress|_|) (value: string) =
        match IPAddress.TryParse value with
        | true, v -> Some(v)
        | _ -> None

    let rec argparse (args: string list) flag (defval: 'T): 'T = 
        match args with
        | arg :: value :: rest when arg = flag ->
            match box defval,value with  
            | :? uint32, UInt32 parsed -> 
                unbox parsed

            | :? IPAddress, IPAddress parsed -> 
                unbox parsed

            | _ ->  
                defval

        | _ :: rest -> 
            argparse rest flag defval

        | [] -> 
            defval
