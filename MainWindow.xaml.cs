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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Iya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool m_loggedIn = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenDictionary_Click(object sender, RoutedEventArgs e)
        {
            var dictionaryWindow = new DictionaryWindow();
            dictionaryWindow.Owner = this;
            dictionaryWindow.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxUsername.Text))
            {
                return;
            }

            string enteredName = tbxUsername.Text.Trim();

            using (var dbContext = new Models2.IyaUsersDbContext())
            {
                var user = dbContext.Users.SingleOrDefault(u => u.Name == enteredName);
                if (user == null)
                {
                    MessageBox.Show(this, "Usuario no encontrado.", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                dbContext.Entry(user).Collection(u => u.Collections).Load();

                Application.Current.Resources["user"] = user;
            }
            m_loggedIn = true;

            btnLogin.IsEnabled = false;
            btnManageUsers.IsEnabled = false;
            tbxUsername.IsReadOnly = true; // Visual help to know who is logged in.

            btnMyCollections.IsEnabled = true;
            btnCreateJlptKanjiCollections.IsEnabled = true;
            btnCreateJlptWordsCollections.IsEnabled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (!m_loggedIn)
            {
                return;
            }

            Application.Current.Resources["user"] = null;

            btnLogin.IsEnabled = true;
            btnManageUsers.IsEnabled = true;
            tbxUsername.IsReadOnly = false;

            btnMyCollections.IsEnabled = false;
            btnCreateJlptKanjiCollections.IsEnabled = false;
            btnCreateJlptWordsCollections.IsEnabled = false;
        }

        private void btnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            var usersListWindow = new UsersListWindow();
            usersListWindow.Owner = this;
            usersListWindow.ShowDialog();
        }

        private void btnMyCollections_Click(object sender, RoutedEventArgs e)
        {
            var userCollectionsWindow = new CollectionsWindow();
            userCollectionsWindow.Owner = this;
            userCollectionsWindow.ShowDialog();
        }
    }
}
