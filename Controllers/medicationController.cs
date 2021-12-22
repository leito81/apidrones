using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apidrones.Data;
using apidrones.Model;

namespace apidrones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class medicationController : ControllerBase
    {
        private readonly ApidronesContext _context;

        public medicationController(ApidronesContext context)
        {
            _context = context;
        }

        // GET: api/medication/all
        [HttpGet("all")]
        public IEnumerable<MedicationClass> GetMedication()
        {
            return _context.Medication;
        }

        // GET: api/medication/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicationClass([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicationClass = await _context.Medication.FindAsync(id);

            if (medicationClass == null)
            {
                return NotFound();
            }

            return Ok(medicationClass);
        }

        // PUT: api/medication/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicationClass([FromRoute] string id, [FromBody] MedicationClass medicationClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicationClass.Code)
            {
                return BadRequest();
            }

            _context.Entry(medicationClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicationClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/medication
        [HttpPost]
        public async Task<IActionResult> PostMedicationClass([FromBody] MedicationClass medication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Medication.Find(medication.Code) != null)
              return StatusCode(StatusCodes.Status500InternalServerError, "The medication exists in the database");

            _context.Medication.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicationClass", new { id = medication.Code }, medication);
        }

        // DELETE: api/medication/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicationClass([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicationClass = await _context.Medication.FindAsync(id);
            if (medicationClass == null)
            {
                return NotFound();
            }

            _context.Medication.Remove(medicationClass);
            await _context.SaveChangesAsync();

            return Ok(medicationClass);
        }

        private bool MedicationClassExists(string id)
        {
            return _context.Medication.Any(e => e.Code == id);
        }
    }
}