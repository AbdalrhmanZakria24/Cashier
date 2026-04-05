using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fixawy.DataAccess
{
   public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
{
    public ApplicationDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();

        optionsBuilder.UseSqlServer(
            "Data Source=.;Initial Catalog=Fixawy;Integrated Security=True;TrustServerCertificate=True"
        );

        return new ApplicationDBContext(optionsBuilder.Options);
    }
}
}
