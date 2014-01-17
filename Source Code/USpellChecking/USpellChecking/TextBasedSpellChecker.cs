/*
 * Yezilghan waqti: 2008.6.15
 * Yaghuqi: Mardan Hoshur
 *  
 * Ozgertix Tarihi:
 *    1: 
 *          Ozgertuku:
 *          waqti:
 *    2:
 *    3:
 */
using System;
using System.Collections.Generic;
using System.IO;
using Net.Uyghurdev.Spelling.Interfaces;

namespace Net.Uyghurdev.Spelling
{
    /// <summary>
    /// تېكست ھۆججەت ئچىدىكى سۆزلۈك ئامبىرىنى ئاساس قىلىدىغان، كوررىكتورلۇق تىپى
    /// 
    /// </summary>
    public class TextBasedSpellChecker : IInitialable, ISpellCheckable
    {

        #region ISpellCheckable Members

        public bool IsCorrect(string word)
        {
           return  !this.isMisspelled(word);
        }

        public IList<string> GetSuggestions(string missWord, int neededCount, params object[] opntions)
        {
            return this.getSuggestionsTemp(missWord, 15, true, false);
        }

        public bool HasReplacePeer(string missWord)
        {
            return this.hasReplacePear(missWord);
        }

        public string GetReplacePeer(string missWord)
        {
            return getReplacePear(missWord);
        }

        #endregion

        #region IInitialable Members

        public bool Intitial(IDictionary<string, object> paramDictionary)
        {
            if (paramDictionary == null)
            {
                if (!paramDictionary.ContainsKey("path"))
                    throw new ArgumentNullException("paramDictionary");
            }
            if (!paramDictionary.ContainsKey("path"))
                throw new ArgumentException("پارامېتىرى تولدۇرۇلمىغان path دەشلەپلەشتۈرگۈچىنىڭ  ");

            string path = paramDictionary["path"] as string;
            if (!path.EndsWith("\\"))
                path += "\\";


            foreach(string filename in this.filelist)
            {
                if (!this.paths.ContainsKey(filename))
                    this.paths.Add(filename, Path.Combine(path, filename));
                else
                    this.paths[filename] = Path.Combine(path, filename);
            }

            try
            {
                this.LoadDic();
            }
            catch { return false; }
            try
            {
                this.LoadRepDic();
            }
            catch { }
            return true;

        }

        #endregion

        #region Ozgerguqiler
        
        private System.Collections.Generic.Dictionary<String, bool> _wordlist;
        private System.Collections.Generic.Dictionary<string, string> _repdictionary;
        private System.Collections.Generic.List<int> DicTemp;
        private System.Collections.Generic.Dictionary<string, string> paths;


        private char[] ouou = { '\u0648', '\u06c7', '\u06c6', '\u06c8' };
        private char[] ououei = { '\u06d0', '\u0649', '\u0648', '\u06c7', '\u06c6', '\u06c8' };
        private char[] ououeiae = { '\u0627', '\u06d5', '\u06d0', '\u0649', '\u0648', '\u06c7', '\u06c6', '\u06c8' };
        private char[] ei = { '\u06d0', '\u0649' };
        private char[] ae = { '\u0627', '\u06d5' };
        private string[] filelist = { "dic.dic", "user.dic", "Rep.dic", "userRep.dic" };

        private int ignoredLength = 3;// 两个词的字母长度区别大于这个值,则不推荐它

        #endregion
    
        #region constructor
        public TextBasedSpellChecker()
        {
            this._repdictionary = new Dictionary<string, string>();
            this._wordlist = new Dictionary<string, bool>();
            this.paths = new Dictionary<string, string>();
        }
        #endregion

        #region Other methods
      
        private IList<string> getSuggestionsTemp(string word, int needCount, bool firstLetterSame, bool lastLetterSame)
        {
            //检查词库是否被加载, 如果没有加载,则返回空字符串数组
            char first, second, secondKey;
            float disf;
            if (word.Length <= 2)
            { return new List<string>(); }
            List<SingleResult> resultList = new List<SingleResult>();
          
            #region //从词库一个一个地读取词
            first = word[0];
            string key;
            foreach (string s in this._wordlist.Keys)
            {
                key = s;
                if (Math.Abs(word.Length - key.Length) > this.ignoredLength)
                    continue;
                if (key.Length < 3)
                    continue;
                //如果大开了智能功能,则执行以下算法 (可以设)

                if (firstLetterSame == true)
                {
                    #region  智能分析

                    if (ContainChar(first, ououeiae))
                    {
                        //如果被检词的第一个字母时没有加 "ئ" 的元音的话暂时不提供智能分析
                    }
                    else if (first != 'ئ')
                    {

                        #region  如果第一个字母维复音
                        if (word[0] != key[0])
                            continue;
                        else
                        {
                            second = word[1];
                            secondKey = key[1];
                            if (ContainChar(second, ei))
                            {
                                if (!ContainChar(secondKey, ei))
                                    continue;
                            }
                            else if (ContainChar(second, ouou))
                            {
                                if (!ContainChar(secondKey, ouou))
                                    continue;
                            }
                            else
                            {
                                //因为维语中有一些不符合维语传统规格的词(外来),所以取消了这部分. 如:(كرېدىتلىق , پسىخىكىسىدىمۇ)
                                //if (!ei.IsMatch(secondKey))
                                //{
                                //    continue;
                                //}
                            }
                        }
                    }
                        #endregion
                    else
                    {
                        #region 如果被检此地第一个字母是元音的前缀 如:"ئ"
                        second = word[1];
                        secondKey = key[1];
                        if (ContainChar(second, ei))
                        {
                            if (key[0] != 'ئ' || !ContainChar(secondKey, ei))
                                continue;
                        }
                        else if (ContainChar(second, ouou))
                        {
                            if (key[0] != 'ئ' || !ContainChar(secondKey, ouou))
                                continue;
                        }
                        else if (ContainChar(second, ae))
                        {
                            if (key[0] != 'ئ' || !ContainChar(secondKey, ae))
                                continue;
                        }
                        else
                        {
                            continue;
                        }
                        #endregion
                    }
                    #endregion
                }
                //if (lastLetterSame == true)
                //{
                //    if (word.Length > 12)
                //    {
                //        if (getLevenshtein_distance(word.Substring(word.Length - 3, 3), word.Substring(key.Length - 3, 3)) > 1)
                //        { continue; }
                //    }
                //}
                
                //两个词的最后字母不同,则忽略(可以设)
                //if (lastLetterSame == true && word[word.Length - 1] != key[key.Length - 1])
                //    continue;
                
                if ((disf = this.getLDPercent(word, key)) > 0.6F)
                     resultList.Add(new SingleResult(key,disf));
            }//while((link = link.Next) != null );
            #endregion

            resultList.Sort();
            List<string> finalResultList = new List<string>();
            for (int ii = 0; ii < resultList.Count; ii++)
            {
                if (ii == needCount)
                    break;
                finalResultList.Add(resultList[ii].Word);
            }
            return finalResultList;
        }

        /// <summary>
        /// 检查给定的词是否在词库里面, 如果有返回为true,否则返回为false. 如果词典没有加载成功的条件下调用本函数,会返回false.
        /// 本方法的依据是:"是否在词库里面"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isMisspelled(string word)
        {
            if (word.Length < 3)
            { return false; }
            if (this._wordlist.Count == 0)
                return false;
            return !this._wordlist.ContainsKey(word);
        }

        /// <summary>
        /// 判断某个拼写词有没有,直接替换的候选词
        /// </summary>
        /// <param name="mispellword"></param>
        /// <returns></returns>
        private bool hasReplacePear(string mispellword)
        {
            return this._repdictionary.ContainsKey(mispellword);
        }

        private string getReplacePear(string mispellword)
        {
            string sug = mispellword;
            if (this._repdictionary.TryGetValue(mispellword, out sug))
                return sug;
            return mispellword;
        }

        #endregion

        #region load dic methods

        /// <summary>
        /// 载入词典词库 (载入的默认词库文件为同目目录下的DicKey.dic)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        private void LoadDic()
        {
            this._repdictionary = new Dictionary<string, string>();
            if (!this.paths.ContainsKey("dic.dic"))
                throw new Exception("تىپ دەسلەپلەشتۈرۈلمىگەن");
            if (!File.Exists(this.paths["dic.dic"]))
                throw new FileNotFoundException("تۆۋەندىكى ھۆججەت يۈتكەن:" + Environment.NewLine + "  " + this.paths["dic.dic"]);
            StreamReader sr = File.OpenText(this.paths["dic.dic"]);
            this.DicTemp = new List<int>();
            this._wordlist = new Dictionary<string, bool>();
            string inStr;
            //开始读
            while (sr.Peek() >= 0)
            {
                inStr = sr.ReadLine().Trim();
                if (!_wordlist.ContainsKey(inStr))
                {
                    this._wordlist.Add(inStr, false);
                }
            }
            //read user.dic
            if (!this.paths.ContainsKey("user.dic"))
            { return; }
            if (!File.Exists(this.paths["user.dic"]))
            { return; }
            sr = File.OpenText(this.paths["user.dic"]);
            //开始读
            while (sr.Peek() >= 0)
            {
                inStr = sr.ReadLine().Trim();
                if (!_wordlist.ContainsKey(inStr))
                {
                    _wordlist.Add(inStr, false);
                }
            }
        }

        private void LoadRepDic()
        {
            this._repdictionary = new Dictionary<string, string>();

            string[] inStrs;
            try
            {
                StreamReader sr = File.OpenText(this.paths["Rep.dic"]);
                //开始读
                while (sr.Peek() >= 0)
                {
                    inStrs = sr.ReadLine().Split(new char[] { '\t' });
                    if (inStrs.Length != 2)
                        continue;
                    if (!_repdictionary.ContainsKey(inStrs[0]))
                        this._repdictionary.Add(inStrs[0], inStrs[1]);
                }
            }
            catch { }

            try
            {
                StreamReader sr = File.OpenText(this.paths["userRep.dic"]);
                //开始读
                while (sr.Peek() >= 0)
                {
                    inStrs = sr.ReadLine().Split(new char[] { '#' });
                    if (inStrs.Length != 2)
                        continue;
                    if (!_repdictionary.ContainsKey(inStrs[0]))
                    {
                        this._repdictionary.Add(inStrs[0], inStrs[1]);
                    }
                }
            }
            catch
            { }
        }
#endregion

        #region Helper Methods

        /// <summary>
        /// 检查某个字符是否在某个字符数组里面
        /// </summary>
        /// <param name="c"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        private bool ContainChar(char c, char[] cc)
        {
            //for (int i = 0; i < cc.Length; i++)
            //{
            //    if (c == cc[i])
            //        return true;
            //}
            return Array.IndexOf<char>(cc, c) >= 0;
            //return false;
        }
        #endregion

        #region Distance Helpers

        /// <summary>
        /// 按相似度排列    //mardan
        /// </summary>
        /// <param name="list"></param>
        /// <param name="str"></param>
        private void Sort(ref float[] list, ref string[] str)
        {
            float temp;
            string sTemp;
            int i, j;
            bool done = false;
            j = 1;
            while ((j < list.Length) && (!done))
            {
                done = true;
                for (i = 0; i < list.Length - j; i++)
                {
                    if (list[i] < list[i + 1])
                    {
                        done = false;
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                        //
                        sTemp = str[i];
                        str[i] = str[i + 1];
                        str[i + 1] = sTemp;
                    }
                }
                j++;
            }
        }
        /// <summary>
        /// 获取两个字符的 Levenshtein Distance 相似值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private int getLD(string firstWord, string secondWord)
        {

            int n = firstWord.Length; //length of s
            int m = secondWord.Length; //length of t
            int[,] d = new int[n + 1, m + 1]; // matrix
            int cost; // cost
            // Step 1
            if (n == 0) return m;
            if (m == 0) return n;
            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;
            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    //----------------< 这部分被为了提高性能,被mardan修改
                    //cost = (secondWord.Substring(j - 1, 1) == firstWord.Substring(i - 1, 1) ? 0 : 1);
                    cost = (secondWord[j - 1] == firstWord[i - 1] ? 0 : 1);
                    //--------------------------------------------------->

                    // Step 6
                    d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                              d[i - 1, j - 1] + cost);


                }
            }
            // Step 7
            return d[n, m];
        }

        /// <summary>
        /// Levenshtein Distance 相似值 变换为半分值,(小数精度为5)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private float getLDPercent(string s, string t)
        {
            int l = s.Length > t.Length ? s.Length : t.Length;
            int d = getLD(s, t);
            float db = 1 - ((float)d / l);
            return (float)Math.Round(db, 5);

        }

        #endregion

        #region Temp


        /*  بۇرۇن سىناقق ئۈچۈن ئىشلەتكەن ئۇسۇل.    مەلۇم ھەرپنىڭ ئۈزۈك تاۋۇش ئىكەنلىكىگە ھۆكۈم قىلىدۇ
        private Boolean Uzuk(char c)                             //判断字母的辅音属性
        {
             
            int asci = (int)c;
            if (asci == 1662 || asci == 1578 || asci == 1670 || asci == 1582 || asci == 1587 || asci == 1588 || asci == 1601 || asci == 1602 || asci == 1603 || asci == 1726 || asci == 1576 || asci == 1583 || asci == 1585 || asci == 1586 || asci == 1688 || asci == 1580 || asci == 1594 || asci == 1711 || asci == 1709 || asci == 1604 || asci == 1605 || asci == 1606 || asci == 1739 || asci == 1610)

                return true;
            else
                return false;
        }
         */

        /*  بۇرۇن سىناقق ئۈچۈن ئىشلەتكەن ئۇسۇل.    مەلۇم سۆزنىڭ سوزۇق تاۋۇش قىسمىنى ئېلىپ بېرىدۇ
        private Boolean Sozuq(char c)                            //判断字母的元音属性
        {


            int asci = (int)c;
            if (asci == 1575 || asci == 1608 || asci == 1609 || asci == 1744 || asci == 1749 || asci == 1734 || asci == 1735 || asci == 1736)
            {
                return true;

            }
            else
                return false;

        }
         * /
        /// <summary>
        /// sozning sozuk tawuxlirini qikiewetix
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
            
        /* بۇرۇن سىناقق ئۈچۈن ئىشلەتكەن ئۇسۇل.    مەلۇم سۆزنىڭ ئۈزۈك تاۋۇش قىسمىنى ئېلىپ بېرىدۇ
            
        private string getUzuk(string word)
        {
            string uzukWord =  string.Empty;
            foreach (char c in word)
            {
                if (this.Uzuk(c))
                {
                    uzukWord = uzukWord + c.ToString();
                }
            }
            return uzukWord;

        }
             
         */

        #endregion

        #region deleted

        //public int addWord(string word, string path)    //mardan
        //{
        //    if (!this._wordlist.ContainsKey(word))
        //    {
        //        this._wordlist.Add(word, false);
        //        System.IO.StreamWriter sw = File.AppendText(Path.Combine(path, "user.dic"));
        //        try
        //        {
        //            sw.WriteLine(word);
        //        }
        //        catch
        //        {  }
        //    }
        //    return this._wordlist.Count;
        //}

        //public int addWordRange(string[] word, string path)    //mardan
        //{
        //    try
        //    {
        //        System.IO.StreamWriter sw = File.AppendText(Path.Combine(path, "user.dic"));
        //        for (int i = 0; i < word.Length; i++)
        //        {
        //            if (this._wordlist.ContainsKey(word[i]))
        //                continue;
        //            this._wordlist.Add(word[i], false);
        //            sw.WriteLine(word[i]);
        //        }
        //    }
        //    catch
        //    { }
        //    return this._wordlist.Count;
        //}

        //public void AddToUserRepdicWithoutSave(string misWord, string rtWord)
        //{
        //    if (this._repdictionary.ContainsKey(misWord))
        //        return;
        //    this._repdictionary.Add(misWord, rtWord);
        //}

        //public void AddToUserDicWithoutSave(string word)
        //{
        //    if (this._wordlist.ContainsKey(word))
        //        return;
        //    this._wordlist.Add(word, false);
        //}

        #endregion

        #region Inner class

        private class SingleResult: IComparable<SingleResult>
        {
            private string word;
            private float score;


            public string Word
            {
                get { return word; }
                set { word = value; }
            }


            public float Score
            {
                get { return score; }
                set { score = value; }
            }

            public SingleResult(string word, float score)
            {
                this.word = word;
                this.score = score;
            }
            public override string ToString()
            {
                return this.word;
            }



            #region IComparable<SingleResult> Members

            public int CompareTo(SingleResult other)
            {
                return other.score.CompareTo(this.score);
            }

            #endregion
        }
        #endregion
    }
}
