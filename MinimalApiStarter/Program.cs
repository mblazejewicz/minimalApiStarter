using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MinimalApiStarter.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection(AuthOptions.Name));
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSpaStaticFiles(config => config.RootPath = "/wwwroot");//"ClientApp/dist");

// Add JWT configuration
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
//add easy cache
builder.Services.AddEasyCaching(options =>
{
    options.UseInMemory("inMemoryCache");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//Endpoints
app.UseEndpoints(builder =>
{
    builder.UseAuthEndpoints();
    builder.UseMyEndpoints();
});


app.UseDefaultFiles();

if (!app.Environment.IsDevelopment())
    app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp";
        spa.UseProxyToSpaDevelopmentServer("http://localhost:5173");

    });


    Task.Run(() =>
    {

        try
        {
            // The following call to Start succeeds if test.txt exists.
            Console.WriteLine("\nTrying to launch 'text.txt'...");
            var psiNpmRunDev = new ProcessStartInfo
            {
                CreateNoWindow = true,
                FileName = "cmd",
                RedirectStandardInput = true,
                // Arguments ="start npm run dev",
                WorkingDirectory = Path.Combine(app.Environment.ContentRootPath, "ClientApp")
            };
            var pNpmRunDev = Process.Start(psiNpmRunDev);
            pNpmRunDev.StandardInput.WriteLine("npm run dev");
            pNpmRunDev.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


    });
}

app.Run();

public partial class Program { }
