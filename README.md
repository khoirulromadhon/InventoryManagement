Scaffold Command :
dotnet ef dbcontext scaffold "Server=localhost;Database=inventory;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context AppDbContext --output-dir Models --force
