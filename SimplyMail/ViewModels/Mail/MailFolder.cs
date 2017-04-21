using MailKit;
using MailKit.Search;
using SimplyMail.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.ViewModels.Mail
{
    class MailFolder : ObservableObject
    {
        IMailFolder _sourceFolder;
        ImapService _service;
        
        public ObservableTask<ObservableCollection<MailFolder>> SubFoldersTask { get; }

        static CancellationTokenSource _messagesTaskCts;
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

        public MailFolder(IMailFolder folder, ImapService service)
        {
            _sourceFolder = folder;
            _service = service;
            SubFoldersTask = new ObservableTask<ObservableCollection<MailFolder>>(GetSubFolders());
        }

        void OnSelected(bool selected)
        {
            if (!selected || MessagesTask?.IsSuccessfullyCompleted == true)
                return;
            MessagesTask = new ObservableTask<ObservableCollection<MailMessage>>(GetMessages());
        }

        async Task<ObservableCollection<MailFolder>> GetSubFolders()
        {
            var foldersCol = new ObservableCollection<MailFolder>();
            var folders = await _service.GetSubFolders(_sourceFolder).ConfigureAwait(false);
            foreach (var folder in folders)
                foldersCol.Add(new MailFolder(folder, _service));
            return foldersCol;
        }

        async Task<ObservableCollection<MailMessage>> GetMessages()
        {
            try { _messagesTaskCts?.Cancel(); }
            catch (ObjectDisposedException) { /* The implicated task already finished */ }

            using (_messagesTaskCts = new CancellationTokenSource())
            {
                return new ObservableCollection<MailMessage>(
                    (await _service.GetMessages(
                        _sourceFolder, SearchQuery.All, _messagesTaskCts.Token)
                        .ConfigureAwait(false))
                    .Select(mimeMsg => new MailMessage(mimeMsg)));
            }
        }
    }
}
