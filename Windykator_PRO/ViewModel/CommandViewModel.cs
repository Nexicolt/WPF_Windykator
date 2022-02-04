using System;
using System.Text;
using System.Windows.Input;

namespace Windykator_PRO.ViewModel
{
    public class CommandViewModel : BaseViewModel
    {
        #region Properties

        public ICommand Command { get; private set; }

        #endregion Properties

        #region Constructor

        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            this.DisplayName = displayName;

            this.Command = command;

            //Nazwa ikony, to nazwa elementu, ale bez polskich znaków
            string iconPath = "/Icons/Menu/" + Encoding.ASCII.GetString(
                     Encoding.GetEncoding("Cyrillic").GetBytes(displayName)
                 ) + ".png";

            this.iconPath = iconPath;

            //NAzwa klikniętej ikony (dodana tylko dlatego, że w XAMl'u nie można łączyć stringów)
            this.iconPathClicked = "/Icons/Menu/" + Encoding.ASCII.GetString(
                     Encoding.GetEncoding("Cyrillic").GetBytes(displayName)
                 ) + "_clicked.png";
        }

        #endregion Constructor
    }
}