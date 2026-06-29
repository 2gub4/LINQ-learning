var employees = MockData.GetEmployees();
var projects = MockData.GetProjects();
var departments = MockData.GetDepartments();

//exercise 1.
var empOver1000Sal = employees
    .Where(e => e.Salary > 10000)
    .Select(e => $"{e.FirstName} {e.LastName}")
    .ToList();
string res = String.Join(", ", empOver1000Sal);
Console.WriteLine($"\nExercise 1: {res}");

//exercise 2.
var secondPage = employees
    .Skip(3)
    .Take(3)
    .OrderByDescending(e => e.Salary)
    .OrderBy(e => e.LastName)
    .Select(e => $"{e.FirstName} {e.LastName}")
    .ToList();
Console.WriteLine($"\nExercise 2: \n\t{String.Join(",\n\t", secondPage)}");

// exercise 3.
var groupedByDepartment = employees
    .GroupBy(e => e.DepartmentId)
    .Select(g => new
    {
        DepartmentId = g.Key,
        EmployeesCount = g.Count(),
        AverageSalary = g.Average(e => e.Salary)
    })
    .ToList();

Console.WriteLine($"\nExercise 3: \n\t" +
    $"{String.Join("\n\t", groupedByDepartment)}");

// exercise 4.
var uniqueSkills = employees
    .SelectMany(e => e.Skills)
    .Distinct()
    .ToList();

Console.WriteLine($"\nExercise 4: {String.Join(", ", uniqueSkills)}");

// exercise 5.
var enameDname = employees
    .Join(
        departments,
        e => e.DepartmentId,
        d => d.Id,
        (e, d) => new
        {
            EmployeeName = e.FirstName,
            DepartmentName = d.Name
        }
    )
    .ToList();

Console.WriteLine($"\nExercise 5: \n\t{String.Join("\n\t", enameDname)}");


// exercise 6.
var deptEmp = departments
    .GroupJoin(
        employees,
        d => d.Id,
        e => e.DepartmentId,
        (d, e) => new
        {
            Dept = d,
            Emps = e

        }
    )
    .SelectMany(
        gjr => gjr.Emps.DefaultIfEmpty(),
        (gjr, emp) => new
        {
            DeptName = gjr.Dept.Name,
            EmployeeName = emp?.FirstName ?? "No Employees"
        }
    )
    .ToList();

Console.WriteLine($"\nExercise 6: \n\t{String.Join("\n\t", deptEmp)}");

// exercise 7.

(bool AnyFsharp, bool AllOver4k) fsharpAnyOver4kAll = (
    employees.Any(e => e.Skills.Contains("F#")),
    employees.All(e => e.Salary > 4000m)
);

Console.WriteLine($"\nExercise 7: \n\tThere is an employee with F# skill: " +
    $"{fsharpAnyOver4kAll.AnyFsharp}\n\tAll employees earn more than $4000 " +
    $"a month: {fsharpAnyOver4kAll.AllOver4k}");


// exercise 8.
var projectAEmployees = projects
    .Where(p => p.Id == 1)
    .SelectMany(p => p.EmployeeIds)
    .Join(
        employees,
        eid => eid,
        emp => emp.Id,
        (empId, emp) => emp
    );

var projectBEmployees = projects
    .Where(p => p.Id == 2)
    .SelectMany(p => p.EmployeeIds)
    .Join(
        employees,
        eid => eid,
        emp => emp.Id,
        (empId, emp) => emp
    );

var bothProjectsEmployees = projectAEmployees
    .Intersect(projectBEmployees);

var singleProjectEmployees = projectAEmployees
    .Union(projectBEmployees)
    .Except(bothProjectsEmployees)
    .OrderBy(e => e.Id);

Console.WriteLine("\nExercise 8: \n\t" +
    $"Employees assigned to project A: " + $"{
        String.Join(
            ", ", 
            projectAEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )
    }" + $"\n\tEmployees assigned to project B: " + $"{
        String.Join(
            ", ", 
            projectBEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )
    }" + $"\n\tEmployees assigned to both projects: " + $"{
        String.Join(
            ", ", 
            bothProjectsEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )
    }" + $"\n\tEmployees assigned to only one project: " + $"{
        String.Join(
            ", ", 
            singleProjectEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )
    }");

//exercise 9.
var empNamesString = employees
    .Select(e => e.FirstName)
    .Aggregate(string.Empty, (n1, n2) => String.Concat(n1, ", ", n2));

Console.WriteLine($"\nExercise 9: {empNamesString}");

//exercise 10.
var empPairs = employees
    .Select(e => $"{e.FirstName} {e.LastName}")
    .Chunk(2)
    .ToList();

Console.WriteLine("\nExercise 10:");
foreach (var empPair in empPairs)
{
    Console.WriteLine("\t[" + String.Join(", ", empPair) + "]");
}
