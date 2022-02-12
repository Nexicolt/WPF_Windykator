using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{

    public class DocumentGridView
    {
        public long Id { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class AllDocumentsViewModel : AllViewModel<DocumentGridView>
    {
        private VindicationDatabase db = null;

        public AllDocumentsViewModel()
        {
            this.DisplayName = "Dokumenty";
            db = new VindicationDatabase();

            ClearAllProperties(this);
        }

        public ICommand AddNewDocumentCommand { get => MainWindowViewModel.MainWindowHandler.CreateDocumentCommand; }
        public ICommand DeleteDocumentCommand { get => new BaseCommand(() => DeleteDocument()); }

        public string SearchCriteria_DocNo { get; set; }
        public string SearchCriteria_DocType { get; set; }
        public string SearchCriteria_Author { get; set; }

        public List<string> AuthorsForCombobox { get => db.User.Where(x => x.IsEnable).Select(x => x.Login).ToList(); }

        public List<string> DocumentTypesForCombobox { get => db.DocumentType.Where(x => x.IsEnable).Select(x => x.Name).ToList(); }

        private DocumentGridView _SelectedItemOnGrid;

        public DocumentGridView SelectedItemOnGrid
        {
            get => _SelectedItemOnGrid; 
            
            set
            {
                _SelectedItemOnGrid = value;
                if(value != null)
                    MainWindowViewModel.MainWindowHandler.DocumentDetailsToShowID = value.Id;
            }
        }

        private void DeleteDocument()
        {
            if (SelectedItemOnGrid is null)
            {
                ShowErrorMessageBox("Wskaż element na liście");
                return;
            }


            var documentHeader = db.DocumentHeader.Where(row => row.IsEnable && row.InternalNo == SelectedItemOnGrid.DocNo).FirstOrDefault();
            if (db.DocumentHeader is null)
            {
                ShowErrorMessageBox("Dokument nie istnieje");
                return;
            }

            bool attachToIssue = db.DocumentToIssue.Where(row => row.DocumentHeader.Id == documentHeader.Id).Count() > 0;
            if (attachToIssue)
            {
                ShowErrorMessageBox("Dokument jest podpięty pod sprawę. Usunięcie niemożliwe");
                return;
            }

            //Usuń pozycje
            foreach (var documentPosotion in db.DocumentItem.Where(row => row.DocumentHeader.Id == documentHeader.Id))
            {
                documentPosotion.Delete();
            }

            documentHeader.Delete();

            db.SaveChanges();
            GridList.Remove(SelectedItemOnGrid);



        }


        public ICommand ShowDocumentDetailsCommand { get => MainWindowViewModel.MainWindowHandler.DocumentDetailsCommand; }
        protected override void LoadGridData()
        {
            GridList = new System.Collections.ObjectModel.ObservableCollection<DocumentGridView>(db.DocumentHeader.Where(x => x.IsEnable).Select(row => new DocumentGridView
            {
                Id = row.Id,
                DocNo = row.InternalNo,
                DocType = row.DocumentType.Name,
                Author = row.Author.Login,
                CreateDate = (DateTime)row.CreateDate
            }).ToList());
        }


        protected override void ClearSearchCriteria()
        {
            ClearAllProperties(this);
            LoadGridData();
        }
        protected override void Search()
        {
            var baseQuery = db.DocumentHeader.Where(row => row.IsEnable);
            if (!string.IsNullOrEmpty(SearchCriteria_DocNo))
            {
                baseQuery = baseQuery.Where(x => x.InternalNo.ToLower().Contains(SearchCriteria_DocNo.ToLower()));
            }
            if (!string.IsNullOrEmpty(SearchCriteria_DocType))
            {
                baseQuery = baseQuery.Where(row => row.DocumentType.Name == SearchCriteria_DocType);
            }
            if (!string.IsNullOrEmpty(SearchCriteria_Author))
            {
                baseQuery = baseQuery.Where(row => row.Author.Login == SearchCriteria_Author);
            }
            GridList = new System.Collections.ObjectModel.ObservableCollection<DocumentGridView>(baseQuery.Select(row => new DocumentGridView
            {
                Id = row.Id,
                DocNo = row.InternalNo,
                DocType = row.DocumentType.Name,
                Author = row.Author.Login,
                CreateDate = (DateTime)row.CreateDate
            }).ToList());
        }

    }
}
