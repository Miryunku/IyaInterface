using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
//using Microsoft.EntityFrameworkCore;

namespace Iya
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            bool canConnect;

            using (var dbContext = new Models.IyaDbContext())
            {
                canConnect = dbContext.Database.CanConnect();
            }
            if (!canConnect)
            {
                MessageBox.Show("No se encontró la base de datos Iya. Cerrando la aplicación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            using (var dbContext = new Models2.IyaUsersDbContext())
            {
                canConnect = dbContext.Database.CanConnect();
            }
            if (!canConnect)
            {
                MessageBox.Show("No se encontró la base de datos de IyaUsers. Cerrando la aplicación.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            Dictionary<int, string> tagsSpanish = new Dictionary<int, string>
            {
                {0, "Sin clasificar"},
                {1, "Determinante"},
                {2, "Cuantificador"},
                {3, "Conjunción"},
                {4, "Adposición"},
                {5, "Interjección"},
                {6, "Ideófono"},
                {7, "Ideófono/Nominal"},
                {8, "Ideófono/Adjetival"},
                {9, "Ideófono/する"},
                {10, "Ideófono/Adverbio"},
                {11, "Ideófono/Adverbio-と"},
                {12, "Pronombre"},
                {13, "Sustantivo propio"},
                {14, "Sustantivo común"},
                {15, "Sustantivo する"},
                {16, "Adjetivo-な"},
                {17, "Adjetivo-い"},
                {18, "Adjetivo-の"},
                {19, "Adjetival prenominal"},
                {20, "Godan, Transitivo"},
                {21, "Godan, Intransitivo"},
                {22, "Godan, Transitivo/Intransitivo"},
                {23, "Ichidan, Transitivo"},
                {24, "Ichidan, Intransitivo"},
                {25, "Ichidan, Transitivo/Intransitivo"},
                {26, "Verbo Irregular"},
                {27, "Auxiliar"},
                {28, "Adverbio"},
                {29, "Adverbio-と"},
                {30, "Prefijo"},
                {31, "Sufijo"},
                {32, "Contador-Japonés"},
                {33, "Expresión"}
            };

            Dictionary<int, string> subtagsSpanish = new Dictionary<int, string>
            {
                {0, ""},
                {1, "Coloquial"},
                {2, "Jerga/Internet"},
                {3, "Jerga/Manga-Anime"},
                {4, "Figurativo"},
                {5, "Peyorativo"},
                {6, "Jocoso"},
                {7, "Sonkeigo"},
                {8, "Kenjougo"},
                {9, "Arcaico"},
                {10, "Lugar"}
            };

            Application.Current.Resources["IyaTags"] = tagsSpanish;
            Application.Current.Resources["IyaSubtags"] = subtagsSpanish;

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
