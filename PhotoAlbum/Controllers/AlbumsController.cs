using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Models;
using PhotoAlbum.Service;

namespace PhotoAlbum.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly AlbumService _albumService;
        private readonly ImagesService _imagesService;
        private readonly IHostingEnvironment _environment;

        public AlbumsController(AlbumService albumService, IHostingEnvironment environment, ImagesService imagesService)
        {
            _albumService = albumService;
            _environment = environment;
            _imagesService = imagesService;
        }

        // GET:
        public async Task<IActionResult> Index()
        {
            return View(await _albumService.FindAllAsync());
        }

        // GET:
        [HttpGet("/Visualizar-album")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _albumService.FindByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Album obj, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if(img != null)
                {
                    var path = Path.Combine(_environment.WebRootPath, "albums");
                    using (var fs = new FileStream(Path.Combine(path, img.FileName), FileMode.Create))
                    {
                        obj.Cover = "~/albums/" + img.FileName;
                        await img.CopyToAsync(fs);   
                    }
                }
                else
                {
                    obj.Cover = obj.Name + ".jpg";
                }
                await _albumService.InsertAsync(obj);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_Create", obj);
        }

        // EDIT GET:
        [HttpGet("/Editar-Album")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.FindByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            TempData["cover"] = album.Cover.ToString();
            TempData.Keep();

            return View(album);
        }

        // Post:
        [HttpPost("/Editar-Album")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Album obj, IFormFile img)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            obj.Cover = TempData["cover"].ToString();

            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    var path = Path.Combine(_environment.WebRootPath, "albums");
                    using (var fs = new FileStream(Path.Combine(path, img.FileName), FileMode.Create))
                    {
                        obj.Cover = "~/albums/" + img.FileName;
                        await img.CopyToAsync(fs);
                    }
                }
                await _albumService.UpdateAsync(obj);
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }



        // DELETE POST: 
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var images = await _imagesService.FindByAlbumIdAsync(id);

            foreach(var file in images)
            {
                var img = file.Path.Replace("~", "wwwroot");
                System.IO.File.Delete(img);
                await _imagesService.RemoveAsync(file.Id);
            }
            Album obj = await _albumService.FindByIdAsync(id);
            var cover = obj.Cover;
            cover = cover.Replace("~", "wwwroot");
            System.IO.File.Delete(cover);

            await _albumService.RemoveAsync(id);
            return Json(true);
        }
    }

}