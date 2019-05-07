using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Service
{
    public class ImagesService
    {
        private readonly Context _context;

        public ImagesService(Context context)
        {
            _context = context;
        }

        // Listar todos:
        public async Task<List<Images>> FindByAlbumIdAsync(int id)
        {
            return await _context.Images.Where(i => i.AlbumId == id).ToListAsync();
        }

        // Buscar por Id:
        public async Task<Images> FindByIdAsync(int? id)
        {
            return await _context.Images.FirstOrDefaultAsync(t => t.Id == id);
        }

        // INSERIR:
        public async Task InsertAsync(Images obj)
        {
            _context.Images.Add(obj);
            await _context.SaveChangesAsync();
        }

        public int Count()
        {
            return  _context.Images.Count();
        }

        // REMOVER:
        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Images.FindAsync(id);
            _context.Remove(obj);
            await _context.SaveChangesAsync();

        }
    }
}
