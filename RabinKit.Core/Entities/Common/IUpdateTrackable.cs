namespace RabinKit.Core.Entities.Common
{
    /// <summary>
    /// Сущность, для которой сохраняются данные о времени изменения
    /// </summary>
    public interface IUpdateTrackable
    {
        /// <summary>
        /// Дата последнего изменения сущности
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
