using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;

namespace Windykator_PRO.ViewModel
{
    public class IssueDetailsViewModel : WorkspaceViewModel
    {
        private readonly VindicationDatabase db = null;

        public IssueDetailsViewModel(long? issueId)
        {
            base.DisplayName = "Szczególy sprawy";
            db = new VindicationDatabase();

            LoadIssueData((long)issueId);

        }

        public string InternalNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLocation { get; set; }
        public string DebtorName { get; set; }
        public string DebtorLocation { get; set; }
        public string DebtorDue { get; set; }
        public string Currency { get; set; }

        private DocumentGridView _SelectedDocumentOnGrid;
        public DocumentGridView SelectedDocumentOnGrid
        {
            get => _SelectedDocumentOnGrid;
            set
            {
                _SelectedDocumentOnGrid = value;
                MainWindowViewModel.MainWindowHandler.DocumentDetailsToShowID = value.Id;
            }
        }

        public ICommand DocumentDetailsCommand { get => MainWindowViewModel.MainWindowHandler.DocumentDetailsCommand; }
        public ObservableCollection<DocumentGridView> GridList { get; set; }

        private List<string> _CurrencySelectList;
        public List<string> CurrencySelectList
        {
            get
            {
                if (_CurrencySelectList == null)
                    _CurrencySelectList = db.Currency.Where(row => row.IsEnable).Select(x => x.Name).ToList();
                return _CurrencySelectList;
            }
        }

        private void LoadIssueData(long issueId)
        {
            var issueDto = db.Issue.Find(issueId);

            InternalNo = issueDto.InternalNo;
            CustomerName = issueDto.Customer.Name;
            CustomerLocation = issueDto.Customer.Adress.City + " ul. " + issueDto.Customer.Adress.Street;

            DebtorName = issueDto.Debtor.Name;
            DebtorLocation = issueDto.Debtor.Adress.City + " ul. " + issueDto.Debtor.Adress.Street;

            Currency = issueDto.Currency.Name;

            DebtorDue = issueDto.Cost.ToString();

            var IssueDocumentsList = db.DocumentToIssue.Where(row => row.Issue.Id == issueId).Select(x => x.Id).ToArray();

            GridList = new ObservableCollection<DocumentGridView>(db.DocumentHeader.Where(row => row.IsEnable && IssueDocumentsList.Contains(row.Id))
                .Select(row => new DocumentGridView
                {
                    Id = row.Id,
                    DocNo = row.InternalNo,
                    DocType = row.DocumentType.Name,
                    Author = row.Author.Login,
                    CreateDate = (DateTime)row.CreateDate
                }).ToList());
            OnPropertyChanged(() => GridList);
        }
    }
}
