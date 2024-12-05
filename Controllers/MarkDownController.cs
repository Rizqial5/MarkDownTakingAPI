using System.ComponentModel.DataAnnotations;
using MarkDownTaking.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Markdig;
using SQLitePCL;

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
            var allFiles = await _context.MDDatas.Select(data => new ShowData
            {
                Id = data.Id,
                Title = data.Title!,
                ItemType = data.ContentType,
                ItemSize = data.FileSize


            }).ToListAsync();


            return Ok(allFiles) ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MDData>> GetById(int id)
        {
            var selectedData = await _context.MDDatas.FirstOrDefaultAsync(p=> p.Id == id);

            var showedData = new ShowData{
                Id = selectedData!.Id,
                Title = selectedData.Title,
                ItemType = selectedData.ContentType,
                ItemSize = selectedData.FileSize,
            };

            return Ok(showedData);
        }

        [HttpPost("upload")]
        public async Task<ActionResult> PostMdFile( IFormFile fileUpload)
        {
            if(fileUpload == null || fileUpload.Length == 0)  return BadRequest("No file uploaded");
            
            string permittedExtension =  ".md";

            var extension = Path.GetExtension(fileUpload.FileName).ToLowerInvariant();

            if(!permittedExtension.Contains(extension)) return BadRequest("Wrong file extension must MarkDown File");

    
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

    public class ShowData
    {
        public int Id {set;get;}
        public string? Title{set;get;}
        public string? ItemType{set;get;}
        public long ItemSize{set;get;}
    }
}
