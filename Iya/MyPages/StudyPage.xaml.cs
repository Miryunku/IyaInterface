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
    /// Interaction logic for StudyPage.xaml
    /// </summary>
    public partial class StudyPage : Page
    {
        private object m_elements = null; // List<T>
        private Random m_randomEngine = new Random();
        private int m_elemPtr = -1;
        private int m_collectionType = 0;
        private Models2.Collection m_collection = null;

        public StudyPage(Models2.Collection collection)
        {
            InitializeComponent();

            m_collection = collection;
            switch (collection.Type)
            {
                case "Component":
                {
                    m_collectionType = 1;
                    var elements = new List<Models.Component>(collection.Size);

                    using (var dbContext1 = new Models2.IyaUsersDbContext())
                    {
                        dbContext1.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                        foreach (short id in dbContext1.ComponentCollectionContents.Where(ccc => ccc.CollectionId == collection.CollectionId).Select(ccc => ccc.ComponentId))
                        {
                            using (var dbContext2 = new Models.IyaDbContext())
                            {
                                dbContext2.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                                elements.Add(dbContext2.Components.Find(id));
                            }
                        }
                    }

                    m_elements = elements;
                    tblRemaining.Text = elements.Count.ToString();
                    m_elemPtr = m_randomEngine.Next(0, elements.Count);
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }

                case "Kanji":
                {
                    m_collectionType = 2;
                    var elements = new List<Models.Kanji>(collection.Size);

                    using (var dbContext1 = new Models2.IyaUsersDbContext())
                    {
                        dbContext1.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                        foreach (short id in dbContext1.KanjiCollectionContents.Where(kcc => kcc.CollectionId == collection.CollectionId).Select(kcc => kcc.KanjiId))
                        {
                            using (var dbContext2 = new Models.IyaDbContext())
                            {
                                dbContext2.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                                elements.Add(dbContext2.Kanjis.Find(id));
                            }
                        }
                    }

                    m_elements = elements;
                    tblRemaining.Text = elements.Count.ToString();
                    m_elemPtr = m_randomEngine.Next(0, elements.Count);
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }

                case "Word":
                {
                    m_collectionType = 3;
                    var elements = new List<Models.Word>(collection.Size);

                    using (var dbContext1 = new Models2.IyaUsersDbContext())
                    {
                        dbContext1.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                        foreach (int id in dbContext1.WordCollectionContents.Where(wcc => wcc.CollectionId == collection.CollectionId).Select(wcc => wcc.WordId))
                        {
                            using (var dbContext2 = new Models.IyaDbContext())
                            {
                                dbContext2.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                                elements.Add(dbContext2.Words.Find(id));
                            }
                        }
                    }

                    m_elements = elements;
                    tblRemaining.Text = elements.Count.ToString();
                    m_elemPtr = m_randomEngine.Next(0, elements.Count);
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }
            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            var page = new CollectionsPage(Application.Current.Resources["user"] as Models2.User);
            (this.Parent as Window).Content = page;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            switch (m_collectionType)
            {
                case 1:
                {
                    List<Models.Component> elements = m_elements as List<Models.Component>;
                    elements.RemoveAt(m_elemPtr);
                    if (elements.Count == 0)
                    {
                        m_collection.LastVisit = DateTime.UtcNow.ToString("s");
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            dbContext.Collections.Update(m_collection);
                            dbContext.SaveChanges();
                        }

                        (this.Parent as Window).Content = new CollectionsPage(Application.Current.Resources["user"] as Models2.User);
                        break;
                    }

                    tblRemaining.Text = elements.Count.ToString();
                    m_elemPtr = m_randomEngine.Next(0, elements.Count);
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }

                case 2:
                {
                    List<Models.Kanji> elements = m_elements as List<Models.Kanji>;
                    elements.RemoveAt(m_elemPtr);
                    if (elements.Count == 0)
                    {
                        m_collection.LastVisit = DateTime.UtcNow.ToString("s");
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            dbContext.Collections.Update(m_collection);
                            dbContext.SaveChanges();
                        }

                        (this.Parent as Window).Content = new CollectionsPage(Application.Current.Resources["user"] as Models2.User);
                        break;
                    }

                    tblRemaining.Text = elements.Count.ToString();
                    m_elemPtr = m_randomEngine.Next(0, elements.Count);
                    tblNiponElement.Text = elements[m_elemPtr].ToString();

                    break;
                }

                case 3:
                {
                    List<Models.Word> elements = m_elements as List<Models.Word>;
                    elements.RemoveAt(m_elemPtr);
                    if (elements.Count == 0)
                    {
                        m_collection.LastVisit = DateTime.UtcNow.ToString("s");
                        using (var dbContext = new Models2.IyaUsersDbContext())
                        {
                            dbContext.Collections.Update(m_collection);
                            dbContext.SaveChanges();
                        }

                        (this.Parent as Window).Content = new CollectionsPage(Application.Current.Resources["user"] as Models2.User);
                        break;
                    }

                    tblRemaining.Text = elements.Count.ToString();
                    m_elemPtr = m_randomEngine.Next(0, elements.Count);
                    tblNiponElement.Text = elements[m_elemPtr].ToString();

                    break;
                }
            }
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            switch (m_collectionType)
            {
                case 1:
                {
                    var elements = m_elements as List<Models.Component>;
                    int previous = m_elemPtr;
                    for (int i = 0; i < 3; i++)
                    {
                        m_elemPtr = m_randomEngine.Next(0, elements.Count);
                        if (m_elemPtr != previous)
                        {
                            break;
                        }
                    }
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }

                case 2:
                {
                    var elements = m_elements as List<Models.Kanji>;
                    int previous = m_elemPtr;
                    for (int i = 0; i < 3; i++)
                    {
                        m_elemPtr = m_randomEngine.Next(0, elements.Count);
                        if (m_elemPtr != previous)
                        {
                            break;
                        }
                    }
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }

                case 3:
                {
                    var elements = m_elements as List<Models.Word>;
                    int previous = m_elemPtr;
                    for (int i = 0; i < 3; i++)
                    {
                        m_elemPtr = m_randomEngine.Next(0, elements.Count);
                        if (m_elemPtr != previous)
                        {
                            break;
                        }
                    }
                    tblNiponElement.Text = elements[m_elemPtr].ToString();
                    break;
                }
            }
        }

        private void tblNiponElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (m_collectionType)
            {
                case 1:
                {
                    var wnd = new FlashcardWindow((m_elements as List<Models.Component>)[m_elemPtr], DictElems.Component);
                    wnd.Owner = this.Parent as Window;
                    wnd.Show();
                    break;
                }

                case 2:
                {
                    var wnd = new FlashcardWindow((m_elements as List<Models.Kanji>)[m_elemPtr], DictElems.Kanji);
                    wnd.Owner = this.Parent as Window;
                    wnd.Show();
                    break;
                }

                case 3:
                {
                    var wnd = new FlashcardWindow((m_elements as List<Models.Word>)[m_elemPtr], DictElems.Word);
                    wnd.Owner = this.Parent as Window;
                    wnd.Show();
                    break;
                }
            }
        }
    }
}
