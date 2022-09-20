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

using Microsoft.EntityFrameworkCore;

namespace Iya.MyPages
{
    /// <summary>
    /// Interaction logic for CollectionsPage.xaml
    /// </summary>
    public partial class CollectionsPage : Page
    {
        bool m_loggedIn = false;
        private Models2.User m_user = null;
        private MyUserControls.CollectionBanner m_focusedBanner = null;

        public CollectionsPage()
        {
            InitializeComponent();

            tblInfo.Text = "Colecciones de componentes: -\nColecciones de kanji: -\nColecciones de palabras: -";
        }

        public CollectionsPage(Models2.User currentUser)
        {
            InitializeComponent();

            m_loggedIn = true;
            m_user = currentUser;

            btnManageUsers.IsEnabled = false;
            tbxUsername.Text = m_user.Name;
            tbxUsername.IsReadOnly = true;
            tblInfo.Text = $"Colecciones de componentes: {m_user.ComponentCollectionsQuantity}.\nColecciones de kanji: {m_user.KanjiCollectionsQuantity}.\nColecciones de palabras: {m_user.WordCollectionsQuantity}.";
            
            foreach (Models2.Collection collection in m_user.Collections)
            {
                switch (collection.Type)
                {
                    case "Component":
                    {
                        var banner = new MyUserControls.CollectionBanner(collection);
                        banner.MyMouseLeftButtonUpEvent += FocusElement;

                        stkComponentCollections.Children.Add(banner);
                        break;
                    }

                    case "Kanji":
                    {
                        var banner = new MyUserControls.CollectionBanner(collection);
                        banner.MyMouseLeftButtonUpEvent += FocusElement;

                        stkKanjiCollections.Children.Add(banner);
                        break;
                    }

                    case "Word":
                    {
                        var banner = new MyUserControls.CollectionBanner(collection);
                        banner.MyMouseLeftButtonUpEvent += FocusElement;

                        stkWordCollections.Children.Add(banner);
                        break;
                    }
                }
            }
        }

        public void FocusElement(object sender, MouseButtonEventArgs args)
        {
            if (m_focusedBanner != null)
            {
                m_focusedBanner.BorderP.BorderBrush = Brushes.Gainsboro;
            }
            m_focusedBanner = sender as MyUserControls.CollectionBanner;
            m_focusedBanner.BorderP.BorderBrush = Brushes.Coral;
        }

        private void btnOpenDictionary_Click(object sender, RoutedEventArgs e)
        {
            var dictionaryWindow = new DictionaryWindow();
            dictionaryWindow.Owner = this.Parent as Window;
            dictionaryWindow.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxUsername.Text) || m_loggedIn)
            {
                return;
            }

            string enteredName = tbxUsername.Text.Trim();

            using (var dbContext = new Models2.IyaUsersDbContext())
            {
                var user = dbContext.Users.SingleOrDefault(u => u.Name == enteredName);
                if (user == null)
                {
                    MessageBox.Show(this.Parent as Window, "Usuario no encontrado.", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                dbContext.Entry(user).Collection(u => u.Collections).Load();

                Application.Current.Resources["user"] = user;
                m_user = user;
            }

            m_loggedIn = true;

            btnManageUsers.IsEnabled = false;
            tbxUsername.IsReadOnly = true; // Visual help to know who is logged in.

            tblInfo.Text = $"Colecciones de componentes: {m_user.ComponentCollectionsQuantity}.\nColecciones de kanji: {m_user.KanjiCollectionsQuantity}.\nColecciones de palabras: {m_user.WordCollectionsQuantity}.";
            foreach (Models2.Collection collection in m_user.Collections)
            {
                switch (collection.Type)
                {
                    case "Component":
                    {
                        var banner = new MyUserControls.CollectionBanner(collection);
                        banner.MyMouseLeftButtonUpEvent += FocusElement;

                        stkComponentCollections.Children.Add(banner);
                        break;
                    }

                    case "Kanji":
                    {
                        var banner = new MyUserControls.CollectionBanner(collection);
                        banner.MyMouseLeftButtonUpEvent += FocusElement;

                        stkKanjiCollections.Children.Add(banner);
                        break;
                    }

                    case "Word":
                    {
                        var banner = new MyUserControls.CollectionBanner(collection);
                        banner.MyMouseLeftButtonUpEvent += FocusElement;

                        stkWordCollections.Children.Add(banner);
                        break;
                    }
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (!m_loggedIn)
            {
                return;
            }

            tblInfo.Text = "Colecciones de componentes: -\nColecciones de kanji: -\nColecciones de palabras: -";
            m_focusedBanner = null;

            stkComponentCollections.Children.Clear();
            stkKanjiCollections.Children.Clear();
            stkWordCollections.Children.Clear();

            Application.Current.Resources["user"] = null;

            btnManageUsers.IsEnabled = true;
            tbxUsername.IsReadOnly = false;

            m_loggedIn = false;
        }

        private void btnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            var usersListWindow = new UsersListWindow();
            usersListWindow.Owner = this.Parent as Window;
            usersListWindow.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (m_focusedBanner != null)
            {
                var msgBoxResult = MessageBox.Show(this.Parent as Window, "¿Estás segur@ de que quieres eliminar la colección?", "Eliminar colección", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (msgBoxResult)
                {
                    case MessageBoxResult.Yes:
                    {
                        // First, erase the collection and its related content from the database.
                        Models2.Collection collection = m_focusedBanner.CollectionP;
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            switch (collection.Type)
                            {
                                case "Component":
                                {
                                    // TODO: Add support for ComponentCollectionContents
                                    break;
                                }
                                case "Kanji":
                                {
                                    //dbContext.KanjiCollectionContents.RemoveRange(dbContext.KanjiCollectionContents.Where(kcc => kcc.CollectionId == collection.CollectionId));
                                    dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM kanji_collection_contents WHERE collection_id = {collection.CollectionId};");

                                    m_user.KanjiCollectionsQuantity -= 1;
                                    dbContext.Users.Update(m_user);
                                    break;
                                }
                                case "Word":
                                {
                                    //dbContext.WordCollectionContents.RemoveRange(dbContext.WordCollectionContents.Where(wcc => wcc.CollectionId == collection.CollectionId));
                                    dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM word_collection_contents WHERE collection_id = {collection.CollectionId};");

                                    m_user.WordCollectionsQuantity -= 1;
                                    dbContext.Users.Update(m_user);
                                    break;
                                }
                            }
                            dbContext.Collections.Remove(collection);
                            dbContext.SaveChanges();
                        }
                        // Second, erase the CollectionBanner from the StackPanel.
                        switch (collection.Type)
                        {
                            case "Component":
                                stkComponentCollections.Children.Remove(m_focusedBanner);
                                break;
                            case "Kanji":
                                stkKanjiCollections.Children.Remove(m_focusedBanner);
                                break;
                            case "Word":
                                stkWordCollections.Children.Remove(m_focusedBanner);
                                break;
                        }
                        // Third, reset the focused banner.
                        m_focusedBanner = null;
                        break;
                    }
                    case MessageBoxResult.No:
                    {
                        break;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!m_loggedIn)
            {
                return;
            }

            var dialogWindow = new CollectionTypeDialogWindow();
            dialogWindow.Owner = this.Parent as Window;
            dialogWindow.ShowDialog();

            if (dialogWindow.DialogResult == true)
            {
                (this.Parent as Window).Content = new AddCollectionPage2((DictElems)(dialogWindow.ChosenType + 1));
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (m_focusedBanner != null)
            {
                switch (m_focusedBanner.CollectionP.Type)
                {
                    case "Component":
                        (this.Parent as Window).Content = new EditCollectionPage(m_focusedBanner.CollectionP, DictElems.Component);
                        break;

                    case "Kanji":
                        (this.Parent as Window).Content = new EditCollectionPage(m_focusedBanner.CollectionP, DictElems.Kanji);
                        break;

                    case "Word":
                        (this.Parent as Window).Content = new EditCollectionPage(m_focusedBanner.CollectionP, DictElems.Word);
                        break;
                }
            }
        }

        private void btnStudy_Click(object sender, RoutedEventArgs e)
        {
            if (m_focusedBanner != null)
            {
                (this.Parent as Window).Content = new StudyPage(m_focusedBanner.CollectionP);
            }
        }
    }
}
