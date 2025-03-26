namespace MotorCycleRentail.Worker.Configurations;

public class ErrorHandler
{
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(ILogger<ErrorHandler> logger)
    {
        _logger = logger;
    }

    public void HandleUnhandledExceptions()
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
        {
            if (eventArgs.ExceptionObject is Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
            }
        };
    }
}

public static class ErrorHandlerExtensions
{
    public static void AddErrorHandler(this IHostApplicationBuilder builder)
    {
        var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ErrorHandler>>();
        var errorHandler = new ErrorHandler(logger);
        errorHandler.HandleUnhandledExceptions();
    }
}
