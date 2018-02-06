module AutomaticInk.Functions

open AutomaticInk
open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.EdIlyin.Core
open Fable.EdIlyin.Core.Http
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import
open Fable.Import.Node.Globals
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Library
open Library.Types
open Html4

module JD = Fable.EdIlyin.Core.Json.Decode


let ip context =
    promiseResult {
        let! ip = Fetch.get "http://icanhazip.com" [] Fetch.text
        let res = createObj [ "body" ==> ip ]
        do !!context?log("res", res)
        return context?``done``(null, res)
    }
        |> PowerPack.Promise.start


let tryFetch (url: string) (init: RequestProperties list) =
    GlobalFetch.fetch(RequestInfo.Url url, requestProps init)
        |> Promise.bind (fun response ->
            if response.Ok then Ok response |> Promise.lift else
                Promise.map
                    (ofJson<Epson.ErrorMessage>
                        >> function
                            | { Epson.details = Some details } -> details
                            | { Epson.message = Some message
                                field = Some field
                              } -> message + ": " + field
                            | { Epson.message = Some message } -> message
                            | { Epson.code = code } ->
                                let first = System.Char.ToUpper code.[0]
                                let rest = code.Substring(1).ToLowerInvariant()
                                System.String[|first|] + rest
                                    |> (fun s -> s.Replace('_',' '))
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


let tryPostRecord<'T> (url: string) (record: 'T) (properties: RequestProperties list) =
    let defaultProps =
        [ RequestProperties.Method HttpMethod.POST
        ; requestHeaders [ContentType "application/json"]
        ; RequestProperties.Body !^(toJson record)]
    // Append properties after defaultProps to make sure user-defined values
    // override the default ones if necessary
    List.append defaultProps properties |> tryFetch url


let [<PassGenerics>]tryPostRecordAs<'T,'U> (url: string) (record: 'T) (properties: RequestProperties list) =
    let defaultProps =
        [ RequestProperties.Method HttpMethod.POST
        ; requestHeaders [ContentType "application/json"]
        ; RequestProperties.Body !^(toJson record)]
    // Append properties after defaultProps to make sure user-defined values
    // override the default ones if necessary
    List.append defaultProps properties |> tryFetchAs<'U> url


let log context tag a =
    do sprintf "%s: %A" tag a |> !!context?log
    a


let responseMessage (statusCode: int) (userMessage: string) (data: obj) =
    do data?version <- Config.version
    do data?status <- statusCode
    do data?userMessage <- userMessage
    createObj [ "status" ==> statusCode; "body" ==> data ]


let azureHttpFunction context promise =
    Promise.result promise
        |> Promise.map (
            fun result ->
            Result.bind id result
                |> function
                    | Error (x: System.Exception) ->
                        responseMessage 409 x.Message createEmpty
                    | Ok data -> responseMessage 200 "Ok" data
                |> log context "$return"
        )


let azureFunction context =
    Promise.result
    >> Promise.map (Result.bind id >> function
        | Error (x: System.Exception) -> context?``done``(x.Message)
        | Ok ret ->
            do sprintf "$return: %A" ret |> !!context?log
            context?``done``((), ret)
    )
    >> Promise.start


let getOrders context _ =
    promiseResult {
        let url =
            "https://readyink-apitest.epson.eu/service/rest/api/v1/orders"

        do !!context?log("url:", url)

        let props =
            [ requestHeaders [ Custom ("Api-Key", Config.apiKey) ] ]

        let! cartridges = tryFetchAs<Epson.OrderList> url props
        do !!context?log("cartridges", sprintf "%A" cartridges)
        return cartridges.orders
    }
        |> azureFunction context


let cartridgeListItem color ean someInstalled someDescription =
    Option.map2
        (fun description installed ->
            tr [] [
                td [ Width "35" ]
                    [ img [ "cid:" + color |> Src ] ]
                td [ Width "15" ] []
                td [ Width "480" ] [
                    a [ "http://alexhockdemo.online-reseller.de/eshop.php?action=like_search&amp;shopfilter_category=&amp;s_group_id=*&amp;s_order_name=&amp;onlygroups=0&amp;s_available=&amp;articlelist_type=search&amp;s_volltext="
                            + ean
                            |> Href
                        Target "_blank"
                        Style [
                            !!( "font-family", "Arial" )
                            !!( "font-size", "14px" )
                            !!( "line-height", "25px" )
                            !!( "text-align", "left" )
                            !!( "background", "white" )
                            !!( "color", "#555555" )
                        ]
                    ] [ str description ]
                ]
            ] |> tuple installed
        )
        someDescription
        someInstalled


let processOrder context (order: Epson.Order) (printer: Printer) =
    promiseResult {
        do log context "order" order |> ignore
        do log context "printer" printer |> ignore
        let! supplies = Config.suppliesPromise
        let! user = ActiveDirectory.getUser printer.userId
        do log context "user" user |> ignore

        let recipients =
            user.signInNames
                |> Array.filter (fun s -> s.``type`` = "emailAddress")
                |> Array.map
                    (fun signInName ->
                        SendGrid.email user.displayName
                            signInName.value
                    )

        let inkColors =
            Option.defaultValue [||] order.cartridges
                |> Array.map (fun c -> Epson.colorToString c.color)

        let subject =
            match inkColors with
                | [||] -> "Ink"
                | [|color|] -> color + " Ink"
                | _ -> Array.reduce (sprintf "%s, %s") inkColors + " Inks"
                |> log context "inks"

        let neededInks, alternativeInks =
            Option.defaultValue [||] order.cartridges
                |> Array.collect
                    (fun c ->
                        let color = Epson.colorToString c.color
                        [|  c.singlepack.std
                            c.singlepack.xl
                            c.singlepack.xxl
                            c.multipack.std
                            c.multipack.xl
                        |]
                        |> Array.choose id
                        |> Array.map
                            (fun cm ->
                                Map.tryFind cm.ean supplies
                                    |> cartridgeListItem color cm.ean
                                        cm.installed
                            )
                    )
                |> Array.choose id
                |> List.ofArray
                |> List.partition fst

        let! css = Config.emailCssPromise

        let html =
            Email.lowInk css user.displayName subject
                printer.printerSku
                printer.printerModelName
                printer.id
                (List.map snd neededInks)
                (List.map snd alternativeInks)
                Config.voucherCode
                "Elkjøp"
                |> ReactDomServer.renderToStaticMarkup
                |> (+) "<!DOCTYPE html>\n"

        let! header = Config.headerPromise
        let! black = Config.blackPromise
        let! cyan = Config.cyanPromise
        let! magenta = Config.magentaPromise
        let! yellow = Config.yellowPromise

        let message =
            [   SendGrid.inlineJpeg header "header"
                SendGrid.inlineJpeg black "black"
                SendGrid.inlineJpeg cyan "cyan"
                SendGrid.inlineJpeg magenta "magenta"
                SendGrid.inlineJpeg yellow "yellow"
            ]   |> SendGrid.htmlMessageWithAttachments "Automatic Ink"
                    "noreply@automaticink.eu"
                    recipients
                    html

        return message
    }
        |> azureFunction context


let printers context req (printers: Printer []) =
    do log context "context" context |> ignore
    do log context "req" req |> ignore

    match !!req?headers?``x-ms-client-principal-id`` with
    | None -> !!context?``done``("Unauthorized")

    | Some userId ->
        do log context "userId" userId |> ignore

        let userPrinters =
            Array.where (fun p -> p.userId = userId) printers

        do log context "user's printers" userPrinters |> ignore
        context?``done``((),userPrinters)


let azureAuthHttpFunction context func =
    match !!context?req?headers?``x-ms-client-principal-id`` with
        | None -> exn "Unauthorized" |> Error |> Promise.lift
        | Some userId ->
            do log context "userId" userId |> ignore
            func userId
    |> Promise.result
    |> Promise.map (
        fun result ->
        Result.bind id result
            |> function
                | Error (x: System.Exception) ->
                    responseMessage 409 x.Message createEmpty
                | Ok data -> responseMessage 200 "Ok" data
            |> log context "$return"
    )


let private validatePrinterSerialNumberAndGetPrinterInfo (serial: string) country =
    promiseResult {
        let buffer = Buffer.from serial

        let url =
            sprintf
                "https://readyink-apitest.epson.eu/service/rest/api/v1/printers/%s/%s"
                (buffer.toString "base64")
                country

        let props = [
            Fetch.requestHeaders [ Custom ("Api-Key", Config.apiKey) ]
        ]

        let! response = tryFetchAs<Epson.PrinterInfo> url props

        let! printerInfo =
            if response.isOptIn
                then exn "Serial number already registered" |> Error
                else Ok response
                |> Promise.lift

        return printerInfo
    }


let validatePrinterSerialNumber context (claims: Types.PrinterClaims) =
    do log context "claims" claims |> ignore

    validatePrinterSerialNumberAndGetPrinterInfo
        claims.printerSerialNumber
        claims.country
        |> azureHttpFunction context


let private _userProfile firstName lastName country email
    personalDataSharing marketingCommunications phone : Epson.UserProfile =
    {   firstName = firstName
        lastName = lastName
        country = country
        email = email
        personalDataSharing = personalDataSharing
        marketingCommunications = marketingCommunications
        phone = Some phone
    }


let private _createSubscription country serialNumber storeType userProfile =
    promiseResult {
        let subscription: Epson.Subscription = {
            country = country
            serviceOptIn = true
            serialNumber = serialNumber
            storeType = Some storeType
            userProfile = Some userProfile
        }

        let url =
            "https://readyink-apitest.epson.eu/service/rest/api/v1/subscriptions"

        let props = [
            requestHeaders [
                Custom ("Api-Key", Config.apiKey)
                ContentType "application/json"
            ]
        ]

        let! response =
            tryPostRecord<Epson.Subscription> url subscription props

        let! text = response.text() |> Promise.result

        let responseInfo =
            sprintf "response %i: %s: %s" response.Status
                response.StatusText
                text

        return responseInfo
    }


let private _messageAboutNewPrinter name email userId serial modelName isPma brand =
    promiseResult {
        let! css = Config.emailCssPromise

        let html =
            Email.newPrinter css name userId serial modelName isPma
                brand
                |> ReactDomServer.renderToStaticMarkup
                |> (+) "<!DOCTYPE html>\n"

        let! header = Config.headerPromise

        let message =
            SendGrid.htmlMessageWithOneInlineJpegToSingleRecipient
                Config.fromName Config.fromEmail name email html
                header "header.jpg" "header"

        return message
    }


let createSubscription context (claims: Printer) =
    promiseResult {
        do !!context?log(sprintf "data: %A" claims)

        let! responseInfo =
            _userProfile claims.userFirstName claims.userLastName
                claims.country
                claims.userEmail
                claims.personalDataSharing
                claims.marketingCommunications
                claims.userPhone
                |> _createSubscription claims.country claims.id
                    claims.storeType

        do !!context?log(responseInfo)
        return createEmpty
    }
        |> azureHttpFunction context


let saveAndWelcome context (claims: Printer) =
    promiseResult {
        do !!context?log(sprintf "context: %A" context)
        do !!context?log(sprintf "data: %A" claims)
        do context?bindings?printer <- { claims with isOptIn = true }

        // send welcome email
        let! message =
            _messageAboutNewPrinter claims.userDisplayName
                claims.userEmail claims.userId claims.id
                claims.printerModelName claims.printerPmaCompatible
                "Elkjøp"

        do context?bindings?message <- message
        return createEmpty
    }
        |> azureHttpFunction context


let private _subscriptionClaims id country printerEmaCompatible
    printerPmaCompatible printerModelName printerSku printerEan
    userEmail userDisplayName userFirstName userLastName userPhone
    marketingCommunications personalDataSharing storeType userId
    isOptIn : Printer =
    {   id = id
        country = country
        printerEmaCompatible = printerEmaCompatible
        printerPmaCompatible = printerPmaCompatible
        printerModelName = printerModelName
        printerSku = printerSku
        printerEan = printerEan
        userEmail = userEmail
        userDisplayName = userDisplayName
        userFirstName = userFirstName
        userLastName = userLastName
        userPhone = userPhone
        marketingCommunications = marketingCommunications
        personalDataSharing = personalDataSharing
        storeType = storeType
        userId = userId
        isOptIn = isOptIn
    }


let addPrinter context req (printers: Printer []) =
    do log context "req" req |> ignore

    azureAuthHttpFunction context
        <| fun userId ->
            promiseResult {
                let data = req?body
                let serial = !!data?serial
                let country = !!data?country

                // get printer info from Epson
                let prinfoPromise =
                    validatePrinterSerialNumberAndGetPrinterInfo
                        serial country

                // get user claims from AD B2C
                let userPromise = ActiveDirectory.getUser userId

                // resolve promises
                let! prinfo = prinfoPromise
                let! user = userPromise

                // get user claims from AD B2C
                do log context "user" user |> ignore

                let! email =
                    user.signInNames
                        |> Array.filter (fun s -> s.``type`` = "emailAddress")
                        |> Array.map
                            (fun signInName -> signInName.value)
                        |> Array.tryHead
                        |> Result.ofOption (exn "No email address found")
                        |> Promise.lift

                // subscribe in Epson
                let! creationResponseInfo =
                    _userProfile user.givenName user.surname
                        user.country
                        email
                        user.extension_420b48be065d4beb83b346aba1bda2d3_PersonalDataSharing
                        user.extension_420b48be065d4beb83b346aba1bda2d3_MarketingCommunications
                        user.telephoneNumber
                        |> _createSubscription country serial
                            user.extension_420b48be065d4beb83b346aba1bda2d3_StoreType

                do log context "creation response info" creationResponseInfo |> ignore

                //save to Azure Cosmos DB
                let newPrinter =
                    _subscriptionClaims serial
                        country
                        prinfo.emaCompatible
                        prinfo.pmaCompatible
                        prinfo.modelName
                        prinfo.sku
                        prinfo.ean
                        email
                        user.displayName
                        user.givenName
                        user.surname
                        user.telephoneNumber
                        user.extension_420b48be065d4beb83b346aba1bda2d3_MarketingCommunications
                        user.extension_420b48be065d4beb83b346aba1bda2d3_PersonalDataSharing
                        user.extension_420b48be065d4beb83b346aba1bda2d3_StoreType
                        userId
                        true

                do context?bindings?printer <- newPrinter

                // send welcome email
                let! message =
                    _messageAboutNewPrinter user.displayName email
                        userId serial prinfo.modelName
                        prinfo.pmaCompatible
                        !!req?query?brand

                do log context "message" message |> ignore
                do context?bindings?message <- message

                //return updated list of printers
                let userPrinters =
                    printers
                        |> Array.where (fun p -> p.userId = userId)
                        |> Array.map (fun p -> p.id, p)
                        |> Map.ofArray
                        |> Map.add serial newPrinter
                        |> Map.toArray
                        |> Array.map snd

                return userPrinters
            }


let unregisterPrinter context req (printer: Printer) =
    do log context "context bindings" context?bindings |> ignore

    azureAuthHttpFunction context
        <| fun userId ->
            promiseResult {
                do log context "req body" req?body |> ignore
                do log context "printer in" printer |> ignore
                let serial: string = !!req?body?serial

                // check if printer belongs to user
                do! if printer.userId = userId
                    then Ok ()
                    else exn "Not user's printer" |> Error
                    |> Promise.lift

                // unregister printers
                do! Epson.deactivateSubscription serial

                // mark as unregistered in Cosmos DB
                do context?bindings?printerOut <-
                    { printer with isOptIn = false }

                return createEmpty
            }


let mapPrinters context _ (printers: Printer []) =
    let ps = printers |> Array.map (fun p -> { p with isOptIn = true})
    context?``done``((),ps)


type Headers = {
    ``Content-Type``: string
}


type Response = {
    headers: Headers
    body: string
}


let b2cHtml context req =
    promise {
        do log context "params" req?``params`` |> ignore
        do log context "query" req?query |> ignore
        let brand = !!req?query?brand
        let logo = View.logo brand
        let purpose = !!context?bindingData?purpose

        let body =
            Library.View.page logo (View.brandName brand) purpose
                |> ReactDomServer.renderToStaticMarkup
                |> (+) "<!DOCTYPE html>\n"

        let response =
            {   headers =
                    { ``Content-Type`` = "text/html; charset=UTF-8" }
                body = body
            }

        return response
    }


let updateSubscription context (claims: Types.UpdateSubscriptionClaims) (printers: Printer []) =
    promiseResult {
        do log context "claims" claims |> ignore
        do log context "printers" printers |> ignore

        let user =
            Epson.userProfileUpdate claims.country
                <| Some claims.firstName
                <| Some claims.lastName
                <| Some claims.marketingCommunications
                <| claims.personalDataSharing
                <| Some claims.phone
                <| Some claims.email

        do! printers
                |> Array.map
                    (fun p -> Epson.updateSubscription user p.id)
                |> Promise.Parallel
                |> Promise.map
                    (Result.combineArray >> Result.map ignore)

        return createEmpty
    }
        |> azureHttpFunction context


let unsubscribe context req (printers: Printer []) =
    do log context "printers" printers |> ignore

    azureAuthHttpFunction context
        <| fun userId ->
            promiseResult {
                do! if userId = !!req?body?userId then Ok ()
                    else exn "It's not yours userId" |> Error
                    |> Promise.lift

                do! printers
                    |> Array.map
                        (fun p -> Epson.deactivateSubscription p.id)
                    |> Promise.Parallel
                    |> Promise.map
                        (Result.combineArray >> Result.map ignore)

                // mark as unregistered in Cosmos DB
                do context?bindings?printersOut <-
                    printers |> Array.map (fun p ->
                        { p with isOptIn = false }
                    )

                // delete AD user
                let! response = ActiveDirectory.deleteUser userId
                do log context "ad delete" response |> ignore

                return createEmpty
            }
