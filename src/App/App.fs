module App.View

open App.State
open Elmish
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Elmish.Debug
open Elmish.HMR
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Global
open Types
open Fable.Import.Browser


let footer =
    div [ Id "footer"; ClassName "en" ] [
        table [ FrameBorder "0"; CellSpacing "0"; CellPadding "0" ] [
            tbody [] [
                tr [] [
                    td [ ClassName "cookie"; Id "cookieMessage" ] []
                ]
            ]
        ]
        div [ ClassName "container" ] [
            div [ ClassName "col" ] [
                h4 [] [
                    sprintf
                        "About %s Automatic Ink - Powered by Epson ReadyInk"
                        brandName
                        |> str
                ]
                p [] [
                    sprintf
                        "%s Automatic Ink - Powered by Epson ReadyInk is a pay-as-you-go, hassle-free automated print service to make sure your printer never runs out of ink."
                        brandName
                        |> str
                ]
            ]
            div [ ClassName "col" ] []
        ]
    ]


let root model dispatch =
    let pageHtml =
        function
            | Page.Home ->
                HomeMsg >> dispatch |> Home.View.root model.home

            | Page.Account ->
                AccountMsg
                    >> dispatch
                    |> Account.View.root model.account

    div [ Id "wrapper"; sprintf "en %s>" brandName |> ClassName ]
        [   str model.log
            NavbarMsg >> dispatch |> Navbar.View.root model.navbar
            pageHtml model.currentPage
            footer
        ]

// App
let hashParams =
    window.location.hash.Substring(1).Split('&')
        |> Seq.map toKeyValuePair
        |> Seq.choose id
        |> Map.ofSeq

// do printfn "hash params: %A" hashParams

// match Map.tryFind "id_token" hashParams with
//     | None ->
Program.mkProgram init update root
|> Program.toNavigable (parseHash pageParser) urlUpdate
#if DEBUG
|> Program.withDebugger
|> Program.withHMR
#endif
|> Program.withReact "app"
|> Program.run

    // | Some _ -> ()
