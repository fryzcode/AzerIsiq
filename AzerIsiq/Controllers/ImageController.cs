using AzerIsiq.Services;
using AzerIsiq.Services.ILogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerIsiq.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var image = await _imageService.UploadImageAsync(file);
        return Ok(new { image.Id, image.ImageName });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        var imageBytes = await _imageService.GetImageBytesAsync(id);
        if (imageBytes.Length == 0) return NotFound();

        return File(imageBytes, "image/jpeg");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var success = await _imageService.DeleteImageAsync(id);
        return success ? Ok("Deleted") : NotFound();
    }
}
