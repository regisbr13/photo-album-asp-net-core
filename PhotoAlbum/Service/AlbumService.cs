using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Service
{
    public class AlbumService
    {
        private readonly Context _context;

        public AlbumService(Context context)
        {
            _context = context;
        }

        // Listar todos:
        public async Task<List<Album>> FindAllAsync()
        {
            return await _context.Albums.ToListAsync();
        }

        // Buscar por Id:
        public async Task<Album> FindByIdAsync(int? id)
        {
            return await _context.Albums.FirstOrDefaultAsync(t => t.Id == id);
        }

        // INSERIR:
        public async Task InsertAsync(Album obj)
        {
            _context.Albums.Add(obj);
            await _context.SaveChangesAsync();
        }

        // Atualizar:
        public async Task UpdateAsync(Album obj)
        {
            bool hasAny = await _context.Albums.AnyAsync(t => t.Id == obj.Id);
            if (!hasAny)
            {
                throw new Exception("not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // REMOVER:
        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Albums.FindAsync(id);
            _context.Remove(obj);
            await _context.SaveChangesAsync();

        }

        public int Count()
        {
            return _context.Albums.Count();
        }
    }
}
