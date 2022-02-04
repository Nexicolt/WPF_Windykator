using System.Windows.Input;

namespace Windykator_PRO.ViewModel
{
    internal class IssuesViewModel : WorkspaceViewModel
    {
        public IssuesViewModel()
        {
            this.DisplayName = "Sprawy";
        }

        public ICommand CreateClientCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.CreateIssueCommand;
            }
        }
    }
}