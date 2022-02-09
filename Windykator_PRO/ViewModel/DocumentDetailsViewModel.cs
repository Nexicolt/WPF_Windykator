using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windykator_PRO.Database;

namespace Windykator_PRO.ViewModel
{
    public class DocumentDetailsViewModel : WorkspaceViewModel
    {
        private readonly VindicationDatabase db = null;
        public DocumentDetailsViewModel(long? documentId)
        {
            if(documentId == null) OnRequestClose();
            base.DisplayName = "Szczegóły dokumentu";

            db = new VindicationDatabase();

            if (documentId != null)
                LoadDocumentData((long)documentId);
        }
        public List<string> AuthorsForCombobox { get => db.User.Where(x => x.IsEnable).Select(x => x.Login).ToList(); }

        public List<string> DocumentTypesForCombobox => db.DocumentType.Where(x => x.IsEnable).Select(x => x.Name).ToList();

        public string DocumentNo { get; set; }

        public string DocumentType { get; set; }

        public string Author { get; set; }

        public ObservableCollection<AssortmentOnDocumentGridItem> GridList { get; set; }


        private void LoadDocumentData(long docId)
        {
            var docDto = db.DocumentHeader.Find(docId);
            var docPositions = db.DocumentItem.Where(row => row.DocumentHeader.Id == docDto.Id);

            DocumentNo = docDto.InternalNo;
            DocumentType = docDto.DocumentType.Name;
            Author = docDto.Author.Login;

            GridList = new ObservableCollection<AssortmentOnDocumentGridItem> (docPositions.Select(row => new AssortmentOnDocumentGridItem
            {
                Name = row.Service == null ? row.Assortment.Name : row.Service.Name,
                Type = row.Service == null ? "Asortyment" : "Usługa",
                Quantity = row.Quantity
            }).ToList());
        }
    }
}
