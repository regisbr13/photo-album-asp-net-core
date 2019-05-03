using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Models
{
    public class Images
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="campo obrigatório")]
        public string Path { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }
    }
}
