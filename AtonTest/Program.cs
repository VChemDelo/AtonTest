using AtonTest.Models;
using AtonTest.Services;
using Microsoft.EntityFrameworkCore;

namespace AtonTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AtDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<EntityService>();

            var app = builder.Build();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{*catchall}");

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });


            app.UseHttpsRedirection();


            app.Run();
        }
    }
}
