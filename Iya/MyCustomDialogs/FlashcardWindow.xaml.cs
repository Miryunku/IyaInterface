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
using System.Windows.Shapes;

namespace Iya
{
    /// <summary>
    /// Interaction logic for FlashcardWindow.xaml
    /// </summary>
    public partial class FlashcardWindow : Window
    {
        public FlashcardWindow(object dbRecord, DictElems type)
        {
            InitializeComponent();

            switch (type)
            {
                case DictElems.Component:
                    stkFlashcards.Children.Add(new MyUserControls.ComponentFlashcard(dbRecord as Models.Component));
                    break;
                case DictElems.Kanji:
                    stkFlashcards.Children.Add(new MyUserControls.KanjiFlashcard(dbRecord as Models.Kanji));
                    break;
                case DictElems.Word:
                    stkFlashcards.Children.Add(new MyUserControls.WordFlashcard(dbRecord as Models.Word));
                    break;
            }
        }
    }
}
