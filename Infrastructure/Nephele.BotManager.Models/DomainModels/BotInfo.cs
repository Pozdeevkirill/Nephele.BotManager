using Nephele.BotManager.Models.Enums;

namespace Nephele.BotManager.Models.DomainModels;

/// <summary>
/// Информация о боте
/// </summary>
public class BotInfo : Entity
{
    public BotInfo(string name, string token)
    {
        CreateDate = DateTime.Now;
        Name = name;
        Token = token;
        IsActive = false;
    }
    
    /// <summary>
    /// Название бота
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// токен
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// Активность бота
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Дата создание бота
    /// </summary>
    public DateTime CreateDate { get; set; }
    
    /// <summary>
    /// Дата старта бота
    /// </summary>
    public DateTime DateStart { get; set; }
}