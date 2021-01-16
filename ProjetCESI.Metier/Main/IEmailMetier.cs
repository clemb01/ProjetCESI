using ProjetCESI.Core;
using System.Threading.Tasks;

namespace ProjetCESI.Metier.Main
{
    public interface IEmailMetier
    {
        Task SendEmailAsync(string destinataire, string subject, string message);
    }
}