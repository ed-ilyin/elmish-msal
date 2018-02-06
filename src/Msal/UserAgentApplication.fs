// ts2fable 0.5.2
module rec UserAgentApplication
open System
open Fable.Core
open Fable.Import.JS

type Logger = __Logger.Logger
type User = __User.User

type [<AllowNullLiteral>] IExports =
    abstract UserAgentApplication: UserAgentApplicationStatic

module Global =

    type [<AllowNullLiteral>] Window =
        abstract msal: Object with get, set
        abstract callBackMappedToRenewStates: Object with get, set
        abstract callBacksMappedToRenewStates: Object with get, set

type tokenReceivedCallback =
    (string -> string -> string -> string -> unit)

type [<AllowNullLiteral>] UserAgentApplication =
    abstract _cacheLocations: obj with get, set
    abstract _cacheLocation: obj with get, set
    abstract cacheLocation: string
    abstract _logger: obj with get, set
    abstract _loginInProgress: obj with get, set
    abstract _acquireTokenInProgress: obj with get, set
    abstract _renewStates: obj with get, set
    abstract _activeRenewals: obj with get, set
    abstract _clockSkew: obj with get, set
    abstract _cacheStorage: obj with get, set
    abstract _tokenReceivedCallback: obj with get, set
    abstract _user: obj with get, set
    abstract clientId: string with get, set
    abstract authorityInstance: obj with get, set
    abstract authority: string with get, set
    abstract validateAuthority: bool with get, set
    abstract _redirectUri: obj with get, set
    abstract _postLogoutredirectUri: obj with get, set
    abstract _openedWindows: obj with get, set
    abstract _requestType: obj with get, set
    abstract loadFrameTimeout: float with get, set
    abstract processCallBack: hash: obj -> unit
    abstract loginRedirect: ?scopes: Array<string> * ?extraQueryParameters: string -> unit
    abstract loginPopup: scopes: Array<string> * ?extraQueryParameters: string -> Promise<string>
    abstract promptUser: urlNavigate: obj -> unit
    abstract openWindow: urlNavigate: obj * title: obj * interval: obj * instance: obj * ?resolve: obj * ?reject: obj -> unit
    abstract logout: unit -> unit
    abstract clearCache: unit -> unit
    abstract openPopup: urlNavigate: obj * title: obj * popUpWidth: obj * popUpHeight: obj -> unit
    abstract validateInputScope: scopes: obj -> unit
    abstract filterScopes: scopes: obj -> unit
    abstract registerCallback: expectedState: obj * scope: obj * resolve: obj * reject: obj -> unit
    abstract getCachedToken: authenticationRequest: obj * user: obj -> unit
    abstract getAllUsers: unit -> Array<User>
    abstract getUniqueUsers: users: obj -> unit
    abstract getUniqueAuthority: accessTokenCacheItems: obj * property: obj -> unit
    abstract addHintParameters: urlNavigate: obj * user: obj -> unit
    abstract urlContainsQueryStringParameter: name: obj * url: obj -> unit
    abstract acquireTokenRedirect: scopes: Array<string> -> unit
    abstract acquireTokenRedirect: scopes: Array<string> * authority: string -> unit
    abstract acquireTokenRedirect: scopes: Array<string> * authority: string * user: User -> unit
    abstract acquireTokenRedirect: scopes: Array<string> * authority: string * user: User * extraQueryParameters: string -> unit
    abstract acquireTokenPopup: scopes: Array<string> -> Promise<string>
    abstract acquireTokenPopup: scopes: Array<string> * authority: string -> Promise<string>
    abstract acquireTokenPopup: scopes: Array<string> * authority: string * user: User -> Promise<string>
    abstract acquireTokenPopup: scopes: Array<string> * authority: string * user: User * extraQueryParameters: string -> Promise<string>
    abstract acquireTokenSilent: scopes: Array<string> * ?authority: string * ?user: User * ?extraQueryParameters: string -> Promise<string>
    abstract loadIframeTimeout: urlNavigate: obj * frameName: obj * scope: obj -> unit
    abstract loadFrame: urlNavigate: obj * frameName: obj -> unit
    abstract addAdalFrame: iframeId: obj -> unit
    abstract renewToken: scopes: obj * resolve: obj * reject: obj * user: obj * authenticationRequest: obj * ?extraQueryParameters: obj -> unit
    abstract renewIdToken: scopes: obj * resolve: obj * reject: obj * user: obj * authenticationRequest: obj * ?extraQueryParameters: obj -> unit
    abstract getUser: unit -> User
    abstract handleAuthenticationResponse: hash: obj -> unit
    abstract saveAccessToken: authority: obj * tokenResponse: obj * user: obj * clientInfo: obj * idToken: obj -> unit
    abstract saveTokenFromHash: tokenResponse: obj -> unit
    abstract isCallback: hash: string -> bool
    abstract getHash: hash: obj -> unit
    abstract getRequestInfo: hash: obj -> unit
    abstract getScopeFromState: state: obj -> unit
    abstract isInIframe: unit -> unit

type [<AllowNullLiteral>] UserAgentApplicationStatic =
    [<Emit "new $0($1...)">] abstract Create: clientId: string * authority: string option * tokenReceivedCallback: tokenReceivedCallback * ?options: UserAgentApplicationStaticOptions -> UserAgentApplication

type [<AllowNullLiteral>] UserAgentApplicationStaticOptions =
    abstract validateAuthority: bool option with get, set
    abstract cacheLocation: string option with get, set
    abstract redirectUri: string option with get, set
    abstract postLogoutRedirectUri: string option with get, set
    abstract logger: Logger option with get, set
    abstract loadFrameTimeout: float option with get, set
