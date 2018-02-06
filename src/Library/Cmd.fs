module Library.Cmd

open Elmish
open Fable.Import
open Fable.PowerPack


let ofPromiseResult
    (task: _ -> JS.Promise<Result<_,_>>)
    arg
    (ofSuccess: _ -> 'msg)
    (ofError: _ -> 'msg) : Cmd<'msg> =

    let bind dispatch =
        task arg
        |> Promise.result
        |> Promise.iter
            (Result.bind id
                >> function Error m -> ofError m | Ok v -> ofSuccess v
                >> dispatch
            )

    [ bind ]
