using GalaSoft.MvvmLight.Messaging;
using System;
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

        protected void ClearAllProperties<Generic>(Generic viewModel) where Generic : BaseViewModel
        {
            foreach (var prop in viewModel.GetType().GetProperties())
            {
                
                if (prop != null && prop.Name != "DisplayName")
                {
                    var type = prop.PropertyType.FullName;
                    if(type == "System.String")
                    {
                        prop.SetValue(viewModel, String.Empty);
                        viewModel.OnPropertyChanged(prop.Name);
                    }

                    string secondName = (prop.PropertyType.GenericTypeArguments.Length > 0) ? prop.PropertyType.GenericTypeArguments[0].Name : null;
                    if(secondName != null && (secondName == "Integer" || secondName == "Decimal"))
                    {
                        if (type.StartsWith("System.Nullable"))
                            prop.SetValue(viewModel, null);
                        else
                            prop.SetValue(viewModel, 0);
                        viewModel.OnPropertyChanged(prop.Name);


                    }

                    if(type == "System.Boolean")
                    {
                        viewModel.OnPropertyChanged(prop.Name);
                    }
                }
                viewModel.OnPropertyChanged(prop.Name);
            }
        }

        public ICommand ClearCommand { get => new BaseCommand(() => ClearSearchCriteria()); }

        protected abstract void ClearSearchCriteria();
    }
}