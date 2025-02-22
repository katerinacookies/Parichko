using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parichko.Data;
using DataAccess;
using Parichko.ViewModels;
using Parichko.Views;

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

            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<Register>();


            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "ParichkoDb.db");
            
            //string dbPath = Path.Combine("/data/data/com.parichko/databases", "ParichkoDb.db");
            //string dbPath = Path.Combine("C:\\Parichko\\DataAccess\\bin\\Debug\\net8.0", "ParichkoDb.db");
            builder.Services.AddDbContext<ParichkoDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));

            builder.Services.AddSingleton<ParichkoDbContext>(provider =>
                new ParichkoDbContext(dbPath)); //  Pass the path to the context constructor!
            

            //builder.Services.AddDbContext<ParichkoDbContext>(options =>
            //  options.UseSqlite($"Data Source={dbPath}"));


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
