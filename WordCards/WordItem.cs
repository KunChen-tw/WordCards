using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCards
{
    /// <summary>
    /// WordItem 類別代表一個單字項目，包含單字、音標、發音路徑和解釋。
    /// </summary>
    public class WordItem
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

        /// <summary>
        /// 將 WordItem 物件轉換為字串，格式為 "Word"。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Word}";
        }

        /// <summary>
        /// 將 WordItem 物件轉換為字串
        /// </summary>
        /// <returns>一行字串，格式為 "Word\tPhonogram\tSoundPath\tExplain"。</returns>
        public string ToLineString()
        {
            string strExplain = Explain.Replace(Environment.NewLine, "\t");
           
            return $"{Word}\t{Phonogram}\t{SoundPath}\t{strExplain}";
        }
    }
}
