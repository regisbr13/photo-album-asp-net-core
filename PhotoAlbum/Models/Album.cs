using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Display(Name= "Nome")]
        [Required(ErrorMessage ="campo obrigatório")]
        [StringLength(50, ErrorMessage ="use até {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Local")]
        public string Place { get; set; }

        [Display(Name = "Capa")]
        public string Cover { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        public ICollection<Images> Images { get; set; }
    }
}
