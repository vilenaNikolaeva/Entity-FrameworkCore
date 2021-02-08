﻿using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HospitalDatabase.Data.Models
{
    public class Diagnose
    {
        [Key]
        public int DiagnoseId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Comments { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
