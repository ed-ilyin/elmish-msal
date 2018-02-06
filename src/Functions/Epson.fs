module Epson

open AutomaticInk
open Fable.Core.JsInterop
open Fable.PowerPack
open Fable.PowerPack.Fetch
open System
open Fable.Core
open Library.Types


[<StringEnum>]
type OrderColor =
    | [<CompiledName("k")>] Black
    | [<CompiledName("c")>] Cyan
    | [<CompiledName("m")>] Magenta
    | [<CompiledName("y")>] Yellow


type OrderCartrdidgeModel = {
    ean: string
    installed: bool option
    sku: string
}


type OrderAvailableMultipackCartridges = {
    std: OrderCartrdidgeModel option
    xl: OrderCartrdidgeModel option
}


type OrderAvailableSinglepackCartridges = {
    std: OrderCartrdidgeModel option
    xl: OrderCartrdidgeModel option
    xxl: OrderCartrdidgeModel option
}


type OrderCartridge = {
    color: OrderColor
    multipack: OrderAvailableMultipackCartridges
    singlepack: OrderAvailableSinglepackCartridges
}


type Order = {
    cartridges: OrderCartridge [] option
    serialNumber: string
}


type OrderList = {
    orders: Order []
}


type PrinterInfo = {
    ean: string
    emaCompatible: bool
    isOptIn: bool
    modelName: string
    pmaCompatible: bool
    sku: string
}


type UserProfile = {
    country: string
    email: string
    firstName: string
    lastName: string
    marketingCommunications: bool
    personalDataSharing: bool
    phone: string option
}


type Subscription = {
    country: string
    serialNumber: string
    serviceOptIn: bool
    storeType: StoreType option
    userProfile: UserProfile option
}


type ErrorMessage = {
    code: string
    details: string option
    field: string option
    message: string option
}


type UserProfileUpdate = {
    country: string
    firstName: string option
    lastName: string option
    marketingCommunications: bool option
    personalDataSharing: bool
    phone: string option
    email: string option
}


type SubscriptionUpdate = {
    serialNumber: string
    userProfile: UserProfileUpdate
}


let colorToString =
    function
        | Black -> "Black"
        | Cyan -> "Cyan"
        | Magenta -> "Magenta"
        | Yellow -> "Yellow"


let tryFetch method endpoint recordOption =
    let props =
        [   RequestProperties.Method method |> Some
            requestHeaders [
                Custom ("Api-Key", Config.apiKey)
                ContentType "application/json"
            ] |> Some
            Option.map (toJson >> U3.Case3 >> RequestProperties.Body)
                recordOption
        ] |> List.choose id

    let url = Config.api + endpoint

    GlobalFetch.fetch(RequestInfo.Url url, requestProps props)
        |> Promise.bind (fun response ->
            if response.Ok then Ok response |> Promise.lift else
                Promise.map
                    (ofJson<ErrorMessage>
                        >> function
                            | { details = Some details } -> details
                            | { message = Some message
                                field = Some field
                              } -> message + ": " + field
                            | { message = Some message } -> message
                            | { code = code } ->
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


let deactivateSubscription serialNumber =
    tryFetch HttpMethod.DELETE ("subscriptions/" + serialNumber) None
        |> Promise.mapResult ignore


let userProfileUpdate country firstName lastName
    marketingCommunications personalDataSharing phone email = {
    country = country
    firstName = firstName
    lastName = lastName
    marketingCommunications = marketingCommunications
    personalDataSharing = personalDataSharing
    phone = phone
    email = email
}


let updateSubscription userProfileUpdate serialNumber =
    let update = {
        serialNumber = serialNumber
        userProfile = userProfileUpdate
    }

    tryFetch HttpMethod.PUT "subscriptions"
        <| Some update
        |> Promise.mapResult ignore
