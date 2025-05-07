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
        /// 播放目前選取的單字
        /// </summary>
        private void PlaySelectedWord()
        {
            // 判斷目前選的項目是否為空
            if (lstWordList.SelectedItem != null)
            {
                // 取得目前選取的單字索引
                int idx = lstWordList.SelectedIndex;
                // 顯示單字
                ShowWord(_WordList[idx]);
                // 播放單字的發音
                PlayWord(_WordList[idx]);
            }
        }


        /// <summary>
        /// 將單字加入到播放清單
        /// </summary>
        private void UpdateWordList()
        {
            lstWordList.Items.Clear();
            // 將單字加入到播放清單，畫面停止更新
            lstWordList.BeginUpdate();
            foreach (WordItem item in _WordList)
            {
                lstWordList.Items.Add(item);
            }
            // 單字更新完成，畫面更新
            lstWordList.EndUpdate();
        }

        /// <summary>
        /// 將單字清單的選項移到下一個
        /// </summary>
        private void NextWordList()
        {
            // 將焦點移到單字清單
            lstWordList.Focus();

            // 判斷目前選的下一項是否超過清單的項目數
            if (lstWordList.SelectedIndex + 1 >= _WordList.Count)
                lstWordList.SelectedIndex = 0; // 如果超過就回到第一項
            else
                lstWordList.SelectedIndex++;   // 如果沒有就選擇下一項

            // 計算目前 lstWordList 顯示的行數
            int lstRows = lstWordList.Height / lstWordList.GetItemHeight(0);

            // 如果目前選的項目大於 lstRows / 2
            if (lstWordList.SelectedIndex >= lstRows / 2)
                // 將 lstWordList 的 選項保持在中間
                lstWordList.TopIndex = lstWordList.SelectedIndex - lstRows / 2;
        }


        private void frmWordCards_Load(object sender, EventArgs e)
        {

            string[] lines;

            if (File.Exists(strWordFile))
            {
                lines = File.ReadAllLines(strWordFile);
                // 檢查檔案是否有內容
                if (lines.Length > 0)
                {
                    _WordList.LoadFromStringArray(lines);
                    UpdateWordList();
                    // 將 lstWordList 的選項設為第一個
                    lstWordList.SelectedIndex = 0;
                    // 將第一個單字顯示在畫面上
                    ShowWord(_WordList[0]);
                    tsslMessage.Text = $"載入 {_WordList.Count} 個單字";
                }
                
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
                return;
            }
            if (lstWordList.SelectedItem != null)
            {
                if (lstWordList.SelectedItem.ToString().Length > 0)
                {
                    // 顯示並播放目前選取的單字
                    PlaySelectedWord();
                }
            }
           
        }

        private void timPlayer_Tick(object sender, EventArgs e)
        {
            // 移到下一個單字
            NextWordList();

            // 顯示並播放目前選取的單字
            PlaySelectedWord();
        }

        private void btnAutoPlay_Click(object sender, EventArgs e)
        {
            // 將焦點移到單字清單
            lstWordList.Focus();

            // 若目前不是自動播放
            if (isPlay == false)
            {
                btnAutoPlay.Text = "Stop";
                isPlay = true;

                // 顯示並播放目前選取的單字
                PlaySelectedWord();

                timPlayer.Start();
            }
            else
            {
                timPlayer.Stop();
                isPlay = false;
                btnAutoPlay.Text = "Play";
            }

        }

        private void frmWordCards_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isPlay == true)
                return;

            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    // 當按下 Enter 時，播放下一個單字
                    NextWordList();

                    // 顯示並播放目前選取的單字
                    PlaySelectedWord();

                    e.Handled = true;
                    break;

                case (char)Keys.Space:
                    // 當按下 Space 時，重複播放目前單字
                    if (lstWordList.SelectedIndex >= 0)
                    {
                        // 顯示並播放目前選取的單字
                        PlaySelectedWord();
                    }
                        
                    e.Handled = true;
                    break;
            }

        }
    }
}
