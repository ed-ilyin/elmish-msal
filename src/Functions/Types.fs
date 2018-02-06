module AutomaticInk.Types

open Fable.Core
open Library.Types


type SignInName = {
    ``type``: string
    value: string
}


type User = {
    displayName: string
    signInNames: SignInName []
    givenName: string
    surname: string
    country: string
    extension_420b48be065d4beb83b346aba1bda2d3_PersonalDataSharing: bool
    extension_420b48be065d4beb83b346aba1bda2d3_MarketingCommunications: bool
    telephoneNumber: string
    extension_420b48be065d4beb83b346aba1bda2d3_StoreType: StoreType
}


type PrinterClaims = {
    printerSerialNumber: string
    country: string
}

type UpdateSubscriptionClaims = {
    userId: string
    country: string
    firstName: string
    lastName: string
    marketingCommunications: bool
    personalDataSharing: bool
    phone: string
    email: string
}
