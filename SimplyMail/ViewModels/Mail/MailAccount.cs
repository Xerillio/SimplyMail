//
// File: MailAccount.cs
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
using SimplyMail.Models;
using SimplyMail.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.ViewModels.Mail
{
    public class MailAccount
    {
        ImapService _service;
        internal ImapService Service => _service;

        public ObservableTask<ObservableCollection<MailFolder>> FoldersTask { get; }
        public string Email { get; }

        public MailAccount(string email, ImapService service)
        {
            Email = email;
            _service = SafetyChecker.RequireArgumentNonNull(service, "service");
            FoldersTask = new ObservableTask<ObservableCollection<MailFolder>>(GetFolders());
        }

        async Task<ObservableCollection<MailFolder>> GetFolders()
        {
            var folderCol = new ObservableCollection<MailFolder>();
            var folders = await _service.GetFolders().ConfigureAwait(false);
            foreach (var folder in folders)
                folderCol.Add(new MailFolder(folder, this));
            return folderCol;
        }
    }
}
