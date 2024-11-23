namespace RabinKit.Core.Exceptions
{
	/// <summary>
	/// Исключение "Не задана конфигурация токена аутентификации"
	/// </summary>
	public class TokenConfigurationException : ApplicationExceptionBase
	{
		/// <summary>
		/// Исключение "Не задана конфигурация токена аутентификации"
		/// </summary>
		public TokenConfigurationException()
			: base("Не задана конфигурация токена аутентификации")
		{
		}

		/// <summary>
		/// Исключение "Не задана конфигурация токена аутентификации: поле"
		/// </summary>
		/// <param name="field">Поле конфигурации токенов</param>
		public TokenConfigurationException(string field)
			: base($"Не задана конфигурация токена аутентификации: поле '{field}'")
		{
		}
	}
}
