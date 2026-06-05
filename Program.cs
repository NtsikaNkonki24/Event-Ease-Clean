using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Event_Ease_2026_Ntsika_Nkonki.Models;
using Event_Ease_2026_Ntsika_Nkonki.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var blobConnection = builder.Configuration["AzureBlobStorage"];

if (string.IsNullOrEmpty(blobConnection))
{
    throw new Exception("Azure Blob connection string is missing!");
}

builder.Services.AddSingleton(new BlobServiceClient(blobConnection));

builder.Services.AddSingleton<BlobService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
