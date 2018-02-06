// ts2fable 0.5.2
module rec Fable.Import.Msal.ClientInfo
open System
open Fable.Core
open Fable.Import.JS


type [<AllowNullLiteral>] IExports =
    abstract ClientInfo: ClientInfoStatic

type [<AllowNullLiteral>] ClientInfo =
    abstract _uid: obj with get, set
    abstract uid: string with get, set
    abstract _utid: obj with get, set
    abstract utid: string with get, set

type [<AllowNullLiteral>] ClientInfoStatic =
    [<Emit "new $0($1...)">] abstract Create: rawClientInfo: string -> ClientInfo
