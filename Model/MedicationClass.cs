using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace apidrones.Model
{
    public class MedicationClass
    {
        [RegularExpression("^[A-Za-z0-9-_]+$", ErrorMessage = "Characters are not allowed.")]
        public String Name { get; set; }
        public int Weight { get; set; }

        [RegularExpression("^[A-Z0-9_]+$", ErrorMessage = "Characters are not allowed.")]
        public String Code { get; set; }
        public String Image { get; set; }

        //relations
        public ICollection<ShipmentDetails> Shipments { get; set; }
    }

}
