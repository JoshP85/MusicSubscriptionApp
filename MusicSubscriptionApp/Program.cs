using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.WebHost.UseUrls("http://*:5000");

var credentials = new BasicAWSCredentials(
    File.ReadAllText(@"access-key.txt"), File.ReadAllText(@"private-key.txt"));

var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = RegionEndpoint.APSoutheast2
};

var client = new AmazonDynamoDBClient(credentials, config);

builder.Services.AddSingleton<IAmazonDynamoDB>(client);

builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

var configS3 = new AmazonS3Config()
{
    RegionEndpoint = RegionEndpoint.APSoutheast2
};

var clientS3 = new AmazonS3Client(credentials, configS3);

builder.Services.AddSingleton<IAmazonS3>(clientS3);

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();