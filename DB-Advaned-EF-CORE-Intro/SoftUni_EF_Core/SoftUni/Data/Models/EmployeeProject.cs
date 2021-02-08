using System;
using System.Collections.Generic;

namespace SoftUni.Data.Models
{
    public partial class EmployeeProjects
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
    }
}
