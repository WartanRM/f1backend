
using Microsoft.EntityFrameworkCore;
using F1Backend.Models;
using F1Backend.Data;

namespace F1Backend.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly F1DbContext _context;

        public GalleryService(F1DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Accomplishments>> GetAllAccomplishmentsAsync()
        {
            return await _context.accomplishments.ToListAsync();
        }
    }
}
