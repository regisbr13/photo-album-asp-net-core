using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="campo obrigatório")]
        [StringLength(50, ErrorMessage ="use até {1} caracteres")]
        public string Name { get; set; }

        public string Place { get; set; }

        [Required(ErrorMessage ="campo obrigatório")]
        public string Cover { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
    }
}
