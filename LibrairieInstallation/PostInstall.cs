using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Windows;

[RunInstaller(true)]
public class PostInstall : Installer
{
    public override void Commit(System.Collections.IDictionary savedState)
    {
        base.Commit(savedState);
        string targetDir = Context.Parameters["targetdir"];
        string exePath = Path.Combine(targetDir, "InitialisationBD.exe");
        if (File.Exists(exePath))
        {
            System.Diagnostics.Process.Start(exePath);
        }
        else
        {
            MessageBox.Show($"Fichier introuvable : {exePath}");
        }
    }
}