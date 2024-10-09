using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week12Practice3_Survivor.Context;
using Week12Practice3_Survivor.Entities;

namespace Week12Practice3_Survivor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorsController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CompetitorsController(SurvivorDbContext context)
        {
            _context = context;
        }



        //GET /api/competitors - Tüm yarışmacıları listele.

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompetitorEntitiy>>> GetAll()
        {
            var competitor = await _context.Competitors.ToListAsync();

            if (competitor is null || !competitor.Any())
                return NotFound();

            return Ok(competitor);
        }

        //GET /api/competitors/{id} - Belirli bir yarışmacıyı getir.

        [HttpGet("{id}")]
        public async Task<ActionResult<CompetitorEntitiy>> GetById(int id)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(x => x.Id == id);
            if(competitor is null)
                return NotFound($" {id} li yarışmacı bulunamadı");
            return Ok(competitor);

        }

        //GET /api/competitors/categories/{CategoryId} - Kategori Id'ye göre yarışmacıları getir.

        [HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<IEnumerable<CompetitorEntitiy>>> GetByCategoryId(int CategoryId)
        {
            var competitor = await _context.Competitors
                                           .Where(c => c.CategoryId == CategoryId)
                                           .ToListAsync();

            if(competitor is null || !competitor.Any())
                return NotFound($"{CategoryId} li kategoride yarışmacı bulunamadı");
            return Ok(competitor);
                                
        }


        //POST /api/competitors - Yeni bir yarışmacı oluştur.

        [HttpPost]
        public async Task<ActionResult<CompetitorEntitiy>> Create(CompetitorEntitiy competitor)
        {
            
            _context.Competitors.Add(competitor);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = competitor.Id }, competitor);
        }

        //PUT /api/competitors/{id} - Belirli bir yarışmacıyı güncelle.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CompetitorEntitiy updateCompetitor)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(c => c.Id == id);

            if (updateCompetitor is null)
                return NotFound($"{id} li yarışmacı bulunamadı");

            competitor.FirstName = updateCompetitor.FirstName;
            competitor.LastName = updateCompetitor.LastName;
            competitor.CategoryId = updateCompetitor.CategoryId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(500, "Yarışmacı güncellenirken bir hata oluştu");
            }
            return NoContent();

        }

        //DELETE /api/competitors/{id} - Belirli bir yarışmacıyı sil.

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var competitor = await _context.Competitors.FindAsync(id);
            if(competitor is null)
                return NotFound($"{id} id li yarışmacı silinemedi");

            competitor.IsDeleted = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(500, "Yarışmacı silinirken bir hata oluştu");
            }

            return NoContent();

        }

    }
}
