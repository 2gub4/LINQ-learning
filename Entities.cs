public record Department(int Id, string Name);
public record Employee(int Id, string FirstName, string LastName, decimal Salary, int DepartmentId, List<string> Skills);
public record Project(int Id, string Name, List<int> EmployeeIds);
