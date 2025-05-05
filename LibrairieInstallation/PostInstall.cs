using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieInstallation
{
    [RunInstaller(true)]
    public class PostInstall : Installer
    {
        public override void Commit(System.Collections.IDictionary
        savedState)
        {
            base.Commit(savedState);

            string targetDir = Context.Parameters["targetdir"];
            string exePath = Path.Combine(targetDir, "InitialisationBD.exe");
            if (File.Exists(exePath))
            {
                Process.Start(exePath);
            }
        }
    }
}
