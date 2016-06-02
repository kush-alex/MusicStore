using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Song
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название песни")]
        [StringLength(255)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public int AlbumId { get; set; }

        [Required(ErrorMessage = "Введите длительность песни")]
        [Display(Name = "Длительность")]
        //[RegularExpression(@"^[0-9]{2}\:[0-9]{2}$", ErrorMessage = "Введите длительность в формате 00:00")]
        public int Duration { get; set; }


        [Required(ErrorMessage = "Выберите жанр песни")]
        [Display(Name = "Жанр")]
        public Genre Category { get; set; }

        public virtual Album Album { get; set; }
    }
}
