//
// File: MailMessage.cs
// Author: Casper Sørensen
//
//   Copyright 2017 Casper Sørensen
//	 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//	 
//       http://www.apache.org/licenses/LICENSE-2.0
//	 
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
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
