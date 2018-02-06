module Account.Types


open Library.Types


type Model =
    {   printers: Printer list
        accessToken: string option
        addPrinterSerial: string
        addPrinterCountry: string
        addPrinterButtonDisabled: bool
        error: string option
        success: string option
        brandId: string
        unsubscribeDisabled: bool
    }


type AccessToken = string


type ErrorMessage = string


type Msg =
    | Failure of System.Exception
    | GetTokenSilent
    | GetTokenRedirect of ErrorMessage
    | SaveTokenAndGetPrinters of AccessToken
    | UpdatePrinters of Printer list
    | UpdatePrinterSerial of string
    | UpdatePrinterCountry of string
    | AddPrinter
    | UnregisterPrinter of string
    | ShowSuccess of string
    | UpdateBrandId of string
    | ToggleUnsubscribe
    | Unsubscribe


type AzureFuncErrorMessage = {
    userMessage: string
}
