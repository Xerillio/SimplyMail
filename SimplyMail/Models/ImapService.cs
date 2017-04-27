//
// File: ImapService.cs
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
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using SimplyMail.Utils;
using SimplyMail.Utils.Immutables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimplyMail.Models
{
    public class ImapService
    {
        ImapClient Client { get; set; }

        public ImapService()
        {
            Client = new ImapClient();
        }

        async Task EnsureConnected()
        {
            if (!Client.IsConnected)
                await Client.ConnectAsync("imap.gmail.com", 993).ConfigureAwait(false);
        }

        public async Task LoginAsync(string username, string password)
        {
            await EnsureConnected().ConfigureAwait(false);
            await Client.AuthenticateAsync(username, password).ConfigureAwait(false);
        }

        public async Task<IList<IMailFolder>> GetFolders()
        {
            return await PolicyWrapper.WrapRetryOnNotConnected(async () =>
            {
                var ns = Client.PersonalNamespaces?[0] ?? Client.OtherNamespaces?[0] ?? Client.SharedNamespaces?[0];
                if (ns == null) throw new InvalidOperationException("No folder namespace found");
                return await Client.GetFoldersAsync(ns).ConfigureAwait(false);
            },
            EnsureConnected).ConfigureAwait(false);
        }

        public async Task<IEnumerable<IMailFolder>> GetSubFolders(IMailFolder parentFolder)
        {
            return await PolicyWrapper.WrapRetryOnNotConnected(async () =>
            {
                return await parentFolder.GetSubfoldersAsync().ConfigureAwait(false);
            },
            EnsureConnected).ConfigureAwait(false);
        }

        public async Task<List<MimeMessage>> GetMessages(IMailFolder sourceFolder, SearchQuery query, CancellationToken ct)
        {
            return await PolicyWrapper.WrapRetryOnNotConnected(async () =>
            {
                await sourceFolder.OpenAsync(FolderAccess.ReadOnly).ConfigureAwait(false);
                var uids = (await sourceFolder.SearchAsync(query).ConfigureAwait(false))
                    .Take(10); // TODO Only for test for better performance

                var messages = new List<MimeMessage>();
                foreach (var uid in uids)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    try
                    {
                        var msg = await sourceFolder.GetMessageAsync(uid).ConfigureAwait(false);
                        messages.Add(msg);
                    }
                    catch (FolderNotOpenException)
                    {
                        break;
                    }
                }
                return messages;
            },
            EnsureConnected).ConfigureAwait(false);
        }
    }
}
