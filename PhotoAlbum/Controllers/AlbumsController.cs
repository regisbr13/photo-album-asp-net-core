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
        private IHostingEnvironment _environment;

        public AlbumsController(AlbumService albumService, IHostingEnvironment environment)
        {
            _albumService = albumService;
            _environment = environment;
        }

        // GET:
        public async Task<IActionResult> Index()
        {
            return View(await _albumService.FindAllAsync());
        }

        // GET:
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

        // CREATE POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Album obj, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if(img != null)
                {
                    var path = Path.Combine(_environment.WebRootPath, "Images");
                    using (var fs = new FileStream(Path.Combine(path, obj.Name), FileMode.Create))
                    {
                        await img.CopyToAsync(fs);
                        obj.Cover = path + obj.Name;
                    }
                }
                await _albumService.InsertAsync(obj);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_Create", obj);
        }

        // EDIT GET:
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
            return View(album);
        }

        // Post:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Album obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _albumService.UpdateAsync(obj);
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        // DELETE POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _albumService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}