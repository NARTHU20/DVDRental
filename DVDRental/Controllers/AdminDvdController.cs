using DVDRental.DTOs.RequestDTO;
using DVDRental.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DVDRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDvdController : ControllerBase
    {

        private readonly IAdminDvdService _dvdService;

        public AdminDvdController(IAdminDvdService dvdService)
        {
            _dvdService = dvdService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dvds = await _dvdService.GetAllAsync();
            return Ok(dvds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var dvd = await _dvdService.GetByIdAsync(id);
            if (dvd == null)
            {
                return NotFound();
            }
            return Ok(dvd);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] DVDRequestDTO movieDvdRequest)
        {
           /* if (movieDvdRequest.Image != null && movieDvdRequest.Image.Length > 0)
            {
                var filePath = Path.Combine("path_to_save", movieDvdRequest.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await movieDvdRequest.Image.CopyToAsync(stream);
                }
            }*/

            /**/
            if (movieDvdRequest.Image != null && movieDvdRequest.Image.Length > 0)
            {
               
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Path.GetFileName(movieDvdRequest.Image.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await movieDvdRequest.Image.CopyToAsync(stream);
                }
            }

            await _dvdService.AddAsync(movieDvdRequest);
            return Ok();
        }



       /* [HttpPost]
        public async Task<IActionResult> Add([FromBody] DVDRequestDTO movieDvdRequest)
        {
            await _dvdService.AddAsync(movieDvdRequest);
            return Ok();
        }*/

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] DVDRequestDTO movieDvdRequest)
        {
            await _dvdService.UpdateAsync(id, movieDvdRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dvdService.DeleteAsync(id);
            return NoContent();
        }

    }
}
