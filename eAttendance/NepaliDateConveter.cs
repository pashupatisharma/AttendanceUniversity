using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace eAttendance
{
    public class NepaliDateConverter
    {
        // Fields
        private int day;
        private int month;
        private int year;
        public static string filepath = "/DateConversion/";
        public static ArrayList dateFormat;

        // Methods
        static NepaliDateConverter()
        {
            ArrayList list = new ArrayList { "yyyy-MM-DD" };
            dateFormat = list;
        }

        public NepaliDateConverter()
        {
            this.day = 0;
            this.month = 0;
            this.year = 0;
        }

        public NepaliDateConverter(DateTime engdate)
        {
            NepaliDateConverter converter = ConvertToNepali(engdate);
            this.Day = converter.Day;
            this.Month = converter.Month;
            this.Year = converter.Year;
        }

        public NepaliDateConverter(int yy, int mm, int dd)
        {
            this.year = yy;
            this.month = mm;
            this.day = dd;
        }

        public static DateTime ConvertToEnglish(NepaliDateConverter nepalidate)
        {
            DateTime time = new DateTime();
            DateTime[] month = GetMonth(nepalidate.Year, nepalidate.Month);
            int year = month[0].Year;
            int num3 = month[0].Month;
            int day = month[0].Day;
            TimeSpan span = month[1].Subtract(month[0]).Add(new TimeSpan(1, 0, 0, 0));
            if (nepalidate.day > span.Days)
            {
                throw new Exception("दिएको मिति मिलेन।");
            }
            time = new DateTime(month[0].Year, month[0].Month, month[0].Day);
            for (int i = 1; i < nepalidate.Day; i++)
            {
                time = time.AddDays(1.0);
            }
            return time;
        }

        public static NepaliDateConverter ConvertToNepali(DateTime engdate)
        {
            TimeSpan span = new TimeSpan();
            int yy = engdate.Year + 0x38;
            int mm = engdate.Month + 8;
            if (mm > 12)
            {
                mm -= 12;
                yy++;
            }
            DateTime[] month = GetMonth(yy, mm);
            span = month[1].Subtract(month[0]).Add(new TimeSpan(1, 0, 0, 0));
            int dd = 1;
            while (month[0].ToShortDateString() != engdate.ToShortDateString())
            {
                dd++;
                month[0] = month[0].AddDays(1.0);
            }
            if (dd > span.Days)
            {
                dd -= span.Days;
                mm++;
                if (mm > 12)
                {
                    mm -= 12;
                    yy++;
                }
            }
            return new NepaliDateConverter(yy, mm, dd);
        }

        public static string ConvertToNepali(DateTime engdate, string format)
        {
            TimeSpan span = new TimeSpan();
            int yy = engdate.Year + 0x38;
            int mm = engdate.Month + 8;
            if (mm > 12)
            {
                mm -= 12;
                yy++;
            }
            DateTime[] month = GetMonth(yy, mm);
            span = month[1].Subtract(month[0]).Add(new TimeSpan(1, 0, 0, 0));
            int dd = 1;
            while (month[0] != engdate)
            {
                dd++;
                month[0] = month[0].AddDays(1.0);
            }
            if (dd > span.Days)
            {
                dd -= span.Days;
                mm++;
                if (mm > 12)
                {
                    mm -= 12;
                    yy++;
                }
            }
            NepaliDateConverter converter = new NepaliDateConverter(yy, mm, dd);
            if (dateFormat.IndexOf(format) == 0)
            {
                string str2;
                string str3;
                string str = converter.year.ToString();
                if (converter.Month.ToString().Length == 1)
                {
                    str2 = "0" + converter.month;
                }
                else
                {
                    str2 = converter.month.ToString();
                }
                if (converter.day.ToString().Length == 1)
                {
                    str3 = "0" + converter.day;
                }
                else
                {
                    str3 = converter.day.ToString();
                }
                return (str + "-" + str2 + "-" + str3);
            }
            return converter.ToString();
        }

        public static string ConvertToNepaliDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "आइतवार";

                case DayOfWeek.Monday:
                    return "सोमवार";

                case DayOfWeek.Tuesday:
                    return "मगलवार";

                case DayOfWeek.Wednesday:
                    return "बुधवार";

                case DayOfWeek.Thursday:
                    return "बिहिवार";

                case DayOfWeek.Friday:
                    return "शुक्रवार";

                case DayOfWeek.Saturday:
                    return "शनिवार";
            }
            return "";
        }

        public static string ConvertToNepaliShortDayName(DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "आईत";

                case DayOfWeek.Monday:
                    return "सोम";

                case DayOfWeek.Tuesday:
                    return "मंगल";

                case DayOfWeek.Wednesday:
                    return "बुध";

                case DayOfWeek.Thursday:
                    return "बिही";

                case DayOfWeek.Friday:
                    return "शुक्र";

                case DayOfWeek.Saturday:
                    return "शनि";
            }
            return "";
        }

        public static NepaliDateConverter Format(string nepalDate)
        {
            string[] strArray = nepalDate.Split(new char[] { '-' });
            NepaliDateConverter converter = new NepaliDateConverter();
            if (strArray.Length == 3)
            {
                converter = new NepaliDateConverter(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]));
            }
            return converter;
        }

        public static DateTime[] GetMonth(int yy, int mm)
        {
            DateTime[] timeArray = new DateTime[2];
            int num = yy;
            string path1 = "~/App_Data/" + ((num - (num % 10))).ToString() + ".xml";

            String path = HttpContext.Current.Server.MapPath(path1);

            if (!File.Exists(path))
            {

                throw new Exception("Date must be between 2070(BS) To 2090(BS).");
            }
            XmlReader reader = XmlReader.Create(path);
            while (reader.Read())
            {
                if (((reader.NodeType == XmlNodeType.Element) && (reader.Name == "Year")) && (Convert.ToInt32(reader.GetAttribute("yearid")) == yy))
                {
                    while (reader.Read())
                    {
                        if (((reader.NodeType != XmlNodeType.Element) || (reader.Name != "Month")) || (Convert.ToInt32(reader.GetAttribute("id")) != mm))
                        {
                            continue;
                        }
                        while (reader.Read())
                        {
                            string[] strArray;
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "StartDate"))
                            {
                                reader.Read();
                                strArray = reader.Value.Split(new char[] { '/' });
                                timeArray[0] = new DateTime(int.Parse(strArray[2]), int.Parse(strArray[0]), int.Parse(strArray[1]));
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "EndDate"))
                            {
                                reader.Read();
                                strArray = reader.Value.Split(new char[] { '/' });
                                timeArray[1] = new DateTime(int.Parse(strArray[2]), int.Parse(strArray[0]), int.Parse(strArray[1]));
                                break;
                            }
                        }
                        reader.Close();
                        return timeArray;
                    }
                }
            }
            reader.Close();
            return null;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { this.Year, "-", this.Month, "-", this.Day });
        }

        public int Day
        {
            get
            {
                return this.day;
            }
            set
            {
                this.day = value;
            }
        }

        public int Month
        {
            get
            {
                return this.month;
            }
            set
            {
                this.month = value;
            }
        }

        public int Year
        {
            get
            {
                return this.year;
            }
            set
            {
                this.year = value;
            }
        }


       
    }










}