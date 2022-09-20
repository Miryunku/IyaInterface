using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iya
{
    public enum DictElems // DictionaryElements
    {
        None,
        Component,
        Kanji,
        Word
    }

    public class WordMeaningArranger
    {
        private int m_tag = 0;
        private List<string> m_meanings = new List<string>();
        private List<int> m_subtags = new List<int>();
        private List<string> m_examples = new List<string>();

        public WordMeaningArranger()
        {
            // Empty.
        }

        public int Tag { get => m_tag; set => m_tag = value; }
        public List<string> Meanings { get => m_meanings; set => m_meanings = value; }
        public List<int> Subtags { get => m_subtags; set => m_subtags = value; }
        public List<string> Examples { get => m_examples; set => m_examples = value; }
    }
}
