using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Uyghurdev.Spelling.Interfaces
{
    /// <summary>
    /// كوررېكتورنىڭ ئەڭ ئاساسلىق بولغان توغرىلىقىغا ھۆكۈم قىلىۋە نامزات سۆز تېپىش ئىقتىدارى مۇشۇ ئېغىزغا ئورۇنلاشتۇرۇلغان.
    /// مەزكۇر ئېغىزنى ھەرقانداق كوررېكتور چوقۇم ئەمەلگە ئاشۇرۇشى كېرەك.
    /// </summary>
    public interface ISpellCheckable
    {
        /// <summary>
        /// پارامتېر ئارقىلىق بېرىلگەن سۆزنىڭ توغرا-خاتېرلىقىغا ھۆكۈم قىلىدۇ
        /// </summary>
        /// <param name="word">توغرا خاتالىقى تەكشۈرۈلمەكچى بولغا سۆز</param>
        /// <returns>نى قايتۇرىدۇ false نى، خاتا بولسا true ئەگەر تەكشۈرۈلگەن سۆزنىڭ ئىملايى توغرا بولسا  </returns>
        bool IsCorrect(string word);

        /// <summary>
        /// بېرىلگەن سۆزنىڭ نامزات سۆزلىرىنى قايتۇرۇپ بېرىدۇ (بېرلگەن سۆزنىڭ توغرا خاتالىقىنى تەكشۈرمەيدۇ)؛؛؛
        /// </summary>
        /// <param name="missWord">نامزات سۆزلىرىگە ئېرىشمەكچى بولغا سۆز</param>
        /// <param name="neededCount">ئېرىشىلگەن نامزات سۆزلەرنىڭ سانى مۇشۇقىممەتتىن ئېشىپ كەتمەيدۇ</param>
        /// <param name="opntions">قوشۇمچە پارامېتېرلار، ھەركىم ئۆزىگە خاس پارامېتېرلىرىنى مۇشۇ ئارقىلىق بەرسە بولىدۇ</param>
        /// <returns>نامزات سۆزلەرنى ئۆزئىچىگە ئالغان تىزىملىك</returns>
        IList<string> GetSuggestions(string missWord, int neededCount, params object[] opntions);

        /// <summary>
        /// پارامېتىر ئارقىلىق بېرىلگەن سۆزنىڭ بىۋاستە ئالماشتۇرۇشقا بولىدىغان كاندىدات سۆزىنىڭ بار يوقلىقىتى تەكشۈرىدۇ
        /// </summary>
        /// <param name="missWord"></param>
        /// <returns></returns>
        bool HasReplacePeer(string missWord);

        /// <summary>
        /// بېرىلگەن خاتا سۆزنىڭ بىۋاستە ئالماشتۇرۇشتا بولىدىغان كاندىدار سۆزنىنى قايتۇرۇپ بېرىدۇ
        /// </summary>
        /// <param name="missWord"></param>
        /// <returns></returns>
        string GetReplacePeer(string missWord);

    }
}
