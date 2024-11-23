namespace RabinKit.Core.Exceptions
{
	/// <summary>
	/// Исключение валидации данных в домене
	/// </summary>
	public class ValidationException : ApplicationExceptionBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public ValidationException()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="message">Сообщение об ошибке</param>
		public ValidationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="message">Сообщение об ошибке</param>
		/// <param name="innerException">Внутренняя ошибка</param>
		public ValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
