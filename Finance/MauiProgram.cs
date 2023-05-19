using Empreendedores.Data.Repositories;
using Empreendedores.MVVM.Views;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Empreendedores;

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
            })
            .RegisterDatabaseAndRepositories()
            .RegisterViews();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder RegisterDatabaseAndRepositories(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LiteDatabase>(
            options =>
            {
                return new LiteDatabase($"Filename={AppSettings.DatabasePath};Connection=Shared;");
            });
        builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<TransactAdd>();
        builder.Services.AddTransient<TransactEdit>();
        builder.Services.AddTransient<TransactList>();
        return builder;
    }
}
