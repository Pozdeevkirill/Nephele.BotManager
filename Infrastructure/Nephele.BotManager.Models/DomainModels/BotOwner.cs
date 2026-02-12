namespace Nephele.BotManager.Models.DomainModels;

/// <summary>
/// Привязка бота к сервису
/// </summary>
public class BotOwner : Entity
{
    protected BotOwner()
    {
    }
    
    public BotOwner(string name, string endpoint)
    {
        Name = name;
        Endpoint = endpoint;
    }
    
    /// <summary>
    /// Имя сервиса
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Ссылка на сервис
    /// </summary>
    public string Endpoint { get; set; }
}