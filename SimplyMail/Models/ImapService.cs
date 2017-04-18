using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using SimplyMail.Utils.Immutables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Models
{
    class ImapService
    {
        ImapClient Client { get; set; }

        public ImapService()
        {
            Client = new ImapClient();
        }

        public async Task LoginAsync(string username, string password)
        {
            await Client.ConnectAsync("imap.gmail.com", 993).ConfigureAwait(false);
            await Client.AuthenticateAsync(username, password).ConfigureAwait(false);
        }

        public async Task<IList<IMailFolder>> GetFolders()
        {
            var ns = Client.PersonalNamespaces?[0] ?? Client.OtherNamespaces?[0] ?? Client.SharedNamespaces?[0];
            if (ns == null) throw new InvalidOperationException("No folder namespace found");
            return await Client.GetFoldersAsync(ns).ConfigureAwait(false);
        }
    }
}
