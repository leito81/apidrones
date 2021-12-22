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
    public class DronesController : ControllerBase
    {
        private readonly ApidronesContext _context;

        public DronesController(ApidronesContext context)
        {
            _context = context;
        }

        // GET: api/Drones
        [HttpGet]
        public IEnumerable<DroneClass> GetDrones()
        {
            return _context.Drones;
        }

        // GET: api/Drones/availables
        [HttpGet("availables")]
        public async Task<IActionResult> GetDronesAvailable()
        {
            var availables = await _context.Drones
                                            .Where(W => W.droneState.DroneStateId == DroneStateId.IDLE)
                                            .Select(S =>new {S.Serial_number, S.droneModel.ModelName, S.Weight_limit, S.Battery_capacity, S.droneState.StateName })
                                            .ToListAsync();
            return Ok(availables);
        }

        // GET: api/Drones/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDroneClass([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var droneClass = await _context.Drones.FindAsync(id);

            if (droneClass == null)
            {
                return NotFound();
            }

            return Ok(droneClass);
        }

        // GET: api/Drones/5/battery
        [HttpGet("{id}/battery")]
        public async Task<IActionResult> GetBatteryLevel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var droneClass = await _context.Drones.FindAsync(id);

            if (droneClass == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The drone not exists in the database"); ;
            }

            return Ok(new { battery_level = droneClass.Battery_capacity});
        }
        // PUT: api/Drones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDroneClass([FromRoute] string id, [FromBody] DroneClass droneClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != droneClass.Serial_number)
            {
                return BadRequest();
            }

            _context.Entry(droneClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DroneClassExists(id))
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

        // POST: api/Drones
        [HttpPost]
        public async Task<IActionResult> PostDroneClass([FromBody] DroneClass drone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Drones.Find(drone.Serial_number) != null)
               return StatusCode(StatusCodes.Status500InternalServerError, "The drone exists in the database");

            _context.Drones.Add(drone);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDroneClass", new { id = drone.Serial_number }, drone);
        }

        // POST: api/Drones/4/loading
        [HttpPost("{id}/loading")]
        public async Task<IActionResult> LoadingDrones([FromRoute] string id, [FromBody] List<ShipmentItems> medicationItems)
        {
            List<String> codes = medicationItems.Select(S => S.MedicationCode).ToList();
            List<MedicationClass> items = _context.Medication.Where(M => codes.Contains(M.Code)).ToList();
            DroneClass DronetoLoad = _context.Drones.Find(id);

            if (DronetoLoad.droneStateId != DroneStateId.IDLE)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The drone is loaded.");
            }

            int totalWeight = 0;
            foreach(MedicationClass ite in items)
            {
                int quatity = medicationItems.Find(F => F.MedicationCode == ite.Code).Quantity;
                totalWeight += quatity * ite.Weight;
            }

            if (DronetoLoad.Weight_limit < totalWeight)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The total weight of the drug items is greater than the weight supported by the drone.");
            }

            if (DronetoLoad.Battery_capacity < 25)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The drone cannot be charged. Battery capacity is less than 25 percent.");
            }

            _context.Shipments.Add(new ShipmentClass() { DroneId = id});
            _context.SaveChanges();


            foreach (ShipmentItems itemsSend in medicationItems)
            {
                _context.ShipmentsDetails.Add(new ShipmentDetails() {
                    IdShipment=_context.Shipments.Last(L => L.DroneId == id).IdShipment,
                    MedicationId=itemsSend.MedicationCode,
                    Quantity=itemsSend.Quantity
                });
            }

            DronetoLoad.droneStateId = DroneStateId.LOADED;

            await _context.SaveChangesAsync();

            return Ok("Loaded Drone");
        }

        
        [HttpGet("{id}/medicationsloaded")]
        public async Task<IActionResult> GetMedicationLoaded([FromRoute] string Id)
        {
            DroneClass drone = _context.Drones.Find(Id);
            if (drone == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The drone not exists in the database");
            }

            if (drone.droneStateId < DroneStateId.LOADED)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The drone is not loaded.");
            }

            int Shipment = _context.Shipments.Where(W => W.DroneId == Id).Max(M => M.IdShipment);
            var Medications = await _context.ShipmentsDetails
                                                .Where(W => W.IdShipment == Shipment)
                                                .Join(_context.Medication,
                                                ShipM => ShipM.MedicationId,
                                                medication => medication.Code,
                                                (ShipM, medication) => new { medication.Code, medication.Name, medication.Weight, ShipM.Quantity})
                                                .ToListAsync();

            return Ok(Medications);
        }
                      
        // DELETE: api/Drones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDroneClass([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var droneClass = await _context.Drones.FindAsync(id);
            if (droneClass == null)
            {
                return NotFound();
            }

            _context.Drones.Remove(droneClass);
            await _context.SaveChangesAsync();

            return Ok(droneClass);
        }

        private bool DroneClassExists(string id)
        {
            return _context.Drones.Any(e => e.Serial_number == id);
        }
    }

    public class ShipmentItems
    {
        public string MedicationCode { get; set; }
        public int Quantity { get; set; }
    };
}