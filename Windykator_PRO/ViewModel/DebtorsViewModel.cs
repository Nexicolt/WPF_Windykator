using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Windykator_PRO.ViewModel
{
    class DebtorsViewModel : WorkspaceViewModel
    {
        public DebtorsViewModel()
        {
            this.DisplayName = "Dłużnicy";
        }


        public ICommand CreateDebtorCommand
        {
            get
            {
                return MainWindowViewModel.MainWindowHandler.CreateDebtorCommand;
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
