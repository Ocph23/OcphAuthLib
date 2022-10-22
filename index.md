# Server Side

    1. Add Service 
        ```
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddOcphAuth<ApplicationDbContext>(connectionString);

        ```
    2. Create AplicationDbContext
        ```C#
           public class ApplicationDbContext : OcphAuthServer.OcphAuthContext
            {
                public ApplicationDbContext(DbContextOptions options) : base(options)
                {
                }
            }
        ```

      
