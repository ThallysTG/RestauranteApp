using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// services
builder.Services.AddRazorPages(options =>
{
    // Protegidas (precisa estar logado)
    options.Conventions.AuthorizeFolder("/Pedidos");
    options.Conventions.AuthorizeFolder("/Reservas");
    options.Conventions.AuthorizeFolder("/Mesas");
    options.Conventions.AuthorizeFolder("/Ingredientes");
    options.Conventions.AuthorizeFolder("/SugestoesDoChefe");
    options.Conventions.AuthorizeFolder("/Enderecos");     // se quiser endereços privados

    // Públicas
    options.Conventions.AllowAnonymousToFolder("/ItensCardapio"); // cardápio
    options.Conventions.AllowAnonymousToPage("/Index");           // Home
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PedidoPricingService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages().WithStaticAssets();
await DbInitializer.SeedAsync(app.Services);

app.Run();
