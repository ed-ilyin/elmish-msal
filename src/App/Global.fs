module Global

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser
open Fable.Import
open Fable.PowerPack


do printfn "global location: %A" window.location


type Page = Home | Account


[<Import("*","msal")>]
let Msal: obj = jsNative


let b2cLogin = "https://login.microsoftonline.com/"


let (|Www|_|) (host:string) (s:string) =
    if host = s || "www." + host = s then Some s else None


let brandId, tenant, api, brandName =
    match window.location.host with
        | "localhost:8080" | Www "elkjop.azurepack.com" _ ->
            "elkjop", "automaticinkelkjop", "automaticink-elkjop", "Elkjøp"

        | Www "automaticink.eu" _ ->
            "also", "automaticink", "automaticink-api", "ALSO Retail"

        | host -> host, "automaticink", "automaticink-api", host.ToUpper()


let b2cClientId =
    match brandId with
        | "elkjop" -> "0041318d-9770-47b5-be03-f9d7f94d2793"
        | _ -> "19eaea5a-6b26-4485-be16-faae338fc6d2"


let b2cAuthority policyName =
    sprintf "%stfp/%s.onmicrosoft.com/B2C_1A_%s"
        b2cLogin
        tenant
        policyName


let internal toKeyValuePair (segment:string) =
    match segment.Split('=') with
    | [| key; value |] ->
        Elmish.Browser.Option.tuple
            (Elmish.Browser.Option.ofFunc JS.decodeURI key)
            (Elmish.Browser.Option.ofFunc JS.decodeURI value)
    | _ -> None


let b2c policy =
    sprintf
        "%s%s.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1A_%s&client_id=%s&nonce=defaultNonce&redirect_uri=%s&scope=openid&response_type=id_token&brand=%s"
        b2cLogin
        tenant
        policy
        b2cClientId
        window.location.origin
        brandId


// let loginCallback errorDesc = //token error tokenType =
//     printfn
//         "error description: %A"//\nerror: %A\ntoken: %A\ntoken type: %A"
//         errorDesc //error tokenType


// let loggerCallback logLevel message piiLoggingEnabled =
//     printfn "msal: %s" message


// let logger =
//     createNew Msal?Logger
//         ( loggerCallback
//         , createObj [
//             "level" ==> Msal?LogLevel?Verbose
//             "piiLoggingEnabled" ==> true
//         ]
//         )


// let msalOptions =
//     createObj [
//         // "logger" ==> logger
//         // "redirectUri" ==> "http://localhost:8080/abba"
//         "validateAuthority" ==> false
//     ]

let msalApp p =
    createNew Msal?UserAgentApplication (b2cClientId, b2cAuthority p)


let msalSusi = msalApp "signup_signin"


let b2cScopes =
    [| sprintf "https://%s.onmicrosoft.com/api/read" tenant |]


let b2cRedirect policy =
    do !!msalApp(policy)?loginRedirect(b2cScopes, "brand=" + brandId)


let hashParams =
    window.location.hash.Split('&')
        |> Seq.map toKeyValuePair
        |> Seq.choose id
        |> Map.ofSeq


// let msalApp =
//     match Map.tryFind "error_description" hashParams with
//     | None -> msal ()

//     | Some desc ->
//         if desc.StartsWith "AADB2C90118" then
//             let u = b2c "PasswordReset"
//             // do printfn "password reset: %s" u
//             do window.location.href <- u
//             createEmpty

//         else msal ()


let toHash =
    function
        | Home -> "#home"
        | Account -> "#account"


let webApi = sprintf "https://%s.azurewebsites.net/api/" api


// let usr = createNew Msal?User ()
// do printfn "usr: %A" usr
// do !!msalApp?acquireTokenSilent(b2cScopes, b2cAuthority, createNew)
//     |> Promise.map (printfn "token: %A")
//     |> Promise.result
//     |> Promise.map (printfn "result: %A")
//     |> Promise.start
