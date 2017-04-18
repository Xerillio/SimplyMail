using MailKit;
using MailKit.Search;
using SimplyMail.Utils.Immutables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.ViewModels.Mail
{
    class MailFolder : ObservableObject
    {
        IMailFolder _sourceFolder;
        
        public ObservableTask<ObservableCollection<MailFolder>> SubFoldersTask { get; }

        ObservableTask<ObservableCollection<MailMessage>> _messagesTask;
        public ObservableTask<ObservableCollection<MailMessage>> MessagesTask
        {
            get { return _messagesTask; }
            set { _messagesTask = value; OnPropertyChanged("MessagesTask"); }
        }
        public string Name => _sourceFolder.Name;

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnSelected(value);
                OnPropertyChanged("IsSelected");
            }
        }

        public MailFolder(IMailFolder folder)
        {
            _sourceFolder = folder;
            SubFoldersTask = new ObservableTask<ObservableCollection<MailFolder>>(GetSubFolders());
        }

        async Task<ObservableCollection<MailFolder>> GetSubFolders()
        {
            var foldersCol = new ObservableCollection<MailFolder>();
            var folders = await _sourceFolder.GetSubfoldersAsync();
            foreach (var folder in folders)
                foldersCol.Add(new MailFolder(folder));
            return foldersCol;
        }

        void OnSelected(bool selected)
        {
            if (!selected || MessagesTask != null)
                return;
            MessagesTask = new ObservableTask<ObservableCollection<MailMessage>>(GetMessages());
        }

        async Task<ObservableCollection<MailMessage>> GetMessages()
        {
            var messages = new ObservableCollection<MailMessage>();
            await _sourceFolder.OpenAsync(FolderAccess.ReadOnly).ConfigureAwait(false);
            var uids = (await _sourceFolder.SearchAsync(SearchQuery.All))
                .Take(10); // TODO Only for test for better performance
            var msgs = await Task.WhenAll(
                uids.Select(async uid => await _sourceFolder.GetMessageAsync(uid)));
            foreach (var msg in msgs)
                messages.Add(new MailMessage(msg));
            return messages;
        }
    }
}
