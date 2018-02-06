module Xml

open Fable.Core
open Fable.Core.JsInterop


module Props = type IProp = interface end


open Props


type XmlElement =
    | String of string
    | XmlElement of (string * IProp list * XmlElement list)



let inline el tag props children = XmlElement (tag, props, children)
let inline voidEl tag props = XmlElement (tag, props, [])
let inline noAttrEl tag children = XmlElement (tag, [], children)
let inline noAttrVoidEl tag = XmlElement (tag, [], [])
let inline str s = String s


let propsToString props  =
    keyValueList CaseRules.None props
        |> inflate<Map<string,string>>
        |> Map.toList
        |> List.map (fun (k, v) -> sprintf " %s=\"%s\"" k v)
        |> String.concat ""


let rec toString = function
    | String s -> s

    | XmlElement (tag, props, []) ->
        sprintf "<%s%s />" tag <| propsToString props

    | XmlElement (tag, props, es) ->
        sprintf "<%s%s>%s</%s>"
            tag
            (propsToString props)
            (List.map toString es
                |> function | [] -> "" | l -> List.reduce (+) l
            )
            tag
