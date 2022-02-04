using System;

namespace Windykator_PRO.Database.Models.Base
{
    /// <summary>
    /// Model, po którym dziedziczą wszystkie pozostałe, w których konieczna jest data edycji i utworzenia
    /// </summary>
    public class TimeStampBaseModel : BaseModel
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}