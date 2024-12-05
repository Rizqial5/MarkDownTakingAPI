using MarkDownTaking.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public async Task<ActionResult<MDData>> PostMdFile(MDData mDData)
        {
            _context.MDDatas.Add(mDData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMDData", new{id = mDData.Id}, mDData);
        }
            
        
    }
}
