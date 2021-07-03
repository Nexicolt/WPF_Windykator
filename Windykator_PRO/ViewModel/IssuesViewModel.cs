using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Windykator_PRO.ViewModel
{
    class IssuesViewModel : WorkspaceViewModel
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
