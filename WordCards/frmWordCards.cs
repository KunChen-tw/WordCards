using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WMPLib;

namespace WordCards
{
    public partial class frmWordCards: Form
    {
        /// <summary>
        /// 單字清單
        /// </summary>
        WordCollection _WordList = new WordCollection();

        /// <summary>
        /// Windows Media Player 播放器
        /// </summary>
        WindowsMediaPlayer wmp = new WindowsMediaPlayer();

        /// <summary>
        /// 單字檔名
        /// </summary>
        string strWordFile = "WordCards.txt";

        /// <summary>
        /// 是否自動播放
        /// </summary>
        bool isPlay = false;



        public frmWordCards()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 顯示單字
        /// </summary>
        /// <param name="word">單字物件</param>
        private void ShowWord(WordItem word)
        {
            txtWord.Text = word.Word;
            txtPhonogram.Text = word.Phonogram;
            txtExplain.Text = word.Explain;

        }

        /// <summary>
        /// 播放單字的發音
        /// </summary>
        /// <param name="word"></param>
        private void PlayWord(WordItem word)
        {
            // 檢查單字的發音路徑是否存在
            if (File.Exists(word.SoundPath))
            {
                // 播放單字的發音
                wmp.URL = word.SoundPath;
                wmp.settings.autoStart = false; // 自動播放
                wmp.settings.mute = false;
                wmp.controls.play();

            } else
            {
                tsslMessage.Text = $"{word.SoundPath} 找不到發音檔案，請確認檔案是否存在。";
            }
        }

        /// <summary>
        /// 將單字加入到播放清單
        /// </summary>
        private void UpdateWordList()
        {
            lstWordList.Items.Clear();
            foreach (WordItem item in _WordList)
            {
                lstWordList.Items.Add(item);
            }
        }

        private void frmWordCards_Load(object sender, EventArgs e)
        {

            string[] lines;

            if (File.Exists(strWordFile))
            {
                lines = File.ReadAllLines(strWordFile);
                _WordList.LoadFromStringArray(lines);
                UpdateWordList();
            }
            else
            {
                MessageBox.Show("找不到單字檔案，請確認檔案是否存在。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }

        private void lstWordList_Click(object sender, EventArgs e)
        {
            if (isPlay)
            {
                btnAutoPlay.PerformClick();
            }
            if (lstWordList.SelectedItem != null)
            {
                if (lstWordList.SelectedItem.ToString().Length > 0)
                {
                    // 取得目前選取的單字索引
                    int idx = lstWordList.SelectedIndex;
                    // 顯示單字
                    ShowWord(_WordList[idx]);
                    // 播放單字的發音
                    PlayWord(_WordList[idx]);
                }
            }
           
        }
    }
}
