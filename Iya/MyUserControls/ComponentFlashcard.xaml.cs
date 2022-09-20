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

namespace Iya.MyUserControls
{
    /// <summary>
    /// Interaction logic for ComponentFlashcard.xaml
    /// </summary>
    public partial class ComponentFlashcard : UserControl
    {
        public ComponentFlashcard(Models.Component component)
        {
            InitializeComponent();

            tblComponent.Text = component.Component1;

            tblMeaningContent.Text = component.Meaning;

            if (component.IsCustomMng == 0)
            {
                tblCustomMeaning.Text = "¿Significado personalizado?: No.";
            }
            else
            {
                tblCustomMeaning.Text = "¿Significado personalizado?: Sí.";
            }

            if (component.IsMngLost == 0)
            {
                tblMeaningLost.Text = "¿Significado perdido?: No.";
            }
            else
            {
                tblMeaningLost.Text = "¿Significado perdido?: Sí.";
            }

            if (component.IsKanji == 0)
            {
                tblIsKanji.Text = "¿Es kanji por sí mismo?: No.";
            }
            else
            {
                tblIsKanji.Text = "¿Es kanji por sí mismo?: Sí.";
            }
        }
    }
}
