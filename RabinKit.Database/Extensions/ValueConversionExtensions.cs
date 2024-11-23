using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace RabinKit.Database.Extensions
{
    /// <summary>
    /// Расширение преобразования значений типа jsonb
    /// </summary>
    public static class ValueConversionExtensions
    {
        /// <summary>
        /// Установить тип и преобразование
        /// </summary>
        /// <typeparam name="T">Десериализованный тип</typeparam>
        /// <param name="propertyBuilder">Строитель</param>
        /// <returns>Строитель</returns>
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
            where T : class?
        {
            var settings = new JsonSerializerOptions()
            {
            };
            var converter = new ValueConverter<T, string>(
                v => JsonSerializer.Serialize(v, settings),
                v => JsonSerializer.Deserialize<T>(v, settings)!);

            propertyBuilder.HasConversion(converter);
            propertyBuilder.HasColumnType("TEXT");

            return propertyBuilder;
        }
    }
}
