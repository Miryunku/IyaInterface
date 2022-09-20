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
    /// Interaction logic for DictionaryWindow.xaml
    /// </summary>

    public partial class DictionaryWindow : Window
    {
        private ObservableCollection<Models.Component> m_componentHistory = new ObservableCollection<Models.Component>();
        private ObservableCollection<Models.Kanji> m_kanjiHistory = new ObservableCollection<Models.Kanji>();
        private ObservableCollection<Models.Word> m_wordHistory = new ObservableCollection<Models.Word>();

        private string m_searchStatusOk = "Estado de la búsqueda: OK.";
        private string m_searchStatusNotFound = "Estado de la búsqueda: Término no encontrado.";

        public DictionaryWindow()
        {
            InitializeComponent();

            ltbComponentHistory.ItemsSource = m_componentHistory;
            ltbKanjiHistory.ItemsSource = m_kanjiHistory;
            ltbWordHistory.ItemsSource = m_wordHistory;
        }

        private void tbxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(tbxSearch.Text))
            {
                string lastSearch = tbxSearch.Text.Trim();
                tblLastSearch.Text = $"Último item buscado: {lastSearch}";
                tblStatus.Text = m_searchStatusOk; // This is an optimization.

                switch (cmbSearchFlags.SelectedIndex)
                {
                    case 0:
                    {
                        using (var dbContext = new Models.IyaDbContext())
                        {
                            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                            Models.Component component = dbContext.Components.SingleOrDefault(c => c.Component1 == lastSearch);
                            if (component == null)
                            {
                                tblStatus.Text = m_searchStatusNotFound;
                                return;
                            }

                            m_componentHistory.Add(component);

                            stkFlashCards.Children.Clear();
                            stkFlashCards.Children.Add(new MyUserControls.ComponentFlashcard(component));
                        }
                        break;
                    }
                    case 1:
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

                            m_kanjiHistory.Add(kanji);

                            stkFlashCards.Children.Clear();
                            stkFlashCards.Children.Add(new MyUserControls.KanjiFlashcard(kanji));
                        }
                        break;
                    }
                    case 2:
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

                            m_wordHistory.Add(word);

                            stkFlashCards.Children.Clear();
                            stkFlashCards.Children.Add(new MyUserControls.WordFlashcard(word));
                        }
                        break;
                    }
                }
            }
        }

        private void ltbComponentHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && ltbComponentHistory.SelectedIndex != -1)
            {
                stkFlashCards.Children.Clear();
                stkFlashCards.Children.Add(new MyUserControls.ComponentFlashcard(ltbComponentHistory.SelectedItem as Models.Component));
            }
        }

        private void ltbKanjiHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && ltbKanjiHistory.SelectedIndex != -1)
            {
                stkFlashCards.Children.Clear();
                stkFlashCards.Children.Add(new MyUserControls.KanjiFlashcard(ltbKanjiHistory.SelectedItem as Models.Kanji));
            }
        }

        private void ltbWordHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && ltbWordHistory.SelectedIndex != -1)
            {
                stkFlashCards.Children.Clear();
                stkFlashCards.Children.Add(new MyUserControls.WordFlashcard(ltbWordHistory.SelectedItem as Models.Word));
            }
        }
    }
}
