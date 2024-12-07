using System.ComponentModel.DataAnnotations;
using MarkDownTaking.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Markdig;
using System.Text;
using System.Windows.Input;


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
           


            var allFiles = await _context.MDDatas.Select(data=> new ShowListData{
                Id = data.Id,
                Title = data.Title
            }).ToListAsync();

            return Ok(allFiles) ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShowData>> GetById(int id)
        {
            var selectedData = await _context.MDDatas.FirstOrDefaultAsync(p=> p.Id == id);

            var htmlConvertFile = Markdown.ToHtml(selectedData!.MDString!);

            

            var showedData = new ShowData{
                Id = selectedData!.Id,
                Title = selectedData.Title,
                ItemType = selectedData.ContentType,
                ItemSize = selectedData.FileSize,
                MarkDownFile = htmlConvertFile,
                
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

                Console.WriteLine(memoryStream.ToArray().ToString());

                var uploadedFile = new MDData
                {
                    Title = fileUpload.FileName,
                    ContentType = fileUpload.ContentType,
                    FileSize = fileUpload.Length,
                    MDFile = memoryStream.ToArray()
                };

                memoryStream.Position = 0;

                using (var reader = new StreamReader(memoryStream))
                {
                    uploadedFile.MDString = await reader.ReadToEndAsync();
                }

                _context.MDDatas.Add(uploadedFile);

                await _context.SaveChangesAsync();  

            }
            
            

            return Ok(new{message = "File Uploaded Succesfully"});
        }

        [HttpPost("generate")]
        public IActionResult GenerateMDFile(RequestContent inputText)
        {
            if(string.IsNullOrWhiteSpace(inputText.Content)) return BadRequest("Text cannot be empty");

            var byteArray = Encoding.UTF8.GetBytes(inputText.Content);

            return File(byteArray, "text/markdown",$"{inputText.NameFile}.md");
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var selectedData =await _context.MDDatas.FirstOrDefaultAsync(d=> d.Id == id);

            if(selectedData == null) return NotFound();

            _context.MDDatas.Remove(selectedData);

            await _context.SaveChangesAsync();

            return Ok("Data deleted");
            

        }
        

        
        
    }

    public class ShowData
    {
        public int Id {set;get;}
        public string? Title{set;get;}
        public string? ItemType{set;get;}
        public string? MarkDownFile{set;get;}
        public long ItemSize{set;get;}

        public string StringConveter(byte[] dataInput)
        {
            return dataInput.ToString()!;
        }
    }

    public class ShowListData
    {
        public int Id {set;get;}
        public string? Title {set;get;}
    }

    public class RequestContent
    {
        public string? NameFile {set;get;}
        public string? Content{set;get;}
    }

    public class WordCheck
    {
        public string? Word{set;get;}
    }
}
