
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Extensions;
using WebApplication1.Profiles;

namespace WebApplication1;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddOpenApi();    
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddConnectionString(builder.Configuration);
        builder.Services.AddAutoMapper(cfg => { }, typeof(FilmeProfile));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
