using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.Service
{
    public static class MenuService
    {

      
            public static string Info = "जानकारी";
            public static string Action = "Action";
            public static string Inactive = "निष्क्रिय";
            public static string Edit = "सम्पादन गर्नुहोस्";
            public static string Transfer = "स्थानान्तरण";
           
            public static string InternalTransfer = "आन्तरिक स्थानान्तरण";
            public static string ExternalTransfer = "बाह्य स्थानान्तरण";
            public static string FromOffice = "कार्यालय देखि";
            public static string ToOffice = "कार्यालयमा";
            public static string DeviceType = "उपकरण किसिम";
            public static string More = "अधिक";
           
            public static string InTime = "आउने समय";
            public static string OutTime = "जाने समय";
         
            public static string CompanyInformation = "कम्पनीको जानकारी";
            public static string CurrentAttendance = "वर्तमान उपस्थिति";
            public static string NumberOfAttendance = "उपस्थिति को संख्या";

            public static string EmployeeLeaveTransfer = "कर्मचारीको बिदा स्थानान्तरण गर्नुहोस्";
          
            public static string CompanyName = "गृह मन्त्रालय";
       
            public static string CompanyAddress = "काठमाडौँ";

            public static string AdminDashboard = "व्यवस्थापक ड्यासबोर्ड";
          
            public static string PeriodicAttendanceReport = "आवधिक हाजरि रिपोर्ट";
            public static string Time = "समय";
            public static string TimeRemarks = "समयको कैफियत";
            public static string WorkedHour = "काम गरेको समय";
            public static string NoRecordFound = "रेकर्ड भेटिएन";
            public static string SNo = "क्र.सं";
            public static string AnnualReport = "वार्षिक रिपोर्ट";
            public static string VisitPosting = "काज पोस्टिंग";
            public static string MonthlyInOutReport = "ईन आउट रिपोर्ट";
            public static string ChangePassword = "पासवर्ड परिवर्तन";
            public static string Password = "पासवर्ड";
            public static string Remainings = "बाँकी ";
            public static string EmployeeNameEng = "कर्मचारीको नाम (अंग्रेजी)";
            public static string EmployeeNameNep = "कर्मचारीको नाम (नेपाली)";
            public static string Enrollment = "नामांकन";
            public static string ConfirmPassword = "पासवर्ड सुनिश्चित गर्नुहोस";
            public static string Dashboard = "ड्यासबोर्ड";
            public static string GeneralSetup = "सामान्य सेटअप";
            public static string Search = "खोज";
            public static string FiscalYear = " वर्ष";
            public static string Employee = "कर्मचारी";
     
            public static string ForgetEntry = "हाजिरी छुटेको";
    
            public static string Calender = "भित्तेपात्रो";
            public static string LeaveType = "बिदा किसिम";
            public static string Leave = "बिदा";
            public static string LeaveApplication = "बिदा आवेदन ";
            public static string OpeningBalance = "पछिल्लो वर्ष को बिदा";
            public static string NoOfLeave = "यो वर्ष को बिदा";
            public static string EmployeeNo = "कर्मचारी को कोड";
            public static string EmployeeName = "\tकर्मचारी को नाम";
            public static string Gender = "लिंग";
         
            public static string EmailId = "इमेल आइढी";
         
            public static string UserId = "युजरको नाम";
            public static string Pzone = "स्थायी अञ्चल";
          
            public static string Tzone = "अस्थायी अञ्चल";
           
            public static string Nationality = "राष्ट्रियता";
            public static string Status = "स्थिति";
         
            public static string Remarks = "टिप्पणी";
            public static string HolidayType = "सार्वजनिक बिदाको प्रकार";
            public static string FromDate = "मिति देखि";
            public static string ToDate = "मिति सम्म";
            public static string ApplicationDate = "आवेदन मिति";
            public static string Date = "मिति";
          
            public static string Message = "सन्देश";
          
            public static string BranchName = "शाखा नाम";
            public static string MainBranch = "मुख्य शाखा";
            public static string Branch = "शाखा";
            public static string Alias = "छोटकरी नाम";
            public static string Description = "विवरण";
            
            public static string Country = "देश";
       
            public static string Department = "विभाग";

            public static string LevelId = "श्रेणी";
            public static string DesignationName = "पद को नाम";
            public static string Designation = "पद";
           

            public static string FiscalYearName = " वर्ष";
            public static string ApplicableFor = "लागू लागि";
           
            public static string Holiday = "सार्वजनिक बिदा";
           
            public static string LevelName = "तहको नाम";
            public static string OfficeId = "कार्यालय";
          
            public static string VisitApplication = "काज आवेदन";
            public static string VisitType = "काजको प्रकार";
         
            public static string LicenseNo = "अनुमति नम्बर";
 
            public static string Port = "पोर्ट";
            public static string Code = "कोड";
            public static string Address = "ठेगाना";
            public static string ServiceName = "सेवा को नाम";
            public static string Service = "सेवा";
            public static string ShiftName = "सिफ्ट नाम";
            public static string ShiftTime = "सिफ्ट समय";
            public static string Shift = "सिफ्ट";
     
            public static string ShiftStartTime = "सुरुआतसमय सिफ्ट";
            public static string ShiftEndTime = "अन्त समय सिफ्ट";
     
            public static string ShiftLateInAfter = "सिफ्ट लेट इन आफ्टर";
            public static string ShiftEarlyOutBefore = "\tसिफ्ट अरली आउट बेफोरे";
           
            public static string OverTimeAfter = "ओवेर टाइम आफ्टर";
            
            public static string VisitReport = "काजको सारांश";
      
            public static string Subject = "विषय";
          
            public static string AssignAllEmployeeLeave = "सबै कर्मचारीलाई बिदा";
            public static string LeavePosting = "बिदा पोस्टिंग";
            public static string Total = " जम्मा";
            public static string TotalAssignedLeave = " जम्मा लिन पाउने बिदा";
            public static string TotalLeaveTaken = " जम्मा लिएका बिदा";
            public static string TotalRemainingLeave = " जम्मा बाँकी बिदा";
            public static string TotalSaturdays = " जम्मा शनिवार";
            public static string TotalHolidays = " जम्मा सार्वजनिक बिदा";
   
            public static string TotalSelectedDays = " जम्मा छानिएको दिन";
         
            public static string TotalDays = " जम्मा दिन";
            public static string TotalWorkingDays = " जम्मा काम गर्ने दिन";
            public static string OffDays = " सार्वजनिक छुट्टी ";
            public static string Present = " उपस्थिती";
            public static string Absent = "  अनुपस्थित";
            public static string TakenLeave = " लिएको";
            public static string Taken = " लिएको बिदा";
            public static string TotalHours = " कुल घण्टा ";
            public static string Remaining = " बाकी बिदा";
            public static string DailyAttendanceReport = "दैनिक हाजिरी";
            public static string Filter = "फिल्टर";
            public static string Report = "रिपोर्ट";
            public static string Office = "कार्यालय";
            public static string Branch_MainBranch = "शाखा | महाशाखा";
            public static string Level = "तह";
            public static string MonthlyAttendanceReport = "उपस्थित अनुपस्थित";
            public static string MonthlyAttendanceReportSumarry = "उपस्थित रिपोर्ट संक्षेप";
            public static string Month = "महिना";
            public static string Year = "साल";
            public static string Attendance = "हाजिरी";
            public static string Profile = "प्रोफाइल";
            public static string SignOut = "साईन आउट";
            public static string Visit = "निरिक्षण";
            public static string ProfileInformation = "ब्यक्तिगत परिचय";
            public static string OfficeInformation = "कार्यालयको परिचय";
            public static string PermanentAddress = "स्थायी ठेगाना";
            public static string TemporaryAddress = "अस्थायी ठेगाना";
            public static string ShiftInformation = "सिफ्ट";
            public static string AccountInformation = "अकाउन्ट  विवरण";
            public static string Active = "सक्रिय";
   
            public static string Deactive = "निष्क्रिय";
     
            public static string Account = "अकाउन्ट";
      
            public static string Finish = "समाप्त";
            public static string Summary = "सारांश";
          
            public static string Next = "अर्को ";
            public static string Save = "सुरक्षित गर्नुहोस";
            public static string Cancel = "रद्द गर्नु  ";
            public static string User = "प्रयोगकर्ता";
            public static string UserRoles = "प्रयोगकर्ता भूमिका";
            public static string LateInEarlyOutAttendanceReport = "ढिलो आउने वा छिटो जाने";
            public static string DailyPresentAttendanceReport = "उपस्थित";
            public static string DailyAbsentAttendanceReport = "अनुपस्थित";
            public static string DeviceSyncRequest = "उपकरण सिङ्क अनुरोध";
            public static string Applyed = "दरखास्त";
            public static string Approved = "शुयिकृत";
            public static string ResetPassword = "पासवर्ड रिसेट गर्नु";
            public static string Pending = "फैसला गर्न बाँकी रहेको ";
            public static string Reject = "अस्वीकार";



        ///For Message
            ///public static string Save = "Record Created Successfully !";
            public static string SaveFailed = "Record Creation Failed !";
            public static string EditMessage = "Record Updated Successfully !";
            public static string EditFailed = "Record Updation Failed !";
            public static string Delete = "Record Deleted Successfully !";
            public static string Activation = "Record Activated Successfully !";
            public static string ActivationDeactionFailed = "Record Activation/Deactivation Failed !";
            public static string DeactiveMessage = "Record Deactivated Successfully !";
            public static string DeleteFailed = "Record Deletion Failed !";
            public static string StartDateGreterthenEndDate = "You Must be Select Start Date less then End Date!";
            public static string HasValues = "Check Start And End Date and Insert Unique Date Which is Not Insert In Database!";
            public static string AlredyExitThisItems = "This is Already Exit Please Try Agan";
            public static string StartTimeGreterThenEndTime = "You Must be Select Start Time less then End Time!";
            public static string BalanceLeaveisGreterThenDays = "You can not Access Grater Then Balance Leave Please Try Agan Other Leave";
            public static string PleaseSelectAnItem = "कृपया विकल्प चयन गर्नुहोस्";
            public static string PleaseFillRequiredInfo = "कृपया आवश्यक जानकारी भर्नुहोस् ";
        }

 

 

    }
