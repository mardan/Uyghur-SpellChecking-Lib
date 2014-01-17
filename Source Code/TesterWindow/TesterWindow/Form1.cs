using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TesterWindow
{
    public partial class Form1 : Form
    {

        private Net.Uyghurdev.Spelling.Interfaces.ISpellCheckable _spellChecker;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _spellChecker = new Net.Uyghurdev.Spelling.TextBasedSpellChecker();

            Net.Uyghurdev.Spelling.Interfaces.IInitialable init
                = ((Net.Uyghurdev.Spelling.TextBasedSpellChecker)_spellChecker) as Net.Uyghurdev.Spelling.Interfaces.IInitialable;

            Dictionary<string, object> paramlar = new Dictionary<string, object>();
            
            //soz ammarlirining turux orininining adrisini "path" ning value si kilip yollap berix kerek.  masilan:  "c:\spel\words\"
            /*
             
                string p = HttpContext.Current.Request.ApplicationPath;
                if (p == "/")
                p = "";
                string filepath = Server.MapPath(p + "/App_Data/");
                paramlar.Add("path", filepath);
             
             */
            paramlar.Add("path", Application.StartupPath);

            bool succ = init.Intitial(paramlar);
            
            if (!succ)
            {
                MessageBox.Show("دەسلەپلەشتۈرۈش جەريانىدا خاتالىق كۆرۈلدى");
                Application.Exit();
            }
        }

        private void textBoxKey_TextChanged(object sender, EventArgs e)
        {
            string word = this.textBoxKey.Text;
            
            if (!_spellChecker.IsCorrect(word)) //agar soz hata bolup kalsa
            {


                if (_spellChecker.HasReplacePeer(word)) // agar biwaste almaxturuxka bolidighan namzat bolsa
                {
                    string namzat = _spellChecker.GetReplacePeer(word);

                    this.listBoxSugs.DataSource = new string[] { namzat };
                }
                else // biwase almaxturuxka bollidighan namzat soz bolmisa , GetSuggestions(...) arkilix namzatlargha erixix
                {

                    IList<string> list = _spellChecker.GetSuggestions(word, 15, null);

                    if (list.Count == 0)
                    {
                        this.listBoxSugs.DataSource = new string[] { "[نامزات سۆز تېپىلمىدى]" };
                    }
                    else
                    {
                        this.listBoxSugs.DataSource = list;
                    }
                }
            }
            else
            {
                this.listBoxSugs.DataSource = new string[] { "[ئىملا خاتالىقى يوق]" };
            }

        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            textBoxKey_TextChanged(null, null);
        }
    }
}
