using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    /// Interaction logic for FlashCard.xaml
    /// </summary>
    public partial class WordFlashcard : UserControl
    {
        public WordFlashcard(Word word)
        {
            InitializeComponent();

            Dictionary<int, string> iyaTags = Application.Current.Resources["IyaTags"] as Dictionary<int, string>;
            Dictionary<int, string> iyaSubtags = Application.Current.Resources["IyaSubtags"] as Dictionary<int, string>;

            // - - - Structure of the Flashcard for words:
            // First: the word
            // Second: the readings of the kanji if any
            // Third: other forms of the word if any
            // Fourth: attributes of the word
            // Fifth: the meanings of the word
            // Sixth: the comment about the word

            tblWord.Text = word.Word1;

            // Parse word.KanjiReading just to add colors.
            string pattern = @"\[(.+?)\]\((.+?)\);"; // Used for Word.KanjiReading and Word.OtherForms

            if (string.IsNullOrEmpty(word.KanjiReading))
            {
                tblKanjiReadingsContent.Text = "Sin kanji.";
            }
            else
            {
                string[] kanjiReadings = word.KanjiReading.Split('/');
                foreach (string s in kanjiReadings)
                {
                    MatchCollection myMatches = Regex.Matches(s, pattern, RegexOptions.None, TimeSpan.FromSeconds(5.0));
                    foreach (Match match in myMatches)
                    {
                        for (int i = 1; i < match.Groups.Count; i++)
                        {
                            if (i % 2 != 0)
                            {
                                tblKanjiReadingsContent.Inlines.Add("[");
                                tblKanjiReadingsContent.Inlines.Add(new Run(match.Groups[i].Value) { Foreground = Brushes.LightSalmon });
                                tblKanjiReadingsContent.Inlines.Add("]");
                            }
                            else
                            {
                                tblKanjiReadingsContent.Inlines.Add("(");
                                tblKanjiReadingsContent.Inlines.Add(new Run(match.Groups[i].Value) { Foreground = Brushes.LightSkyBlue });
                                tblKanjiReadingsContent.Inlines.Add("); ");
                            }
                        }
                    }
                    tblKanjiReadingsContent.Inlines.Add("\n");
                }
                tblKanjiReadingsContent.Inlines.Remove(tblKanjiReadingsContent.Inlines.LastInline);
            }

            // Parse word.OtherForms just to add colors.
            MatchCollection formsMatches = Regex.Matches(word.OtherForms, pattern, RegexOptions.None, TimeSpan.FromSeconds(5.0));
            if (formsMatches.Any()) // There are words without other forms.
            {
                foreach (Match match in formsMatches)
                {
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        if (i % 2 != 0)
                        {
                            tblOtherFormsContent.Inlines.Add("[");
                            tblOtherFormsContent.Inlines.Add(new Run(match.Groups[i].Value) { Foreground = Brushes.LightSalmon });
                            tblOtherFormsContent.Inlines.Add("]");
                        }
                        else
                        {
                            tblOtherFormsContent.Inlines.Add("(");
                            tblOtherFormsContent.Inlines.Add(new Run(match.Groups[i].Value) { Foreground = Brushes.LightSkyBlue });
                            tblOtherFormsContent.Inlines.Add(");");
                            tblOtherFormsContent.Inlines.Add("\n");
                        }
                    }
                }
                tblOtherFormsContent.Inlines.Remove(tblOtherFormsContent.Inlines.LastInline);
            }
            else
            {
                tblOtherFormsContent.Text = "Sin otras formas.";
            }

            // *** Attributes ***

            // JLPT Level.
            if (word.JlptLvl == 0)
            {
                tblJlptLevel.Text = $"Nivel JLPT: Sin nivel.";
            }
            else
            {
                tblJlptLevel.Text = $"Nivel JLPT: {word.JlptLvl}";
            }

            // ***  End of Attributes ***

            // Parse the meanings
            string meaningRegex = @"\[\[(\d+)\]\](?:([^\[\]\{\}]+)\[(\d+)\]\{([^\[\]\{\}]+)\})+";
            MatchCollection meaningMatches = Regex.Matches(word.Meaning, meaningRegex, RegexOptions.None, TimeSpan.FromSeconds(5.0));
            foreach (Match match in meaningMatches)
            {
                WordMeaningArranger arranger = new WordMeaningArranger();

                // Populate the arranger.
                for (int group_ctr = 1; group_ctr < match.Groups.Count; group_ctr++)
                {
                    for (int capture_ctr = 0; capture_ctr < match.Groups[group_ctr].Captures.Count; capture_ctr++)
                    {
                        switch (group_ctr)
                        {
                            case 1: // The tag
                            {
                                arranger.Tag = int.Parse(match.Groups[group_ctr].Captures[capture_ctr].Value);
                                break;
                            }
                            case 2: // The meanings
                            {
                                arranger.Meanings.Add(match.Groups[group_ctr].Captures[capture_ctr].Value);
                                break;
                            }
                            case 3: // The sub-tags
                            {
                                arranger.Subtags.Add(int.Parse(match.Groups[group_ctr].Captures[capture_ctr].Value));
                                break;
                            }
                            case 4: // The examples sentences
                            {
                                arranger.Examples.Add(match.Groups[group_ctr].Captures[capture_ctr].Value);
                                break;
                            }
                        }
                    }
                }

                tblMeaningsContent.Inlines.Add(new Run(iyaTags[arranger.Tag]) { Foreground = Brushes.Peru });
                for (int i = 0; i < arranger.Meanings.Count; i++)
                {
                    tblMeaningsContent.Inlines.Add($"\n[{i + 1}] {arranger.Meanings[i]} ");
                    tblMeaningsContent.Inlines.Add(new Run(iyaSubtags[arranger.Subtags[i]]) { FontSize = 18, Foreground = Brushes.Orange });
                    if (arranger.Examples[i] != "0")
                    {
                        tblMeaningsContent.Inlines.Add("\n");
                        tblMeaningsContent.Inlines.Add(new Run(arranger.Examples[i]) { Foreground = Brushes.LightSeaGreen });
                    }
                }
                tblMeaningsContent.Inlines.Add("\n");
            }
            tblMeaningsContent.Inlines.Remove(tblMeaningsContent.Inlines.LastInline);

            // Comment
            if (string.IsNullOrEmpty(word.Comment))
            {
                tblCommentContent.Text = "Sin comentario.";
            }
            else
            {
                tblCommentContent.Text = word.Comment;
            }
        }
    }
}
