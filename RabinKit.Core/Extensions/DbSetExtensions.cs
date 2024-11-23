using RabinKit.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace RabinKit.Core.Extensions;

public static class DbSetExtensions
{
    public static async Task MutateAsync<T>(
        this DbSet<T> dbSet,
        IEnumerable<T> subjects)
        where T : EntityBase
    {
        ArgumentNullException.ThrowIfNull(dbSet);
        ArgumentNullException.ThrowIfNull(subjects);

        foreach (var subject in subjects)
        {
            await dbSet.MutateAsync(subject);
        }
    }

    public static async Task MutateAsync<T>(
        this DbSet<T> dbSet,
        T subject)
        where T : EntityBase
    {
        ArgumentNullException.ThrowIfNull(dbSet);
        ArgumentNullException.ThrowIfNull(subject);
        
        var entryExists =
            await dbSet.AsNoTracking().AnyAsync(q => q.Id == subject.Id);

        if (!entryExists)
        {
            dbSet.Add(subject);
            return;
        }

        var entry = dbSet.Attach(subject);

        typeof(T)
            .GetProperties()
            .Where(q =>
                q.Name != nameof(EntityBase.Id)
                && q.CanWrite
                && !(q.PropertyType.IsAssignableTo(typeof(EntityBase))
                    || q.PropertyType.GenericTypeArguments
                        .Any(x => x.IsAssignableTo(typeof(EntityBase))))
                && q.GetValue(subject) != GetDefault(q.GetType())
                && q.GetValue(subject) != null)
            .Select(q => q.Name)
            .ToList()
            .ForEach(property => entry.Property(property).IsModified = true);
    }

    private static object? GetDefault(Type type)
        => type.IsValueType
            ? Activator.CreateInstance(type)
            : null;
}