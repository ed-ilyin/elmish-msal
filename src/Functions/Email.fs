module Email

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Html4


let template css userDisplayName reseller content =
    html [] [
        head [] [
            title [] [
                reseller + " AutomaticInk" |> str
                ]
            meta [ CharSet "utf-8" ]
            style [ Type "text/css" ] [ str css ]
        ]
        body [ Style [ !!( "background", "#f2f2f2" ); !!( "margin", "0" ); !!( "-webkit-text-size-adjust", "none" ); !!( "-webkit-font-smoothing", "antialiased" ) ] ] [
            center [] [
                table [ Border "0"; CellPadding "0"; CellSpacing "0"; Width "600"; ClassName "em_wrapper"; Style [ !!( "width", "600px" ) ] ] [
                    tr [] [
                        td [ Width "600"; Height "250"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "14px" ); !!( "line-height", "22px" ); !!( "text-align", "center" ) ] ] [
                            br []
                            img [ Src "cid:header"; Width "600"; Height "250"; Style [ !!( "display", "block" ); !!( "border", "0" ) ]; Alt "WePrintSmarter" ]
                        ]
                    ]
                    tr [] [
                        td [ ClassName "content"; Width "600"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "14px" ); !!( "line-height", "22px" ); !!( "text-align", "center" ) ] ] [
                            table [ Width "600"; Border "0"; CellPadding "0"; CellSpacing "0" ] [
                                tr [] [
                                    td [ Width "60"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "14px" ); !!( "line-height", "25px" ); !!( "text-align", "left" ); !!( "background", "white" ); !!( "color", "#555555" ) ] ] [
                                        ]
                                    td [ Width "480"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "14px" ); !!( "line-height", "25px" ); !!( "text-align", "left" ); !!( "background", "white" ); !!( "color", "#555555" ) ] ] [
                                        br []
                                        br []
                                        sprintf "Dear %s," userDisplayName |> str
                                        br []
                                        br []
                                        content
                                        br []
                                        br []
                                        str "Kind Regards"
                                        br []
                                        br []
                                        sprintf "Your %s AutomaticInk team" reseller |> str
                                        br []
                                        br []
                                    ]
                                    td [ Width "60"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "14px" ); !!( "line-height", "25px" ); !!( "text-align", "left" ); !!( "background", "white" ); !!( "color", "#555555" ) ] ] [
                                        ]
                                ]
                            ]
                        ]
                    ]
                    tr [] [
                        td [ ClassName "footer"; Width "600"; Height "10"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "12px" ); !!( "line-height", "22px" ); !!( "text-align", "center" ); !!( "background", "#000000" ); !!( "color", "white" ) ] ] [
                            ]
                    ]
                ]
                br []
                span [ ClassName "smallLine"; Style [ !!( "font-family", "Arial" ); !!( "font-size", "11px" ); !!( "color", "#a6a4a4" ) ] ] [
                    a [ Href "[[MQ:UNSUBSCRIBE]]"; Style [ !!( "color", "#a6a4a4" ); !!( "text-decoration", "underline" ) ] ] [
                        str "Unsubscribe"
                    ]
                    str " from this mailing list - "
                    a [ Href "[[MQ:ARCHIVELINK]]"; Style [ !!( "color", "#a6a4a4" ); !!( "text-decoration", "underline" ) ] ] [
                        str "View in browser"
                    ]
                ]
                br []
                br []
            ]
        ]
    ]


let lowInk css userDisplayName subject printerSku printerModelName
    printerSerialNumber neededInks alternativeInk voucherCode reseller =
    div [] [
        str "Automatic Ink system reported us that the ink level of "
        b [] [ str subject ]
        str " of your Printer "
        b [] [
            sprintf "%s - Epson %s" printerSku printerModelName |> str
        ]
        str " with Serial "
        b [] [ str printerSerialNumber ]
        str " has fallen below the set Threshold."
        br []; br []
        "We want to ensure that you´re never actually running out of ink. Please find below the link to re-order the needed ink in our webshop:"
            |> str

        table [
            Border "0"
            CellPadding "0"
            CellSpacing "0"
            Width "480"
        ] [ tbody [] neededInks ]
        br []; br []
        str "Alternative Product-List:"
        table [] [ tbody [] alternativeInk ]
        br []; br []
        str "Please use Voucher Code: "
        b [] [ str voucherCode ]
        " to receive a 10% discount for participation in the Automatic Ink program."
            |> str
    ] |> template css userDisplayName reseller


let pma isPma =
    if isPma then
        [   p [] [
                "For this service to work properly for your printer you need to install additional software on your PC or local network. This way the ink-level can be reported to Automatic Ink."
                    |> str
            ]
            p [] [
                "This software can be downloaded below here:"
                    |> str
            ]
            ([  "WIN", "ftp://download.epson-europe.com/pub/rma/EpsonReadyInkAgent_Win.exe"
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
    else []


let newPrinter css userDisplayName userId serial modelName isPma reseller =
    div [] [
        p [] [ sprintf "Welcome to %s Automatic Ink!" reseller |> str
        ]
        p [] [
            "You can access your account on www.autoamticink.eu with your registered user name: "
                + userId
                |> str
        ]
        p [] [ str "The printer you registered on the program is:" ]
        p [] [
            sprintf "%s with serial number %s" modelName serial |> str
        ]
        pma isPma |> div []
        p [] [
            "You can modify your contact details or add/move printers by accessing your account on "
                |> str
            a [ Href "www.automaticink.eu" ]
                [ str "www.automaticink.eu" ]
            ". Once your printer is running out of supply the program will send you an email notification so you can re-order your supplies on time."
                |> str
        ]
    ] |> template css userDisplayName reseller

