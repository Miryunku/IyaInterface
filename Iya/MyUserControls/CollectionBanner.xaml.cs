using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Iya.MyUserControls
{
    /// <summary>
    /// Interaction logic for CollectionBanner.xaml
    /// </summary>
    public partial class CollectionBanner : UserControl
    {
        private Models2.Collection m_collection = null;

        public CollectionBanner(Models2.Collection collection)
        {
            InitializeComponent();

            m_collection = collection;

            tblName.Text = collection.Name;

            tblDateDetails.Inlines.Add("Fecha de creación: ");
            DateTime date;
            if (DateTime.TryParseExact(collection.Creation, "yyyy'-'MM'-'dd'T'HH':'mm':'ss", null, DateTimeStyles.AssumeUniversal, out date))
            {
                tblDateDetails.Inlines.Add(date.ToLocalTime().ToString("s"));
            }
            else
            {
                tblDateDetails.Inlines.Add("*");
            }

            tblDateDetails.Inlines.Add(" | Última visita: ");
            if (DateTime.TryParseExact(collection.LastVisit, "yyyy'-'MM'-'dd'T'HH':'mm':'ss", null, DateTimeStyles.AssumeUniversal, out date))
            {
                tblDateDetails.Inlines.Add(date.ToLocalTime().ToString("s"));

                TimeSpan timeSpan = DateTime.UtcNow - date;
                if (timeSpan.TotalDays >= 5.0)
                {
                    rtgColor.Fill = Brushes.Crimson;
                }
                else if (timeSpan.TotalDays >= 4.0)
                {
                    rtgColor.Fill = Brushes.Orange;
                }
                else if (timeSpan.TotalDays >= 3.0)
                {
                    rtgColor.Fill = Brushes.Yellow;
                }
                else if (timeSpan.TotalDays >= 2.0)
                {
                    rtgColor.Fill = Brushes.LawnGreen;
                }
                else
                {
                    rtgColor.Fill = Brushes.Aqua;
                }
            }
            else
            {
                tblDateDetails.Inlines.Add("*");
                rtgColor.Fill = Brushes.Gray; // Gray represents undetermined.
            }

            switch (collection.Type)
            {
                case "Component":
                {
                    tblContentDetails.Inlines.Add($"Tipo: Componente | Items: {collection.Size}");
                    break;
                }
                case "Kanji":
                {
                    tblContentDetails.Inlines.Add($"Tipo: Kanji | Items: {collection.Size}");
                    break;
                }
                case "Word":
                {
                    tblContentDetails.Inlines.Add($"Tipo: Palabra | Items: {collection.Size}");
                    break;
                }
            }
        }

        public Border BorderP { get { return bdrBorder; } }

        public Models2.Collection CollectionP { get { return m_collection; } }

        public event MouseButtonEventHandler MyMouseLeftButtonUpEvent;

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MyMouseLeftButtonUpEvent?.Invoke(this, e);
        }
    }
}
