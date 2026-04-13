using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using MvcCoreAzureStorage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string azureKeys = builder.Configuration.GetValue<string>("AzureKeys:StorageAccount");

BlobServiceClient blobServiceClient = new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);

builder.Services.AddTransient<ServiceStorageFiles>();
builder.Services.AddTransient<ServiceStorageBlobs>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
