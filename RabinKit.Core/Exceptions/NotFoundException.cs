namespace RabinKit.Core.Exceptions
{
	/// <summary>
	/// Исключение для обозначения, что какие-то данные не найдены
	/// </summary>
	public class NotFoundException : ApplicationExceptionBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public NotFoundException()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="message">Сообщение об ошибке</param>
		public NotFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="message">Сообщение об ошибке</param>
		/// <param name="innerException">Внутреннее исключение</param>
		public NotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
