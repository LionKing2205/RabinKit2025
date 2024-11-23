namespace RabinKit.Core.Entities.Common
{
    /// <summary>
    /// Сущность, для которой сохраняются данные о времени создания
    /// </summary>
    public interface IAddTrackable
    {
        /// <summary>
        /// Дата создания сущности
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
