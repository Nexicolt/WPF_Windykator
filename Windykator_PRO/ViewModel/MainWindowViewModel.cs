using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static MainWindowViewModel MainWindowHandler = null;

        public MainWindowViewModel()
        {
            MainWindowViewModel.MainWindowHandler = this;
        }

        #region Fields

        //Kolekcja z komendami (komenda = wowołanie nowej zakładki)
        private ReadOnlyCollection<CommandViewModel> _Commands;

        //Kolekcja z otwartymi zakładkami
        private ObservableCollection<WorkspaceViewModel> _Workspaces;

        #endregion Fields

        #region ToolBarComands

        /**
         * Główna funkcja, wykoszystywana w spersonalizowanych komendach. Sprawdza czy przekazany ViewModel jest NULL'em i na tej podstawie
         * zwraca jego nową instancję, lub już istniejącą
         */

        private BaseCommand getCommand(BaseCommand _command, WorkspaceViewModel workspace)
        {
            if (_command == null)
                _command = new BaseCommand(() => Create(workspace));
            return _command;
        }

        //Zmienna, reprezntującą komendę. Wykorzystywaną w 'GetCommand" do porównania i zwrócenia nowej instancji, czy instiejącej
        private BaseCommand _CreateClientCommand;

        private BaseCommand _EditlientCommand;

        private BaseCommand _CreateDebtorCommand;
        private BaseCommand _CreateIssueCommand;
        private BaseCommand _Import;

        /**
         * Komenda wywoływana przy kliknięciu na określony link (W lewym menu lub w górnym. Wywoływana poprzez "binding"
         * (Dotyczy każdej funkcji w przestrzeni 'ToolBarComands')
         */

        public ICommand CreateClientCommand
        {
            get
            {
                return getCommand(_CreateClientCommand, new AddNewClientViewModel());
            }
        }

        public void EditClientCommand(long customerId)
        {
            Create(new EditClientViewModel(customerId));
        }


        public ICommand DebtorsToSelectModalCommand
        {
            get
            {
                return new BaseCommand(() => ShowDebtorsModal());
            }
        }

        public ICommand ClientsToSelectModalCommand
        {
            get
            {
                return new BaseCommand(() => ShowClientsModal());
            }
        }

        public void EditDebtorCommand(long customerId)
        {
            Create(new EditDebtorViewModel(customerId));
        }

        public ICommand CreateDebtorCommand
        {
            get
            {
                return getCommand(_CreateDebtorCommand, new AddNewDebtorViewModel());
            }
        }

        public ICommand CreateIssueCommand
        {
            get
            {
                return getCommand(_CreateIssueCommand, new AddNewIssueViewModel());
            }
        }

        private BaseCommand _CreateDocumentCommand;
        public ICommand CreateDocumentCommand
        {
            get
            {
                return getCommand(_CreateDocumentCommand, new AddNewDocumentViewModel());
            }
        }

        public ICommand Import
        {
            get
            {
                return getCommand(_Import, new ImportOptionViewModel());
            }
        }

        #endregion ToolBarComands

        #region Commands


        private void ShowClientsModal()
        {
            Create(new ClientsViewModel(true));
        }
        private void ShowDebtorsModal()
        {
            Create(new DebtorsViewModel(true));
        }
        //Kolekcja, zawierająca wszystkie funkcje
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                //sprawdzamy czy lista linków po lewej stronie jest pusta
                if (_Commands == null)
                {
                    //Jeżeli lista linków jest pusta, to tworzymy tę listę przy pomocy funkcji CreateCommads();
                    List<CommandViewModel> cmds = this.CreateCommands();
                    //tworzymy
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }

        /**
         * Funkcja generuje linki w menu, po lewej stronie
         */

        private List<CommandViewModel> CreateCommands()
        {
            //Zwracana lista z linkami, którę będą w lewym menu
            return new List<CommandViewModel>
            {
                 new CommandViewModel("Sprawy",new BaseCommand(()=>this.ShowIssues())),
                 new CommandViewModel("Klienci",new BaseCommand(()=>this.ShowClients())),
                 new CommandViewModel("Dłużnicy",new BaseCommand(()=>this.ShowDebtors())),
                 new CommandViewModel("Dokumenty",new BaseCommand(()=>this.ShowDocuments())),
                 new CommandViewModel("Ustawienia",new BaseCommand(()=>this.ShowSettings())),
            };
        }

        #endregion Commands

        #region Workspaces

        //Kolekcja przechowuje wszystkie otwarte karty
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    //DOpisanie wydarzenie "OnChange", wywoływane przy dodaniu lub usunięciu zakładki
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }

        /**
         * Funkcja wywoływana przy dodaniu lub usunięciu zakładki
         * Odświeża kolekcję i na nowo prezentuje ją w formie widoku dla użytkownika
         */

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        /**
         * Metoda wywoływana przy zamknięciu zakładki
         */

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }

        #endregion Workspaces

        #region PrivateHelpers

        /**
         * Metoda dodaje nową zakładke do kolekcji
         */

        private void Create(WorkspaceViewModel workspace)
        {
            this.Workspaces.Add(workspace);
            //Ustaw nową zakłądkę na aktywną i odśwież "workspace"
            this.SetActiveWorkspace(workspace);
        }

        private void ShowIssues()
        {
            //Sprawdź, czy kolekcja zawiera już rządaną zakładkę
            IssuesViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is IssuesViewModel) as IssuesViewModel;
            //Utwórz nową i ją dodaj, jeśli nie
            if (workspace == null)
            {
                workspace = new IssuesViewModel();
                this.Workspaces.Add(workspace);
            }
            //Ustaw na aktywną, jeśli tak
            this.SetActiveWorkspace(workspace);
        }

        private void ShowDebtors()
        {
            //Sprawdź, czy kolekcja zawiera już rządaną zakładkę
            DebtorsViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is DebtorsViewModel) as DebtorsViewModel;
            //Utwórz nową i ją dodaj, jeśli nie
            if (workspace == null)
            {
                workspace = new DebtorsViewModel();
                this.Workspaces.Add(workspace);
            }
            //Ustaw na aktywną, jeśli tak
            this.SetActiveWorkspace(workspace);
        }

        private void ShowClients()
        {
            //Sprawdź, czy kolekcja zawiera już rządaną zakładkę
            ClientsViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is ClientsViewModel) as ClientsViewModel;
            //Utwórz nową i ją dodaj, jeśli nie
            if (workspace == null)
            {
                workspace = new ClientsViewModel();
                this.Workspaces.Add(workspace);
            }
            //Ustaw na aktywną, jeśli tak
            this.SetActiveWorkspace(workspace);
        }

        private void ShowDocuments()
        {
            //Sprawdź, czy kolekcja zawiera już rządaną zakładkę
            AllDocumentsViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is AllDocumentsViewModel) as AllDocumentsViewModel;
            //Utwórz nową i ją dodaj, jeśli nie
            if (workspace == null)
            {
                workspace = new AllDocumentsViewModel();
                this.Workspaces.Add(workspace);
            }
            //Ustaw na aktywną, jeśli tak
            this.SetActiveWorkspace(workspace);
        }

        private void ShowSettings()
        {
            //Sprawdź, czy kolekcja zawiera już rządaną zakładkę
            SettingsViewModel workspace = this.Workspaces.FirstOrDefault(vm => vm is SettingsViewModel) as SettingsViewModel;
            //Utwórz nową i ją dodaj, jeśli nie
            if (workspace == null)
            {
                workspace = new SettingsViewModel();
                this.Workspaces.Add(workspace);
            }
            //Ustaw na aktywną, jeśli tak
            this.SetActiveWorkspace(workspace);
        }

        /**
         * Metoda ustawia "workspace", na ten przekazany w parametrze (odświeża widok)
         */

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            //Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion PrivateHelpers
    }
}