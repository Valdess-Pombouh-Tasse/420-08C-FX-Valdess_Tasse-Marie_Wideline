using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
   public class FacturePdf
    {
        public static void GenererFacturePDF(Facture facture, string cheminComplet)
        {
            if (facture == null || facture.ProduitsFacture == null || facture.ProduitsFacture.Count == 0)
                throw new ArgumentException("La facture est vide ou invalide.");

            using (PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument())
            {
                document.Info.Title = $"Facture {facture.Id}";
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont titreFont = new XFont("Verdana", 16);
                XFont corpsFont = new XFont("Verdana", 12);
                XFont totalFont = new XFont("Verdana", 14);

                int y = 40;

                gfx.DrawString("Cosmetic Shop - Facture", titreFont, XBrushes.MediumVioletRed,
                    new XRect(0, y, page.Width, 0), XStringFormats.TopCenter);
                y += 40;

                gfx.DrawString($"Facture No : {facture.Id}", corpsFont, XBrushes.Black, 40, y); y += 25;
                gfx.DrawString($"Date : {facture.DateCreation}", corpsFont, XBrushes.Black, 40, y); y += 25;
                //gfx.DrawString($"Client : {facture.}", corpsFont, XBrushes.Black, 40, y); y += 30;

                gfx.DrawString("Produits :", corpsFont, XBrushes.Black, 40, y); y += 20;

                for (int i = 0; i < facture.ProduitsFacture.Count; i++)
                {
                    var item = facture.ProduitsFacture[i];
                    string ligne = $"{item.Produit.Nom} | Qté: {item.Quantite} | Prix: {item.Produit.Prix:C2} | Total: {(item.Produit.Prix * item.Quantite):C2}";
                    gfx.DrawString(ligne, corpsFont, XBrushes.Black, 50, y);
                    y += 20;
                }

                y += 30;
                gfx.DrawString($"Total TTC : {facture.MontantTotal:C2}", totalFont, XBrushes.DarkGreen, 40, y);

                // Enregistre le PDF
                document.Save(cheminComplet);
            }
        }
    }

}

