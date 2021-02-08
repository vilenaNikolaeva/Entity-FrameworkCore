using P01_StudentSystem.Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }
        [StringLength(50)]
        public int Name { get; set; }
        public string Url { get; set; }
        public RecourceType RecourceType { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }

}
