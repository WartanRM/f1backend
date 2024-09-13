using Microsoft.AspNetCore.Mvc;
using F1Backend.Services;

namespace F1Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccomplishments()
        {
            var accomplishments = await _galleryService.GetAllAccomplishmentsAsync();
            return Ok(accomplishments);
        }
    }
}
