var employees = MockData.GetEmployees();
var projects = MockData.GetProjects();
var departments = MockData.GetDepartments();

//exercise 1.

    // method syntax
var empOver1000Sal = employees
    .Where(e => e.Salary > 10000)
    .Select(e => $"{e.FirstName} {e.LastName}")
    .ToList();
string res = String.Join(", ", empOver1000Sal);
Console.WriteLine($"Exercise 1: {res}");

    // query syntax


//exercise 2.
    // method syntax
var secondPage = employees
    .Skip(3)
    .Take(3)
    .OrderByDescending(e => e.Salary)
    .OrderBy(e => e.LastName)
    .ToList();
Console.WriteLine($"Exercise 2: \n\t{String.Join(",\n\t", secondPage)}");

// exercise 3.

    // method syntax
var groupedByDepartment = employees
    .GroupBy(e => e.DepartmentId)
    .Select(g => new
    {
        DepartmentId = g.Key,
        EmployeesCount = g.Count(),
        AverageSalary = g.Average(e => e.Salary)
    })
    .ToList();

Console.WriteLine($"Exercise 3: \n\t" +
    $"{String.Join("\n\t", groupedByDepartment)}");

    // query syntax


// exercise 4.
    //method syntax
var uniqueSkills = employees
    .SelectMany(e => e.Skills)
    .Distinct()
    .ToList();

Console.WriteLine($"Exercise 4: {String.Join(", ", uniqueSkills)}");

//query syntax

// exercise 5.
// method syntax
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

Console.WriteLine($"Exercise 5: \n\t{String.Join("\n\t", enameDname)}");

//query syntax

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

Console.WriteLine($"Exercise 6: \n\t{String.Join("\n\t", deptEmp)}");

// exercise 7.

(bool AnyFsharp, bool AllOver4k) fsharpAnyOver4kAll = (
    employees.Any(e => e.Skills.Contains("F#")),
    employees.All(e => e.Salary > 4000m)
);

Console.WriteLine($"Exercise 7: \n\tThere is an employee with F# skill: " +
    $"{fsharpAnyOver4kAll.AnyFsharp}\n\tAll employees earn more than $4000 " +
    $"a month: {fsharpAnyOver4kAll.AllOver4k}");


