using ABCRetailStorageApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register services correctly
builder.Services.AddSingleton<TableStorageService>();
builder.Services.AddSingleton<QueueStorageService>();
builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddSingleton<FileStorageService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
