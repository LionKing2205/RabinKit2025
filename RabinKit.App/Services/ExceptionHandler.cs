using RabinKit.Core.Exceptions;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace RabinKit.App.Services
{
    public class ExceptionHandler
    {
        private readonly ISnackbar _snackBar;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(
            ISnackbar snackBar,
            ILogger<ExceptionHandler> logger)
        {
            _snackBar = snackBar;
            _logger = logger;
        }

        public async Task<T?> HandleAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (ApplicationExceptionBase e)
            {
                _logger.LogWarning(e, "Ошибка");
                _snackBar.Add(e.Message, Severity.Error);
            }

            return default;
        }

        public async Task HandleAsync(
            Func<Task> func,
            Action? catchFunc = null,
            Action? finallyFunc = null,
            bool catchAll = false)
        {
            try
            {
                await func();
            }
            catch (ApplicationExceptionBase e)
            {
                _logger.LogWarning(e, "Ошибка");
                _snackBar.Add(e.Message, Severity.Error);

                catchFunc?.Invoke();
            }
            catch (Exception e)
            {
                if (!catchAll)
                    throw;

                _logger.LogWarning(e, "Ошибка");
                _snackBar.Add("Ошибка", Severity.Error);

                catchFunc?.Invoke();
            }
            finally
            {
                finallyFunc?.Invoke();
            }
        }
    }
}
