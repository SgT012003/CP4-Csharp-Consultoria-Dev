namespace Consultoria.Core.Interfaces;

public interface IWhatsAppService
{
    string GenerateWhatsAppLink(string phone, string message);
}
