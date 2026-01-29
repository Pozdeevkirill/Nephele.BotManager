namespace Nephele.BotManager.Models.DomainModels;

/// <summary>
/// Базовая сущность обьекта
/// </summary>
public class Entity
{
    /// <summary>
    /// Ид сущности
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Версия сущности
    /// </summary>
    public DateTime Version { get; set; }
}