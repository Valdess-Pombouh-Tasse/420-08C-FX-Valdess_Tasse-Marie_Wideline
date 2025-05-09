using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplicationGestionProduitCosmetique.Views
{
    /// <summary>
    /// Logique d'interaction pour FormVideo.xaml
    /// </summary>
    public partial class FormVideo : Window
    {
        public FormVideo()
        {
            InitializeComponent();
        }
        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            mediaCosmetic.Play();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaCosmetic.Pause();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaCosmetic.Stop();
        }

    }
}
