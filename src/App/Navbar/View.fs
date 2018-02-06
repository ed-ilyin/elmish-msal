module Navbar.View

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Library
open Types


let root (model: Types.Model) dispatch =
    header [] [
        div [ ClassName "container" ] [
            div [ Id "topbar" ] [
                (match model.user with
                    | None ->
                        a [ OnClick (fun _ -> dispatch SignInOrSignup)
                            Disabled model.signInOrSignupAnchorDisabled
                            Href "#signin"
                        ] [
                            img [ Src "assets//img/login.svg" ]
                            str "Sign in"
                        ]
                    | Some user ->
                        span [] [
                            a [] [
                                img [ Src "assets//img/login.svg" ]
                                str user
                            ]
                            a [ Href "#signout"
                                OnClick (fun _ -> !!Global.msalSusi?logout())
                            ] [ str "Sign out" ]
                            a [ Global.b2c "ProfileEdit" |> Href ]
                                [ str "Profile" ]
                        ]
                )
                a [ Href "#english" ] [
                    img [ Src "assets//img/land.svg" ]
                    str "English"
                ]
            ]
            div [
                Id "logo"
                ClassName "eventListener navigate"
                !!("data-navigate-to-page", "home")
            ] [ View.logo Global.brandId ]
            div [ Id "menu"; ClassName "en" ] [
                ul [] [
                    li [ ClassName "eventListener navigate home active"; !!("data-navigate-to-page", "home") ] [
                        a [ Href "#how" ] [
                            img [ Src "assets//img/cogwheel.svg" ]
                            str "How it works"
                        ]
                    ]
                    li [ ClassName "eventListener navigate home active"; !!("data-navigate-to-page", "home") ] [
                        a [ Href "#why" ] [
                            img [ Src "assets//img/inkdrop.svg" ]
                            str "Why Automatic Ink"
                        ]
                    ]
                    li [ ClassName "eventListener navigate home active"; !!("data-navigate-to-page", "home") ] [
                        a [ Href "#register" ] [
                            img [ Src "assets//img/register.svg"; ClassName "register_menu" ]
                            str "Register"
                        ]
                    ]
                ]
            ]
        ]
    ]
