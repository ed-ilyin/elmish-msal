// ts2fable 0.5.2
module rec Fable.Import.Msal.Msal

open Fable.Core


type UserAgentApplication = UserAgentApplication.UserAgentApplication
type User = User.User


[<Import("UserAgentApplication","msal")>]
let userAgentApplication: UserAgentApplication.UserAgentApplicationStatic =
    jsNative
