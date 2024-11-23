using RabinKit.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabinKit.Database.Extensions
{
    /// <summary>
    /// Методы расширения для конфигурации сущностей
    /// </summary>
    public static class PropertyBuilderExtensions
	{
		private const string NowCommand = "now()";

		/// <summary>
		/// Конфигурация поля CreatedOn
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности</typeparam>
		/// <param name="builder">Билдер</param>
		public static PropertyBuilder<DateTime> ConfigureCreatedOn<TEntity>(this EntityTypeBuilder<TEntity> builder)
			where TEntity : class, IAddTrackable
			=> builder.Property(x => x.CreatedAt)
				.IsRequired()
				.HasComment("Время создания записи")
				.HasDefaultValueSql(NowCommand);

		/// <summary>
		/// Конфигурация поля ModifiedOn
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности</typeparam>
		/// <param name="builder">Билдер</param>
		public static PropertyBuilder<DateTime> ConfigureModifiedOn<TEntity>(this EntityTypeBuilder<TEntity> builder)
			where TEntity : class, IUpdateTrackable
			=> builder.Property(x => x.UpdatedAt)
				.HasComment("Время изменения записи")
				.IsRequired()
				.HasDefaultValueSql(NowCommand);

		/// <summary>
		/// Конфигурация поля DeletedAt
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности</typeparam>
		/// <param name="builder">Билдер</param>
		public static PropertyBuilder<DateTime?> ConfigureDeletedAt<TEntity>(this EntityTypeBuilder<TEntity> builder)
			where TEntity : class, ISoftDeletable
			=> builder.Property(x => x.DeletedAt)
				.HasComment("Дата удаления записи");

		/// <summary>
		/// Конфигурация полей CreatedOn и ModifiedOn
		/// </summary>
		/// <typeparam name="TEntity">Тип сущности</typeparam>
		/// <param name="builder">Билдер</param>
		public static void ConfigureTimeTrackableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
			where TEntity : class, ITimeTrackable
		{
			builder.ConfigureCreatedOn();
			builder.ConfigureModifiedOn();
		}
	}
}
