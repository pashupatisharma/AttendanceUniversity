using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class SetupShiftTime : BaseModel
    {
        [Key()]
        public int ShiftTimeId { get; set; }

        public int ShiftId { get; set; }
        public virtual Shift Shift{get;set;}
        [Display(Name = "कार्यालय")]
        public int? OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }
        public int WeekDay { get; set; }

        public string WeekDayNameEn { get; set; }

        public string WeekDayNameNp { get; set; }

        public bool IsWeekOff { get; set; }

        [Required, StringLength(100), Display(Name = "सिफ्टको नाम  ")]
        public string ShiftName { get; set; }

        [Display(Name = "उपनाम ")]
        public string Alias { get; set; }

        [Display(Name = "विवरण  ")]
        public string Description { get; set; }

        [Display(Name = "प्रारम्भ सिफ्ट समय ")]
        public string ShiftStartTime { get; set; }

        [Display(Name = "अन्त्य  सिफ्ट समय  ")]
        public string ShiftEndTime { get; set; }

        [Display(Name = "सिफ्ट घण्टा")]
        public string ShiftHours { get; set; }

        [Display(Name = "ढिलो सिफ्ट पछी")]
        public string ShiftLateInAfter { get; set; }

        [Display(Name = "चाडो सिफ्ट ठीक अगाडि")]
        public string ShiftEarlyOutBefore { get; set; }

        [Display(Name = "बिश्राम प्रारम्भ समय")]
        public string BreakStartTime { get; set; }

        [Display(Name = "बिश्राम अन्त्य समय")]
        public string BreakEndTime { get; set; }

        [Display(Name = "बिश्राम अवधि ")]
        public string BreakDuration { get; set; }

        [Display(Name = "ब्रेक लेट इन आफ्टर  ")]
        public string BreakLateInAfter { get; set; }

        [Display(Name = "ब्रेक एअर्ल्री आउट बेफोरे  ")]
        public string BreakEarlyOutBefore { get; set; }

        [Display(Name = "बढी समय ठीक अगाडि")]
        public string OverTimeBefore { get; set; }

        [Display(Name = "बढी समय पछी ")]
        public string OverTimeAfter { get; set; }

        [Display(Name = "हेराउने क्रम ")]
        public int DisplayOrder { get; set; }

        [Display(Name = "पूर्वनिर्धारित छ ")]
        public bool IsDefault { get; set; }

        [Display(Name = "स्थिति ")]
        public int Status { get; set; }

        public int flage { get; set; }

        [NotMapped()]
        public List<SetupShiftTime> SetupShiftTimeList { get; set; }
    }
}