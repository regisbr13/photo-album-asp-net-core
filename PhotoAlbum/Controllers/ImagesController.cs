using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoAlbum.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Models;

namespace PhotoAlbum.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ImagesService _imagesService;
        private readonly IHostingEnvironment _environment;

        public ImagesController(ImagesService imagesService, IHostingEnvironment environment)
        {
            _imagesService = imagesService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int albumId, IFormFile img)
        {
            var obj = new Images() { AlbumId = albumId };
            if (img != null)
            {
                if(await _imagesService.ImgExist("~/images/" + img.FileName))
                {
                    TempData["erro"] = "Já existe uma imagem com este nome cadastrada. Tente renomeá-la e refaça o upload.";
                }
                else
                {
                    var path = Path.Combine(_environment.WebRootPath, "images");
                    using (var fs = new FileStream(Path.Combine(path, img.FileName), FileMode.Create))
                    {
                        await img.CopyToAsync(fs);
                        obj.Path = "~/images/" + img.FileName;
                        TempData["img"] = obj.Path;
                    }
                    await _imagesService.InsertAsync(obj);
                }
            }
            else
            {
                TempData["erro"] = "Você precisa escolher uma imagem para upload.";
            }
            TempData["id"] = albumId;
            TempData.Keep();
            return RedirectToAction("Details", "Albums", new { id = obj.AlbumId });
        }


        // DELETE POST: 
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            Images file = await _imagesService.FindByIdAsync(id);
            var img = file.Path.Replace("~", "wwwroot");
            System.IO.File.Delete(img);

            await _imagesService.RemoveAsync(id);
            return Json(true);
        }
    }
}