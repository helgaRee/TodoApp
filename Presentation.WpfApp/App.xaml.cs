using Microsoft.Extensions.Hosting;
using Infrastructure.Contexts;
using System.Configuration;
using System.Data;
using System.Windows;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Presentation.WpfApp
{

    public partial class App : Application
    {
        private IHost builder;

        public App()
        {
            builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\EC\SQL\TodoApp\Infrastructure\Data\local_database.mdf;Integrated Security=True;Connect Timeout=30"));

                services.AddScoped<CategoryRepository>();
                services.AddScoped<TaskRepository>();
                services.AddScoped<UserRepository>();
                services.AddScoped<TaskService>();
                services.AddScoped<CalendarRepository>();
                //Sinlgeton för att den ska vara konstant igång hela tiden
                services.AddSingleton<MainWindow>();
            }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            builder.Start();

            var mainWindow = builder.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
