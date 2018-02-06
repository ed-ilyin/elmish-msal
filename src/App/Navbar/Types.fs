module Navbar.Types


type Model = {
    signInOrSignupAnchorDisabled: bool
    user: string option
}


type Msg =
    | NoOp
    | Failure of System.Exception
    | SignInOrSignup
    | UpdateUser of string option
