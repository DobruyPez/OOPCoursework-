using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab.Resources
{
    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Image { get; set; } // Путь к изображению или URL

        [Required]
        [MaxLength(500)]
        public string Link { get; set; } // Ссылка на рекламу

    }
}

