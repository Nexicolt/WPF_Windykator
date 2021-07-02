
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        //po...
        //
        private ReadOnlyCollection<CommandViewModel> _Commands;//każdy link po lewej stronie jest CommandVieModelem
        //zbior zakł
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        #endregion

        #region ToolBarComands
        private BaseCommand getCommand(BaseCommand _command, WorkspaceViewModel workspace)
        {
            if(_command == null)
                _command = new BaseCommand(()=> Create(workspace));
            return _command;
        }
        private BaseCommand _CreateClientCommand;
        
        //to jest komenda, która zostanie podpieta pod przycisk w pasku narzędzi i ta komenda wywyoła funkcje Create
        public ICommand CreateClientCommand
        {
            get
            {
                return getCommand(_CreateClientCommand, new AddNewClientViewModel());
            }
        }
        private BaseCommand _CreateDebtorCommand;

        //to jest komenda, która zostanie podpieta pod przycisk w pasku narzędzi i ta komenda wywyoła funkcje Create
        public ICommand CreateDebtorCommand
        {
            get
            {
                return getCommand(_CreateDebtorCommand, new AddNewDebtorViewModel());
            }
        }

        private BaseCommand _CreateIssueCommand;

        //to jest komenda, która zostanie podpieta pod przycisk w pasku narzędzi i ta komenda wywyoła funkcje Create
        public ICommand CreateIssueCommand
        {
            get
            {
                return getCommand(_CreateIssueCommand, new AddNewIssueViewModel());
            }
        }

        private BaseCommand _ImportClients;

        //to jest komenda, która zostanie podpieta pod przycisk w pasku narzędzi i ta komenda wywyoła funkcje Create
        public ICommand ImportClients
        {
            get
            {
                return getCommand(_ImportClients, new ImportClientsViewModel());
            }
        }



        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
               //sprawdzamy czy lista linków po lewej stronie jest pusta
                if(_Commands==null)
                {
                    //Jeżeli lista linków jest pusta, to tworzymy tę listę przy pomocy funkcji CreateCommads();  
                    List<CommandViewModel> cmds = this.CreateCommands();
                    //tworzymy 
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        private List<CommandViewModel> CreateCommands()
        {
            //w tej metodzie decydujemy jakie linki będą po lewej stronie
            return new List<CommandViewModel>
            {
                 new CommandViewModel("Sprawy",new BaseCommand(()=>this.ShowIssues())), //
                 new CommandViewModel("Klienci",new BaseCommand(()=>this.ShowClients())), //
                 new CommandViewModel("Dłużnicy",new BaseCommand(()=>this.ShowDebtors())), //
            };
        }
        #endregion
        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if(_Workspaces==null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged+= this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }
        #endregion
        #region PrivateHelpers
        private void Create(WorkspaceViewModel workspace) 
        { 
            this.Workspaces.Add(workspace); 
            this.SetActiveWorkspace(workspace); 
        }

        private void ShowIssues()
        {
            //w kolekc...
            IssuesViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is IssuesViewModel) as IssuesViewModel;
            //jeżeli takiej zakładki nie ma
            if (workspace == null)
            {
                //to tworzę nową zakładke
                workspace = new IssuesViewModel();
                //i dodaje do kolekcji wyswietlanych zakładek
                this.Workspaces.Add(workspace);
            }
            //uaktywniam zakładkę, którą znalazłem lub dodałem
            this.SetActiveWorkspace(workspace);
        }

        private void ShowDebtors()
        {
            //w kolekc...
            DebtorsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is DebtorsViewModel) as DebtorsViewModel;
            //jeżeli takiej zakładki nie ma
            if (workspace == null)
            {
                //to tworzę nową zakładke
                workspace = new DebtorsViewModel();
                //i dodaje do kolekcji wyswietlanych zakładek
                this.Workspaces.Add(workspace);
            }
            //uaktywniam zakładkę, którą znalazłem lub dodałem
            this.SetActiveWorkspace(workspace);
        }

        private void ShowClients()
        {
            //w kolekc...
            ClientsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is ClientsViewModel) as ClientsViewModel;
            //jeżeli takiej zakładki nie ma
            if(workspace==null)
            {
                //to tworzę nową zakładke
                workspace = new ClientsViewModel();
                //i dodaje do kolekcji wyswietlanych zakładek
                this.Workspaces.Add(workspace);
            }
            //uaktywniam zakładkę, którą znalazłem lub dodałem
            this.SetActiveWorkspace(workspace);
        }
        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }
}
