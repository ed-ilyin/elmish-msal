module App.State

open Elmish
open Elmish.Browser
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Fable.Core.JsInterop
open Global
open Library
open Types


let pageParser: Parser<Page->Page,Page> =
    oneOf [
        map Home UrlParser.top
        map Home (s "home")
    ]


let getUser () : string option =
    let user: obj option = !!msalSusi?getUser()
    // do printfn "Got user: %A" user
    match user with
        | None -> None
        | Some u -> !!u?name


let urlUpdate (result: Option<Page>) model =
    do printfn "URL update: %A" result

    match result with
        | None ->
            model, toHash model.currentPage |> Navigation.modifyUrl
            // model, Cmd.none

        | Some page ->
            { model with currentPage = page }, Cmd.none
                // , Cmd.ofFunc getUser () UpdateUser Failure


let log t x =
    do printfn "%s: %A" t x
    x


let init result =
    do printfn "App Init"

    let (navbar, navbarCmd) = Navbar.State.init()
    let (home, homeCmd) = Home.State.init()
    let (account, accountCmd) = Account.State.init()

    let (model, cmd) =
        urlUpdate result
            {   currentPage = Home
                navbar = navbar
                home = home
                user = None
                account = account
                log = "none"
            }

    model
        , Cmd.batch
            [   cmd
                Cmd.map NavbarMsg navbarCmd
                Cmd.map HomeMsg homeCmd
                Cmd.map AccountMsg accountCmd
                // Cmd.ofFunc getUser () UpdateUser Failure
                Cmd.ofSub <| fun dispatch ->
                    createNew Msal?UserAgentApplication
                        ( b2cClientId
                        , b2cAuthority "signup_signin"
                        , fun _ ->
                            !!msalSusi?getUser()
                                |> log "user "
                                |> UpdateUser
                                |> dispatch
                        )
                        |> ignore
            ]


let update msg model =
    do printfn "App update: %A" msg

    match msg with
    | AccountMsg msg ->
        let (account, cmd) = Account.State.update msg model.account
        { model with account = account }, Cmd.map AccountMsg cmd

    | NoOp -> model, Cmd.none

    | Failure x ->
        // do printfn "Main failure: %s" x.Message
        { model with log = x.Message }, Cmd.none
        // model, Cmd.none

    | NavbarMsg msg ->
        let (navbar, cmd) = Navbar.State.update msg model.navbar
        { model with navbar = navbar }, Cmd.map NavbarMsg cmd

    | HomeMsg Home.Types.GoToAccountPage ->
        { model with currentPage = Account }, Cmd.none

    | HomeMsg msg ->
        let (home, cmd) = Home.State.update msg model.home
        { model with home = home }, Cmd.map HomeMsg cmd

    | UpdateUser user ->
        { model with
            user = user
            currentPage =
                match user with None -> Home | Some _ -> Account
        }, [
            Navbar.State.updateUser user |> Cmd.map NavbarMsg |> Some
            FSharp.Core.Option.map
                (fun _ ->
                    Cmd.ofMsg Account.Types.GetTokenSilent
                        |> Cmd.map AccountMsg
                )
                user
        ] |> List.choose id |> Cmd.batch
