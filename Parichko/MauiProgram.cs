using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parichko.Data;
using DataAccess;

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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "parichko.db");
            builder.Services.AddDbContext<ParichkoDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
