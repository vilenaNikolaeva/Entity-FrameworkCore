using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(10)]
        public int PhoneNumber { get; set; }
        public DateTime RegistrationOn { get; set; }
        public DateTime? Birthday { get; set; }
        public ICollection<Homework> HomeworkSubmissions { get; set; } = new List<Homework>();
        public ICollection<StudentCourse> CourseEnrollments { get; set; } = new List<StudentCourse>();

    }
}
