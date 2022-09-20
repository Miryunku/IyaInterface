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

            ParseWord(word);
        }

        private void ParseWord(Word word)
        {
            // - - - Structure of the Flashcard for words:
            // First: the word
            // Second: the kanjis of that word if any plus the readings
            // Third: other forms of the word if any plus the readings
            // Fourth: Tiles that contain info about the attributes of the word
            // Fifth: the meanings of the word
            // Sixth: the comment about the word

            tblWord.Inlines.Add(new Run(word.Word1) { Foreground = Brushes.Khaki });
            tblWord.Inlines.Add(" - ");
            tblWord.Inlines.Add(new Run(word.Reading) { Foreground = Brushes.LightSkyBlue });

            // Parse word.Kanjis
            string pattern = @"\[(.+?)\]\((.+?)\);"; // Used for Word.Kanjis and Word.OtherForms

            MatchCollection kanjiMatches = Regex.Matches(word.Kanjis, pattern, RegexOptions.None, TimeSpan.FromSeconds(5.0));
            if (kanjiMatches.Any()) // There are words without kanji.
            {
                foreach (Match match in kanjiMatches)
                {
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        if (i % 2 != 0)
                        {
                            tblKanjisReadings.Inlines.Add("[");
                            tblKanjisReadings.Inlines.Add(new Run(match.Groups[i].Value) { Foreground = Brushes.LightSalmon });
                            tblKanjisReadings.Inlines.Add("]");
                        }
                        else
                        {
                            tblKanjisReadings.Inlines.Add("(");
                            tblKanjisReadings.Inlines.Add(new Run(match.Groups[i].Value) { Foreground = Brushes.LightSkyBlue });
                            tblKanjisReadings.Inlines.Add("); ");
                        }
                    }
                }
            }
            else
            {
                tblKanjisReadings.Inlines.Add(new Run("Sin kanji.") { Foreground = Brushes.WhiteSmoke });
            }

            // Parse word.OtherForms
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
                tblOtherFormsContent.Inlines.Add("Sin otras formas.");
            }
            
            // Populate wrpInfoTiles. If there is a better way, tell me...
            if (word.IsSuruNoun == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/SustantivoSuru.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsPreNounAdj == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/AdjetivoPreSus.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsAdvNoun == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/SustantivoAdverbial.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsNaAdj == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/AdjetivoNa.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsIAdj == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/AdjetivoI.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsGodan == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/VerboGodan.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsIchi == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/VerboIchidan.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsTran == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/VerboTransitivo.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsIntra == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/VerboIntransitivo.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsExpr == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/Expression.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsSon == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/Sonkeigo.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsKen == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/Kenjougo.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.IsArch == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/Arcaico.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.CanNoAdj == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/AdjetivoNo.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }
            if (word.CanToAdv == 1)
            {
                wrpInfoTiles.Visibility = Visibility.Visible;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Iya-Tiles/AdverbioTo.png"));
                image.Stretch = Stretch.None;
                wrpInfoTiles.Children.Add(image);
            }

            if (word.JlptLvl == 0)
            {
                tblJlptLevel.Text = $"Nivel JLPT: Sin nivel.";
            }
            else
            {
                tblJlptLevel.Text = $"Nivel JLPT: {word.JlptLvl}";
            }

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
                
                switch (arranger.Tag)
                {
                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 1:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }

                    case 0:
                    {
                        //tblMeaningsContent.Inlines.Add()
                        break;
                    }
                }
            }

            //

            if (!string.IsNullOrEmpty(word.NounMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Sustantivo\n") { Foreground = Brushes.DodgerBlue });

                string[] nounMeanings = word.NounMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < nounMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {nounMeanings[i]}\n");
                }
            }
            if (!string.IsNullOrEmpty(word.AdjMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Adjetivo\n") { Foreground = Brushes.Red });

                string[] adjMeanings = word.AdjMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < adjMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {adjMeanings[i]}\n");
                }
            }
            if (!string.IsNullOrEmpty(word.VerbMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Verbo\n") { Foreground = Brushes.Lime });

                string[] verbMeanings = word.VerbMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < verbMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {verbMeanings[i]}\n");
                }
            }
            if (!string.IsNullOrEmpty(word.AdvMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Adverbio\n") { Foreground = Brushes.MediumAquamarine });

                string[] advMeanings = word.AdvMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < advMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {advMeanings[i]}\n");
                }
            }
            if (!string.IsNullOrEmpty(word.PrefixMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Prefijo\n") { Foreground = Brushes.Orange });

                string[] prefixMeanings = word.PrefixMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < prefixMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {prefixMeanings[i]}\n");
                }
            }
            if (!string.IsNullOrEmpty(word.SuffixMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Sufijo\n") { Foreground = Brushes.Goldenrod });

                string[] suffixMeanings = word.SuffixMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < suffixMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {suffixMeanings[i]}\n");
                }
            }
            if (!string.IsNullOrEmpty(word.ExprMng))
            {
                tblMeaningsContent.Inlines.Add(new Run("Expresión\n") { Foreground = Brushes.Yellow });

                string[] exprMeanings = word.ExprMng.Split('|', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < exprMeanings.Length; i++)
                {
                    tblMeaningsContent.Inlines.Add($"[{i + 1}] {exprMeanings[i]}\n");
                }
            }

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
