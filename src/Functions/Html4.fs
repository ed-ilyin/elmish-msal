module Html4

open Fable.Helpers.React


type Html4Prop =
    | Border of string
    | Width of string
    | Height of string
    interface Props.IHTMLProp


let center b c = domEl "center" b c
