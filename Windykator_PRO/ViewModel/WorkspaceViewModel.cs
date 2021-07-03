﻿using System;
using System.Windows.Input;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    public abstract class WorkspaceViewModel:BaseViewModel
    {
        #region Constructor
        public WorkspaceViewModel()
        {

        }
        #endregion

        #region CloseCommand
        //Komenda, powiązana z zamykaniem zakładki
        private BaseCommand _CloseCommand;

        /**
         * Metoda wywoływana przy zamykaniu zakładki
         */
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(()=>this.OnRequestClose());
                return _CloseCommand;
            }
        }

        #endregion

        #region Helpers
        public event EventHandler RequestClose;
        private void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion
    }
}
