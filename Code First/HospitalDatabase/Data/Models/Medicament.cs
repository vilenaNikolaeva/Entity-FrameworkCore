using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HospitalDatabase.Data.Models
{
    public class Medicament
    {
        [Key]
        public int MedicamentId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<PatientMedicament> Perscriptions { get; set; }
    }
}
