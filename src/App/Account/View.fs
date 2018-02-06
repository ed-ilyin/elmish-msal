module Account.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Library.Types
open Types
open Fable.Core.JsInterop


let pma = [
    hr []
    div [ ClassName "printer_levels" ] [
        div [ ClassName "printer_levels__title" ] [
            str "Your printer needs additional software to work with the Automatic Ink program"
            ]
    ]
    p [] [ str "Please find software for download here:" ]
    (   [   "PC", "ftp://download.epson-europe.com/pub/rma/EpsonReadyInkAgent_Win.exe"
            "MAC", "ftp://download.epson-europe.com/pub/rma/EpsonReadyInkAgent_Mac.dmg"
        ]
            |> List.map (fun (p, l) ->
                li [] [
                    sprintf "[%s] - " p |> str
                    a [ Href l ] [ str l ]
                ]
            )
            |> ul []
    )
]


let printer dispatch (pr: Printer) =
    div [ ClassName "printer" ] [
        div [ ClassName "printer_info" ] [
            img [ Src "assets/img/printer_icon.svg"; ClassName "printer_image" ]
            div [ ClassName "registered" ] [
                span [ ClassName "registeredprinter" ] [
                    img [ Src "assets/img/suitable.svg" ]
                    str "Your printer is registered"
                ]
                a [ Href "#unregister"
                    OnClick
                        (fun _ -> UnregisterPrinter pr.id |> dispatch)
                ] [ str "Unregister" ]
            ]
            h3 [ ClassName "printer_info__title" ] [
                str pr.printerModelName
                ]
            ul [] [
                li [] [
                    span [ ClassName "printer_info__subtitle" ] [
                        str "Serial number"
                        ]
                    span [ ClassName "printer_info__info" ] [
                        str pr.id
                        ]
                ]
                li [] [
                    span [ ClassName "printer_info__subtitle" ] [
                        str "Printer code"
                        ]
                    span [ ClassName "printer_info__info" ] [
                        str pr.printerSku
                        ]
                ]
                li [] [
                    span [ ClassName "printer_info__subtitle" ] [
                        str "EAN Code"
                        ]
                    span [ ClassName "printer_info__info" ] [
                        str pr.printerEan
                        ]
                ]
            ]
        ]
        div [] <| if pr.printerPmaCompatible then pma else []
    ]


let printers dispatch = function
    | [] -> str "You have no printers registered"
    | ps -> List.map (printer dispatch) ps |> div []


let success message =
    section [
        Id "success"
        ClassName "register register--suitable register--suitable--account"
    ] [
        div [ ClassName "container" ] [
            div [ ClassName "formHolder signupFormHolder registerContent" ] [
                div [ ClassName "form"; Id "signupForm" ] [
                    div [ ClassName "formRow twoThirds" ] [
                        div [ ClassName "formCol" ] [
                            h2 [ ClassName "green" ] [
                                str message
                                br []
                                str "Powered by Epson ReadyInk"
                                img [ Src "assets/img/suitable.svg" ]
                            ]
                            p [] [
                                str ""
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
let error message =
    section [ ClassName "register register--notsuitable register--notsuitable-account" ] [
        div [ ClassName "container" ] [
            hr []
            div [ ClassName "formHolder signupFormHolder registerContent" ] [
                div [ ClassName "form"; Id "signupForm" ] [
                    div [ ClassName "formRow twoThirds" ] [
                        div [ ClassName "formCol" ] [
                            h2 [ ClassName "red" ] [
                                str message
                                img [ Src "assets/img/notsuitable.svg" ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]


let root (model: Types.Model) dispatch =
    div [] [
        section [ ClassName "hero_block"; Id "hero" ] [
            div [ ClassName "container" ] [
                div [ ClassName "hero_blockContent" ] [
                    img [ Src "assets/img/automaticink.png" ]
                ]
            ]
        ]
        section [ ClassName "account" ] [
            div [ ClassName "container" ] [
                div [ ClassName "formHolder signupFormHolder registerContent" ] [
                    div [ ClassName "formCol printers" ] [
                        br []
                        h2 [] [
                            str "Printer overview"
                            ]
                        printers dispatch model.printers
                    ]
                ]
            ]
        ]
        hr []
        section [ ClassName "register another"; Id "register" ] [
            div [ ClassName "container" ] [
                div [ ClassName "formHolder signupFormHolder registerContent" ] [
                    div [ ClassName "form"; Id "signupForm" ] [
                        h2 [] [
                            str "Register another printer"
                            ]
                        p [] [
                            str "To join ReadyInk, first check if you can register your printer by entering below your printer serial number."
                            ]
                        div [ ClassName "formRow three" ] [
                            div [ ClassName "formCol" ] [
                                div [ ClassName "inputHolder firstName required" ] [
                                    label [ HtmlFor "serial"; ClassName "placeholder" ] [
                                        str "Enter your printer serial number"
                                        ]
                                    input [
                                        Id "serial"
                                        Type "text"
                                        Name "serial"
                                        OnChange
                                            <| fun ev ->
                                                !!ev.target?value
                                                    |> UpdatePrinterSerial
                                                    |> dispatch
                                    ]
                                ]
                            ]
                            div [ ClassName "formCol" ] [
                                div [ ClassName "inputHolder lastName required" ] [
                                    label [ HtmlFor "country"; ClassName "placeholder" ] [
                                        str "Select your country"
                                        ]
                                    div [ ClassName "select" ] [
                                        [   "", "Country*"
                                            "UK", "United Kingdom"
                                            "NL", "Netherlands"
                                            "DE", "Germany"
                                        ]
                                            |> List.map
                                                (fun (k, v) ->
                                                    option [ Value k ]
                                                        [ str v ]
                                                )
                                            |> select [
                                                Name "country"
                                                Value model.addPrinterCountry
                                                OnChange
                                                    <| fun ev ->
                                                        !!ev.target?value
                                                            |> UpdatePrinterCountry
                                                            |> dispatch
                                            ]
                                    ]
                                ]
                            ]
                            div [ ClassName "formCol" ] [
                                button [
                                    ClassName "button"
                                    Disabled model.addPrinterButtonDisabled
                                    OnClick (fun _ -> dispatch AddPrinter)
                                ] [ str "Add printer" ]
                            ]
                            div [ ClassName "clear" ] [
                                ]
                        ]
                    ]
                ]
            ]
        ]
        hr []
        section [ ClassName "register another" ] [
            div [ ClassName "container registerContent attrEntry" ] [
                h2 [] [ str "Unsubscribe from service" ]
                p [] [
                    str "If you check checkbox and press the button you and your printers will be immediatly unsubscribed and account deleted"
                ]
                input [
                    Id "sure-to-unsubscribe"
                    Type "checkbox"
                    not model.unsubscribeDisabled |> Checked
                    OnClick (fun _ -> dispatch ToggleUnsubscribe)
                ]
                label [ HtmlFor "sure-to-unsubscribe"; Style [ !!("width","initial")] ] [
                    str "Check this checkbox if you are sure that you want to unsubscribe"
                ]
                button [
                    ClassName "button"
                    Style [ !!("margin", "0") ]
                    Disabled model.unsubscribeDisabled
                    OnClick (fun _ -> dispatch Unsubscribe)
                ] [ str "Unsubscribe" ]
            ]
         ]
        (match model.success with
            | None -> section [ Id "success"; ClassName "hidden" ] []
            | Some message -> success message
        )
        (match model.error with
            | None -> section [ ClassName "hidden" ] []
            | Some message -> error message
        )
    ]
