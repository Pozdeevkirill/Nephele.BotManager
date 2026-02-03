namespace Nephele.BotManager.Models.DomainModels;

/// <summary>
/// Привязка бота к сервису
/// </summary>
public class BotOwner : Entity
{
    /// <summary>
    /// Ссылка на сервис
    /// </summary>
    public string Endpoint { get; set; }
    
    /// <summary>
    /// Ссылка на бота
    /// </summary>
    public Guid BotInfoId { get; set; }
}