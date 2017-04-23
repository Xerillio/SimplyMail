using SimplyMail.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.ViewModels.Mail
{
    class MailAccount
    {
        ImapService _service;

        public ObservableTask<ObservableCollection<MailFolder>> FoldersTask { get; }
        public string Email { get; }

        public MailAccount(string email, ImapService service)
        {
            Email = email;
            _service = service;
            FoldersTask = new ObservableTask<ObservableCollection<MailFolder>>(GetFolders());
        }

        async Task<ObservableCollection<MailFolder>> GetFolders()
        {
            var folderCol = new ObservableCollection<MailFolder>();
            var folders = await _service.GetFolders().ConfigureAwait(false);
            foreach (var folder in folders)
                folderCol.Add(new MailFolder(folder, _service));
            return folderCol;
        }
    }
}
