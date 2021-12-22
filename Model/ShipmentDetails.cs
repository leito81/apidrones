using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apidrones.Model
{
    public class ShipmentDetails
    {
        //relations 
        public String MedicationId { get; set; }
        public MedicationClass Medication { get; set; }

        public int IdShipment { get; set; }
        public ShipmentClass Shipment { get; set; }


        public int Quantity { get; set; }
    }
}
