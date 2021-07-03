using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Windykator_PRO.ViewModel
{
    class ClientsViewModel : WorkspaceViewModel
    {
        #region Constructor
        public ClientsViewModel()
        {
            base.DisplayName = "Klienci";
        }
        #endregion
  
        public ICommand CreateClientCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.CreateClientCommand;
            }
        }

        public ICommand Import
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.Import;
            }
        }

    }
}
