using eAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eAttendance.Controllers
{
    public class CalendarManager
    {
        // Methods
        public WeekForMonth getCalender(int month, int year)
        {
            int num2;
            ApplicationDbContext db = new ApplicationDbContext();
            WeekForMonth month2 = new WeekForMonth
            {
                Week1 = new List<Day>(),
                Week2 = new List<Day>(),
                Week3 = new List<Day>(),
                Week4 = new List<Day>(),
                Week5 = new List<Day>(),
                Week6 = new List<Day>()
            };
            DateTime[] timeArray = NepaliDateConverter.GetMonth(year, month);
            List<NepaliDateConverter> dates = new List<NepaliDateConverter>();
            dates = GetDates(year, month);
            foreach (NepaliDateConverter converter in dates)
            {
                DateTime date = NepaliDateConverter.ConvertToEnglish(converter);
                List<HolidayCalender> list2 = db.HolidayCalender.Where(x => x.FromDate <= date.Date).Where(x => x.ToDate >= date.Date).ToList();
                switch (GetWeekOfMonth(converter))
                {
                    case 1:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week1.Add(item);
                            break;
                        }
                    case 2:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week2.Add(item);
                            break;
                        }
                    case 3:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week3.Add(item);
                            break;
                        }
                    case 4:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week4.Add(item);
                            break;
                        }
                    case 5:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week5.Add(item);
                            break;
                        }
                    case 6:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week6.Add(item);
                            break;
                        }
                }
            }
            while (month2.Week1.Count < 7)
            {
                Day item = null;
                month2.Week1.Insert(0, item);
            }
            if (month == 12)
            {
                num2 = 1;
                num2 = year + 1;
                month2.nextMonth = num2.ToString() + "/" + num2.ToString();
                num2 = month - 1;
                month2.prevMonth = num2.ToString() + "/" + year.ToString();
                return month2;
            }
            if (month == 1)
            {
                num2 = month + 1;
                month2.nextMonth = num2.ToString() + "/" + year.ToString();
                num2 = 12;
                num2 = year - 1;
                month2.prevMonth = num2.ToString() + "/" + num2.ToString();
                return month2;
            }
            month2.nextMonth = ((month + 1)).ToString() + "/" + year.ToString();
            month2.prevMonth = ((month - 1)).ToString() + "/" + year.ToString();
            return month2;
        }

        public WeekForMonth getCalender(int month, int year, int officeId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            object[] objArray;
            int num2;
            WeekForMonth month2 = new WeekForMonth
            {
                Week1 = new List<Day>(),
                Week2 = new List<Day>(),
                Week3 = new List<Day>(),
                Week4 = new List<Day>(),
                Week5 = new List<Day>(),
                Week6 = new List<Day>()
            };
            DateTime[] timeArray = NepaliDateConverter.GetMonth(year, month);
            List<NepaliDateConverter> dates = new List<NepaliDateConverter>();
            dates = GetDates(year, month);
            foreach (NepaliDateConverter converter in dates)
            {
                DateTime date = NepaliDateConverter.ConvertToEnglish(converter);
                List<HolidayCalender> list2 = db.HolidayCalender.ToList().Where<HolidayCalender>(delegate(HolidayCalender v)
                {
                    int? nullable2 = null;
                    int num = 0;
                    if ((v.FromDate.Date <= date.Date) && (v.ToDate.Date >= date.Date))
                    {
                        nullable2 = v.OfficeId;
                        num = officeId;
                    }
                    return (((nullable2.GetValueOrDefault() == num) && nullable2.HasValue) && (v.Status == 1));
                }).ToList<HolidayCalender>();




                switch (GetWeekOfMonth(converter))
                {
                    case 1:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week1.Add(item);
                            break;
                        }
                    case 2:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week2.Add(item);
                            break;
                        }
                    case 3:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week3.Add(item);
                            break;
                        }
                    case 4:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week4.Add(item);
                            break;
                        }
                    case 5:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week5.Add(item);
                            break;
                        }
                    case 6:
                        {
                            Day item = new Day
                            {
                                Date = converter,
                                _Date = converter.ToString(),
                                dateStr = converter.ToString(),
                                dtDay = converter.Day
                            };
                            item.daycolumn = new int?(this.GetDateInfo(item.Date));
                            item.holidayname = list2;
                            month2.Week6.Add(item);
                            break;
                        }
                }
            }
            while (month2.Week1.Count < 7)
            {
                Day item = null;
                month2.Week1.Insert(0, item);
            }
            if (month == 12)
            {
                objArray = new object[5];
                num2 = 1;
                objArray[0] = num2.ToString();
                objArray[1] = "/";
                num2 = year + 1;
                objArray[2] = num2.ToString();
                objArray[3] = "/";
                objArray[4] = officeId;
                month2.nextMonth = string.Concat(objArray);
                objArray = new object[5];
                num2 = month - 1;
                objArray[0] = num2.ToString();
                objArray[1] = "/";
                objArray[2] = year.ToString();
                objArray[3] = "/";
                objArray[4] = officeId;
                month2.prevMonth = string.Concat(objArray);
                return month2;
            }
            if (month == 1)
            {
                objArray = new object[5];
                num2 = month + 1;
                objArray[0] = num2.ToString();
                objArray[1] = "/";
                objArray[2] = year.ToString();
                objArray[3] = "/";
                objArray[4] = officeId;
                month2.nextMonth = string.Concat(objArray);
                objArray = new object[5];
                num2 = 12;
                objArray[0] = num2.ToString();
                objArray[1] = "/";
                objArray[2] = (year - 1).ToString();
                objArray[3] = "/";
                objArray[4] = officeId;
                month2.prevMonth = string.Concat(objArray);
                return month2;
            }
            objArray = new object[] { (month + 1).ToString(), "/", year.ToString(), "/", officeId };
            month2.nextMonth = string.Concat(objArray);
            objArray = new object[5];
            objArray[0] = (month - 1).ToString();
            objArray[1] = "/";
            objArray[2] = year.ToString();
            objArray[3] = "/";
            objArray[4] = officeId;
            month2.prevMonth = string.Concat(objArray);
            return month2;
        }

        public int GetDateInfo(NepaliDateConverter now)
        {
            int num = 0;
            string str = Convert.ToString(NepaliDateConverter.ConvertToEnglish(now).DayOfWeek);
            if (str.ToLower() == "sunday")
            {
                return 0;
            }
            if (str.ToLower() == "monday")
            {
                return 1;
            }
            if (str.ToLower() == "tuesday")
            {
                return 2;
            }
            if (str.ToLower() == "wednesday")
            {
                return 3;
            }
            if (str.ToLower() == "thursday")
            {
                return 4;
            }
            if (str.ToLower() == "friday")
            {
                return 5;
            }
            if (str.ToLower() == "saturday")
            {
                num = 6;
            }
            return num;
        }

        public static List<NepaliDateConverter> GetDates(int year, int month)
        {
            DateTime[] timeArray = NepaliDateConverter.GetMonth(year, month);
            TimeSpan span = (TimeSpan)(timeArray[1] - timeArray[0]);
            int count = span.Days + 1;
            return (from day in Enumerable.Range(1, count) select new NepaliDateConverter(year, month, day)).ToList<NepaliDateConverter>();
        }

        public static int GetWeekOfMonth(NepaliDateConverter nDate)
        {
            NepaliDateConverter nepalidate = new NepaliDateConverter(nDate.Year, nDate.Month, 1);
            while (NepaliDateConverter.ConvertToEnglish(nDate).Date.AddDays(1.0).DayOfWeek != DayOfWeek.Sunday)
            {
                nDate = NepaliDateConverter.ConvertToNepali(NepaliDateConverter.ConvertToEnglish(nDate).AddDays(1.0));
            }
            return (((int)Math.Truncate((double)(NepaliDateConverter.ConvertToEnglish(nDate).Subtract(NepaliDateConverter.ConvertToEnglish(nepalidate)).TotalDays / 7.0))) + 1);
        }
    }










}
