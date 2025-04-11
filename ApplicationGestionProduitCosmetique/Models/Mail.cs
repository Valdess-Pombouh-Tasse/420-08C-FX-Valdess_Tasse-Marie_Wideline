using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class Mail
    {
        public static void EnvoyerFacture(string emailDestinataire, string cheminFacture)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("facturation@cosmeticshop.com", "Cosmetic Shop");
                message.To.Add(emailDestinataire);
                message.Subject = "Votre facture Cosmetic Shop";
                message.Body = "Merci pour votre achat ! Vous trouverez votre facture en pièce jointe.";
                message.Attachments.Add(new Attachment(cheminFacture));


                SmtpClient smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 2525);
                smtp.Credentials = new NetworkCredential("e808b91453481f", "2748405f81bcbb");
                smtp.EnableSsl = true;

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'envoi de l'email : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
