using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for EditCollectionPage.xaml
    /// </summary>
    public partial class EditCollectionPage : Page
    {
        private object m_items = null; // ObservableCollection<T>
        private ICollectionView m_collectionView = null;
        private Models2.Collection m_collectionToEdit = null;
        private DictElems m_collectionType = DictElems.None;
        private object m_lastCorrectSearchItem = null; // Item to be put into m_items when btnAddItem is pressed.

        private Models2.User m_user = Application.Current.Resources["user"] as Models2.User;

        private string m_searchStatusOk = "Estado de la búsqueda: OK.";
        private string m_searchStatusNotFound = "Estado de la búsqueda: Término no encontrado.";

        bool m_collectionNameChanged = false;
        bool m_collectionItemsChanged = false;

        public EditCollectionPage(Models2.Collection collectionToEdit, DictElems collectionType)
        {
            InitializeComponent();

            m_collectionToEdit = collectionToEdit;
            m_collectionType = collectionType;

            tbxCollectionName.Text = collectionToEdit.Name;

            switch (collectionType)
            {
                case DictElems.Component:
                {
                    var items = new ObservableCollection<Models.Component>();
                    m_items = items;
                    // Populate m_items
                    using (var dbContextUsers = new Models2.IyaUsersDbContext())
                    {
                        dbContextUsers.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                        foreach (short id in dbContextUsers.ComponentCollectionContents.Where(ccc => ccc.CollectionId == collectionToEdit.CollectionId).Select(ccc=>ccc.ComponentId))
                        {
                            using (var dbContext = new Models.IyaDbContext())
                            {
                                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                                Models.Component comp = dbContext.Components.Find(id);
                                items.Add(comp);
                            }
                        }
                    }

                    ltbCollectionItems.ItemsSource = items;
                    tblSearchWhat.Text = "Buscar componente:";
                    break;
                }
                case DictElems.Kanji:
                {
                    var items = new ObservableCollection<Models.Kanji>();
                    m_items = items;
                    // Populate m_items
                    using (var dbContextUsers = new Models2.IyaUsersDbContext())
                    {
                        dbContextUsers.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                        foreach (short id in dbContextUsers.KanjiCollectionContents.Where(kcc => kcc.CollectionId == collectionToEdit.CollectionId).Select(kcc => kcc.KanjiId))
                        {
                            using (var dbContext = new Models.IyaDbContext())
                            {
                                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                                Models.Kanji kanji = dbContext.Kanjis.Find(id);
                                items.Add(kanji);
                            }
                        }
                    }

                    ltbCollectionItems.ItemsSource = items;
                    tblSearchWhat.Text = "Buscar kanji:";
                    break;
                }
                case DictElems.Word:
                {
                    var items = new ObservableCollection<Models.Word>();
                    m_items = items;
                    // Populate m_items
                    using (var dbContextUsers = new Models2.IyaUsersDbContext())
                    {
                        dbContextUsers.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                        foreach (int id in dbContextUsers.WordCollectionContents.Where(wcc => wcc.CollectionId == collectionToEdit.CollectionId).Select(wcc => wcc.WordId))
                        {
                            using (var dbContext = new Models.IyaDbContext())
                            {
                                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                                Models.Word word = dbContext.Words.Find(id);
                                items.Add(word);
                            }
                        }
                    }

                    ltbCollectionItems.ItemsSource = items;
                    tblSearchWhat.Text = "Buscar palabra:";
                    break;
                }
            }

            m_collectionView = CollectionViewSource.GetDefaultView(ltbCollectionItems.ItemsSource);
            m_collectionView.Filter = ViewFilter;
        }

        private bool ViewFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(tbxFilter.Text))
            {
                return true;
            }
            else
            {
                switch (m_collectionType)
                {
                    case DictElems.Component:
                        return (obj as Models.Component).Component1.StartsWith(tbxFilter.Text);
                    case DictElems.Kanji:
                        return (obj as Models.Kanji).Kanji1.StartsWith(tbxFilter.Text);
                    case DictElems.Word:
                        return (obj as Models.Word).Word1.StartsWith(tbxFilter.Text);
                    default:
                        return true;
                }
            }
        }

        private void tbxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_collectionView.Refresh();
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Window).Content = new CollectionsPage(m_user);
        }

        private void tbxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(tbxSearch.Text))
            {
                string lastSearch = tbxSearch.Text.Trim();
                tblStatus.Text = m_searchStatusOk; // This is an optimization.
                tblLastSearch.Text = $"Último item buscado: {lastSearch}";

                switch (m_collectionType)
                {
                    case DictElems.Component:
                        using (var dbContext = new Models.IyaDbContext())
                        {
                            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                            Models.Component comp = dbContext.Components.SingleOrDefault(c => c.Component1 == lastSearch);
                            if (comp == null)
                            {
                                tblStatus.Text = m_searchStatusNotFound;
                                return;
                            }

                            m_lastCorrectSearchItem = comp;
                            tblItemToAdd.Text = $"Item a agregar: {comp.Component1}";

                            stkFlashcards.Children.Clear();
                            stkFlashcards.Children.Add(new MyUserControls.ComponentFlashcard(comp));
                        }
                        break;
                    case DictElems.Kanji:
                        using (var dbContext = new Models.IyaDbContext())
                        {
                            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                            Models.Kanji kanji = dbContext.Kanjis.SingleOrDefault(k => k.Kanji1 == lastSearch);
                            if (kanji == null)
                            {
                                tblStatus.Text = m_searchStatusNotFound;
                                return;
                            }

                            m_lastCorrectSearchItem = kanji;
                            tblItemToAdd.Text = $"Item a agregar: {kanji.Kanji1}";

                            stkFlashcards.Children.Clear();
                            stkFlashcards.Children.Add(new MyUserControls.KanjiFlashcard(kanji));
                        }
                        break;
                    case DictElems.Word:
                        using (var dbContext = new Models.IyaDbContext())
                        {
                            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                            Models.Word word = dbContext.Words.SingleOrDefault(w => w.Word1 == lastSearch);
                            if (word == null)
                            {
                                tblStatus.Text = m_searchStatusNotFound;
                                return;
                            }

                            m_lastCorrectSearchItem = word;
                            tblItemToAdd.Text = $"Item a agregar: {word.Word1}";

                            stkFlashcards.Children.Clear();
                            stkFlashcards.Children.Add(new MyUserControls.WordFlashcard(word));
                        }
                        break;
                }
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (m_lastCorrectSearchItem != null)
            {
                var parentWnd = this.Parent as Window;
                var caption = "Notificación";

                switch (m_collectionType)
                {
                    case DictElems.Component:
                    {
                        var items = m_items as ObservableCollection<Models.Component>;
                        var correctSearchItem = m_lastCorrectSearchItem as Models.Component;

                        if (items.Contains(correctSearchItem))
                        {
                            MessageBox.Show(parentWnd, "El item ya existe dentro de la lista.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        items.Add(correctSearchItem);
                        m_collectionItemsChanged = true;
                        break;
                    }
                    case DictElems.Kanji:
                    {
                        var items = m_items as ObservableCollection<Models.Kanji>;
                        var correctSearchItem = m_lastCorrectSearchItem as Models.Kanji;

                        if (items.Contains(correctSearchItem))
                        {
                            MessageBox.Show(parentWnd, "El item ya existe dentro de la lista.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        items.Add(correctSearchItem);
                        m_collectionItemsChanged = true;
                        break;
                    }
                    case DictElems.Word:
                    {
                        var items = m_items as ObservableCollection<Models.Word>;
                        var correctSearchItem = m_lastCorrectSearchItem as Models.Word;

                        if (items.Contains(correctSearchItem))
                        {
                            MessageBox.Show(parentWnd, "El item ya existe dentro de la lista.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        items.Add(correctSearchItem);
                        m_collectionItemsChanged = true;
                        break;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ltbCollectionItems.SelectedIndex != -1)
            {
                switch (m_collectionType)
                {
                    case DictElems.Component:
                        (m_items as ObservableCollection<Models.Component>).Remove(ltbCollectionItems.SelectedItem as Models.Component);
                        break;
                    case DictElems.Kanji:
                        (m_items as ObservableCollection<Models.Kanji>).Remove(ltbCollectionItems.SelectedItem as Models.Kanji);
                        break;
                    case DictElems.Word:
                        (m_items as ObservableCollection<Models.Word>).Remove(ltbCollectionItems.SelectedItem as Models.Word);
                        break;
                }
                m_collectionItemsChanged = true;
            }
        }

        private void btnOpenFlashcard_Click(object sender, RoutedEventArgs e)
        {
            if (ltbCollectionItems.SelectedIndex != -1)
            {
                var flashcardWnd = new FlashcardWindow(ltbCollectionItems.SelectedItem, m_collectionType);
                flashcardWnd.Owner = this.Parent as Window;
                flashcardWnd.Show();
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var parentWnd = this.Parent as Window;
            var caption = "Notificación";

            if (ltbCollectionItems.Items.Count < 1)
            {
                MessageBox.Show(parentWnd, "La colección debe tener items.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (ltbCollectionItems.Items.Count > 50)
            {
                MessageBox.Show(parentWnd, "La colección debe tener menos de 51 items.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbxCollectionName.Text))
            {
                MessageBox.Show(parentWnd, "La colección debe tener un nombre.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var collectionNewName = tbxCollectionName.Text.Trim();
            if (collectionNewName != m_collectionToEdit.Name)
            {
                foreach (var coll in m_user.Collections)
                {
                    if (coll.Name == collectionNewName)
                    {
                        MessageBox.Show(parentWnd, "Ya existe una colección con ese nombre.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                m_collectionToEdit.Name = collectionNewName;
                using (var dbContext = new Models2.IyaUsersDbContext())
                {
                    dbContext.Collections.Update(m_collectionToEdit);
                    dbContext.SaveChanges();
                }
                m_collectionNameChanged = true;
            }

            if (m_collectionItemsChanged)
            {
                switch (m_collectionType)
                {
                    case DictElems.Component:
                    {
                        var items = m_items as ObservableCollection<Models.Component>;
                        m_collectionToEdit.Size = (short)items.Count;

                        // Erase every record related to m_collectionToEdit in component_collection_contents table
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM component_collection_contents WHERE collection_id = {m_collectionToEdit.CollectionId};");

                            // Put the necessary information m_items into component_collection_contents table.
                            foreach (Models.Component comp in items)
                            {
                                dbContext.Database.ExecuteSqlInterpolated($"INSERT INTO component_collection_contents (collection_id, component_id) VALUES ({m_collectionToEdit.CollectionId}, {comp.ComponentId});");
                            }

                            dbContext.Collections.Update(m_collectionToEdit);
                            dbContext.SaveChanges();
                        }

                        MessageBox.Show(parentWnd, "Los cambios han sido guardados.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    case DictElems.Kanji:
                    {
                        var items = m_items as ObservableCollection<Models.Kanji>;
                        m_collectionToEdit.Size = (short)items.Count;

                        // Erase every record related to m_collectionToEdit in kanji_collection_contents table
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM kanji_collection_contents WHERE collection_id = {m_collectionToEdit.CollectionId};");
                            // Put the necessary information from m_items into kanji_collection_contents table
                            foreach (var item in items)
                            {
                                dbContext.Database.ExecuteSqlInterpolated($"INSERT INTO kanji_collection_contents (collection_id, kanji_id) VALUES ({m_collectionToEdit.CollectionId}, {item.KanjiId});");
                            }

                            dbContext.Collections.Update(m_collectionToEdit);
                            dbContext.SaveChanges();
                        }

                        MessageBox.Show(parentWnd, "Los cambios han sido guardados.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    case DictElems.Word:
                    {
                        var items = m_items as ObservableCollection<Models.Word>;
                        m_collectionToEdit.Size = (short)items.Count;

                        // Erase every record related to m_collectionToEdit in word_collection_contents table
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            dbContext.Database.ExecuteSqlInterpolated($"DELETE FROM word_collection_contents WHERE collection_id = {m_collectionToEdit.CollectionId};");
                            // Put the necessary information from m_items into word_collection_contents table
                            foreach (var item in items)
                            {
                                dbContext.Database.ExecuteSqlInterpolated($"INSERT INTO word_collection_contents (collection_id, word_id) VALUES ({m_collectionToEdit.CollectionId}, {item.WordId});");
                            }

                            dbContext.Collections.Update(m_collectionToEdit);
                            dbContext.SaveChanges();
                        }

                        MessageBox.Show(parentWnd, "Los cambios han sido guardados.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                }
            }
            else if (m_collectionNameChanged)
            {
                MessageBox.Show(parentWnd, "Los cambios han sido guardados.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
