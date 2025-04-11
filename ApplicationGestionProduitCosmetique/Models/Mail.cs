using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Mail
    {
        public static void EnvoyerFacture(string emailDestinataire, string cheminFacture)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("facturation@cosmeticshop.com", "Cosmetic Shop");
            message.To.Add(emailDestinataire);
            message.Subject = "Votre facture Cosmetic Shop";
            message.Body = "Merci pour votre achat ! Vous trouverez votre facture en pièce jointe.";
            message.Attachments.Add(new Attachment(cheminFacture));

            SmtpClient smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 2525);
            smtp.Credentials = new NetworkCredential("TON_USERNAME", "TON_PASSWORD"); // Remplace avec tes infos MailTrap
            smtp.EnableSsl = true;

            smtp.Send(message);
        }
    }
}
