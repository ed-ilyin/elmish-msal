module Library.Types

open Fable.Core


type Eligibility =
    | UnknownEligibility
    | Eligible
    | NonEligible of string


[<StringEnum>]
type StoreType =
    | [<CompiledName("ONLINE")>]Online
    | [<CompiledName("OFFLINE")>]Offline


type Printer = {
    country: string
    id: string
    isOptIn: bool
    marketingCommunications: bool
    personalDataSharing: bool
    printerEan: string
    printerEmaCompatible: bool
    printerModelName: string
    printerPmaCompatible: bool
    printerSku: string
    storeType: StoreType
    userDisplayName: string
    userEmail: string
    userFirstName: string
    userId: string
    userLastName: string
    userPhone: string
}


type Brand = {
    id: string
    logoSrc: string
    logoAlt: string
    logoHref: string
}
