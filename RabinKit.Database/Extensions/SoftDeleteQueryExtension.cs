using System.Linq.Expressions;
using System.Reflection;
using RabinKit.Core.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RabinKit.Database.Extensions
{
    /// <summary>
    /// Расширения для soft delete
    /// </summary>
    public static class SoftDeleteQueryExtension
	{
		/// <summary>
		/// Добавить глобальный фильтр для сущности
		/// </summary>
		/// <param name="entityData">Тип сущности</param>
		public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
		{
			var methodToCall = typeof(SoftDeleteQueryExtension)
				.GetMethod(
					nameof(GetSoftDeleteFilter),
					BindingFlags.NonPublic | BindingFlags.Static)
				?.MakeGenericMethod(entityData.ClrType);

			if (methodToCall == null)
				throw new InvalidOperationException($"не удалось вызвать метод {nameof(GetSoftDeleteFilter)}");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			var filter = (LambdaExpression)methodToCall.Invoke(null, Array.Empty<object>());
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			entityData.SetQueryFilter(filter);
		}

		private static LambdaExpression GetSoftDeleteFilter<TEntity>()
			where TEntity : class, ISoftDeletable
				=> (Expression<Func<TEntity, bool>>)(x => !x.DeletedAt.HasValue);
	}
}
