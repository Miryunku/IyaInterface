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

namespace Iya
{
    /// <summary>
    /// Interaction logic for CollectionTypeDialogWindow.xaml
    /// </summary>
    public partial class CollectionTypeDialogWindow : Window
    {
        private int m_chosenType = -1;

        public CollectionTypeDialogWindow()
        {
            InitializeComponent();
        }

        public int ChosenType { get { return m_chosenType; } }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            m_chosenType = cmbTypes.SelectedIndex;
            this.DialogResult = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
