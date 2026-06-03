using System.Collections.Generic;
using System.Text;

namespace FMRookieScouter.Helper
{
    public static class StringHelper
    {
        private static Dictionary<char, string> _alphabetDic = new Dictionary<char, string>();

        static StringHelper()
        {
            // A
            _alphabetDic.Add('À', "A");
            _alphabetDic.Add('Á', "A");
            _alphabetDic.Add('Ǎ', "A");
            _alphabetDic.Add('Ã', "A");
            _alphabetDic.Add('Ȧ', "A");
            _alphabetDic.Add('Â', "A");
            _alphabetDic.Add('Ä', "A");
            _alphabetDic.Add('Å', "A");
            _alphabetDic.Add('Ā', "A");
            _alphabetDic.Add('Ą', "A");
            _alphabetDic.Add('Ă', "A");

            // a
            _alphabetDic.Add('à', "a");
            _alphabetDic.Add('á', "a");
            _alphabetDic.Add('ǎ', "a");
            _alphabetDic.Add('ã', "a");
            _alphabetDic.Add('ȧ', "a");
            _alphabetDic.Add('â', "a");
            _alphabetDic.Add('ä', "a");
            _alphabetDic.Add('å', "a");
            _alphabetDic.Add('ā', "a");
            _alphabetDic.Add('ą', "a");
            _alphabetDic.Add('ă', "a");

            // C
            _alphabetDic.Add('Ć', "C");
            _alphabetDic.Add('Ĉ', "C");
            _alphabetDic.Add('Č', "C");
            _alphabetDic.Add('Ċ', "C");
            _alphabetDic.Add('Ç', "C");

            // c
            _alphabetDic.Add('ć', "c");
            _alphabetDic.Add('ĉ', "c");
            _alphabetDic.Add('č', "c");
            _alphabetDic.Add('ċ', "c");
            _alphabetDic.Add('ç', "c");

            // D
            _alphabetDic.Add('Ď', "D");
            _alphabetDic.Add('Ḑ', "D");

            // d
            _alphabetDic.Add('ď', "d");
            _alphabetDic.Add('ḑ', "d");

            // E
            _alphabetDic.Add('É', "E");
            _alphabetDic.Add('Ě', "E");
            _alphabetDic.Add('Ē', "E");
            _alphabetDic.Add('È', "E");
            _alphabetDic.Add('Ę', "E");
            _alphabetDic.Add('Ẽ', "E");
            _alphabetDic.Add('Ė', "E");
            _alphabetDic.Add('Ê', "E");
            _alphabetDic.Add('Ë', "E");

            // e
            _alphabetDic.Add('é', "e");
            _alphabetDic.Add('ě', "e");
            _alphabetDic.Add('ē', "e");
            _alphabetDic.Add('è', "e");
            _alphabetDic.Add('ę', "e");
            _alphabetDic.Add('ẽ', "e");
            _alphabetDic.Add('ė', "e");
            _alphabetDic.Add('ê', "e");
            _alphabetDic.Add('ë', "e");

            // G
            _alphabetDic.Add('Ĝ', "G");
            _alphabetDic.Add('Ǵ', "G");
            _alphabetDic.Add('Ǧ', "G");
            _alphabetDic.Add('Ģ', "G");
            _alphabetDic.Add('Ğ', "G");

            // g
            _alphabetDic.Add('ĝ', "g");
            _alphabetDic.Add('ǵ', "g");
            _alphabetDic.Add('ǧ', "g");
            _alphabetDic.Add('ģ', "g");
            _alphabetDic.Add('ğ', "g");

            // H
            _alphabetDic.Add('Ȟ', "H");
            _alphabetDic.Add('Ĥ', "H");

            // h
            _alphabetDic.Add('ȟ', "h");
            _alphabetDic.Add('ĥ', "h");

            // I
            _alphabetDic.Add('Ī', "I");
            _alphabetDic.Add('Į', "I");
            _alphabetDic.Add('Í', "I");
            _alphabetDic.Add('Ǐ', "I");
            _alphabetDic.Add('Ï', "I");
            _alphabetDic.Add('Ĩ', "I");
            _alphabetDic.Add('İ', "I");
            _alphabetDic.Add('Î', "I");
            _alphabetDic.Add('Ɨ', "I");
            _alphabetDic.Add('I', "I");

            // i
            _alphabetDic.Add('ī', "i");
            _alphabetDic.Add('į', "i");
            _alphabetDic.Add('í', "i");
            _alphabetDic.Add('ǐ', "i");
            _alphabetDic.Add('ï', "i");
            _alphabetDic.Add('ĩ', "i");
            _alphabetDic.Add('i', "i");
            _alphabetDic.Add('î', "i");
            _alphabetDic.Add('ɨ', "i");
            _alphabetDic.Add('ı', "i");

            // J
            _alphabetDic.Add('Ĵ', "J");

            // j
            _alphabetDic.Add('ĵ', "j");

            // K
            _alphabetDic.Add('Ǩ', "K");
            _alphabetDic.Add('Ḱ', "K");
            _alphabetDic.Add('Ķ', "K");

            // k
            _alphabetDic.Add('ǩ', "k");
            _alphabetDic.Add('ḱ', "k");
            _alphabetDic.Add('ķ', "k");

            // L
            _alphabetDic.Add('Ĺ', "L");
            _alphabetDic.Add('Ļ', "L");
            _alphabetDic.Add('Ł', "L");

            // l
            _alphabetDic.Add('ĺ', "l");
            _alphabetDic.Add('ļ', "l");
            _alphabetDic.Add('ł', "l");

            // M
            _alphabetDic.Add('Ḿ', "M");

            // m
            _alphabetDic.Add('ḿ', "m");

            // N
            _alphabetDic.Add('Ň', "N");
            _alphabetDic.Add('Ń', "N");
            _alphabetDic.Add('Ñ', "N");
            _alphabetDic.Add('Ņ', "N");

            // n
            _alphabetDic.Add('ň', "n");
            _alphabetDic.Add('ń', "n");
            _alphabetDic.Add('ñ', "n");
            _alphabetDic.Add('ņ', "n");

            // O
            _alphabetDic.Add('Õ', "O");
            _alphabetDic.Add('Ǒ', "O");
            _alphabetDic.Add('Ö', "O");
            _alphabetDic.Add('Ő', "O");
            _alphabetDic.Add('Ó', "O");
            _alphabetDic.Add('Ò', "O");
            _alphabetDic.Add('Ø', "O");
            _alphabetDic.Add('Ō', "O");
            _alphabetDic.Add('Ǫ', "O");

            // o
            _alphabetDic.Add('õ', "o");
            _alphabetDic.Add('ǒ', "o");
            _alphabetDic.Add('ö', "o");
            _alphabetDic.Add('ő', "o");
            _alphabetDic.Add('ó', "o");
            _alphabetDic.Add('ò', "o");
            _alphabetDic.Add('ø', "o");
            _alphabetDic.Add('ō', "o");
            _alphabetDic.Add('ǫ', "o");

            // R
            _alphabetDic.Add('Ř', "R");
            _alphabetDic.Add('Ŕ', "R");
            _alphabetDic.Add('Ȓ', "R");

            // r
            _alphabetDic.Add('ř', "r");
            _alphabetDic.Add('ŕ', "r");
            _alphabetDic.Add('ȓ', "r");

            // S
            _alphabetDic.Add('Ş', "S");
            _alphabetDic.Add('Ś', "S");
            _alphabetDic.Add('Š', "S");
            _alphabetDic.Add('Ș', "S");
            _alphabetDic.Add('Ŝ', "S");
            _alphabetDic.Add('Ṡ', "S");

            // s
            _alphabetDic.Add('ş', "s");
            _alphabetDic.Add('ś', "s");
            _alphabetDic.Add('š', "s");
            _alphabetDic.Add('ș', "s");
            _alphabetDic.Add('ŝ', "s");
            _alphabetDic.Add('ṡ', "s");

            // T
            _alphabetDic.Add('Ť', "T");
            _alphabetDic.Add('Ț', "T");
            _alphabetDic.Add('Ŧ', "T");

            // t
            _alphabetDic.Add('ť', "t");
            _alphabetDic.Add('ț', "t");
            _alphabetDic.Add('ŧ', "t");

            // U
            _alphabetDic.Add('Ŭ', "U");
            _alphabetDic.Add('Ü', "U");
            _alphabetDic.Add('Ū', "U");
            _alphabetDic.Add('Ǔ', "U");
            _alphabetDic.Add('Ų', "U");
            _alphabetDic.Add('Ů', "U");
            _alphabetDic.Add('Ű', "U");
            _alphabetDic.Add('Ũ', "U");
            _alphabetDic.Add('Ú', "U");
            _alphabetDic.Add('Ù', "U");

            // u
            _alphabetDic.Add('ŭ', "u");
            _alphabetDic.Add('ü', "u");
            _alphabetDic.Add('ū', "u");
            _alphabetDic.Add('ǔ', "u");
            _alphabetDic.Add('ų', "u");
            _alphabetDic.Add('ů', "u");
            _alphabetDic.Add('ű', "u");
            _alphabetDic.Add('ũ', "u");
            _alphabetDic.Add('ú', "u");
            _alphabetDic.Add('ù', "u");

            // V
            _alphabetDic.Add('Ṽ', "V");

            // v
            _alphabetDic.Add('ṽ', "v");

            // W
            _alphabetDic.Add('Ẃ', "W");

            // w
            _alphabetDic.Add('ẃ', "w");

            // X
            _alphabetDic.Add('Ẋ', "X");

            // x
            _alphabetDic.Add('ẋ', "x");

            // Y
            _alphabetDic.Add('Ȳ', "Y");
            _alphabetDic.Add('Ỹ', "Y");
            _alphabetDic.Add('Ÿ', "Y");
            _alphabetDic.Add('Ý', "Y");

            // y
            _alphabetDic.Add('ȳ', "y");
            _alphabetDic.Add('ỹ', "y");
            _alphabetDic.Add('ÿ', "y");
            _alphabetDic.Add('ý', "y");

            // Z
            _alphabetDic.Add('Ž', "Z");
            _alphabetDic.Add('Ź', "Z");
            _alphabetDic.Add('Ż', "Z");

            // z
            _alphabetDic.Add('ž', "z");
            _alphabetDic.Add('ź', "z");
            _alphabetDic.Add('ż', "z");

            // ae
            _alphabetDic.Add('æ', "ae");
        }

        public static string TrimEnglish(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var builder = new StringBuilder();

            foreach (var c in str)
                builder.Append(_alphabetDic.TryGetValue(c, out string value) ? value : c.ToString());

            return builder.ToString();
        }
    }
}
