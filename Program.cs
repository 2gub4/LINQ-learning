var employees = MockData.GetEmployees();
var projects = MockData.GetProjects();
var departments = MockData.GetDepartments();

//exercise 1.
Console.WriteLine($"\nExercise 1:");

var empOver1000Sal = employees
    .Where(e => e.Salary > 10000)
    .Select(e => $"{e.FirstName} {e.LastName}")
    .ToList();
string res = string.Join(", ", empOver1000Sal);

var empOver1000SalQuery =
    from emp in employees
    where emp.Salary > 10000
    select $"{emp.FirstName} {emp.LastName}";

Console.WriteLine($"\t (method syntax): {res}");
Console.WriteLine($"\t (query syntax): " +
    $"{string.Join(", ", empOver1000SalQuery)}");

//exercise 2.
Console.WriteLine($"\nExercise 2:");

var secondPage = employees
    .OrderByDescending(e => e.Salary)
    .ThenBy(e => e.LastName)
    .Skip(3)
    .Take(3)
    .Select(e => $"{e.FirstName} {e.LastName}")
    .ToList();

var secondPageQuery =
    (from emp in employees
    orderby emp.Salary descending, emp.LastName
    select emp)
    .Skip(3)
    .Take(3)
    .Select(emp => $"{emp.FirstName} {emp.LastName}");

Console.WriteLine($"\t(method syntax): \n\t\t" +
    $"{string.Join(",\n\t\t", secondPage)}");
Console.WriteLine($"\t(query/hybrid syntax): \n\t\t" +
    $"{string.Join(",\n\t\t", secondPageQuery)}");

// exercise 3.
Console.WriteLine($"\nExercise 3:");

var groupedByDepartment = employees
    .GroupBy(e => e.DepartmentId)
    .Join(
        departments,
        g => g.Key,
        d => d.Id,
        (g, d) => new
        {
            DepartmentId = g.Key,
            DepartmentName = d.Name,
            EmployeesCount = g.Count(),
            AverageSalary = g.Average(e => e.Salary)
        }
    )
    .ToList();

var groupedByDepartmentQuery =
    from emp in employees
    group emp by emp.DepartmentId into g
    join dept in departments on g.Key equals dept.Id
    select new
    {
        DepartmentId = g.Key,
        DepartmentName = dept.Name,
        EmployeesCount = g.Count(),
        AverageSalary = g.Average(e => e.Salary)
    };

Console.Write("\t(method syntax):\n");
foreach (var g in groupedByDepartment)
{
    Console.WriteLine($"\t\tdepartment ID: " +
        $"{g.DepartmentId}, department name: " +
        $"{g.DepartmentName}, employees count: " +
        $"{g.EmployeesCount}, average salary: " +
        $"{g.AverageSalary}"
    );
}

Console.WriteLine("\t(query/hybrid syntax):)");
foreach (var g in groupedByDepartmentQuery)
{
    Console.WriteLine($"\t\tdepartment ID: {g.DepartmentId}, " +
        $"department name: {g.DepartmentName}, " +
        $"employees count: {g.EmployeesCount}, average salary:" +
        $" {g.AverageSalary}"
    );
}

// exercise 4.
Console.WriteLine($"\nExercise 4:");

var uniqueSkills = employees
    .SelectMany(e => e.Skills)
    .Distinct()
    .ToList();

var uniqueSkillsQuery =
    (from emp in employees
     from skill in emp.Skills
     select skill)
    .Distinct();

Console.WriteLine($"\t(method syntax): " +
    $"{string.Join(", ", uniqueSkills)}");
Console.WriteLine($"\t(query/hybrid syntax): " +
    $"{string.Join(", ", uniqueSkillsQuery)}");

// exercise 5.
Console.WriteLine($"\nExercise 5:");
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
    .Select(j => $"{j.EmployeeName} -> {j.DepartmentName}");

var enameDnameQuery =
    (from emp in employees
    join dept in departments on emp.DepartmentId equals dept.Id
    select new
    {
        EmployeeName = emp.FirstName,
        DepartmentName = dept.Name
    })
    .Select(j => $"{j.EmployeeName} -> {j.DepartmentName}");

Console.WriteLine($"\t(method syntax):\n\t\t" +
    $"{string.Join("\n\t\t", enameDname)}");
Console.WriteLine($"\t(query/hybrid syntax):\n\t\t" +
    $"{string.Join("\n\t\t", enameDnameQuery)}");



// exercise 6.
Console.WriteLine($"\nExercise 6:");

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
    .Select(gjr => $"{gjr.DeptName} <-- {gjr.EmployeeName}")
    .ToList();

var deptEmpQuery =
    from dept in departments
    join emp in employees on dept.Id equals emp.DepartmentId into empg
    from emp in empg.DefaultIfEmpty()
    select $"{dept.Name} <-- {emp?.FirstName ?? "No Employees"}";


Console.WriteLine($"\t (method syntax): \n\t\t{string.Join("\n\t\t", deptEmp)}");
Console.WriteLine($"\t (query/hybrid syntax): \n\t\t" +
    $"{string.Join("\n\t\t", deptEmpQuery)}");


// exercise 7.
Console.WriteLine($"\nExercise 7:");

(bool AnyFsharp, bool AllOver4k) fsharpAnyOver4kAll = (
    employees.Any(e => e.Skills.Contains("F#")),
    employees.All(e => e.Salary > 4000m)
);

(bool, bool) fsharpAnyOver4kAllQuery = (
    (from emp in employees
     where emp.Skills.Contains("F#")
     select emp).Any(),
    !(from emp in employees
     where emp.Salary <= 4000m
     select emp).Any()
);

Console.WriteLine($"\t(method syntax): \n\t\tThere is an employee with F#" +
    $" skill: {fsharpAnyOver4kAll.AnyFsharp}\n\tAll employees earn more" +
    $" than $4000 a month: {fsharpAnyOver4kAll.AllOver4k}"
);

Console.WriteLine($"\t(query/hybrid syntax): \n\t\tThere is an employee " +
    $"with F# skill: {fsharpAnyOver4kAllQuery.Item1}" +
    $"\n\t\tAll employees earn more than $4000 a month: " +
    $"{fsharpAnyOver4kAllQuery.Item2}"
);


// exercise 8.
Console.WriteLine("\nExercise 8:");

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

var empIdsAQuery =
    from proj in projects
    where proj.Id == 1
    from empid in proj.EmployeeIds
    select empid;

var empIdsBQuery =
    from proj in projects
    where proj.Id == 2
    from empid in proj.EmployeeIds
    select empid;

var projectAEmpsQuery =
    from empId in empIdsAQuery
    join emp in employees on empId equals emp.Id
    select $"{emp.Id} {emp.FirstName} {emp.LastName}";

var projectBEmpsQuery =
    from empId in empIdsBQuery
    join emp in employees on empId equals emp.Id
    select $"{emp.Id} {emp.FirstName} {emp.LastName}";

var bothProjectsEmployeesQuery =
    from peid in empIdsAQuery.Intersect(empIdsBQuery)
    join emp in employees on peid equals emp.Id
    select $"{emp.Id} {emp.FirstName} {emp.LastName}";

var singleProjectEmployeesQuery =
    from peid in empIdsAQuery.Union(empIdsBQuery).Except(empIdsAQuery.Intersect(empIdsBQuery))
    join emp in employees on peid equals emp.Id
    select $"{emp.Id} {emp.FirstName} {emp.LastName}";

Console.WriteLine("\t(method syntax):\n\t\t" +
$"Employees assigned to project A: " + $"{string.Join(
            ", ",
            projectAEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )}" + $"\n\t\tEmployees assigned to project B: " + $"{string.Join(
            ", ",
            projectBEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )}" + $"\n\t\tEmployees assigned to both projects: " + $"{string.Join(
            ", ",
            bothProjectsEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )}" + $"\n\t\tEmployees assigned to only one project: " + $"{string.Join(
            ", ",
            singleProjectEmployees
                .Select(e => $"{e.Id} {e.FirstName} {e.LastName}")
                .ToList()
        )}"
);

Console.WriteLine("\t(query/hybrid syntax):\n\t\t" +
$"Employees assigned to project A: " + 
    $"{string.Join(
        ", ", projectAEmpsQuery
    )}" + $"\n\t\tEmployees assigned to project B: " + $"{string.Join(
        ", ", projectBEmpsQuery
    )}" + $"\n\t\tEmployees assigned to both projects: " + $"{string.Join(
        ", ", bothProjectsEmployeesQuery
    )}" + $"\n\t\tEmployees assigned to only one project: " + $"{string.Join(
        ", ", singleProjectEmployeesQuery
    )}"
);

//exercise 9.
Console.WriteLine($"\nExercise 9:");

var empNamesString = employees
    .Select(e => e.FirstName)
    .Aggregate((n1, n2) => String.Concat(n1, ", ", n2));

var empNamesStringQuery =
    (from emp in employees
     select emp.FirstName)
    .Aggregate((n1, n2) => String.Concat(n1, ", ", n2));


Console.WriteLine($"\t(method syntax): {empNamesString}");
Console.WriteLine($"\t(hybrid syntax): {empNamesStringQuery}");



//exercise 10.
Console.WriteLine("\nExercise 10:");

var empPairs = employees
    .Select(e => $"{e.FirstName} {e.LastName}")
    .Chunk(2)
    .ToList();

var empPairsQuery =
    (from emp in employees
     select $"{emp.FirstName} {emp.LastName}")
    .Chunk(2);

Console.WriteLine("\t(method syntax):");
foreach (var empPair in empPairs)
{
    Console.WriteLine("\t\t[" + string.Join(", ", empPair) + "]");
}

Console.WriteLine("\t(query/hybrid syntax):");
foreach (var empPair in empPairsQuery)
{
    Console.WriteLine("\t\t[" + string.Join(", ", empPair) + "]");
}
