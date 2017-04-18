using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace SimplyMail.ViewModels.Mail
{
    class MailMessage
    {
        MimeMessage _sourceMessage;

        public string From => _sourceMessage.From[0].Name;
        public string Subject => _sourceMessage.Subject;
        public bool IsBodyHtml => _sourceMessage.HtmlBody != null;
        public string Body => _sourceMessage.HtmlBody ?? _sourceMessage.TextBody;

        public MailMessage(MimeMessage message)
        {
            _sourceMessage = message;
        }
    }
}
