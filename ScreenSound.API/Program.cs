using Microsoft.EntityFrameworkCore;
using ScreenSound.Dados;
using ScreenSound.Dominio.Services;
using ScreenSound.Ioc;

namespace ScreenSound.API;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connString = builder.Configuration.GetConnectionString("ScreenSoundConnection");

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddDbContext<ScreenSoundContext>(options => options.UseSqlServer(connString));
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                x => x.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        StartupIoc.ConfigureServices(builder.Services);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.Use(async (context, next) =>
            {
                await next.Invoke();

                var method = context.Request.Method;
                var allowedMethodsToCommit = new string[] { "POST", "PUT", "DELETE" };
                var unitOfWork = context.RequestServices.GetService<IUnitOfWork>();
                if (allowedMethodsToCommit.Contains(method))
                {
                    if (unitOfWork != null) await unitOfWork.Commit();
                }
            });
        }
        
        app.UseCors("AllowSpecificOrigin");
        
        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}