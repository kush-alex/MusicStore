using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }

        [Required(ErrorMessage = "Введите название альбома")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите тираж")]
        [Display(Name = "Тираж")]
        [Range(0, int.MaxValue, ErrorMessage = "Недопустимая зарплата")]
        public int Circulation { get; set; }

        [Required(ErrorMessage = "Введите стоимость диска")]
        [Display(Name = "Стоимость диска")]
        [Range(0, int.MaxValue, ErrorMessage = "Недопустимая зарплата")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Введите дату в формате DD.MM.YYYY")]
        [Display(Name = "Дата выхода")]
        public DateTime ReleaseDate { get; set; }

        public int ArtistId { get; set; }

        public int ProducerId { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual Producer Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

        public virtual ICollection<Occupation> Occupations { get; set; }
    }
}
