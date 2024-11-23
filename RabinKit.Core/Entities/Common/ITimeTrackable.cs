namespace RabinKit.Core.Entities.Common
{
    /// <summary>
    /// Сущность, для которой сохраняются данные о времени изменения и создания
    /// </summary>
    public interface ITimeTrackable : IAddTrackable, IUpdateTrackable
    {
    }
}
