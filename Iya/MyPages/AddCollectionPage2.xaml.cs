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
    /// Interaction logic for AddCollectionPage2.xaml
    /// </summary>
    public partial class AddCollectionPage2 : Page
    {
        private object m_items = null; // ObservableCollection<T>
        private ICollectionView m_collectionView = null;
        private DictElems m_collectionType = DictElems.None;
        private object m_lastCorrectSearchItem = null; // Item to be put into m_items when btnAddItem is pressed.

        private Models2.User m_user = Application.Current.Resources["user"] as Models2.User;

        private string m_searchStatusOk = "Estado de la búsqueda: OK.";
        private string m_searchStatusNotFound = "Estado de la búsqueda: Término no encontrado.";

        public AddCollectionPage2(DictElems collectionType)
        {
            InitializeComponent();

            m_collectionType = collectionType;

            switch (collectionType)
            {
                case DictElems.Component:
                {
                    var items = new ObservableCollection<Models.Component>();
                    m_items = items;
                    ltbCollectionItems.ItemsSource = items;
                    tblSearchWhat.Text = "Buscar componente:";
                    break;
                }
                case DictElems.Kanji:
                {
                    var items = new ObservableCollection<Models.Kanji>();
                    m_items = items;
                    ltbCollectionItems.ItemsSource = items;
                    tblSearchWhat.Text = "Buscar kanji:";
                    break;
                }
                case DictElems.Word:
                {
                    var items = new ObservableCollection<Models.Word>();
                    m_items = items;
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
                    {
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
                    }
                    case DictElems.Kanji:
                    {
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
                    }
                    case DictElems.Word:
                    {
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

        private void btnCreateCollection_Click(object sender, RoutedEventArgs e)
        {
            var parentWnd = this.Parent as Window;
            var caption = "Notificación";

            if (ltbCollectionItems.Items.Count < 1)
            {
                MessageBox.Show(parentWnd, "La colección a crear debe tener items.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (ltbCollectionItems.Items.Count > 50)
            {
                MessageBox.Show(parentWnd, "La colección a crear debe tener menos de 51 items.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbxCollectionName.Text))
            {
                MessageBox.Show(parentWnd, "La colección a crear debe tener un nombre.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var collectionName = tbxCollectionName.Text.Trim();
            foreach (var coll in m_user.Collections)
            {
                if (coll.Name == collectionName)
                {
                    MessageBox.Show(parentWnd, "Ya existe una colección con ese nombre.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            var collection = new Models2.Collection();
            collection.UserId = m_user.UserId;
            collection.Name = collectionName;
            collection.Size = (short)ltbCollectionItems.Items.Count;
            collection.Creation = DateTime.UtcNow.ToString("s");

            // Save it because an id for the collection is needed.
            using (var dbContext = new Models2.IyaUsersDbContext())
            {
                dbContext.Collections.Add(collection);
                dbContext.SaveChanges();

                var collection1 = dbContext.Collections.SingleOrDefault(c => c.Name == collectionName && c.UserId == m_user.UserId);
                switch (m_collectionType)
                {
                    case DictElems.Component:
                    {
                        collection1.Type = "Component";

                        var items = m_items as ObservableCollection<Models.Component>;
                        foreach (var item in items)
                        {
                            dbContext.Database.ExecuteSqlInterpolated($"INSERT INTO component_collection_contents (collection_id, component_id) VALUES ({collection1.CollectionId}, {item.ComponentId});");
                        }

                        dbContext.Collections.Update(collection1);
                        
                        m_user.ComponentCollectionsQuantity += 1;
                        dbContext.Users.Update(m_user);

                        dbContext.SaveChanges();
                        MessageBox.Show(parentWnd, "La colección ha sido agregada.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    case DictElems.Kanji:
                    {
                        collection1.Type = "Kanji";

                        var items = m_items as ObservableCollection<Models.Kanji>;
                        foreach (var item in items)
                        {
                            dbContext.Database.ExecuteSqlInterpolated($"INSERT INTO kanji_collection_contents (collection_id, kanji_id) VALUES ({collection1.CollectionId}, {item.KanjiId});");
                        }

                        dbContext.Collections.Update(collection1);

                        m_user.KanjiCollectionsQuantity += 1;
                        dbContext.Users.Update(m_user);

                        dbContext.SaveChanges();
                        MessageBox.Show(parentWnd, "La colección ha sido agregada.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    case DictElems.Word:
                    {
                        collection1.Type = "Word";

                        var items = m_items as ObservableCollection<Models.Word>;
                        foreach (var item in items)
                        {
                            dbContext.Database.ExecuteSqlInterpolated($"INSERT INTO word_collection_contents (collection_id, word_id) VALUES ({collection1.CollectionId}, {item.WordId});");
                        }

                        dbContext.Collections.Update(collection1);

                        m_user.WordCollectionsQuantity += 1;
                        dbContext.Users.Update(m_user);

                        dbContext.SaveChanges();
                        MessageBox.Show(parentWnd, "La colección ha sido agregada.", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                }
            }
        }
    }
}
