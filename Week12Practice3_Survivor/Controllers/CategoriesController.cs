using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week12Practice3_Survivor.Context;
using Week12Practice3_Survivor.Entities;

namespace Week12Practice3_Survivor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly SurvivorDbContext _context;
        public CategoriesController(SurvivorDbContext context)
        {
            _context = context;
        }


        //GET /api/categories - Tüm kategorileri listele.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryEntitiy>>> GetAll()
        {
            var category = await _context.Categories.ToListAsync();

            if (category is null || !category.Any())
                return NotFound();

            return Ok(category);
        }


        //GET /api/categories/{id} - Belirli bir kategoriyi getir.
        [HttpGet("{id}")]

        public async Task<ActionResult<CategoryEntitiy>> GetById(int id)
        {
            var category = _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return NotFound();
            return Ok(category);

        }

        //POST /api/categories - Yeni bir kategori oluştur.
        [HttpPost]
        public async Task<ActionResult<CategoryEntitiy>> Create(CategoryEntitiy newCategory)
        {
            if (newCategory is null)
                return BadRequest("Geçersiz kategori verisi");

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = newCategory.Id}, newCategory);
        }

        //PUT /api/categories/{id} - Belirli bir kategoriyi güncelle.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryEntitiy updateCategory)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (updateCategory is null)
                return NotFound($"{id} li kategori bulunamadı");
                
            category.Name = updateCategory.Name;
            category.ModifiedDate = DateTime.Now;
            category.Competitors = updateCategory.Competitors;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return StatusCode(500, "kategori güncellenirken bir hata oluştu");
            }
            return NoContent();

        }


        //DELETE /api/categories/{id} - Belirli bir kategoriyi sil.

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteCategory(int id)
        {
            // Kategoriyi veritabanında bul
            var category = await _context.Categories
                                         .Include(c => c.Competitors) // Kategoriyle ilişkilendirilmiş yarışmacıları da dahil et
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound($" {id} li kategori bulunamadı.");
            }

            // Kategoriyi soft delete ile işaretle
            category.IsDeleted = true;
            category.ModifiedDate = DateTime.Now;

            // Kategoriye bağlı tüm yarışmacıları da soft delete yap
            foreach (var competitor in category.Competitors)
            {
                competitor.IsDeleted = true;
                competitor.ModifiedDate = DateTime.Now;
            }

            try
            {
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Silinirken bir hata oluştu");
            }

            return NoContent(); 
        }

    }
}
