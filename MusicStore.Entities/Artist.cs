using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Artist
    {
        public int ArtistId { get; set; }

        [Required(ErrorMessage = "Введите имя исполнителя")]
        [StringLength(255)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите год рождения исполнителя")]
        [Display(Name = "Год рождения")]
        [Range(1900, 2015, ErrorMessage = "Недопустимый год")]
        public int YearOfBeginning { get; set; }

        [Required(ErrorMessage = "Введите телефон в формате 012 345 67 89")]
        [RegularExpression(@"[0-9]{3} [0-9]{3} [0-9]{2} [0-9]{2}", ErrorMessage = "Введите телефон в формате 012 345 67 89")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
