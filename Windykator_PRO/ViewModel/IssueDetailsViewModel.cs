using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Database.Models;
using Windykator_PRO.Helpers;
using Windykator_PRO.View.NewDocument;

namespace Windykator_PRO.ViewModel
{
    public class AddnotationGridItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class IssueDetailsViewModel : WorkspaceViewModel
    {
        private readonly VindicationDatabase db = null;

        private long _issueId;

        public IssueDetailsViewModel(long? issueId)
        {
            base.DisplayName = "Szczególy sprawy";
            db = new VindicationDatabase();

            LoadIssueData((long)issueId);

            _issueId = (long)issueId;

            LoadAddnotations();

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

        /**
         * ADNOTACJE
         */

        public AddnotationGridItem SelectedAddnotationOnGrid { get; set; }

        public ObservableCollection<AddnotationGridItem> AddnotationsGridList { get; set; }

        public string AddnotationTitle { get; set; }
        public string AddnotationContent { get; set; }

        public ICommand AddAddnotationCommand { get => new BaseCommand(() => AddAddnotation()); }
        public ICommand DoubleRowClickCommand { get => new BaseCommand(() => ShowAddnotationInModal()); }


        private void ShowAddnotationInModal()
        {
            if(SelectedAddnotationOnGrid != null)
                new AddnotationModal(SelectedAddnotationOnGrid.Title, SelectedAddnotationOnGrid.Content) { WindowStartupLocation=System.Windows.WindowStartupLocation.CenterScreen }.Show();
        }

        private void LoadAddnotations()
        {
            AddnotationsGridList  = new ObservableCollection<AddnotationGridItem>( db.Addnotation.Where(x => x.IsEnable && x.Issue.Id == _issueId).Select(x => new AddnotationGridItem
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Author = x.Author.Login,
                CreateDate = (DateTime)x.CreateDate
            }).ToList());
        }
        private void AddAddnotation()
        {
            if(string.IsNullOrEmpty(AddnotationTitle) || string.IsNullOrEmpty(AddnotationContent))
            {
                ShowErrorMessageBox("Tytuł i zawartośc nie moga być puste");
                return;
            }
            long userId = GetLoggedUserId();
            var author = db.User.Where(x => x.IsEnable && x.Id == userId).FirstOrDefault();
            var Issue = db.Issue.Where(x => x.IsEnable && x.Id == _issueId).FirstOrDefault();
            var newAddnotation = new IssueAddnotation { Title = AddnotationTitle, Content = AddnotationContent, Author = author, Issue= Issue };
            db.Addnotation.Add(newAddnotation);
            db.SaveChanges();
            LoadAddnotations();
            OnPropertyChanged(() => AddnotationsGridList);
        }


    }
}
