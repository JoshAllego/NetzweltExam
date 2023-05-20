using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddCors(options =>
//{
     
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                     policy =>
//                     {
//                         policy.WithOrigins("https://localhost:7092/",
//                                             "http://www.contoso.com"); // add the allowed origins  
//                     });
//});


//Session service added
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use Session
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}");

app.Run();
