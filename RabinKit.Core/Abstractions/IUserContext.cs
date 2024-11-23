namespace RabinKit.Core.Abstractions
{
    /// <summary>
    /// Контекст текущего пользователя
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// ИД текущего пользователя
        /// </summary>
        Guid CurrentUserId { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? UserName { get; }
    }
}
