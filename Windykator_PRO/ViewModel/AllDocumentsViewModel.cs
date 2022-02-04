using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windykator_PRO.Database;

namespace Windykator_PRO.ViewModel
{

    public class DocumentGridView
    {
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
        }

        public ICommand AddNewDocumentCommand { get => MainWindowViewModel.MainWindowHandler.CreateDocumentCommand; }

        public string SearchCriteria_DocNo { get; set; }
        public string SearchCriteria_DocType { get; set; }
        public string SearchCriteria_Author { get; set; }

        public List<string> AuthorsForCombobox { get => db.User.Where(x => x.IsEnable).Select(x=>x.Login).ToList(); }

        public List<string> DocumentTypesForCombobox { get => db.DocumentType.Where(x => x.IsEnable).Select(x => x.Name).ToList(); }

        public int SelectedItemOnGrid { get; set; }
        protected override void LoadGridData()
        {
            GridList = new System.Collections.ObjectModel.ObservableCollection<DocumentGridView>(db.DocumentHeader.Where(x => x.IsEnable).Select(row => new DocumentGridView
            {
                DocNo = row.InternalNo,
                DocType = row.DocumentTy.Name,
                Author = row.Author.Login,
                CreateDate = (DateTime)row.CreateDate
            }).ToList());
        }

        protected override void Search()
        {
            throw new System.NotImplementedException();
        }
    }
}
