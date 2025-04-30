using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTSVFile
{
    class WordItem
    {
        #region 成員變數
        public string Word { get; set; }
        public string Phonogram { get; set; }
        public string SoundPath { get; set; }
        public string Explain { get; set; }
        #endregion

        public WordItem(string str)
        {
            string[] strLists = str.Split('\t');
            if (strLists.Length < 3)
            {
                throw new ArgumentException("Invalid input string format.");
            }
            else // list.Length >= 3
            {
                Word = strLists[0];
                Phonogram = strLists[1];
                SoundPath = strLists[2];
                Explain = string.Join(Environment.NewLine, strLists.Skip(3)); 
            }
        }
    }
}
