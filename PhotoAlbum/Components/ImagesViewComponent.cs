using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Data;
using PhotoAlbum.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Components
{
    public class ImagesViewComponent : ViewComponent
    {
        private readonly ImagesService _imagesService;

        public ImagesViewComponent(ImagesService imageService)
        {
            _imagesService = imageService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View(await _imagesService.FindByAlbumIdAsync(id));
        }
    }
}
