module Cli

open TrustFramework
open Fable.Import.Node.Exports
open Fable.Core
open Fable.Import.Node.Buffer
open Fable.Import.Node.Globals
open Email
open Fable.Import
open Fable.Helpers.React


match ``process``.argv |> Array.ofSeq with
    | [|_;_;"b2c"|] ->
        let xmlString = Base.policy "automaticinktest" |> Xml.toString
        do fs.writeFileSync(
            "b2c/generated/TrustFrameworkBase.generated.xml",
            xmlString,
            JsInterop.createEmpty
        )
    | [|_;_;"base64"|] ->
        fs.readFileSync(
            "functions/assets/cyan.jpg",
            Base64
        )   |> printfn "%A"

    | [|_;_;"email"|] ->
        let html =
            Email.lowInk "" "Basil Pupken" "Testing" "SKUSHKA"
                "Supa Printa 320" "PrInTeRiD" [str ""] [str ""]
                "PROMOCODE" "Abba"
                |> ReactDomServer.renderToStaticMarkup
                |> (+) "<!DOCTYPE html>\n"

        do fs.writeFileSync ("testEmail.html", html)


    | argv -> printfn "no such command: %A" argv
