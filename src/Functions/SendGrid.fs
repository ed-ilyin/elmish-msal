module SendGrid

open Fable.Core
open Fable.Import.Node.Buffer


type Email = { name: string option; email: string }


type Personalization = { ``to``: Email [] }


[<StringEnum>]
type MimeType =
    | [<CompiledName("text/html")>] TextHtml
    | [<CompiledName("image/jpeg")>] ImageJpeg


type Content = {
    ``type``: MimeType
    value: string
}


[<StringEnum>]
type Disposition =
    | [<CompiledName("inline")>] Inline
    | [<CompiledName("attachment")>] Attachment


type Attachment = {
    content: string
    ``type``: MimeType option
    filename: string
    disposition: Disposition option
    content_id: string option
}


type Message = {
    personalizations: Personalization []
    from: Email
    content: Content []
    attachments: Attachment [] option
}


let htmlMessageWithOneInlineJpegToSingleRecipient fromName fromEmail toName toEmail html jpegBase64 jpegFilename jpegContentId =
    {   personalizations =
            [| { ``to`` = [| { name = Some toName; email = toEmail } |] }
            |]
        from = { name = Some fromName; email = fromEmail }
        content = [| { ``type`` = TextHtml; value = html } |]
        attachments =
            Some [|
                {   content = jpegBase64
                    ``type`` = Some ImageJpeg
                    filename = jpegFilename
                    disposition = Some Inline
                    content_id = Some jpegContentId
                }
            |]
    }


let inlineJpeg jpegBase64 contentId = {
    content = jpegBase64
    ``type`` = Some ImageJpeg
    filename = contentId + ".jpg"
    disposition = Some Inline
    content_id = Some contentId
}


let email name email = { name = Some name; email = email }


let htmlMessageWithAttachments fromName fromEmail recipients html attachments =
    {   personalizations = [| { ``to`` = recipients } |]
        from = { name = Some fromName; email = fromEmail }
        content = [| { ``type`` = TextHtml; value = html } |]
        attachments = Array.ofList attachments |> Some
    }
