using RabinKit.Core.Abstractions;
using RabinKit.Core.Entities;
using RabinKit.Core.Entities.Common;
using RabinKit.Database.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RabinKit.Database
{
    /// <summary>
    /// Контекст EF Core для приложения
    /// </summary>
    public class EfContext : DbContext, IDbContext
    {
        private const string DefaultSchema = "public";
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMediator _domainEventsDispatcher;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Параметры подключения к БД</param>
        /// <param name="userContext">Контекст текущего пользователя</param>
        /// <param name="dateTimeProvider">Провайдер даты и времени</param>
        /// <param name="domainEventsDispatcher">Медиатор для доменных событий</param>
        public EfContext(
            DbContextOptions<EfContext> options,
            IDateTimeProvider dateTimeProvider,
            IMediator domainEventsDispatcher)
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _domainEventsDispatcher = domainEventsDispatcher ?? throw new ArgumentNullException(nameof(domainEventsDispatcher));
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <inheritdoc />
        public DbSet<TaskComponent> TaskComponents { get; set; }

        /// <inheritdoc />
        public DbSet<TaskAttempt> TaskAttempts { get; set; }

        /// <inheritdoc />
        public DbSet<TestValue> TestValues { get; set; }

        /// <inheritdoc />
        public DbSet<TaskStatusR> TaskStatus { get; set; }
        /// <inheritdoc />
      //  public DbSet<TaskTestAttemptRelation> TaskTestAttemptRelations { get; set; }

        /// <inheritdoc />
      //  public DbSet<PerformanceTest> PerformanceTests { get; set; }

        /// <inheritdoc />
      //  public DbSet<PerformanceTestAttempt> PerformanceTestAttempts { get; set; }

        /// <inheritdoc/>
        public bool IsInMemory => Database.IsInMemory();

        /// <inheritdoc/>
        public void Clean()
        {
            var changedEntriesCopy = ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or
                            EntityState.Modified or
                            EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        /// <inheritdoc/>
        public override int SaveChanges()
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            => SaveChangesAsync(true, default).GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

        /// <inheritdoc/>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entityEntries = ChangeTracker.Entries().ToArray();

            // перед применением событий получаем их все из доменных сущностей во избежание дубликации в рекурсии
            var domainEvents = entityEntries
                .Select(po => po.Entity)
                .OfType<EntityBase>()
                .SelectMany(x => x.RetrieveDomainEvents())
                .ToArray();

            foreach (var @event in domainEvents)
                await _domainEventsDispatcher.Publish(@event, cancellationToken);

            if (entityEntries.Length > 10)
                entityEntries.AsParallel().ForAll(OnSave);
            else
                foreach (var entityEntry in entityEntries)
                    OnSave(entityEntry);

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <inheritdoc/>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await SaveChangesAsync(true, cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(DefaultSchema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfContext).Assembly);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Глобальные фильтры на soft delete
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                    entityType.AddSoftDeleteQueryFilter();

                if (entityType.IsKeyless)
                {
                    continue;
                }

                // Перевод дат сущностей в UTC
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }

        private void SoftDeleted(EntityEntry entityEntry)
        {
            if (entityEntry?.Entity is not null
                && entityEntry.Entity is ISoftDeletable softDeleteable)
            {
                softDeleteable.DeletedAt = _dateTimeProvider.UtcNow;
                entityEntry.State = EntityState.Modified;
            }
        }

        private void OnSave(EntityEntry entityEntry)
        {
            // TODO: вынести в домен
            if (entityEntry.State != EntityState.Unchanged)
            {
                UpdateTimestamp(entityEntry);
            }

            if (entityEntry.State == EntityState.Deleted)
                SoftDeleted(entityEntry);
        }

        private void UpdateTimestamp(EntityEntry entityEntry)
        {
            var entity = entityEntry.Entity;
            if (entity is null)
                return;

            // TODO: лучше бы вызывать функцию бд now() at time zone 'utc', но не нашел как адекватно вмешаться в апдейт
            if (entity is IUpdateTrackable updateTrackable)
                updateTrackable.UpdatedAt = _dateTimeProvider.UtcNow;

            if (entityEntry.State == EntityState.Added
                && entity is IAddTrackable addTrackable)
                addTrackable.CreatedAt = _dateTimeProvider.UtcNow;
        }
    }
}
