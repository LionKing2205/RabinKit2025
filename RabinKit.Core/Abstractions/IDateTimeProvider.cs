namespace RabinKit.Core.Abstractions
{
	/// <summary>
	/// Провайдер даты
	/// </summary>
	public interface IDateTimeProvider
	{
		/// <summary>
		/// Сейчас
		/// </summary>
		DateTime UtcNow { get; }
	}
}
