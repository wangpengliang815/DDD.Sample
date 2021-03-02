using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DomainCore.Safety
{
    public class SqlSecurity
    {
        /// <summary>
        /// 对字符串进行SQL安全转化
        /// </summary>
        /// <param name="text">要安全转化的文本</param>
        /// <returns></returns>
        public static string SafeSqlString(string text)
        {
            if (text == null)
                return null;
            string restr = System.Text.RegularExpressions.Regex.Replace(text, "exec", "ｅｘｅｃ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "declare", "ｄｅｃｌａｒｅ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "update", "ｕｐｄａｔｅ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "delete", "ｄｅｌｅｔｅ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "select", "ｓｅｌｅｃｔ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = restr.Replace("'", "''");
            return restr;
        }

        /// <summary>
        /// 过滤危险字符防止
        /// </summary>
        /// <param name="strInput">输入的字符串</param>
        /// <returns></returns>
        public static string FilterInput(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput))
                return string.Empty;
            string sourceStr = strInput;// 保留原有字符串
            // 过滤的关键字
            string[] Lawlesses = { " or ", " and ", " not ", "exec ", "xp_", "select ", "insert ", "delete ", "update ", "drop ", "alter ", "create ", "sp_" };

            //构造正则表达式
            string str_Regex = "(";
            for (int i = 0; i < Lawlesses.Length - 1; i++)
            {
                str_Regex = str_Regex + Lawlesses[i] + "|";
            }
            str_Regex += Lawlesses[Lawlesses.Length - 1] + ")+";

            // 是否匹配
            if (Regex.Matches(strInput, str_Regex).Count > 0)
            {
                strInput = Regex.Replace(strInput, str_Regex, "", RegexOptions.IgnoreCase);
            }

            strInput = strInput.Replace("'", "''");

            return strInput;
        }

        /// <summary>
        /// 过滤脚本
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterScript(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            string regexstr = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";//@"<script.*</script>";
            content = Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<script([^>])*>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(content, "</script>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤框架
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterIFrame(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            string regexstr = @"(?i)<iframe([^>])*>(\w|\W)*</iframe([^>])*>";//@"<script.*</script>";
            content = Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<iframe([^>])*>", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Replace(content, "</iframe>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 对字符串进行MD加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(value);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }
    }
}
