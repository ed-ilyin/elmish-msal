module Navbar.State

open Elmish
open Fable.Core.JsInterop
open Library
open Types


module JE = Fable.EdIlyin.Core.Json.Encode


let init () : Model * Cmd<Msg> =
    {   signInOrSignupAnchorDisabled = false
        user = None
    }   , Cmd.none


let update msg (model: Model) =
    // do printfn "Navbar: %A" msg

    match msg with
        | NoOp -> model, Cmd.none
        | Failure x ->
            // do printfn "Navbar: Failure: %s" x.Message
            model, Cmd.none

        | SignInOrSignup ->
            { model with signInOrSignupAnchorDisabled = true }
                , Cmd.ofFunc
                    (fun s ->
                        // do printfn "susi scopes: %A" s
                        let query = "brand=" + Global.brandId
                        do !!Global.msalSusi?loginRedirect(s, query)
                    )
                    Global.b2cScopes
                    <| fun _ -> NoOp
                    <| Failure

        | UpdateUser user -> { model with user = user }, Cmd.none


let updateUser user = UpdateUser user |> Cmd.ofMsg
