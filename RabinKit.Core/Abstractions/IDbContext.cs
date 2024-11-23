using RabinKit.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace RabinKit.Core.Abstractions
{
    /// <summary>
    /// Контекст EF Core для приложения
    /// </summary>
    public interface IDbContext
    {
        public DbSet<TaskComponent> TaskComponents { get; }

        public DbSet<TaskAttempt> TaskAttempts { get; }

        public DbSet<TestValue> TestValues { get; set; }

        public DbSet<TaskStatusR> TaskStatus { get; set; }

        //public DbSet<TaskTestAttemptRelation> TaskTestAttemptRelations { get; set; }

        //public DbSet<PerformanceTest> PerformanceTests { get; }

        //public DbSet<PerformanceTestAttempt> PerformanceTestAttempts { get; }

        /// <summary>
        /// БД в памяти
        /// </summary>
        bool IsInMemory { get; }

        /// <summary>
        /// Очистить UnitOfWork
        /// </summary>
        void Clean();

        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Применить изменения при успешном сохранении в контекст</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Количество обновленных записей</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Количество обновленных записей</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
