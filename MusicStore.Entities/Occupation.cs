using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Occupation
    {
        public int OccupationId { get; set; }

        public int AlbumId { get; set; }

        public int RoomId { get; set; }

        [Required(ErrorMessage = "Введите дату в формате DD.MM.YYYY")]
        [Display(Name = "Дата записи")]
        public DateTime DayOfOccupation { get; set; }

        public virtual Room Room { get; set; }

        public virtual Album Album { get; set; }
    }
}
