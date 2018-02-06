module Account.State

open Elmish
open Fable.Core.JsInterop
open Global
open Library
open Types
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.Core
open Fable.EdIlyin.Core
open Fable.Import.Browser

let init () : Model * Cmd<Msg> =
    {   accessToken = None
        printers = []
        addPrinterSerial = ""
        addPrinterCountry = ""
        addPrinterButtonDisabled = true
        error = None
        success = None
        brandId = ""
        unsubscribeDisabled = true
    }
        , Cmd.none


let addPrinterButtonStatus model =
    { model with
        addPrinterButtonDisabled =
            [   model.addPrinterSerial
                model.addPrinterCountry
            ] |> List.map ((=) "") |> List.reduce (||)
    }


let tryFetch (url: string) (init: RequestProperties list) =
    GlobalFetch.fetch(RequestInfo.Url url, requestProps init)
        |> Promise.bind (fun response ->
            if response.Ok then Ok response |> Promise.lift else
                Promise.map
                    (ofJson<AzureFuncErrorMessage>
                        >> fun e -> e.userMessage
                        >> exn
                        >> Error
                    )
                    <| response.text()
        )
        |> Promise.result |> Promise.map (Result.bind id)


let [<PassGenerics>]tryFetchAs<'T> (url: string) (init: RequestProperties list) =
    promiseResult {
        let! response = tryFetch url init
        let! text = response.text() |> Promise.result
        return ofJson<'T> text
    } |> Promise.result |> Promise.map (Result.bind id)


let addPrinter accessToken serial country =
    let data =
        createObj [
            "serial" ==> serial
            "country" ==> country
        ]

    let defaultProps = [
        RequestProperties.Method HttpMethod.POST
        requestHeaders [
            ContentType "application/json"
            "Bearer " + accessToken |> Authorization
        ]
        RequestProperties.Body !!(toJson data)
    ]

    tryFetchAs<Printer list> (webApi + "add-printer") defaultProps


let unregisterPrinter accessToken serial =
    let data = createObj [ "serial" ==> serial ]

    let defaultProps = [
        RequestProperties.Method HttpMethod.POST
        requestHeaders [
            ContentType "application/json"
            "Bearer " + accessToken |> Authorization
        ]
        RequestProperties.Body !!(toJson data)
    ]

    tryFetchAs<unit> (webApi + "unregister-printer") defaultProps


let unsubscribe accessToken =
    promiseResult {
        let userId: string = !!msalSusi?getUser()?idToken?sub
        let data = createObj [ "userId" ==> userId ]
        let bearer = "Bearer " + accessToken |> Authorization

        let unsubscribeProps = [
            RequestProperties.Method HttpMethod.POST
            requestHeaders [ bearer; ContentType "application/json" ]
            RequestProperties.Body !!(toJson data)
        ]

        do! tryFetchAs<unit> (webApi + "unsubscribe") unsubscribeProps
        do !!msalSusi?logout()
        return ()
    }


let update msg (model: Model) =
    // do printfn "Account page: %A" msg

    match msg with
    | Failure x ->
        // do printfn "Account page failure: %s" x.Message
        { model with error = Some x.Message }
            |> addPrinterButtonStatus
            , Cmd.none

    | GetTokenSilent ->
        model
            , Cmd.ofPromise (fun s -> !!msalSusi?acquireTokenSilent(s))
                b2cScopes
                SaveTokenAndGetPrinters
                !!GetTokenRedirect

    | GetTokenRedirect errorMessage ->
        // do printfn "Token silent acquire failure: %s" errorMessage
        model
            , Cmd.attemptFunc
                (fun s ->
                    let query = "brand=" + model.brandId
                    !!msalSusi?acquireTokenRedirect(s, (), (), query)
                )
                b2cScopes
                (!!exn >> Failure)

    | SaveTokenAndGetPrinters accessToken ->
        { model with accessToken = Some accessToken },
            Cmd.ofPromise
                (fetchAs<Printer list> (webApi + "printers"))
                [   requestHeaders
                        [ "Bearer " + accessToken |> Authorization ]
                ]
                UpdatePrinters
                Failure

    | UpdatePrinters printers ->
        { model with printers = printers }, Cmd.none

    | UpdatePrinterSerial serial ->
        { model with
            addPrinterSerial = serial
            success = None
            error = None
        }   |> addPrinterButtonStatus
            , Cmd.none

    | UpdatePrinterCountry country ->
        { model with
            addPrinterCountry = country
            success = None
            error = None
        }   |> addPrinterButtonStatus
            , Cmd.none

    | AddPrinter ->
        { model with addPrinterButtonDisabled = true; error = None }
            , match model.accessToken with
                | None -> Cmd.none
                | Some token ->
                    Cmd.ofPromiseResult
                        (addPrinter token model.addPrinterSerial)
                        model.addPrinterCountry
                        UpdatePrinters
                        Failure

    | UnregisterPrinter serial ->
        { model with
            printers =
                List.filter (fun p -> p.id <> serial) model.printers
            success = None
        }   , match model.accessToken with
                | None -> Cmd.none
                | Some token ->
                    Cmd.ofPromiseResult
                        (unregisterPrinter token)
                        serial
                        (fun () ->
                            sprintf "Printer with serial number %s is just unregistered"
                                serial
                                |> ShowSuccess
                        )
                        Failure

    | ShowSuccess message ->
        { model with success = Some message }
            , Cmd.attemptFunc
                (fun () ->
                    document.getElementById("footer").scrollIntoView()
                )
                ()
                Failure

    | UpdateBrandId brandId ->
        { model with brandId = brandId }, Cmd.none

    | ToggleUnsubscribe ->
        { model with
            unsubscribeDisabled = not model.unsubscribeDisabled
        }   , Cmd.none

    | Unsubscribe ->
        { model with unsubscribeDisabled = true }
            , match model.accessToken with
                | None ->
                    exn "You have to signin to unsubscirbe"
                        |> Failure
                        |> Cmd.ofMsg

                | Some token ->
                    Cmd.ofPromiseResult unsubscribe token
                        (fun () ->
                            ShowSuccess
                                "You have been unsubscribed from service"
                        )
                        Failure

let updateBrandId = Account.Types.UpdateBrandId >> Cmd.ofMsg
