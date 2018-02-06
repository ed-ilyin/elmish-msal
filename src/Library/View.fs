module Library.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Types


let countrySelect id country msg dispatch =
    div [ ClassName "select" ] [
        [   "", "Country*"
            "UK", "United Kingdom"
            "NL", "Netherlands"
            "DE", "Germany"
        ]   |> List.map
                (fun (value, text) ->
                    option [ Value value ] [ str text ]
                )
                |> select [
                    Id id
                    Value country
                    OnChange
                        (fun ev ->
                            !!ev.target?value
                                |> msg
                                |> dispatch
                        )
                    ]
    ]


let signup brandName =
    div [] [
        h2 [ ClassName "green"; Style [ Padding 20 ] ] [
            sprintf "Your printer is suitable for %s Automatic Ink,"
                brandName
                |> str
            br []
            str "Powered by Epson ReadyInk"
            img [ Src "https://automaticink.eu/assets/img/suitable.svg" ]
        ]
        div [ Id "api" ] []
        div [ Style [ Padding 20 ] ] [
            str "* The registration can’t be completed unless you tick “Allow”"
        ]
    ]


let page logo brandName purpose =
    html [ Lang "en-EN" ] [
        head [] [
            title [] [
                sprintf
                    "%s Automatic Ink - Powered by Epson ReadyInk"
                    brandName
                    |> str
                ]
            meta [ CharSet "utf-8" ]
            meta [ Name "apple-mobile-web-app-capable"; Content "yes" ]
            meta [ Name "viewport"; Content "width = device-width, initial-scale = 1, maximum-scale = 1, user-scalable = yes" ]
            meta [ Name "keywords"; Content "WePrintSmarter We Print Smarter online printing business marketplace" ]
            meta [ Name "description"; Content "WePrintSmarter is a new online print marketplace that delivers value-added services to help you optimize your printing and document management needs." ]
            link [ Href "https://fonts.googleapis.com/css?family=Open+Sans:300,400,700"; Rel "stylesheet" ]
            link [ Href "https://automaticink.eu/assets/css/style.css"; Rel "stylesheet"; Type "text/css" ]
        ]
        body [] [
            div [ Id "wrapper"; sprintf "en %s>" brandName |> ClassName ] [
                header [] [
                    div [ ClassName "container" ] [
                        div [ Id "topbar" ] [
                            a [ Href "#english" ] [
                                img [ Src "https://automaticink.eu/assets//img/land.svg" ]
                                str "English"
                            ]
                        ]
                        div [
                            Id "logo"
                            ClassName "eventListener navigate"
                            !!("data-navigate-to-page", "home")
                        ] [ logo ]
                    ]
                ]
                section [ ClassName "hero_block"; Id "hero" ] [
                    div [ ClassName "container" ] [
                        div [ ClassName "hero_blockContent" ] [
                            img [ Src "https://automaticink.eu/assets/img/automaticink.png" ]
                        ]
                    ]
                ]
                (match purpose with
                    | "signup" -> signup brandName
                    | _ -> div [ Id "api" ] []
                )
                div [ Id "footer"; ClassName "en" ] [
                    table [ FrameBorder "0"; CellSpacing "0"; CellPadding "0" ] [
                        tr [] [
                            td [ ClassName "cookie"; Id "cookieMessage" ] [
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
                        div [ ClassName "col" ] [ ]
                    ]
                ]
            ]
        ]
    ]


let logo =
    function
        | "also" ->
            {   id = "also"
                logoSrc = "https://automaticink.eu/assets/img/retailers/also retail.svg"
                logoAlt = "Logo ALSO Retail"
                logoHref = "https://automaticink.eu"
            }

        | "elkjop" ->
            {   id = "elkjop"
                logoSrc = "https://elkjop.azurepack.com/assets/img/retailers/elkjop.png"
                logoAlt = "Logo Elkjøp"
                logoHref = "https://elkjop.azurepack.com"
            }

        | brandId ->
            {   id = brandId
                logoSrc =
                    sprintf
                        "https://%s.automaticink.eu/assets/img/retailers/%s.svg"
                        brandId
                        brandId

                logoAlt = "Logo " + brandId
                logoHref = "https://%s.automaticink.eu" + brandId
            }
        >>  (fun brand ->
                a [ ClassName "logowrapper"; Href brand.logoHref ]
                    [ img [ Src brand.logoSrc; Alt brand.logoAlt ] ]
            )


let brandName =
    function
    | "also" -> "ALSO Retail"
    | "elkjop" -> "Elkjøp"
    | other -> other
