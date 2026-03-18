using Consultoria.Core.Interfaces;

namespace Consultoria.Infrastructure.Services;

public class WhatsAppService : IWhatsAppService
{
    public string GenerateWhatsAppLink(string phone, string message)
    {
        // Remove non-numeric characters for clean phone number
        var cleanPhone = new string(phone.Where(char.IsDigit).ToArray());
        var encodedMessage = Uri.EscapeDataString(message);
        return $"https://wa.me/{cleanPhone}?text={encodedMessage}";
    }
}
