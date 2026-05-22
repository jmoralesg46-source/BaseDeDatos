using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KarateAPI.Data;
using KarateAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KarateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtletasController : ControllerBase
    {
        private readonly KarateDbContext _context;

        public AtletasController(KarateDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de todos los atletas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Atleta>>> GetAtletas()
        {
            return await _context.Atletas
                .Include(a => a.Horario)
                .Include(a => a.Grado)
                .Include(a => a.Encargado)
                .Include(a => a.Institucion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un atleta específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Atleta>> GetAtleta(int id)
        {
            var atleta = await _context.Atletas
                .Include(a => a.Horario)
                .Include(a => a.Grado)
                .Include(a => a.Encargado)
                .Include(a => a.Institucion)
                .FirstOrDefaultAsync(a => a.IdAtleta == id);

            if (atleta == null)
                return NotFound(new { message = "Atleta no encontrado" });

            return atleta;
        }

        /// <summary>
        /// Crea un nuevo atleta
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Atleta>> PostAtleta(Atleta atleta)
        {
            _context.Atletas.Add(atleta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAtleta), new { id = atleta.IdAtleta }, atleta);
        }

        /// <summary>
        /// Actualiza un atleta existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAtleta(int id, Atleta atleta)
        {
            if (id != atleta.IdAtleta)
                return BadRequest(new { message = "ID no coincide" });

            _context.Entry(atleta).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtletaExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un atleta
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAtleta(int id)
        {
            var atleta = await _context.Atletas.FindAsync(id);
            if (atleta == null)
                return NotFound();

            _context.Atletas.Remove(atleta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AtletaExists(int id)
        {
            return _context.Atletas.Any(e => e.IdAtleta == id);
        }
    }
}
