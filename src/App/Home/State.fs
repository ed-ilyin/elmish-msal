module Home.State

open Elmish
open Fable.Core.JsInterop
open Fable.Import
open Global
open Library
open Types


module JE = Fable.EdIlyin.Core.Json.Encode


let init () : Model * Cmd<Msg> =
    {   registerPrinterButtonDisabled = false
    }   , Cmd.none


let update msg (model: Model) =
    // do printfn "Home update: %A" msg

    match msg with
    | Failure err ->
        // do printfn "Home Failure: %s" err.Message
        // do Browser.console.error(err)
        model, Cmd.none

    | RegisterPrinter ->
        { model with registerPrinterButtonDisabled = true }
            , Cmd.attemptFunc b2cRedirect "SignUp" Failure

    | GoToAccountPage -> model, Cmd.none
