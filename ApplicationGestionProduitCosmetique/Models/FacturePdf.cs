using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGestionProduitCosmetique.Models
{
    public class FacturePdf
    {
        public static string GenererPDF(Facture facture, string dossierDestination)
        {
            string cheminFichier = Path.Combine(dossierDestination, $"{facture.NumeroFacture}.pdf");

            PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
            document.Info.Title = "Facture Cosmetic Shop";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titreFont = new XFont("Arial", 18);
            XFont textFont = new XFont("Arial", 12);

            int y = 40;

            gfx.DrawString("COSMETIC SHOP", titreFont, XBrushes.DarkViolet, new XPoint(40, y));
            y += 40;

            gfx.DrawString($"Facture No : {facture.NumeroFacture}", textFont, XBrushes.Black, new XPoint(40, y));
            y += 20;

            gfx.DrawString($"Client : {facture.NomClient}", textFont, XBrushes.Black, new XPoint(40, y));
            y += 20;

            gfx.DrawString($"Date : {facture.DateCreation.ToShortDateString()}", textFont, XBrushes.Black, new XPoint(40, y));
            y += 40;

            gfx.DrawString("Détails des produits :", textFont, XBrushes.DarkBlue, new XPoint(40, y));
            y += 20;

            foreach (var p in facture.ProduitsFacture)
            {
                gfx.DrawString($"{p.Produit.Nom} x {p.Quantite} @ {p.PrixUnitaire:C2} = {p.SousTotal:C2}", textFont, XBrushes.Black, new XPoint(60, y));
                y += 20;
            }

            y += 20;
            gfx.DrawString($"Sous-total : {facture.MontantSousTotal:C2}", textFont, XBrushes.Black, new XPoint(40, y)); y += 20;
            gfx.DrawString($"TPS (5%) : {facture.MontantTPS:C2}", textFont, XBrushes.Black, new XPoint(40, y)); y += 20;
            gfx.DrawString($"TVQ (9.975%) : {facture.MontantTVQ:C2}", textFont, XBrushes.Black, new XPoint(40, y)); y += 20;
            gfx.DrawString($"Total à payer : {facture.MontantTotal:C2}", textFont, XBrushes.DarkGreen, new XPoint(40, y)); y += 40;

            gfx.DrawString("Merci pour votre achat !", textFont, XBrushes.DarkViolet, new XPoint(40, y));
            y += 20;
            gfx.DrawString(Facture.AdresseMagasin, textFont, XBrushes.Gray, new XPoint(40, y));
            y += 20;
            gfx.DrawString(Facture.EmailContact, textFont, XBrushes.Gray, new XPoint(40, y));

            document.Save(cheminFichier);
            return cheminFichier;
        }
    }

}

