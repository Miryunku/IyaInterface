using System;
using System.Collections.Generic;
using System.Globalization;
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

using Iya.Models;

namespace Iya
{
    /// <summary>
    /// Interaction logic for AddWordWindow.xaml
    /// </summary>
    public partial class AddWordWindow : Window
    {
        public AddWordWindow(bool isEdit, string word = "")
        {
            InitializeComponent();
            if (isEdit)
            {
                this.Title = "Editar palabra";

                btnAdd.Content = "OK";
                btnAdd.Click += btnOK_Click;
            }
            else
            {
                this.Title = "Agregar palabra";

                btnAdd.Content = "Agregar";
                btnAdd.Click += btnAdd_Click;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var word = new Word();
            word.Word1 = tbxWord.Text;
            word.Kanjis = tbxKanjis.Text;
            word.OtherForms = tbxForms.Text;
            word.Reading = tbxReading.Text;
            word.JlptLvl = (short)cmbJlptLevel.SelectedIndex;
            word.Comment = tbxComment.Text;

            using (var dbContext = new IyaDbContext())
            {
                dbContext.Words.Add(word);
                dbContext.SaveChanges();
            }

            MessageBox.Show(this, "La palabra ha sido agregada.", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        // Accept editions made to the word.
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // To be implemented.
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    /*
    public class NumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            short number = 0;

            // Is a number?
            if (!short.TryParse(value as string, out number))
            {
                return new ValidationResult(false, "El valor no es un número");
            }

            // Is in range?
            if (number < 0 || number > 5)
            {
                return new ValidationResult(false, "El valor debe estar entre 0 y 5");
            }

            return ValidationResult.ValidResult;
        }
    }
    */
}
