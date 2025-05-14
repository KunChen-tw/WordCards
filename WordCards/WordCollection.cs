using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WordCards
{
    /// <summary>
    /// WordCollection 類別代表單字的集合，繼承自 Collection<WordItem>。
    /// </summary>
    class WordCollection : Collection<WordItem>
    {
        /// <summary>
        /// 從字串陣列載入資料
        /// /// </summary>
        /// <param name="lines">輸入的單字字串陣列</param>
        public void LoadFromStringArray(string[] lines)
        {
            // 將字串陣列的資料載入到 WordCollection 物件中
            foreach (string line in lines)
            {
                // 檢查字串是否為空或只包含空白字元
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // 產生 item 物件
                    WordItem item = new WordItem(line);
                    this.Add(item);
                }
            }
        }

        /// <summary>
        /// 將 WordCollection 物件的資料儲存到檔案中
        /// </summary>
        /// <param name="filePath">檔案名稱</param>

        public void SaveToFile(string filePath)
        {
            // 將 WordCollection 物件的資料儲存到檔案中
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (WordItem item in this)
                {
                    // 將每個單字項目轉換為字串並寫入檔案
                    writer.WriteLine(item.ToLineString());
                }
            }
        }
    }
}
