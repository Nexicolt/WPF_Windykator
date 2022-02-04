using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    public abstract class AllViewModel<T> : WorkspaceViewModel
    {
        private ObservableCollection<T> _gridList;

        public AllViewModel()
        {
            Messenger.Default.Register<string>(this, AnalyzeSendedString);
        }

        protected void AnalyzeSendedString(string text)
        {
            if (text == "REFRESH")
            {
                Search();
            }
        }

        public ObservableCollection<T> GridList
        {
            get
            {
                if (_gridList == null)
                    LoadGridData();
                return _gridList;
            }
            set
            {
                _gridList = value;
                OnPropertyChanged(() => GridList);
            }
        }

        private BaseCommand _search;

        public ICommand SearchCommand
        {
            get
            {
                if (_search == null)
                {
                    _search = new BaseCommand(() => Search());
                }
                return _search;
            }
        }

        protected abstract void LoadGridData();

        protected abstract void Search();
    }
}