module TrustFramework.Base

open Policy
open Xml


let policy tenant =
    trustFrameworkPolicy [
        ``Xmlns:xsi`` "http://www.w3.org/2001/XMLSchema-instance"
        ``Xmlns:xsd`` "http://www.w3.org/2001/XMLSchema"
        Xmlns "http://schemas.microsoft.com/online/cpim/schemas/2013/06"
        PolicySchemaVersion "0.3.0.0"
        tenant + ".onmicrosoft.com" |> TenantId
        PolicyId "B2C_1A_TrustFrameworkBase"
        sprintf
            "http://%s.onmicrosoft.com/B2C_1A_TrustFrameworkBase"
            tenant
            |> PublicPolicyUri
    ] [
        buildingBlocks [
            claimsSchema [
                "socialIdpUserId", [
                    displayName "Username"
                    dataType "string"
                    userHelpText []
                    userInputType "TextBox"
                    restriction [
                        pattern
                            "^[a-zA-Z0-9]+[a-zA-Z0-9_-]*$"
                            "The username you provided is not valid. It must begin with an alphabet or number and can contain alphabets, numbers and the following symbols: _ -"
                    ]
                ]
                "tenantId", [
                    displayName "User's Object's Tenant ID"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "tid"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "tid"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.microsoft.com/identity/claims/tenantid"
                        ]
                    ]
                    userHelpText [
                        str "Tenant identifier (ID) of the user object in Azure AD."
                    ]
                ]
                "objectId", [
                    displayName "User's Object ID"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "oid"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "oid"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.microsoft.com/identity/claims/objectidentifier"
                        ]
                    ]
                    userHelpText [
                        str "Object identifier (ID) of the user object in Azure AD."
                    ]
                ]
                "signInName", [
                    displayName "Sign in name"
                    dataType "string"
                    userHelpText []
                    userInputType "TextBox"
                ]
                "signInNames.emailAddress", [
                    displayName "Email Address"
                    dataType "string"
                    userHelpText [
                        str "Email address to use for signing in."
                    ]
                    userInputType "TextBox"
                ]
                "password", [
                    displayName "Password"
                    dataType "string"
                    userHelpText [
                        str "Enter password"
                    ]
                    userInputType "Password"
                ]
                "newPassword", [
                    displayName "New Password"
                    dataType "string"
                    userHelpText [
                        str "Enter new password"
                    ]
                    userInputType "Password"
                    restriction [
                        pattern
                            """^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)|(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])|(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9])|(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]))([A-Za-z\d@#$%^&*\-_+=[\]{}|\\:',?/`~"();!]|\.(?!@)){8,16}$"""
                            """8-16 characters, containing 3 out of 4 of the following: Lowercase characters, uppercase characters, digits (0-9), and one or more of the following symbols: @ # $ % ^ & * - _ + = [ ] { } | \ : ' , ? / ` ~ " ( ) ; ."""
                    ]
                ]
                "reenterPassword", [
                    displayName "Confirm New Password"
                    dataType "string"
                    userHelpText [
                        str "Confirm new password"
                    ]
                    userInputType "Password"
                    restriction [
                        pattern
                            """^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)|(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])|(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9])|(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]))([A-Za-z\d@#$%^&*\-_+=[\]{}|\\:',?/`~"();!]|\.(?!@)){8,16}$"""
                            " "
                    ]
                ]
                "passwordPolicies", [
                    displayName "Password Policies"
                    dataType "string"
                    userHelpText [
                        str "Password policies used by Azure AD to determine password strength, expiry etc."
                    ]
                ]
                "client_id", [
                    displayName "client_id"
                    dataType "string"
                    adminHelpText "Special parameter passed to EvoSTS."
                    userHelpText [
                        str "Special parameter passed to EvoSTS."
                    ]
                ]
                "resource_id", [
                    displayName "resource_id"
                    dataType "string"
                    adminHelpText "Special parameter passed to EvoSTS."
                    userHelpText [
                        str "Special parameter passed to EvoSTS."
                    ]
                ]
                "sub", [
                    displayName "Subject"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "sub"
                        ]
                    ]
                    userHelpText []
                ]
                "identityProvider", [
                    displayName "Identity Provider"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "idp"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "idp"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.microsoft.com/identity/claims/identityprovider"
                        ]
                    ]
                    userHelpText []
                ]
                "displayName", [
                    displayName "Display Name"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "unique_name"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "name"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                        ]
                    ]
                    userHelpText [ str "Your display name." ]
                    userInputType "TextBox"
                ]
                "email", [
                    displayName "Email Address"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "email"
                        ]
                    ]
                    userHelpText [
                        str "Email address that can be used to contact you."
                    ]
                    userInputType "TextBox"
                    restriction [
                        pattern
                            @"^[a-zA-Z0-9.!#$%&'^_`{}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$"
                            "Please enter a valid email address."
                    ]
                ]
                "otherMails", [
                    displayName "Alternate Email Addresses"
                    dataType "stringCollection"
                    userHelpText [
                        str "Email addresses that can be used to contact the user."
                    ]
                ]
                "userPrincipalName", [
                    displayName "UserPrincipalName"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "upn"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "upn"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.microsoft.com/identity/claims/userprincipalname"
                        ]
                    ]
                    userHelpText [
                        str "Your user name as stored in the Azure Active Directory."
                    ]
                ]
                "upnUserName", [
                    displayName "UPN User Name"
                    dataType "string"
                    userHelpText [
                        str "The user name for creating user principal name."
                    ]
                ]
                "newUser", [
                    displayName "User is new"
                    dataType "boolean"
                    userHelpText []
                ]
                "executed-SelfAsserted-Input", [
                    displayName "Executed-SelfAsserted-Input"
                    dataType "string"
                    userHelpText [
                        str "A claim that specifies whether attributes were collected from the user."
                    ]
                ]
                "authenticationSource", [
                    displayName "AuthenticationSource"
                    dataType "string"
                    userHelpText [
                        str "Specifies whether the user was authenticated at Social IDP or local account."
                    ]
                ]
                "nca", [
                    displayName "nca"
                    dataType "string"
                    userHelpText [
                        str "Special parameter passed for local account authentication to login.microsoftonline.com."
                    ]
                ]
                "grant_type", [
                    displayName "grant_type"
                    dataType "string"
                    userHelpText [
                        str "Special parameter passed for local account authentication to login.microsoftonline.com."
                    ]
                ]
                "scope", [
                    displayName "scope"
                    dataType "string"
                    userHelpText [
                        str "Special parameter passed for local account authentication to login.microsoftonline.com."
                    ]
                ]
                "objectIdFromSession", [
                    displayName "objectIdFromSession"
                    dataType "boolean"
                    userHelpText [
                        str "Parameter provided by the default session management provider to indicate that the object id has been retrieved from an SSO session."
                    ]
                ]
                "isActiveMFASession", [
                    displayName "isActiveMFASession"
                    dataType "boolean"
                    userHelpText [
                        str "Parameter provided by the MFA session management to indicate that the user has an active MFA session."
                    ]
                ]
                "givenName", [
                    displayName "Given Name"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "given_name"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "given_name"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"
                        ]
                    ]
                    userHelpText [
                        str "Your given name (also known as first name)."
                    ]
                    userInputType "TextBox"
                ]
                "surname", [
                    displayName "Surname"
                    dataType "string"
                    defaultPartnerClaimTypes [
                        protocol [
                            Name "OAuth2"
                            PartnerClaimType "family_name"
                        ]
                        protocol [
                            Name "OpenIdConnect"
                            PartnerClaimType "family_name"
                        ]
                        protocol [
                            Name "SAML2"
                            PartnerClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"
                        ]
                    ]
                    userHelpText [
                        str "Your surname (also known as family name or last name)."
                    ]
                    userInputType "TextBox"
                ]
            ]
            claimsTransformations [
                [   Id "CreateOtherMailsFromEmail"
                    TransformationMethod "AddItemToStringCollection"
                ], [
                    inputClaims [
                        [ ClaimTypeReferenceId "email"; TransformationClaimType "item" ]
                        [ ClaimTypeReferenceId "otherMails"; TransformationClaimType "collection" ]
                    ]
                    outputClaims [
                        [ ClaimTypeReferenceId "otherMails"; TransformationClaimType "collection" ]
                    ]
                ]
            ]
            clientDefinitions [
                clientDefinition [ Id "DefaultWeb" ] [
                    clientUIFilterFlags "LineMarkers, MetaRefresh"
                ]
            ]
            contentDefinitions [
                contentDefinition "api.error" [
                    loadUri "~/tenant/default/exception.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:globalexception:1.1.0"
                    metadata [ "DisplayName", "Error page" ]
                ]
                contentDefinition "api.idpselections" [
                    loadUri "~/tenant/default/idpSelector.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:idpselection:1.0.0"
                    metadata [
                        "DisplayName", "Idp selection page"
                        "language.intro", "Sign in"
                    ]
                ]
                contentDefinition "api.idpselections.signup" [
                    loadUri "~/tenant/default/idpSelector.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:idpselection:1.0.0"
                    metadata [
                        "DisplayName", "Idp selection page"
                        "language.intro", "Sign up"
                    ]
                ]
                contentDefinition "api.signuporsignin" [
                    loadUri "~/tenant/default/unified.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:unifiedssp:1.0.0"
                    metadata [
                        "DisplayName", "Signin and Signup"
                    ]
                ]
                contentDefinition "api.selfasserted" [
                    loadUri "~/tenant/default/selfAsserted.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:selfasserted:1.1.0"
                    metadata [
                        "DisplayName", "Collect information from user page"
                    ]
                ]
                contentDefinition "api.selfasserted.profileupdate" [
                    loadUri "~/tenant/default/updateProfile.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:selfasserted:1.1.0"
                    metadata [
                        "DisplayName", "Collect information from user page"
                    ]
                ]
                contentDefinition "api.localaccountsignup" [
                    loadUri "~/tenant/default/selfAsserted.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:selfasserted:1.1.0"
                    metadata [
                        "DisplayName", "Local account sign up page"
                    ]
                ]
                contentDefinition "api.localaccountpasswordreset" [
                    loadUri "~/tenant/default/selfAsserted.cshtml"
                    recoveryUri "~/common/default_page_error.html"
                    dataUri "urn:com:microsoft:aad:b2c:elements:selfasserted:1.1.0"
                    metadata [
                        "DisplayName", "Local account change password page"
                    ]
                ]
            ]
        ]
        claimsProviders [
            claimsProvider [
                displayName "Local Account SignIn"
                technicalProfiles [
                    technicalProfile "login-NonInteractive" [
                        displayName "Local Account SignIn"
                        protocol [ Name "OpenIdConnect" ]
                        metadata [
                            "UserMessageIfClaimsPrincipalDoesNotExist", "We can't seem to find your account"
                            "UserMessageIfInvalidPassword", "Your password is incorrect"
                            "UserMessageIfOldPasswordUsed", "Looks like you used an old password"
                            "ProviderName", "https://sts.windows.net/"
                            "METADATA", "https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration"
                            "authorization_endpoint", "https://login.microsoftonline.com/{tenant}/oauth2/token"
                            "response_types", "id_token"
                            "response_mode", "query"
                            "scope", "email openid"
                            "UsePolicyInRedirectUri", "false"
                            "HttpBinding", "POST"
                        ]
                        inputClaims [
                            [ ClaimTypeReferenceId "signInName"; PartnerClaimType "username"; Required "true" ]
                            [ ClaimTypeReferenceId "password"; Required "true" ]
                            [ ClaimTypeReferenceId "grant_type"; DefaultValue "password" ]
                            [ ClaimTypeReferenceId "scope"; DefaultValue "openid" ]
                            [ ClaimTypeReferenceId "nca"; PartnerClaimType "nca"; DefaultValue "1" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "objectId"; PartnerClaimType "oid" ]
                            [ ClaimTypeReferenceId "tenantId"; PartnerClaimType "tid" ]
                            [ ClaimTypeReferenceId "givenName"; PartnerClaimType "given_name" ]
                            [ ClaimTypeReferenceId "surName"; PartnerClaimType "family_name" ]
                            [ ClaimTypeReferenceId "displayName"; PartnerClaimType "name" ]
                            [ ClaimTypeReferenceId "userPrincipalName"; PartnerClaimType "upn" ]
                            [ ClaimTypeReferenceId "authenticationSource"; DefaultValue "localAccountAuthentication" ]
                        ]
                    ]
                ]
            ]
            claimsProvider [
                displayName "Azure Active Directory"
                technicalProfiles [
                    technicalProfile "AAD-Common" [
                        displayName "Azure Active Directory"
                        protocol [
                            Name "Proprietary"
                            Handler "Web.TPEngine.Providers.AzureActiveDirectoryProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        ]
                        cryptographicKeys [
                            key "issuer_secret" "B2C_1A_TokenSigningKeyContainer"
                        ]
                        includeInSso "false"
                        useTechnicalProfileForSessionManagement "SM-Noop"
                    ]
                    technicalProfile "AAD-UserWriteUsingLogonEmail" [
                        metadata [
                            "Operation", "Write"
                            "RaiseErrorIfClaimsPrincipalAlreadyExists", "true"
                        ]
                        includeInSso "false"
                        inputClaims [
                            [ ClaimTypeReferenceId "email"; PartnerClaimType "signInNames.emailAddress"; Required "true" ]
                        ]
                        persistedClaims [
                            [ ClaimTypeReferenceId "email"; PartnerClaimType "signInNames.emailAddress" ]
                            [ ClaimTypeReferenceId "newPassword"; PartnerClaimType "password" ]
                            [ ClaimTypeReferenceId "displayName"; DefaultValue "unknown" ]
                            [ ClaimTypeReferenceId "passwordPolicies"; DefaultValue "DisablePasswordExpiration" ]
                            [ ClaimTypeReferenceId "givenName" ]
                            [ ClaimTypeReferenceId "surname" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "newUser"; PartnerClaimType "newClaimsPrincipalCreated" ]
                            [ ClaimTypeReferenceId "authenticationSource"; DefaultValue "localAccountAuthentication" ]
                            [ ClaimTypeReferenceId "userPrincipalName" ]
                            [ ClaimTypeReferenceId "signInNames.emailAddress" ]
                        ]
                        includeTechnicalProfile "AAD-Common"
                        useTechnicalProfileForSessionManagement "SM-AAD"
                    ]
                    technicalProfile "AAD-UserReadUsingEmailAddress" [
                        metadata [
                            "Operation", "Read"
                            "RaiseErrorIfClaimsPrincipalDoesNotExist", "true"
                            "UserMessageIfClaimsPrincipalDoesNotExist", "An account could not be found for the provided user ID."
                        ]
                        includeInSso "false"
                        inputClaims [
                            [ ClaimTypeReferenceId "email"; PartnerClaimType "signInNames"; Required "true" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "authenticationSource"; DefaultValue "localAccountAuthentication" ]
                            [ ClaimTypeReferenceId "userPrincipalName" ]
                            [ ClaimTypeReferenceId "displayName" ]
                            [ ClaimTypeReferenceId "otherMails" ]
                            [ ClaimTypeReferenceId "signInNames.emailAddress" ]
                        ]
                        includeTechnicalProfile "AAD-Common"
                    ]
                    technicalProfile "AAD-UserWritePasswordUsingObjectId" [
                        metadata [
                            "Operation", "Write"
                            "RaiseErrorIfClaimsPrincipalDoesNotExist", "true"
                        ]
                        includeInSso "false"
                        inputClaims [
                            [ ClaimTypeReferenceId "objectId"; Required "true" ]
                        ]
                        persistedClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "newPassword"; PartnerClaimType "password" ]
                        ]
                        includeTechnicalProfile "AAD-Common"
                    ]
                    technicalProfile "AAD-UserWriteProfileUsingObjectId" [
                        metadata [
                            "Operation", "Write"
                            "RaiseErrorIfClaimsPrincipalAlreadyExists", "false"
                            "RaiseErrorIfClaimsPrincipalDoesNotExist", "true"
                        ]
                        includeInSso "false"
                        inputClaims [
                            [ ClaimTypeReferenceId "objectId"; Required "true" ]
                        ]
                        persistedClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "givenName" ]
                            [ ClaimTypeReferenceId "surname" ]
                        ]
                        includeTechnicalProfile "AAD-Common"
                    ]
                    technicalProfile "AAD-UserReadUsingObjectId" [
                        metadata [
                            "Operation", "Read"
                            "RaiseErrorIfClaimsPrincipalDoesNotExist", "true"
                        ]
                        includeInSso "false"
                        inputClaims [
                            [ ClaimTypeReferenceId "objectId"; Required "true" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "signInNames.emailAddress" ]
                            [ ClaimTypeReferenceId "displayName" ]
                            [ ClaimTypeReferenceId "otherMails" ]
                            [ ClaimTypeReferenceId "givenName" ]
                            [ ClaimTypeReferenceId "surname" ]
                        ]
                        includeTechnicalProfile "AAD-Common"
                    ]
                ]
            ]
            claimsProvider [
                displayName "Self Asserted"
                technicalProfiles [
                    technicalProfile "SelfAsserted-ProfileUpdate" [
                        displayName "User ID signup"
                        protocol [ Name "Proprietary"; Handler "Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" ]
                        metadata [
                            "ContentDefinitionReferenceId", "api.selfasserted.profileupdate"
                        ]
                        includeInSso "false"
                        inputClaims [
                            [ ClaimTypeReferenceId "userPrincipalName" ]
                            [ ClaimTypeReferenceId "givenName" ]
                            [ ClaimTypeReferenceId "surname" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "executed-SelfAsserted-Input"; DefaultValue "true" ]
                            [ ClaimTypeReferenceId "givenName" ]
                            [ ClaimTypeReferenceId "surname" ]
                        ]
                        validationTechnicalProfiles [
                            validationTechnicalProfile "AAD-UserWriteProfileUsingObjectId"
                        ]
                    ]
                ]
            ]
            claimsProvider [
                displayName "Local Account"
                technicalProfiles [
                    technicalProfile "LocalAccountSignUpWithLogonEmail" [
                        displayName "Email signup"
                        protocol [ Name "Proprietary"; Handler "Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" ]
                        metadata [
                            "IpAddressClaimReferenceId", "IpAddress"
                            "ContentDefinitionReferenceId", "api.localaccountsignup"
                            "language.button_continue", "Create"
                        ]
                        cryptographicKeys [ key "issuer_secret" "B2C_1A_TokenSigningKeyContainer" ]
                        inputClaims [ [ ClaimTypeReferenceId "email" ] ]
                        outputClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "email"; PartnerClaimType "Verified.Email"; Required "true" ]
                            [ ClaimTypeReferenceId "newPassword"; Required "true" ]
                            [ ClaimTypeReferenceId "reenterPassword"; Required "true" ]
                            [ ClaimTypeReferenceId "executed-SelfAsserted-Input"; DefaultValue "true" ]
                            [ ClaimTypeReferenceId "authenticationSource" ]
                            [ ClaimTypeReferenceId "newUser" ]
                            [ ClaimTypeReferenceId "givenName" ]
                            [ ClaimTypeReferenceId "surName" ]
                        ]
                        useTechnicalProfileForSessionManagement "SM-AAD"
                    ]
                    technicalProfile "SelfAsserted-LocalAccountSignin-Email" [
                        displayName "Local Account Signin"
                        protocol [ Name "Proprietary"; Handler "Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" ]
                        metadata [
                            "SignUpTarget", "SignUpWithLogonEmailExchange"
                            "setting.operatingMode", "Email"
                            "ContentDefinitionReferenceId", "api.selfasserted"
                        ]
                        includeInSso "false"
                        inputClaims [ [ ClaimTypeReferenceId "signInName" ] ]
                        outputClaims [
                            [ ClaimTypeReferenceId "signInName"; Required "true" ]
                            [ ClaimTypeReferenceId "password"; Required "true" ]
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "authenticationSource" ]
                        ]
                        validationTechnicalProfiles [
                            validationTechnicalProfile "login-NonInteractive"
                        ]
                        useTechnicalProfileForSessionManagement "SM-AAD"
                    ]
                    technicalProfile "LocalAccountDiscoveryUsingEmailAddress" [
                        displayName "Reset password using email address"
                        protocol [ Name "Proprietary"; Handler "Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" ]
                        metadata [
                            "IpAddressClaimReferenceId", "IpAddress"
                            "ContentDefinitionReferenceId", "api.localaccountpasswordreset"
                        ]
                        cryptographicKeys [
                            key "issuer_secret" "B2C_1A_TokenSigningKeyContainer"
                        ]
                        includeInSso "false"
                        outputClaims [
                            [ ClaimTypeReferenceId "email"; PartnerClaimType "Verified.Email"; Required "true" ]
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "userPrincipalName" ]
                            [ ClaimTypeReferenceId "authenticationSource" ]
                        ]
                        validationTechnicalProfiles [
                            validationTechnicalProfile "AAD-UserReadUsingEmailAddress"
                        ]
                    ]
                    technicalProfile "LocalAccountWritePasswordUsingObjectId" [
                        displayName "Change password (username)"
                        protocol [ Name "Proprietary"; Handler "Web.TPEngine.Providers.SelfAssertedAttributeProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" ]
                        metadata [
                            "ContentDefinitionReferenceId", "api.localaccountpasswordreset"
                        ]
                        cryptographicKeys [
                            key "issuer_secret" "B2C_1A_TokenSigningKeyContainer"
                        ]
                        inputClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "newPassword"; Required "true" ]
                            [ ClaimTypeReferenceId "reenterPassword"; Required "true" ]
                        ]
                        validationTechnicalProfiles [
                            validationTechnicalProfile "AAD-UserWritePasswordUsingObjectId"
                        ]
                    ]
                ]
            ]
            claimsProvider [
                displayName "Session Management"
                technicalProfiles [
                    technicalProfile "SM-Noop" [
                        displayName "Noop Session Management Provider"
                        protocol [
                            Name "Proprietary"
                            Handler "Web.TPEngine.SSO.NoopSSOSessionProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        ]
                    ]
                    technicalProfile "SM-AAD" [
                        displayName "Session Mananagement Provider"
                        protocol [
                            Name "Proprietary"
                            Handler "Web.TPEngine.SSO.DefaultSSOSessionProvider, Web.TPEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                        ]
                        persistedClaims [
                            [ ClaimTypeReferenceId "objectId" ]
                            [ ClaimTypeReferenceId "signInName" ]
                            [ ClaimTypeReferenceId "authenticationSource" ]
                            [ ClaimTypeReferenceId "identityProvider" ]
                            [ ClaimTypeReferenceId "newUser" ]
                            [ ClaimTypeReferenceId "executed-SelfAsserted-Input" ]
                        ]
                        outputClaims [
                            [ ClaimTypeReferenceId "objectIdFromSession"; DefaultValue "true" ]
                        ]
                    ]
                ]
            ]
            claimsProvider [
                displayName "Trustframework Policy Engine TechnicalProfiles"
                technicalProfiles [
                    technicalProfile "TpEngine_c3bd4fe2-1775-4013-b91d-35f16d377d13" [
                        displayName "Trustframework Policy Engine Default Technical Profile"
                        protocol [ Name "None" ]
                        metadata [ "url", "{service:te}" ]
                    ]
                ]
            ]
            claimsProvider [
                displayName "Token Issuer"
                technicalProfiles [
                    technicalProfile "JwtIssuer" [
                        displayName "JWT Issuer"
                        protocol [ Name "None" ]
                        outputTokenFormat "JWT"
                        metadata [
                            "client_id", "{service:te}"
                            "issuer_refresh_token_user_identity_claim_type", "objectId"
                            "SendTokenResponseBodyWithJsonNumbers", "true"
                        ]
                        cryptographicKeys [
                            key "issuer_secret" "B2C_1A_TokenSigningKeyContainer"
                        ]
                        inputClaims []
                        outputClaims []
                    ]
                ]
            ]
        ]
        userJourneys [] [
            userJourney [ Id "SignUpOrSignIn" ] [
                orchestrationSteps [] [
                    orchestrationStep [ Order "1"; Type "CombinedSignInAndSignUp"; ContentDefinitionReferenceId "api.signuporsignin" ] [
                        claimsProviderSelections [] [
                            claimsProviderSelection [ ValidationClaimsExchangeId "LocalAccountSigninEmailExchange" ] []
                        ]
                        claimsExchanges [] [
                            claimsExchange [ Id "LocalAccountSigninEmailExchange"; TechnicalProfileReferenceId "SelfAsserted-LocalAccountSignin-Email" ] []
                        ]
                    ]
                    orchestrationStep [ Order "2"; Type "ClaimsExchange" ] [
                        preconditions [] [
                            precondition [ Type "ClaimsExist"; ExecuteActionsIf "true" ] [
                                value [] [ str "objectId" ]
                                action [] [
                                    str "SkipThisOrchestrationStep"
                                ]
                            ]
                        ]
                        claimsExchanges [] [
                            claimsExchange [ Id "SignUpWithLogonEmailExchange"; TechnicalProfileReferenceId "LocalAccountSignUpWithLogonEmail" ] []
                        ]
                    ]
                    orchestrationStep [ Order "3"; Type "ClaimsExchange" ] [
                        claimsExchanges [] [
                            claimsExchange [ Id "AADUserReadWithObjectId"; TechnicalProfileReferenceId "AAD-UserReadUsingObjectId" ] []
                        ]
                    ]
                    orchestrationStep [ Order "4"; Type "SendClaims"; CpimIssuerTechnicalProfileReferenceId "JwtIssuer" ] []
                ]
                clientDefinition [ ReferenceId "DefaultWeb" ] [
                    ]
            ]
            userJourney [ Id "ProfileEdit" ] [
                orchestrationSteps [] [
                    orchestrationStep [ Order "1"; Type "ClaimsProviderSelection"; ContentDefinitionReferenceId "api.idpselections" ] [
                        claimsProviderSelections [] [
                            claimsProviderSelection [ TargetClaimsExchangeId "LocalAccountSigninEmailExchange" ] []
                        ]
                    ]
                    orchestrationStep [ Order "2"; Type "ClaimsExchange" ] [
                        claimsExchanges [] [
                            claimsExchange [ Id "LocalAccountSigninEmailExchange"; TechnicalProfileReferenceId "SelfAsserted-LocalAccountSignin-Email" ] []
                        ]
                    ]
                    orchestrationStep [ Order "3"; Type "ClaimsExchange" ] [
                        claimsExchanges [] [
                            claimsExchange [ Id "AADUserReadWithObjectId"; TechnicalProfileReferenceId "AAD-UserReadUsingObjectId" ] []
                        ]
                    ]
                    orchestrationStep [ Order "4"; Type "ClaimsExchange" ] [
                        claimsExchanges [] [
                            claimsExchange [ Id "B2CUserProfileUpdateExchange"; TechnicalProfileReferenceId "SelfAsserted-ProfileUpdate" ] []
                        ]
                    ]
                    orchestrationStep [ Order "5"; Type "SendClaims"; CpimIssuerTechnicalProfileReferenceId "JwtIssuer" ] []
                ]
                clientDefinition [ ReferenceId "DefaultWeb" ] []
            ]
            userJourney [ Id "PasswordReset" ] [
                orchestrationSteps [] [
                    orchestrationStep [ Order "1"; Type "ClaimsExchange" ] [
                        claimsExchanges [] [
                            claimsExchange [ Id "PasswordResetUsingEmailAddressExchange"; TechnicalProfileReferenceId "LocalAccountDiscoveryUsingEmailAddress" ] []
                        ]
                    ]
                    orchestrationStep [ Order "2"; Type "ClaimsExchange" ] [
                        claimsExchanges [] [
                            claimsExchange [ Id "NewCredentials"; TechnicalProfileReferenceId "LocalAccountWritePasswordUsingObjectId" ] []
                        ]
                    ]
                    orchestrationStep [ Order "3"; Type "SendClaims"; CpimIssuerTechnicalProfileReferenceId "JwtIssuer" ] []
                ]
                clientDefinition [ ReferenceId "DefaultWeb" ] []
            ]
        ]
    ]
