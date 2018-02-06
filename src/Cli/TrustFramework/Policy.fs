module TrustFramework.Policy

open Xml
open Xml.Props


type Attribute =
    | ``Xmlns:xsd`` of string
    | ``Xmlns:xsi`` of string
    | ClaimTypeReferenceId of string
    | HelpText of string
    | Id of string
    | Name of string
    | PartnerClaimType of string
    | PolicyId of string
    | PolicySchemaVersion of string
    | PublicPolicyUri of string
    | RegularExpression of string
    | TenantId of string
    | TransformationClaimType of string
    | TransformationMethod of string
    | Xmlns of string
    | Key of string
    | Required of string
    | DefaultValue of string
    | Handler of string
    | StorageReferenceId of string
    | ReferenceId of string
    | Order of string
    | Type of string
    | ContentDefinitionReferenceId of string
    | ValidationClaimsExchangeId of string
    | TechnicalProfileReferenceId of string
    | ExecuteActionsIf of string
    | CpimIssuerTechnicalProfileReferenceId of string
    | TargetClaimsExchangeId of string
    interface IProp


let adminHelpText s = noAttrEl "AdminHelpText" [ str s ]
let buildingBlocks c = noAttrEl "BuildingBlocks" c
let claimsProvider c = noAttrEl "ClaimsProvider" c
let claimsProviders c = noAttrEl "ClaimsProviders" c
let claimType id c = el "ClaimType" [ Id id ] c
let claimsTransformation b c = el "ClaimsTransformation" b c
let clientDefinition b c = el "ClientDefinition" b c
let clientDefinitions c = noAttrEl "ClientDefinitions" c
let clientUIFilterFlags s = noAttrEl "ClientUIFilterFlags" [ str s ]
let contentDefinition id c = el "ContentDefinition" [ Id id ] c
let contentDefinitions c = noAttrEl "ContentDefinitions" c
let cryptographicKeys c = noAttrEl "CryptographicKeys" c
let dataType s = noAttrEl "DataType" [ str s ]
let dataUri s = noAttrEl "DataUri" [ str s ]
let defaultPartnerClaimTypes c = noAttrEl "DefaultPartnerClaimTypes" c
let displayName s = noAttrEl "DisplayName" [ str s ]
let includeInSso s = noAttrEl "IncludeInSso" [ str s ]
let inputClaim b = voidEl "InputClaim" b
let inputClaims bs = List.map inputClaim bs |> noAttrEl "InputClaims"
let item key s = el "Item" [ Key key ] [ str s ]
let key id storageReferenceId = voidEl "Key" [ Id id; StorageReferenceId storageReferenceId]
let loadUri s = noAttrEl "LoadUri" [ str s ]
let metadata kvs = List.map (fun (k, v) -> item k v) kvs |> noAttrEl "Metadata"
let outputClaim b = voidEl "OutputClaim" b
let outputClaims bs = List.map outputClaim bs |> noAttrEl "OutputClaims"
let persistedClaim b = voidEl "PersistedClaim" b
let persistedClaims bs = List.map persistedClaim bs |> noAttrEl "PersistedClaims"
let protocol b = voidEl "Protocol" b
let recoveryUri s = noAttrEl "RecoveryUri" [ str s ]
let restriction c = noAttrEl "Restriction" c
let technicalProfile id c = el "TechnicalProfile" [ Id id ] c
let technicalProfiles c = noAttrEl "TechnicalProfiles" c
let validationTechnicalProfiles c = noAttrEl "ValidationTechnicalProfiles" c
let trustFrameworkPolicy b c = el "TrustFrameworkPolicy" b c
let userHelpText c = noAttrEl "UserHelpText" c
let userInputType s = noAttrEl "UserInputType" [ str s ]
let outputTokenFormat s = noAttrEl "OutputTokenFormat" [ str s ]


let claimsSchema idscs =
    List.map (fun (id, c) -> claimType id c) idscs
        |> noAttrEl "ClaimsSchema"


let claimsTransformations bs =
    List.map (fun (b, c) -> claimsTransformation b c) bs
        |> noAttrEl "ClaimsTransformations"


let useTechnicalProfileForSessionManagement referenceId =
    voidEl "UseTechnicalProfileForSessionManagement"
        [ ReferenceId referenceId ]


let validationTechnicalProfile referenceId =
    voidEl "IncludeTechnicalProfile" [ ReferenceId referenceId ]


let includeTechnicalProfile referenceId =
    voidEl "IncludeTechnicalProfile" [ ReferenceId referenceId ]


let pattern regularExpression helpText =
    voidEl "Pattern"
        [ RegularExpression regularExpression; HelpText helpText ]


let orchestrationStep b c = el "OrchestrationStep" b c


let orchestrationSteps b c = el "OrchestrationSteps" b c


let userJourney b c = el "UserJourney" b c


let userJourneys b c = el "UserJourneys" b c


let claimsProviderSelections b c = el "ClaimsProviderSelections" b c


let claimsProviderSelection b c = el "ClaimsProviderSelection" b c

let claimsExchanges b c = el "ClaimsExchanges" b c
let claimsExchange b c = el "ClaimsExchange" b c
let preconditions b c = el "Preconditions" b c
let precondition b c = el "Precondition" b c
let value b c = el "Value" b c
let action b c = el "Action" b c
