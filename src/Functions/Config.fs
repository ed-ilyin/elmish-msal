module AutomaticInk.Config

open Fable.Core.JsInterop
open Fable.EdIlyin.Core
open Fable.Import
open Fable.Import.Node.Buffer
open Fable.Import.Node.Exports
open Fable.Import.Node.Globals
open Fable.PowerPack

let api = "https://readyink-apitest.epson.eu/service/rest/api/v1/"
let apiKey = "mheg2621a302df41gypo1foep26f871ak92bsw1g35hw56fgreic7g2ipt3eatghah07er5g1i8sbty78x5g6r66w9f4f49uyea15yx9jghk7woi5nknm95kwncpogzubcy98l94xmsk0d5t10dqehihbr0woezjd32etsifg7xdi6io8ca0fj2xcdxy0dfiliwg5kvjiwwmzu2b7ecuw73pfg1yoaewtyt5v4lxd4dk9qdygjz8bqork59dfb6"
let adal = import<obj> "AuthenticationContext" "adal-node"
let authorityHostUrl = "https://login.windows.net"
let tenant = "automaticink.onmicrosoft.com" // AAD Tenant name.
let authorityUrl = authorityHostUrl + "/" + tenant
let applicationId = "ae043e5e-7278-4144-abc4-fdaa77f82333" // Application Id of app registered under AAD.
let clientSecret = "iOf/AhIoZbxza60bs3opZzNXOemKhMOqfziVf07SW14=" // Secret generated for app. Read this environment variable.
let resource = "https://graph.windows.net" // URI that identifies the resource for which the token is valid.
let context = createNew adal authorityUrl
let voucherCode = "BkRKgChikD3MVDF87Pyv"


let readAsset filename (encoding: BufferEncoding) : JS.Promise<Result<_,System.Exception>> =
    Promise.create
        <| fun resolve reject ->
            let path =
                path.resolve [| __dirname; "assets"; filename |]

            fs.readFile(
                path,
                createObj [ "encoding" ==> encoding ],
                fun err (data: string) ->
                    match err with
                        | Some x -> reject !!x
                        | _ -> resolve data
            )
        |> Promise.result


let suppliesPromise =
    promiseResult {
        let! json = readAsset "supplies.json" Utf8
        return ofJson<Map<string,string>> json
    }

let headerPromise = readAsset "header.jpg" Base64
let blackPromise = readAsset "black.jpg" Base64
let cyanPromise = readAsset "cyan.jpg" Base64
let magentaPromise = readAsset "magenta.jpg" Base64
let yellowPromise = readAsset "yellow.jpg" Base64
let emailCssPromise = readAsset "email.css" Utf8
let version = "1.0.0"
let fromName = "Automatic Ink"
let fromEmail = "noreply@automaticink.eu"
