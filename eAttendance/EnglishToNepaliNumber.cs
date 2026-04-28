using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance
{
    public static class EnglishToNepaliNumber
    {
        public static string ConvertToNepaliNumber(int number)
        {
            if (number < 0)
            {
                return number.ToString();
            }
            IEnumerable<int> enumerable = from digit in number.ToString() select int.Parse(digit.ToString());
            string str = "";
            using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    switch (enumerator.Current)
                    {
                        case 0:
                            str = str + "०";
                            break;

                        case 1:
                            str = str + "१";
                            break;

                        case 2:
                            str = str + "२";
                            break;

                        case 3:
                            str = str + "३";
                            break;

                        case 4:
                            str = str + "४";
                            break;

                        case 5:
                            str = str + "५";
                            break;

                        case 6:
                            str = str + "६";
                            break;

                        case 7:
                            str = str + "७";
                            break;

                        case 8:
                            str = str + "८";
                            break;

                        case 9:
                            str = str + "९";
                            break;
                    }
                }
            }
            return str;
        }



        public static string ConvertToNepaliString(string str)
        {
            if (str == null)
            {
                return "";
            }
            string str2 = "";
            foreach (char ch in str)
            {
                if (char.IsDigit(ch))
                {
                    str2 = str2 + ConvertToNepaliNumber(int.Parse(ch.ToString()));
                }
                else
                {
                    str2 = str2 + ch.ToString();
                }
            }
            return str2;
        }




    }
}