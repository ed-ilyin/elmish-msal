module App.View

open Fable.Core
open Elmish
open Elmish.React
open Fable.Import.Browser
open Fable.Import.Msal
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Elmish.Browser.Navigation

module U = Elmish.Browser.UrlParser

type Page = Home | Account

type Model = {
    error: string option
    user: Msal.User option
    page: Page
}

type Msg =
    | Failure of System.Exception
    | SignIn
    | UpdateUser of Msal.User
    | SignOut


do printfn "window location: %A" window.location
let b2cClientId = "0041318d-9770-47b5-be03-f9d7f94d2793"
let b2cLogin = "https://login.microsoftonline.com/"
let tenant = "automaticinkelkjop"

let b2cAuthority policyName =
    sprintf "%stfp/%s.onmicrosoft.com/B2C_1A_%s"
        b2cLogin
        tenant
        policyName

let msalApp policy ofError ofOk =
    Msal.userAgentApplication.Create(
        b2cClientId,
        b2cAuthority policy |> Some,
        fun errorDesc token error tokenType ->
            match token with
                | null ->
                    sprintf "%s: %s" error errorDesc |> exn |> ofError
                | _ -> ofOk token
    )

let getUser ofOk (susi: Msal.UserAgentApplication) =
    match susi.getUser() with null -> () | user -> ofOk user

let toHash = function Home -> "#home" | Account -> "#account"

let urlUpdate (result: Option<Page>) model =
    do printfn "URL update: %A" result
    match result with
        | None -> model, toHash model.page |> Navigation.modifyUrl
        | Some page -> { model with page = page }, Cmd.none

let init result =
    let (model, cmd) =
        urlUpdate result { error = None; user = None; page = Home }

    model
        , Cmd.ofSub <| fun dispatch ->
            msalApp "signup_signin" (Failure >> dispatch)
                <| fun token ->
                    do printfn "token callback: %s" token
                    msalApp "signup_signin" (Failure >> dispatch)
                        ignore
                        |> getUser (UpdateUser >> dispatch)
                |> getUser (UpdateUser >> dispatch)

let b2cScopes =
    [| sprintf "https://%s.onmicrosoft.com/api/read" tenant |]

let update msg model =
    do printfn "update: %A" msg
    match msg with
        | Failure x -> { model with error = Some x.Message }, Cmd.none

        | SignIn ->
            model
                , Cmd.ofSub <| fun dispatch ->
                    let susi =
                        msalApp "signup_signin" (Failure >> dispatch)
                            ignore
                    do susi.loginRedirect(!!b2cScopes, "brand=elkjop")

        | UpdateUser user -> { model with user = Some user }, Cmd.none

        | SignOut ->
            model
                , Cmd.ofSub <| fun dispatch ->
                    let susi =
                        msalApp "signup_signin" (Failure >> dispatch)
                            ignore
                    do susi.logout ()

let root (model: Model) dispatch =
    div [] [
        div [] [
            match model.error with None -> "all ok" | Some e -> e
                |> str
        ]
        div [] [
            button [ OnClick (fun _ -> dispatch SignIn ) ]
                [ str "sign in" ]
        ]
        div [] [
            match model.user with None -> "None" | Some u -> u.name
                |> str
        ]
        div [] [
            button [ OnClick (fun _ -> dispatch SignOut ) ]
                [ str "sign out" ]
        ]
    ]

let pageParser: U.Parser<Page->Page,Page> =
    U.oneOf [
        U.map Home U.top
        U.map Home (U.s "home")
    ]

Program.mkProgram init update root
|> Program.toNavigable (U.parseHash pageParser) urlUpdate
|> Program.withReact "app"
|> Program.run
