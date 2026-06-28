public static class MockData
{
    public static List<Department> GetDepartments() => new()
    {
        new(1, "IT"),
        new(2, "HR"),
        new(3, "Sales"),
        new(4, "Marketing"),
        new(5, "Helpdesk")
    };

    public static List<Employee> GetEmployees() => new()
    {
        new(1, "Anna", "Kowalska", 15000m, 1, ["C#", "SQL", "Azure"]),
        new(2, "Jan", "Nowak", 12000m, 1, ["F#", "C#", "REST API"]),
        new(3, "Piotr", "Wiśniewski", 8000m, 2, ["Komunikacja", "Rekrutacja"]),
        new(4, "Maria", "Kamińska", 15000m, 1, ["C#", "AWS", "Docker"]),
        new(5, "Tomasz", "Zieliński", 5000m, 3, ["Negocjacje", "CRM"]),
        new(6, "Katarzyna", "Szymańska", 5000m, 1, ["SQL", "Python", "C#"]),
        new(7, "Michał", "Wójcik", 4500m, 2, ["Księgowość", "Excel"]),
        new(7, "Zbigniew", "Walesiuk", 4000m, 2, ["Excel", "Word"])
    };

    public static List<Project> GetProjects() => new()
    {
        new(1, "Backend Rewrite", [1, 2, 4, 6]),
        new(2, "Cloud Migration", [1, 4, 5])
    };
}