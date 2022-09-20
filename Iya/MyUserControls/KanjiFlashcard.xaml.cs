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

using Iya.Models;

namespace Iya.MyUserControls
{
    /// <summary>
    /// Interaction logic for KanjiFlashCard.xaml
    /// </summary>
    public partial class KanjiFlashcard : UserControl
    {
        public KanjiFlashcard(Kanji kanji)
        {
            InitializeComponent();

            tblKanji.Text = kanji.Kanji1;

            tblReadingsContent.Inlines.Add("On'Yomi: ");
            tblReadingsContent.Inlines.Add(new Run(kanji.OnReadings) { Foreground = Brushes.MediumTurquoise });
            tblReadingsContent.Inlines.Add("\nKun'Yomi: ");
            tblReadingsContent.Inlines.Add(new Run(kanji.KunReadings) { Foreground = Brushes.LightSkyBlue });

            if (string.IsNullOrEmpty(kanji.Components))
            {
                tblComponentsContent.Text = "Sin componentes.";
            }
            else
            {
                tblComponentsContent.Text = kanji.Components;
            }

            tblMeaningsContent.Text = kanji.Meaning;

            if (kanji.JlptLvl == 0)
            {
                tblJlptLevel.Text = "Nivel JLPT: Sin nivel.";
            }
            else
            {
                tblJlptLevel.Text = $"Nivel JLPT: {kanji.JlptLvl}";
            }
        }
    }
}
