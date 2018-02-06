module Home.Types

open Library

type Model =
    {   registerPrinterButtonDisabled: bool
    }


type Msg =
    | Failure of System.Exception
    | RegisterPrinter
    | GoToAccountPage
