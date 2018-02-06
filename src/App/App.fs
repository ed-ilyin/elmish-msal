module App.View

open Fable.Core
open Elmish
open Elmish.React
open Fable.Import.Browser
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop


type Msal =

type Model = {
    msal: Msal option
    user: User option
}

type Msg =
    | Failure of System.Exception
    | SignIn
    | UpdateUser of string option
    | SignOut


let [<Import("*","msal")>] Msal: Msal = jsNative
do printfn "global location: %A" window.location
let b2cClientId = "0041318d-9770-47b5-be03-f9d7f94d2793"
let b2cLogin = "https://login.microsoftonline.com/"
let tenant = "automaticinkelkjop"

let b2cAuthority policyName =
    sprintf "%stfp/%s.onmicrosoft.com/B2C_1A_%s"
        b2cLogin
        tenant
        policyName

let msalApp p =
    createNew Msal?UserAgentApplication (b2cClientId, b2cAuthority p)

let getAndUpdateUser susi dispatch =
    match !!susi?getUser() with
        | None -> ()
        | Some user -> UpdateUser !!user?name |> dispatch

let init () =
    { msal = None; user = None }
        , Cmd.ofSub <| fun dispatch ->
            let rec susi =
                createNew Msal?UserAgentApplication (
                    b2cClientId,
                    b2cAuthority "signup_signin",
                    fun _ ->
                        do printfn "token callback"
                        // let susi = msalApp "signup_signin"
                        getAndUpdateUser susi dispatch
                )
            getAndUpdateUser susi dispatch


let b2cScopes =
    [| sprintf "https://%s.onmicrosoft.com/api/read" tenant |]

let update msg model =
    do printfn "update: %A" msg
    match msg with
        | Failure x ->
            do printfn "failure: %s" x.Message
            model, Cmd.none

        | SignIn ->
            model
                , Cmd.ofSub <| fun dispatch ->
                    let susi =
                        createNew Msal?UserAgentApplication (
                            b2cClientId,
                            b2cAuthority "signup_signin",
                            fun _ ->
                                do printfn "token callback"
                                let susi = msalApp "signup_signin"
                                getUser susi dispatch
                        )
                    do !!susi?loginRedirect(b2cScopes, "brand=elkjop")

        | UpdateUser user ->
            match user with None -> "none" | Some u -> u
            , Cmd.none

        | SignOut -> model, Cmd.attemptFunc ()

let root model dispatch =
    div [] [
        div [] [ str model ]
        button [ OnClick (fun _ -> dispatch SignIn ) ] [ str "sign in" ]
        button [ OnClick (fun _ -> dispatch SignOut ) ] [ str "sign out" ]
    ]

Program.mkProgram init update root
|> Program.withReact "app"
|> Program.run
