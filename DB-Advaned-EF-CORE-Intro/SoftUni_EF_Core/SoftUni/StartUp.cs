using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Data.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();
            var result = RemoveTown(context);

            Console.WriteLine(result);
           
        }
        // Problem 1
        public static string GetEmployeeFullInformation(SoftUniContext context)
        {
            var sb= new StringBuilder();

            var employees = context.Employees
               .OrderBy(e => e.EmployeeId)
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   e.MiddleName,
                   e.JobTitle,
                   e.Salary
               });

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} " +
                    $"{employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        // Problem 2
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                });

            foreach (var empl in employees)
            {
                if (empl.Salary>50000)
                {
                    sb.AppendLine($"{empl.FirstName} - {empl.Salary:F2}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 3

        public static string GetEmployeeFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToList();
            foreach (var e in employee)
            {
                sb.AppendLine(
                    $"{e.FirstName} {e.LastName} from {e.DepartmentName} - {e.Salary:f2}");
            }
           
            return sb.ToString().TrimEnd();
        }

        // Problem 4

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var newAddres = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(newAddres);

            context.SaveChanges();

            var addressText = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => new
                {
                   e.Address.AddressText
                })
                .Take(10)
                .ToList();

            foreach (var at in addressText)
            {
                sb.AppendLine(at.AddressText);
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 5
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();
            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001
                  && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName=e.FirstName,
                    LastName=e.LastName,
                    ManegerFirstName = e.Manager.FirstName,
                    ManegerLastName = e.Manager.LastName,
                    Project = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStartDate = ep.Project.StartDate,
                        ProjectEndDate = ep.Project.EndDate
                    })
                })
                .Take(10);
            var manegerResult = new StringBuilder();

            foreach (var empl in employees)
            {
                manegerResult.AppendLine($"{empl.FirstName} {empl.LastName} - Maneger: {empl.ManegerFirstName} {empl.ManegerLastName}");
                foreach (var project in empl.Project)
                {
                    var startDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEndDate.HasValue ?
                        project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";
                    manegerResult.AppendLine($"-- {project.ProjectName} - {startDate} - {endDate}");
                }
            }
            return manegerResult.ToString().TrimEnd();
        }

        // Problem 6
        public static string GetAddressByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Addresses
                .GroupBy(a => new
                {
                    a.AddressId,
                    a.AddressText,
                    a.Town.Name
                },
                (key, group) => new
                {
                    addresText = key.AddressText,
                    Town = key.Name,
                    Count = group.Sum(a => a.Employees.Count)
                })
                .OrderByDescending(o => o.Count)
                .ThenBy(o => o.Town)
                .ThenBy(o => o.addresText)
                .Take(10)
                .ToList();

            foreach (var empl in employees)
            {
                sb.AppendLine($"{empl.addresText}, {empl.Town} - {empl.Count} employees");
            }
            return sb.ToString().TrimEnd();

        }

        //Problem 7 
        public static string GetEmployee146(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Project = e.EmployeesProjects
                        .Select(ep => ep.Project.Name)
                        .OrderBy(p => p)
                        .ToList()
                })
               .First();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - " +
                $"{employee.JobTitle}{Environment.NewLine}{string.Join(Environment.NewLine, employee.Project)}");
            
            return sb.ToString().TrimEnd();     
        }

        //Problem 8 
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                   dName= d.Name,
                   ManegerFirtName= d.Manager.FirstName,
                   ManegerLastname= d.Manager.LastName,
                    Employees = d.Employees
                        .Select(e => new
                        {
                            eFirstName = e.FirstName,
                            eLastName = e.LastName,
                            eJobTitle = e.JobTitle
                        })
                })
                .OrderBy(d => d.Employees.Count())
                .ThenBy(d => d.dName)
                .ToList();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.dName} - {d.ManegerFirtName} {d.ManegerLastname}{Environment.NewLine}");
               
                foreach (var e in d.Employees.OrderBy(e=>e.eFirstName).ThenBy(e=>e.eLastName))
                {
                    sb.AppendLine($"{e.eFirstName} {e.eLastName} -{e.eJobTitle}");
                }
                sb.Append(new string('-', 10));
            }
            return sb.ToString().TrimEnd();
        }

        // Problem 9
        public static string FindLatestProject(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(s => new
                {
                    projectName = s.Name,
                    projectDescription = s.Description,
                    projectStartDate = s.StartDate
                })
                .OrderBy(n => n.projectName)
                .ToArray();

            foreach (var p in projects)
            {
                var startDate = p.projectStartDate.ToString("m/d/yyyy h:mm:ss tt");
                sb.AppendLine($"{p.projectName}");
                sb.AppendLine($"{p.projectDescription}");
                sb.AppendLine($"{startDate}");
            }
            return sb.ToString().TrimEnd();
        }

        // Problem 10
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                           e.Department.Name == "Tool Design" ||
                           e.Department.Name == "Marketing" ||
                           e.Department.Name == "Information Services");
            foreach (var e in employees)
            {
                e.Salary *= 1.12m;
            }

            context.SaveChanges();

            var result = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            foreach (var e in result)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.Salary:f2}");

            }
            return sb.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetemployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12

        public static string DeleteProjectById(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var project = context.Projects
                .First(p => p.ProjectId == 2);

            context.EmployeesProjects
                .ToList()
                .ForEach(ep => context.EmployeesProjects.Remove(ep));
            context.Projects.Remove(project);

            context.SaveChanges();
            context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList()
                .ForEach(p => sb.AppendLine($"{p}"));

            return sb.ToString().TrimEnd();
                
        }

        //Problem 13

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context
                .Towns
                .First(t => t.Name == "Seattle");

            var addressesInTown = context.
                  Addresses
                  .Where(a => a.Town == town);

            var employeesToRemoveAddresses = context
                .Employees
                .Where(e => addressesInTown.Contains(e.Address));

            foreach (var e in employeesToRemoveAddresses)
            {
                e.AddressId = null;
            }

            foreach (var a in addressesInTown)
            {
                context.Addresses.Remove(a);
            }
            var addressesCount = addressesInTown.Count();

            context.Towns
                .Remove(town);

            context.SaveChanges();

            return $"{addressesCount} addresses on Seattle were deleted";
        }
    }

}
