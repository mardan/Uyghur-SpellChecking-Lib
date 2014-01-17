using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Uyghurdev.Spelling.Interfaces
{
    /// <summary>
    /// كوررىكتورنىڭ ئۆزىنى كۆڭۈلدىكى قىمەت ھالەتكە كەلتۈرەلەيدىغانلىقىنى بىلدۈرىدۇ.
    /// ئىشلىتىش ئۈچۈن چوقۇم بىر قىسىم تەييارلىق خىزىمىتىنى قىلىدىغان كوررېكتورلار مۇشۇ ئېغىنى ئەمەلگە ئاشۇرغان بولۇشى كېرەك
    /// </summary>
    public interface IInitialable
    {
        /// <summary>
        /// كوررىكتورنىڭ ئۆزىنى كۆڭۈلدىكى قىمەت ھالەتكە كەلتۈرىدۇ
        /// مەسىلەن: سۆز ئامبىرىنى ئوقۇپ كىرىش، سانداننى توغۇرلاش دىگەندەك ئىشلارنى مۇشۇ مىزات ئارقىلىق قىلسا بولىدۇ
        /// </summary>
        /// <param name="paramDictionary"></param>
        bool Intitial(IDictionary<string, object> paramDictionary);

    }
}
