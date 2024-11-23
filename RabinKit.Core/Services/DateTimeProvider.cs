using RabinKit.Core.Abstractions;

namespace RabinKit.Core.Services
{
	/// <inheritdoc/>
	public class DateTimeProvider : IDateTimeProvider
	{
		/// <inheritdoc/>
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
