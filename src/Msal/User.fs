// ts2fable 0.5.2
module rec Fable.Import.Msal.User
open System
open Fable.Core
open Fable.Import.JS

type ClientInfo = ClientInfo.ClientInfo
type IdToken = IdToken.IdToken

type [<AllowNullLiteral>] IExports =
    abstract User: UserStatic

type [<AllowNullLiteral>] User =
    abstract displayableId: string with get, set
    abstract name: string with get, set
    abstract identityProvider: string with get, set
    abstract userIdentifier: string with get, set
    abstract idToken: Object with get, set

type [<AllowNullLiteral>] UserStatic =
    [<Emit "new $0($1...)">] abstract Create: displayableId: string * name: string * identityProvider: string * userIdentifier: string * idToken: Object -> User
    abstract createUser: idToken: IdToken * clientInfo: ClientInfo * authority: string -> User
