using System.Collections.Generic;
using System.Threading.Tasks;
using F1Backend.Models;

namespace F1Backend.Services
{
    public interface IGalleryService
    {
        Task<IEnumerable<Accomplishments>> GetAllAccomplishmentsAsync();
    }
}
