
using DVDRental.Data;
using DVDRental.Repositories;
using DVDRental.Services;

namespace DVDRental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("Connection");

            /*builder.Services.AddScoped<IAdminDvdRepository, AdminDvdRepository>();*/
            /*builder.Services.AddScoped<IAdminCategoriesRepository, AdminCategoriesRepository>();*/
            builder.Services.AddScoped<IAdminCategoriesService, AdminCategoriesService>();
            builder.Services.AddScoped<IAdminDvdService, AdminDvdService>();
            builder.Services.AddSingleton<IAdminDvdRepository>(provider => new AdminDvdRepository(connectionString));
            builder.Services.AddScoped<IAdminCategoriesRepository>(provider => new AdminCategoriesRepository(connectionString));

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });


            var app = builder.Build();

            app.UseCors();

            var dataInitialiaze = new DataBaseInitializer(connectionString);
            dataInitialiaze.Initialize();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
