using BoletosCore.Data;
using BoletosCore.Models;
using Microsoft.EntityFrameworkCore;

vausing Microsoft.EntityFrameworkCore;
   using BoletosCore.Data;
   using BoletosCore.Models;
   using BoletosCore.Services;

   var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<VentaService>();

var app = builder.Build();

// Configuración del pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicializar datos de prueba
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Cooperativas.Any())
    {
        context.Cooperativas.AddRange(
            new Cooperativa { Nombre = "Cooperativa A", Prioridad = 1, ComisionPorcentaje = 10 },
            new Cooperativa { Nombre = "Cooperativa B", Prioridad = 2, ComisionPorcentaje = 15 }
        );
        context.SaveChanges();
    }
}

app.Run();