# Server Side

OcphAuthServer [![NuGet](https://img.shields.io/nuget/v/dotnet-markdown.svg?maxAge=2592000?style=flat-square)](https://www.nuget.org/packages/OcphAuthServer/)

    1. Add Service 
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddOcphAuth<ApplicationDbContext>(connectionString);

    2. Create AplicationDbContext
           public class ApplicationDbContext : OcphAuthServer.OcphAuthContext
            {
                public ApplicationDbContext(DbContextOptions options) : base(options)
                {
                }
            }

    3. Create Seed

        - Create Seed Initializatioan 
            ```
            public class DbInitial
                {
                    private ApplicationDbContext dbcontext;
                    private IUserManager userManager;

                    public DbInitial(ApplicationDbContext dbcontext, IUserManager userManager)
                    {
                        this.dbcontext = dbcontext;
                        this.userManager = userManager;
                    }

                    internal async Task Init()
                    {
                        //Add Roles
                        if (!dbcontext.Roles.Any())
                        {
                            dbcontext.Roles.Add(new Role { Name = "Admin" });
                            dbcontext.Roles.Add(new Role { Name = "Customer" });
                            dbcontext.Roles.Add(new Role { Name = "Customer" });
                            dbcontext.SaveChanges();
                        }


                        //Add User

                        if(!dbcontext.Users.Any())
                        {
                            var user = new User { UserName = "ocph23@gmail.com", Email = "ocph23@gmail.com"  };
                            var result = await userManager.Register(user, "Sony@77");
                            if(result!=null)
                            {
                                //add role to user
                            var roleResult = await userManager.AddToRoles(user, "Admin");
                            }
                        }
                    }
                }
            ```

        -  Call dbseed

            ```
            //dbseed
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbcontext = services.GetService<ApplicationDbContext>();
                    IUserManager userManager = services.GetService<IUserManager>();

                    if (dbcontext != null)
                    {
                        dbcontext.Database.Migrate();

                        var dbInitial = new DbInitial(dbcontext, userManager);
                        dbInitial.Init();
                    }


                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            ```






    4. 



# Client Blazor

    1. Add Service 
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddOcphAuthView();

    2. On App.razor
        ```
        <CascadingAuthenticationState>
            <Router AppAssembly="@typeof(App).Assembly" AdditionalAssemblies="new [] {typeof(OcphAuthBlazorView.AppOcphAuthBlazor).Assembly}">
                <Found Context="routeData">
                    <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" >
                        <NotAuthorized>
                            <ErrorNotAuthorized />
                        </NotAuthorized>
                    </AuthorizeRouteView>
                    <FocusOnNavigate RouteData="@routeData" Selector="h1" />
                </Found>
                <NotFound>
                    <PageTitle>Not found</PageTitle>
                    <LayoutView Layout="@typeof(MainLayout)">
                        <ErrorNotFoundAddress />
                    </LayoutView>
                </NotFound>
            </Router>
        </CascadingAuthenticationState>

        ```
    3. On _import.razor add
        ```
        @using Microsoft.AspNetCore.Components.Authorization
        @using OcphAuthBlazorView.Accounts
        @using OcphAuthBlazorView.Shared
        ```
    4. 

