using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;


namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(s => new { s.StudentId, s.CoureseId });

            builder.HasOne(s => s.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(s => s.StudentId);

            builder.HasOne(s => s.Course)
                .WithMany(c => c.StudentsEnrolled)
                .HasForeignKey(s => s.CoureseId);
        }
    }
}
