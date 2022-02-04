using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Windykator_PRO.Database.Models
{
    /// <summary>
    /// Bazowy model, po którym dziedziczą wszystkie inne. Tak, żeby każdy wiersz zawierał klucz główny i informację o blokadzie
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Id rekordu
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Czy aktywny/włączony
        /// </summary>
        [NotMapped]
        private bool? _isEnabled;

        public bool IsEnable
        {
            //Nie trzeba przy każdym dodawaniu rekordów, ustawiać IsEnabled. Nieustawione, z automatu wskoczy na "true"
            get
            {
                if (_isEnabled is null)
                {
                    return true;
                }
                return _isEnabled.Value;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public void Delete()
        {
            this.IsEnable = false;
        }
    }
}