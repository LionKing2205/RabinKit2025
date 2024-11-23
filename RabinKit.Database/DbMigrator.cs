using RabinKit.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RabinKit.Database
{
    /// <summary>
    /// Мигратор
    /// </summary>
    public class DbMigrator
    {
        private readonly EfContext _documentDbContext;
        private readonly ILogger<DbMigrator> _logger;
        private readonly IDbSeeder _dbSeeder;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dbContext">Контекст БД</param>
        /// <param name="logger">Логгер</param>
        public DbMigrator(
            EfContext dbContext,
            ILogger<DbMigrator> logger,
            IDbSeeder dbSeeder)
        {
            _documentDbContext = dbContext;
            _logger = logger;
            _dbSeeder = dbSeeder;
        }

        /// <summary>
        /// Мигрировать БД
        /// </summary>
        /// <returns>Ничего</returns>
        public async Task MigrateAsync()
        {
            var operationId = Guid.NewGuid().ToString()[..4];
            _logger.LogInformation("UpdateDatabase:{operationId}: starting...", operationId);
            try
            {
                await _documentDbContext.Database.MigrateAsync().ConfigureAwait(false);
                await _dbSeeder.SeedAsync();
                _logger.LogInformation("UpdateDatabase:{operationId}: successfully done", operationId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "UpdateDatabase:{operationId}: failed", operationId);
                throw;
            }
        }
    }
}
