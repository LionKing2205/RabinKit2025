namespace RabinKit.Core.Exceptions
{
	/// <summary>
	/// Нарушенное ограничение уникальности в таблице БД
	/// </summary>
	public class DuplicateUniqueKeyException : ApplicationExceptionBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="innerException">Внутреннее исключение</param>
		/// <param name="message">Сообщение</param>
		public DuplicateUniqueKeyException(
			Exception innerException,
			string message = "Нарушено ограничение уникальности при обновлении базы данных. Попробуйте повторить запрос.")
			: base(message, innerException)
		{
		}
	}
}
