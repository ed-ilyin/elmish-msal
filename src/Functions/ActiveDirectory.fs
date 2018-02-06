module ActiveDirectory

open AutomaticInk
open Fable.Core
open Fable.Core.JsInterop
open Fable.EdIlyin.Core
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.Import


let adTokenPromise () =
    Promise.create
        <| fun ok error ->
            Config.context?acquireTokenWithClientCredentials(
                Config.resource,
                Config.applicationId,
                Config.clientSecret,
                fun err (tokenResponse: obj) ->
                match err with
                    | null -> ok tokenResponse
                    | _ -> sprintf "AD token: %A" err |> exn |> error
            ) |> ignore


let tryFetch httpMethod userId =
    promise {
        let! token = adTokenPromise ()

        let url =
            sprintf "%s/%s/users/%s?api-version=1.6"
                !!token?resource
                Config.tenant
                userId

        let headers =
            requestHeaders [
                sprintf "%s %s" !!token?tokenType !!token?accessToken
                    |> Fetch_types.Authorization
            ]

        let! response =
            tryFetch url [
                    RequestProperties.Method httpMethod
                    headers
                ]

        return response
    }


let [<PassGenerics>]tryFetchAs<'T> httpMethod userId =
    promiseResult {
        let! response = tryFetch httpMethod userId
        let! text = response.text() |> Promise.result
        return ofJson<'T> text
    } |> Promise.result |> Promise.map (Result.bind id)


let deleteUser userId = tryFetch HttpMethod.DELETE userId


let getUser userId = tryFetchAs<Types.User> HttpMethod.GET userId
