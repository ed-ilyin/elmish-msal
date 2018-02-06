module App.Types

open Global


type Msg =
    | AccountMsg of Account.Types.Msg
    | Failure of System.Exception
    | HomeMsg of Home.Types.Msg
    | NavbarMsg of Navbar.Types.Msg
    | NoOp
    | UpdateUser of string option


type Model =
    {   account: Account.Types.Model
        currentPage: Page
        home: Home.Types.Model
        navbar: Navbar.Types.Model
        user: string option
        log: string
    }
