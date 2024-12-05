using System.ComponentModel.DataAnnotations;
using MarkDownTaking.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Markdig;

namespace MarkDownTaking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkDownController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MarkDownController( ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MDData>>> GetAll()
        {
            return await _context.MDDatas.ToListAsync();
        }

        [HttpPost("upload")]
        public async Task<ActionResult> PostMdFile( IFormFile fileUpload)
        {
            if(fileUpload == null || fileUpload.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            using (var memoryStream = new MemoryStream())
            {
                await fileUpload.CopyToAsync(memoryStream);

                var uploadedFile = new MDData
                {
                    Title = fileUpload.FileName,
                    ContentType = fileUpload.ContentType,
                    FileSize = fileUpload.Length,
                    MDFile = memoryStream.ToArray()
                };

                _context.MDDatas.Add(uploadedFile);

                await _context.SaveChangesAsync();  

            }
            
            

            return Ok(new{message = "File Uploaded Succesfully"});
        }
            
        
    }

    public class BufferedModelUpload
    {
        [BindProperty]
        public SingleFileUpload? FileUpload {get;set;}
    }
  
    public class SingleFileUpload
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile? FormFile{get;set;}
    }
}
