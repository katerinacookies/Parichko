using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parichko.Data;
using Parichko.Utilities;
using DataAccess;
using Parichko.ViewModels;
using Parichko.Views;
using Parichko.Data;

namespace Parichko
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("AtkinsonHyperlegible-Bold.ttf", "AtkinsonBold");
                    fonts.AddFont("AtkinsonHyperlegible-BoldItalic.ttf", "AtkinsonBoldItalic");
                    fonts.AddFont("AtkinsonHyperlegible-Italic.ttf", "AtkinsonItalic");
                    fonts.AddFont("AtkinsonHyperlegible-Regular.ttf", "AtkinsonRegular");
                });

            //chgpt
            
            builder.Services.AddDbContext<ParichkoDbContext>();
            var dbContext = builder.Services.BuildServiceProvider().GetRequiredService<ParichkoDbContext>();
            dbContext.Database.Migrate();
            dbContext.Dispose();
            //ot videoto
            //builder.Services.AddDbContext<ParichkoDbContext>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<Register>();
            builder.Services.AddTransient<QName>();
            builder.Services.AddTransient<QNameViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();
            //var dbContext = new ParichkoDbContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();
            //dbContext.Dispose();
            //ot videoto

            

            /*builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<Register>();

            //string dbPath = Path.Combine(@"C:\Parichko\DataAccess\Parichko.db");
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Parichkodb.db");

           // dbPath = dbPath.TrimStart('/', '\\');
           //File.SetAttributes(dbPath, FileAttributes.Normal);
           //string dbPath = Path.Combine("C:\\Parichko\\DataAccess\\bin\\Debug\\net8.0", "ParichkoDb.db");
            builder.Services.AddDbContext<ParichkoDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
                //options.UseSqlite(dbPath));*/


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
