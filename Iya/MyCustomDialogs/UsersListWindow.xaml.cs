using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Microsoft.EntityFrameworkCore;

namespace Iya
{
    /// <summary>
    /// Interaction logic for UsersListWindow.xaml
    /// </summary>
    public partial class UsersListWindow : Window
    {
        private ObservableCollection<Models2.User> m_userCollection;

        public UsersListWindow()
        {
            InitializeComponent();

            using (var dbContext = new Models2.IyaUsersDbContext())
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                m_userCollection = new ObservableCollection<Models2.User>(dbContext.Users.ToList());
                ltbUsers.ItemsSource = m_userCollection;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxUser.Text))
            {
                tbxInfo.Text = "No puedes registrar un usuario sin nombre.";
                return;
            }

            string enteredName = tbxUser.Text.Trim();

            using (var dbContext = new Models2.IyaUsersDbContext())
            {
                if (dbContext.Users.SingleOrDefault(u => u.Name.Equals(enteredName)) != null)
                {
                    tbxInfo.Text = "Ya existe un usuario con ese nombre.";
                    return;
                }

                var newUser = new Models2.User();
                newUser.Name = enteredName;
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                m_userCollection.Add(newUser);
            }

            tbxInfo.Text = $"Un nuevo usuario ha sido agregado: {enteredName}.";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ltbUsers.SelectedIndex == -1)
            {
                tbxInfo.Text = "Debes seleccionar un usuario en la lista para poder eliminarlo.";
                return;
            }

            var msgBoxResult = MessageBox.Show(this, "¿Estás segur@ de que quieres eliminar al usuario?", "Eliminar usuario", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (msgBoxResult)
            {
                case MessageBoxResult.Yes:
                    using (var dbContext = new Models2.IyaUsersDbContext())
                    {
                        // Before removing the user, remove the records in component_collection_contents table,
                        // kanji_collection_contents table, and word_collection_contents table that are related
                        // to the collections of the user.
                        // This is necessary because those tables don't have any foreign key, which means that
                        // deletion is not going to be cascaded automatically.

                        var user = ltbUsers.SelectedItem as Models2.User;

                        Models2.Collection[] collections = dbContext.Collections.Where(c => c.UserId == user.UserId).ToArray();
                        if (collections.Any())
                        {
                            foreach (var c in collections)
                            {
                                switch (c.Type)
                                {
                                    case "Component":
                                    {
                                        dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM component_collection_contents WHERE collection_id = {c.CollectionId};");
                                        break;
                                    }
                                    case "Kanji":
                                    {
                                        //dbContext.KanjiCollectionContents.RemoveRange(dbContext.KanjiCollectionContents.Where(kcc => kcc.CollectionId == c.CollectionId));
                                        dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM kanji_collection_contents WHERE collection_id = {c.CollectionId};");
                                        break;
                                    }
                                    case "Word":
                                    {
                                        //dbContext.WordCollectionContents.RemoveRange(dbContext.WordCollectionContents.Where(wcc => wcc.CollectionId == c.CollectionId));
                                        dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM word_collection_contents WHERE collection_id = {c.CollectionId};");
                                        break;
                                    }
                                }
                            }
                        }
                        // Removing the user should also delete the records in the collections table.
                        dbContext.Users.Remove(user);
                        dbContext.SaveChanges();
                        m_userCollection.RemoveAt(ltbUsers.SelectedIndex);
                        tbxInfo.Text = $"Un usuario ha sido eliminado: {user.Name}.";
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
