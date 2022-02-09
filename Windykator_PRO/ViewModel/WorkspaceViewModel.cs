using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Input;
using Windykator_PRO.Helpers;

namespace Windykator_PRO.ViewModel
{
    public abstract class WorkspaceViewModel : BaseViewModel
    {
        #region Constructor

        public WorkspaceViewModel()
        {
        }

        #endregion Constructor

        #region CloseCommand

        //Komenda, powiązana z zamykaniem zakładki
        private BaseCommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(() => this.OnRequestClose());
                return _CloseCommand;
            }
        }

        #endregion CloseCommand

        #region Helpers

        public event EventHandler RequestClose;

        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

       

        protected void SendRefreshSignal()
        {
            Messenger.Default.Send("REFRESH");
        }
        protected virtual bool IsValid() => true;

        #endregion Helpers
    }
}