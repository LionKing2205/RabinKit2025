namespace RabinKit.Core.Exceptions
{
	/// <summary>
	/// Исключение для обозначения, некорректно задан параметр сортировки сущностей
	/// </summary>
	public class InvalidOrderByExpressionException : ApplicationExceptionBase
	{
		/// <summary>
		/// Исключение для обозначения, некорректно задан параметр сортировки сущностей
		/// </summary>
		public InvalidOrderByExpressionException()
			: base("Некорректное выражение для сортировки списка сущностей")
		{
		}
	}
}
