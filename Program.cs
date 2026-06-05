using Azure.Storage.Blobs;
using Event_Ease_2026_Ntsika_Nkonki.Models;
using Event_Ease_2026_Ntsika_Nkonki.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var blobConnection = builder.Configuration.GetConnectionString("AzureBlobStorage");

if (!string.IsNullOrWhiteSpace(blobConnection))
{
    builder.Services.AddSingleton(new BlobServiceClient(blobConnection));
}

builder.Services.AddSingleton<BlobService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration error: " + ex.Message);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
