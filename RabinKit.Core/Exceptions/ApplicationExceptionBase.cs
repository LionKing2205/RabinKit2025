using System.Runtime.Serialization;

namespace RabinKit.Core.Exceptions
{
	/// <summary>
	/// Базовое исключение для логики приложения
	/// </summary>
	public class ApplicationExceptionBase : ApplicationException
	{
		/// <summary>
		/// Базовое исключение для логики приложения
		/// </summary>
		public ApplicationExceptionBase()
		{
		}

		/// <summary>
		/// Базовое исключение для логики приложения
		/// </summary>
		/// <param name="message">Сообщение</param>
		public ApplicationExceptionBase(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Базовое исключение для логики приложения
		/// </summary>
		/// <param name="info">info</param>
		/// <param name="context">context</param>
		public ApplicationExceptionBase(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Базовое исключение для логики приложения
		/// </summary>
		/// <param name="message">Сообщение</param>
		/// <param name="innerException">Внутреннее исключение</param>
		public ApplicationExceptionBase(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
