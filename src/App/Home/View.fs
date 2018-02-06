module Home.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Library
open Types
open Global

let registerButton model dispatch =
    button [
        // Global.b2c "SignUp" |> Href
        ClassName "button"
        Disabled model.registerPrinterButtonDisabled
        OnClick (fun _ -> dispatch RegisterPrinter)
    ] [ str "register your printer" ]


let root (model: Types.Model) dispatch =
    div [ Id "wrapper"; sprintf "en %s>" brandName |> ClassName ] [
        section [ ClassName "hero_block"; Id "hero" ] [
            div [ ClassName "container" ] [
                div [ ClassName "hero_blockContent" ] [
                    img [ Src "assets//img/automaticink.png" ]
                    p [] [
                        sprintf
                            "Forget monthly subscriptions – Choose %s's pay-as-you-go, hassle-free print service and get genuine Epson ink cartridges before your printer runs out."
                            Global.brandName
                            |> str
                        ]
                    a [ Href "#register"; ClassName "button" ]
                        [ str "register your printer" ]
                ]
            ]
        ]
        section [ ClassName "intro_block"; Id "how" ] [
            div [ ClassName "container" ] [
                div [ ClassName "intro_block_content" ] [
                    h1 [] [
                        sprintf "Discover how %s Automatic Ink works"
                            brandName
                            |> str
                        br []
                        span [ ClassName "subtitle" ] [
                            str "Powered by Epson ReadyInk"
                            ]
                    ]
                    p [] [
                        sprintf
                            "Thanks to %s Automatic Ink - Powered by
                                Epson ReadyInk, using our hassle-free and subscription-free service, you'll never run out of ink again! Simply check if the Epson printer you bought from us is participating in the program and complete below easy registration. There’s no monthly contracted payments, you only pay after you’ve received our offer for a new cartridge - only when a new cartridge is needed. With this automated system in place, you’ll always have a supply of high-quality ink."
                            Global.brandName
                            |> str
                        ]
                ]
                div [ ClassName "intro_block_video" ] [
                    div [ ClassName "videoWrapper" ] [
                        iframe [
                            HTMLAttr.Width "1280"
                            HTMLAttr.Height "720"
                            Src "https://www.youtube.com/embed/LpUTqH5g6Tg?enablejsapi=1&rel=0&showinfo=0?ecver=1"
                            FrameBorder "0"
                            AllowFullScreen true
                        ] []
                    ]
                ]
            ]
        ]
        section [ ClassName "why_block"; Id "why" ] [
            div [ ClassName "container" ] [
                div [ ClassName "why_block_content" ] [
                    h1 [] [
                        sprintf "Why %s Automatic Ink?" brandName |> str
                        br []
                        span [ ClassName "subtitle" ] [
                            str "Powered by Epson ReadyInk"
                            ]
                    ]
                    div [ ClassName "usps" ] [
                        div [ ClassName "col" ] [
                            div [ ClassName "image" ] [
                                img [ Src "assets//img/icon_ink.svg" ]
                            ]
                            h3 [] [
                                str "Never run out of ink"
                                ]
                            p [] [
                                sprintf
                                    "Don't get caught out; with %s Automatic Ink you can enjoy a continuous and automated supply of ink cartridges."
                                    brandName
                                    |> str
                                ]
                        ]
                        div [ ClassName "col" ] [
                            div [ ClassName "image" ] [
                                img [ Src "assets//img/icon_sale.svg" ]
                            ]
                            h3 [] [
                                str "Special promotions"
                                ]
                            p [] [
                                sprintf
                                    "We will inform you of all special promotions you can take advantage of while using the %s Automatic Ink scheme."
                                    brandName
                                    |> str
                                ]
                        ]
                        div [ ClassName "col" ] [
                            div [ ClassName "image" ] [
                                img [ Src "assets//img/icon_print.svg" ]
                            ]
                            h3 [] [
                                str "No monthly subscription"
                                ]
                            p [] [
                                str "Forget being tied to a monthly contract; you only pay for the ink you need. Without a contract, you can stop at any time."
                                ]
                        ]
                        div [ ClassName "col" ] [
                            div [ ClassName "image" ] [
                                img [ Src "assets//img/icon_drops.svg" ]
                            ]
                            h3 [] [
                                str "Genuine ink"
                                ]
                            p [] [
                                str "Our inks and printers are designed to work in harmony, so that they’ll give you consistent and reliable results without any fuss."
                                ]
                        ]
                        div [ ClassName "clear" ] [
                            ]
                    ]
                ]
            ]
        ]
        section [ ClassName "register"; Id "register" ] [
            div [ ClassName "container" ] [
                div [ ClassName "formHolder signupFormHolder registerContent" ] [
                    div [ ClassName "form"; Id "signupForm" ] [
                        div [ ClassName "formRow two" ] [
                            div [ ClassName "formCol" ] [
                                h2 [] [
                                    str "Register your printer"
                                    ]
                                p [] [
                                    sprintf
                                        "To join %s Automatic Ink, first check if you can register your printer."
                                        brandName
                                        |> str
                                    ]
                                a [ Href "https://www.epson.co.uk/for-home/hassle-free-printing-solutions/readyink#range"
                                    ClassName "textlink"
                                    Target "Epson"
                                ] [ str "Check applicable models" ]
                                registerButton model dispatch
                            ]
                            div [ ClassName "formCol image" ] [
                                div [ ClassName "register_circle" ] [
                                    div [ ClassName "register_circle_text" ] [
                                        str "Sign up now and never run out of ink!"
                                        ]
                                ]
                            ]
                            div [ ClassName "clear" ] [
                                ]
                        ]
                    ]
                ]
            ]
        ]
    ]
