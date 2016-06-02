using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Недопустимый номер зала")]
        [Display(Name = "Номер зала")]
        public int Number { get; set; }

        [Display(Name = "Размер зала")]
        [Range(0, int.MaxValue, ErrorMessage = "Недопустимая зарплата")]
        public int Size { get; set; }

        [Display(Name = "Стоимость в сутки")]
        [Range(0, int.MaxValue, ErrorMessage = "Недопустимая зарплата")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Введите время в формате 00:00")]
        [Display(Name = "Время открытия")]
        [RegularExpression(@"[0-9]{2}:[0-9]{2}", ErrorMessage = "Введите время в формате 00:00")]
        public string openingTime { get; set; }

        [Required(ErrorMessage = "Введите время в формате 00:00")]
        [Display(Name = "Время закрытия")]
        [RegularExpression(@"[0-9]{2}\:[0-9]{2}", ErrorMessage = "Введите время в формате 00:00")]
        public string closingTime { get; set; }

        public virtual ICollection<Occupation> Occupations { get; set; }

    }
}
