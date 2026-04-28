using eAttendance.Models;
using eAttendance.ReportModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace eAttendance
{
    public class Utility
    {
        public static IEnumerable<System.Web.Mvc.SelectListItem> GetRolesList()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            return new SelectList(context.Roles.ToList(), "Name", "Name");
        }

        public static CompanyInfo GetCompanyInfo()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            return context.CompanyInfo.FirstOrDefault();

        }

        public static string GetOfficeNameByUserId(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                return "";
            }
            ApplicationDbContext context = new ApplicationDbContext();
            var userId = context.Users.Where(x => x.UserName == Username).FirstOrDefault().Id;
            if (userId != null)
            {
                var EmployeeId = context.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault();
                if (EmployeeId != null)
                {
                    var officeId = context.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId.EmployeeId).FirstOrDefault().OfficeId;

                    return context.OfficeSetUp.Where(x => x.OfficeId == officeId).FirstOrDefault().OfficeName;
                }

            }

            return "";
        }

        public static int GetEmployeeIdByUserId(string userId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            if (string.IsNullOrEmpty(userId))
            {
                return 0;
            }

            if (userId != null)
            {
                var EmployeeId = entities.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault();
                if (EmployeeId != null)
                {
                    var eid = EmployeeId.EmployeeId;

                    return eid;
                }

            }

            return 0;

        }






        public static IEnumerable<SelectListItem> GetZoneList()
        {

            return new SelectList(new[] { new {
        Id = "0",
        Value = "छान्नुहोश "
    }, new {
        Id = "2",
        Value = "मेची"
    }, new {
        Id = "3",
        Value = "कोशी"
    }, new {
        Id = "4",
        Value = "सगरमाथा"
    }, new {
        Id = "5",
        Value = "जनकपुर"
    }, new {
        Id = "6",
        Value = "बागमति"
    }, new {
        Id = "7",
        Value = "नारायणी"
    }, new {
        Id = "8",
        Value = "गण्डकी"
    }, new {
        Id = "9",
        Value = "लुम्विनी"
    }, new {
        Id = "10",
        Value = "धवलागिरी"
    }, new {
        Id = "11",
        Value = "राप्ती"
    }, new {
        Id = "12",
        Value = "कर्णाली"
    }, new {
        Id = "13",
        Value = "भेरी"
    }, new {
        Id = "14",
        Value = "सेती"
    }, new {
        Id = "15",
        Value = "महाकाली"
    } }, "id", "Value");

        }

        public static List<SelectListItem> NepaliMonthList()
        {

            return new SelectList(new[] { new {
        Id = "1",
        Value = "बैसाख"
    }, new {
        Id = "2",
        Value = "जेठ"
    }, new {
        Id = "3",
        Value = "असार"
    }, new {
        Id = "4",
        Value = "साउन"
    }, new {
        Id = "5",
        Value = "भदौ"
    }, new {
        Id = "6",
        Value = "असोज"
    }, new {
        Id = "7",
        Value = "कार्तिक"
    }, new {
        Id = "8",
        Value = "मंसिर"
    }, new {
        Id = "9",
        Value = "पौस"
    }, new {
        Id = "10",
        Value = "माघ"
    }, new {
        Id = "11",
        Value = "फाल्गुन"
    }, new {
        Id = "12",
        Value = "चैत"
    } }, "id", "Value").ToList<SelectListItem>();
        }

        public static IEnumerable<SelectListItem> GetGender()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Value = "1", Text = "पुरुष" });
            list.Add(new SelectListItem() { Value = "2", Text = "महिला" });
            return list;
        }


        public static IEnumerable<SelectListItem> GetDeviceType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "0", Text = "Time Watch" });
            list.Add(new SelectListItem() { Value = "1", Text = "पुरुष" });
            list.Add(new SelectListItem() { Value = "2", Text = "ZK Device" });
            return list;
        }


        public static IEnumerable<SelectListItem> GetAttendanceype()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "1", Text = "सबै" });
            list.Add(new SelectListItem() { Value = "2", Text = "उपस्थित" });
            list.Add(new SelectListItem() { Value = "3", Text = "अनुपस्थित" });

            return list;
        }



        public static IEnumerable<SelectListItem> GetStatusList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
          
            list.Add(new SelectListItem() { Value = "1", Text = "सक्रिय" });
            list.Add(new SelectListItem() { Value = "2", Text = "निस्क्रिय" });
            list.Add(new SelectListItem() { Value = "3", Text = "आन्तरिक स्थानान्तरण" });
            list.Add(new SelectListItem() { Value = "4", Text = "बाहिय स्थानान्तरण" });


            return list;
        }



        public static IEnumerable<SelectListItem> GetServiceIdName()
        {

            ApplicationDbContext entities = new ApplicationDbContext();
            var list = entities.ServiceSetUp.ToList();


            ServiceSetUp item = new ServiceSetUp
            {
                ServiceId = 0,
                ServiceName = "छान्नुहोस्"
            };
            list.Insert(0, item);

            return new SelectList(list, "ServiceId", "ServiceName");

        }
        public static IEnumerable<SelectListItem> GetLevelIdName()
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            var list = entities.LevelSetUp.ToList();


            LevelSetUp item = new LevelSetUp
            {
                LevelId = 0,
                LevelName = "छान्नुहोस्"
            };
            list.Insert(0, item);

            return new SelectList(list, "LevelId", "LevelName");
        }
        public static IEnumerable<SelectListItem> GetDepartmentIdName()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.DepartmentSetup.ToList(), "DepartmentId", "DepartmentName");
            }
        }

        public static IEnumerable<SelectListItem> GetDesignationIdNameList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return new SelectList(context.DesignationSetUp.ToList(), "DesignationId", "DesignationName");
            }
        }

        public static IEnumerable<SelectListItem> GetOfficeIdName(System.Security.Principal.IPrincipal User)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                return new SelectList(entities.OfficeSetUp.ToList(), "OfficeId", "OfficeName");
            }
        }

      

        public static IEnumerable<SelectListItem> GetShiftIdName()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.Shift.ToList(), "ShiftId", "ShiftName");
            }
        }

        public static IEnumerable<SelectListItem> YearListWithSelect()
        {
            throw new NotImplementedException();
        }



        public static IEnumerable<SelectListItem> GetFiscalList()
        {


            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.FiscalYearSetUp.ToList(), "FiscalYearId", "FiscalYearName");
            }
        }

        public static IEnumerable<SelectListItem> GetShiftNameAndInOut()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.Shift.ToList(), "ShiftId", "ShiftName");
            }
        }

        public static IEnumerable<SelectListItem> GetSetupVisitTypeIdName(System.Security.Principal.IPrincipal User)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.VistTypeSetUp.ToList(), "VisitTypeId", "VisitTypeName");
            }
        }

        public static IEnumerable<SelectListItem> GetApproverEmployeeIdName(System.Security.Principal.IPrincipal User)
        {
            using (var entities = new ApplicationDbContext())
            {
                return new SelectList(entities.EmployeeInfo.ToList(), "EmployeeId", "EmployeeNameNp");
            }
        }



        public static IEnumerable<SelectListItem> GetEmployeeInfoName()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.EmployeeInfo.ToList<EmployeeInfo>(), "EmployeeId", "EmployeeNameNp");
            }
        }














        public static string GetBranch(bool p1, bool p2)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<SelectListItem> GetSelectionStatus()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "0", Text = "छान्नुहोस्" });
            list.Add(new SelectListItem() { Value = "1", Text = "छ" });
            list.Add(new SelectListItem() { Value = "2", Text = "छैन" });
            return list;




        }

        public static IEnumerable<SelectListItem> GetMaritalStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "छान्नुहोस्";
            list.Add(item);
            SelectListItem item2 = new SelectListItem();
            item2.Value = "1";
            item2.Text = "अविवाहित";
            list.Add(item2);
            SelectListItem item3 = new SelectListItem();
            item3.Value = "2";
            item3.Text = "विवाहित";
            list.Add(item3);
            return list;

        }



        public static IEnumerable<SelectListItem> GetNationalityIdName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Value = "1";
            item.Text = "छान्नुहोस";
            list.Add(item);
            SelectListItem item2 = new SelectListItem();
            item2.Value = "2";
            item2.Text = "Nepali";
            list.Add(item2);
            SelectListItem item3 = new SelectListItem();
            item3.Value = "3";
            item3.Text = "Indian";
            list.Add(item3);

            return list;
        }



        public static IEnumerable<SelectListItem> GetBloodGroup()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "0", Text = "छान्नुहोस्" });
            list.Add(new SelectListItem() { Value = "1", Text = "A" });
            list.Add(new SelectListItem() { Value = "2", Text = "A+" });
            list.Add(new SelectListItem() { Value = "3", Text = "B" });
            list.Add(new SelectListItem() { Value = "4", Text = "B+" });
            list.Add(new SelectListItem() { Value = "5", Text = "0" });
            list.Add(new SelectListItem() { Value = "6", Text = "0+" });
            list.Add(new SelectListItem() { Value = "7", Text = "AB+" });
            return list;




        }






        public static IEnumerable<SelectListItem> GetFiscalYearIdNameWithSelect()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.FiscalYearSetUp.ToList(), "FiscalYearId", "FiscalYearName");
            }
        }

        public static IEnumerable<SelectListItem> GetDocumentCatogoryIdWithAll()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.DocumentCatogory.ToList(), "CategoryId", "CategoryName");
            }
        }

        public static IEnumerable<SelectListItem> GetEmployeeIdWithAll()
        {
            using (var entities = new ApplicationDbContext())
            {
                return new SelectList(entities.EmployeeInfo.ToList(), "EmployeeId", "EmployeeNameNp");
            }
        }


        public static IEnumerable<SelectListItem> GetEmployeeIdbyOffice(int? officeId)
        {
           
            using (var entities = new ApplicationDbContext())
            {
                var employeeDetail = entities.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId).ToList();
               foreach(var item in employeeDetail)
                {

                }
                return new SelectList(entities.EmployeeInfo.ToList(), "EmployeeId", "EmployeeNameNp");
            }
        }


        public static List<SelectListItem> GetEmployeeRecommederList(IPrincipal user)
        {
            var list = new List<SelectListItem>();
            using (var entities = new ApplicationDbContext())
            {
                if (user.IsInRole("SuperAdmin"))
                {
                    var newlist = entities.EmployeeOfficeDetail.ToList();
                    foreach (var item in newlist)
                    {
                        var employeenew = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var userIdeach = employeenew.UserId;



                        List<string> RoleName = entities.Database
                           .SqlQuery<String>(" SELECT dbo.AspNetRoles.Name FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId INNER JOIN dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN dbo.EmployeeInfoes ON dbo.AspNetUserRoles.UserId = dbo.EmployeeInfoes.UserId where dbo.AspNetUsers.Id='" + userIdeach + "'").ToList();

                        foreach (var itemrole in RoleName)
                        {
                            if (itemrole == "Recommendatory")
                            {
                                list.Add(new SelectListItem() { Text = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp, Value = item.EmployeeId.ToString() });

                            }
                        }

                    }
                }
                else
                {

                    var userId = user.Identity.Name;
                    var userid = entities.Users.Where(x => x.UserName == userId).FirstOrDefault().Id;

                    var employee = entities.EmployeeInfo.Where(x => x.UserId == userid).FirstOrDefault();
                    var officeid = entities.EmployeeOfficeDetail.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault().OfficeId;

                    var newlist = entities.EmployeeOfficeDetail.Where(x => x.OfficeId == officeid).ToList();
                    foreach (var item in newlist)
                    {
                        var employeenew = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var userIdeach = employeenew.UserId;



                        List<string> RoleName = entities.Database
                           .SqlQuery<String>(" SELECT dbo.AspNetRoles.Name FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId INNER JOIN dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN dbo.EmployeeInfoes ON dbo.AspNetUserRoles.UserId = dbo.EmployeeInfoes.UserId where dbo.AspNetUsers.Id='" + userIdeach + "'").ToList();

                        foreach (var itemrole in RoleName)
                        {
                            if (itemrole == "Recommendatory")
                            {
                                list.Add(new SelectListItem() { Text = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp, Value = item.EmployeeId.ToString() });

                            }
                        }








                    }


                }



                return list;
            }
        }

        public static List<SelectListItem> GetEmployeeApproverList(IPrincipal user)
        {
            var list = new List<SelectListItem>();
            using (var entities = new ApplicationDbContext())
            {

                if (user.IsInRole("SuperAdmin"))
                {
                    var newlist = entities.EmployeeOfficeDetail.ToList();
                    foreach (var item in newlist)
                    {
                        var employeenew = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var userIdeach = employeenew.UserId;



                        List<string> RoleName = entities.Database
                           .SqlQuery<String>(" SELECT dbo.AspNetRoles.Name FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId INNER JOIN dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN dbo.EmployeeInfoes ON dbo.AspNetUserRoles.UserId = dbo.EmployeeInfoes.UserId where dbo.AspNetUsers.Id='" + userIdeach + "'").ToList();

                        foreach (var itemrole in RoleName)
                        {
                            if (itemrole == "Approver")
                            {
                                list.Add(new SelectListItem() { Text = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp, Value = item.EmployeeId.ToString() });

                            }
                        }

                    }
                }


                else
                {
                    var userId = user.Identity.Name;
                    var userid = entities.Users.Where(x => x.UserName == userId).FirstOrDefault().Id;

                    var employee = entities.EmployeeInfo.Where(x => x.UserId == userid).FirstOrDefault();
                    var officeid = entities.EmployeeOfficeDetail.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault().OfficeId;

                    var newlist = entities.EmployeeOfficeDetail.Where(x => x.OfficeId == officeid).ToList();
                    foreach (var item in newlist)
                    {
                        var employeenew = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();




                        var userIdeach = employeenew.UserId;



                        List<string> RoleName = entities.Database
                           .SqlQuery<String>(" SELECT dbo.AspNetRoles.Name FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId INNER JOIN dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN dbo.EmployeeInfoes ON dbo.AspNetUserRoles.UserId = dbo.EmployeeInfoes.UserId where dbo.AspNetUsers.Id='" + userIdeach + "'").ToList();

                        foreach (var itemrole in RoleName)
                        {
                            if (itemrole == "Approver")
                            {
                                list.Add(new SelectListItem() { Text = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp, Value = item.EmployeeId.ToString() });

                            }
                        }




                    }
                }






                return list;
            }
        }

        public static List<SelectListItem> GetEmployeeApproverListAll(IPrincipal user)
        {
            var list = new List<SelectListItem>();
            using (var entities = new ApplicationDbContext())
            {

                //if (user.IsInRole("SuperAdmin"))
                //{
                    var newlist = entities.EmployeeOfficeDetail.ToList();
                    foreach (var item in newlist)
                    {
                        var employeenew = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var userIdeach = employeenew.UserId;



                        List<string> RoleName = entities.Database
                           .SqlQuery<String>(" SELECT dbo.AspNetRoles.Name FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId INNER JOIN dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN dbo.EmployeeInfoes ON dbo.AspNetUserRoles.UserId = dbo.EmployeeInfoes.UserId where dbo.AspNetUsers.Id='" + userIdeach + "'").ToList();

                        foreach (var itemrole in RoleName)
                        {
                            if (itemrole == "Approver")
                            {
                                list.Add(new SelectListItem() { Text = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp, Value = item.EmployeeId.ToString() });

                            }
                        }

                    }
               // }

                return list;
            }
        }

        public static List<SelectListItem> GetEmployeeRecommederListAll(IPrincipal user)
        {
            var list = new List<SelectListItem>();
            using (var entities = new ApplicationDbContext())
            {
                //if (user.IsInRole("SuperAdmin"))
                //{
                    var newlist = entities.EmployeeOfficeDetail.ToList();
                    foreach (var item in newlist)
                    {
                        var employeenew = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                        var userIdeach = employeenew.UserId;



                        List<string> RoleName = entities.Database
                           .SqlQuery<String>(" SELECT dbo.AspNetRoles.Name FROM dbo.AspNetRoles INNER JOIN dbo.AspNetUserRoles ON dbo.AspNetRoles.Id = dbo.AspNetUserRoles.RoleId INNER JOIN dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id INNER JOIN dbo.EmployeeInfoes ON dbo.AspNetUserRoles.UserId = dbo.EmployeeInfoes.UserId where dbo.AspNetUsers.Id='" + userIdeach + "'").ToList();

                        foreach (var itemrole in RoleName)
                        {
                            if (itemrole == "Recommendatory")
                            {
                                list.Add(new SelectListItem() { Text = entities.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp, Value = item.EmployeeId.ToString() });

                            }
                        }

                    }
                //}
             



                return list;
            }
        }

        public static IEnumerable<SelectListItem> GetDistrictList()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.DistrictSetUp.ToList(), "DistrictId", "DistrictName");
            }
        }

        public static IEnumerable<SelectListItem> GetBranchAndMainBranchWithAll()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.BranchSetUp.ToList(), "BranchId", "BranchName");
            }
        }


        public static IEnumerable<SelectListItem> GetBranchIdName(IPrincipal User)
        {

            ApplicationDbContext entities = new ApplicationDbContext();
            var list = entities.BranchSetUp.ToList();


            BranchSetUp item = new BranchSetUp
            {
                BranchId = 0,
                BranchName = "छान्नुहोस्"
            };
            list.Insert(0, item);

            return new SelectList(list, "BranchId", "BranchName");
        }

        public static IEnumerable<SelectListItem> GetDesignationIdName(IPrincipal User)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            var list = entities.DesignationSetUp.ToList();


            DesignationSetUp item = new DesignationSetUp
            {
                DesignationId = 0,
                DesignationName = "छान्नुहोस्"
            };
            list.Insert(0, item);

            return new SelectList(list, "DesignationId", "DesignationName");
        }



        public static IEnumerable<SelectListItem> GetServiceIdName(IPrincipal User)
        {


            ApplicationDbContext entities = new ApplicationDbContext();
            var list = entities.ServiceSetUp.ToList();


            ServiceSetUp item = new ServiceSetUp
            {
                ServiceId = 0,
                ServiceName = "छान्नुहोस्"
            };
            list.Insert(0, item);

            return new SelectList(list, "ServiceId", "ServiceName");

        }


        public static IEnumerable<SelectListItem> GetOfficeIdNameWithSelect(IPrincipal User)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            List<SelectListItem> list = new List<SelectListItem>();
            var listll = entities.OfficeSetUp.ToList();



            if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    foreach (var item in listll)
                    {
                        list.Add(new SelectListItem() { Value = item.OfficeId.ToString(), Text = item.OfficeName });
                    }
                }
                else if (User.IsInRole("Administrator"))
                {
                    var ofcid = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
                    var OfficeName = listll.Where(x => x.OfficeId == ofcid).FirstOrDefault();
                    List<SelectListItem> selectlist = new List<SelectListItem>();

                    list.Add(new SelectListItem() { Value = OfficeName.OfficeId.ToString(), Text = OfficeName.OfficeName });


                    string s = "GetAllOfficeUnderHead" + " " + OfficeName.OfficeId;




                    ((IObjectContextAdapter)entities).ObjectContext.CommandTimeout = 180;

                    var listnew = entities.Database.SqlQuery<EmployeeAttendanceList>(s).ToList();
                    foreach (var item in listnew)
                    {
                        list.Add(new SelectListItem() { Value = item.OfficeId.ToString(), Text = item.OfficeName });

                    }



                }
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var ofcid = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
                var newlist = listll.Where(x => x.OfficeId == ofcid).ToList();
                foreach (var item in newlist)
                {
                    list.Add(new SelectListItem() { Value = item.OfficeId.ToString(), Text = item.OfficeName });
                }
            }

            if (!User.IsInRole("Admin") || !User.IsInRole("Employee"))
            {
                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "कार्यालय छान्नुहोस्";

                list.Insert(0, item);
            }

            return list;
        }
        public static string GetEmailIdByUserId(string UserId)
        {
            string user = string.Empty;
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var UserName = context.Users.Where(x => x.Id == UserId).FirstOrDefault().UserName;
                user= UserName;
            }
            catch (Exception ex)

            {

            }
            return user;
        }




        public static IEnumerable<SelectListItem> GetPageSize()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "10", Text = "10" });
            list.Add(new SelectListItem() { Value = "20", Text = "20" });
            list.Add(new SelectListItem() { Value = "30", Text = "40" });
            list.Add(new SelectListItem() { Value = "50", Text = "50" });

            return list;




        }



        public static IEnumerable<SelectListItem> GetApplicableForIdName()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "1", Text = "सबै" });
            list.Add(new SelectListItem() { Value = "2", Text = "Male" });
            list.Add(new SelectListItem() { Value = "3", Text = "Female" });
            return list;
        }



        public static IEnumerable<SelectListItem> NepaliYearList()
        {
            NepaliDateConverter converter = new NepaliDateConverter(DateTime.Now.Date);
            ArrayList list = new ArrayList {
                new {
                    Id = "0",
                    Value = "वर्ष छान्नुहोस् ..."
                }
            };
            for (int i = converter.Year; i >= 0x80c; i--)
            {
                list.Add(new
                {
                    Id = i,
                    Value = EnglishToNepaliNumber.ConvertToNepaliNumber(i)
                });
            }
            return new SelectList(list, "id", "Value", converter.Year);
        }












        public static string GetZoneName(int p)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var data = db.ZoneSetUp.Where(x => x.ZoneId == p).FirstOrDefault();
            if (data != null)
                return data.ZoneName;
            return "";
        }

        public static IEnumerable<SelectListItem> GetCheckType()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "1", Text = "चेक इन" });
            list.Add(new SelectListItem() { Value = "2", Text = "चेक आउट" });
            list.Add(new SelectListItem() { Value = "3", Text = "ब्रेक इन" });
            list.Add(new SelectListItem() { Value = "4", Text = "ब्रेक आउट" });

            return list;
        }




        public static IEnumerable<SelectListItem> GetLeaveType()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                return new SelectList(entities.LeaveTypeSetUp.ToList(), "LeaveTypeId", "LeaveTypeName");
            }
        }



        public static int? GetShiftTimeByEmployeeId(int? empId, DateTime date)
        {

            SetupShiftTime time = new SetupShiftTime();
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                //YearlyShift yearshift = entities.YearlyShift.Where(x => x.StartDate >= date).Where(x => x.EndDate <= date).FirstOrDefault();
                //List<SetupShiftTime> source = entities.SetupShiftTime.Where(x => x.ShiftId == yearshift.ShiftId).ToList();
                //if ((source != null) && (source.Count<SetupShiftTime>() > 0))
                //{
                //    return source.First<SetupShiftTime>();
                //}
                //return entities.SetupShiftTime.First<SetupShiftTime>();
                ApplicationDbContext db = new ApplicationDbContext();
                var data = db.EmployeeShiftTime.Where(x => x.EmployeeId == empId).FirstOrDefault();
                if (data != null)
                    return data.ShiftId;
                return 0;
            }
        }





        public static OfficeSetUp GetOfficeInfoByOfficeId(int? _officeId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.OfficeSetUp.Where(x => x.OfficeId == _officeId).FirstOrDefault();

        }

        public static ServiceSetUp GetServiceByServiceId(int _ServiceId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.ServiceSetUp.Where(x => x.ServiceId == _ServiceId).FirstOrDefault();
        }

        public static BranchSetUp GetBranchByMainBranchId(int? _branchId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.BranchSetUp.Where(x => x.BranchId == _branchId).FirstOrDefault();
        }

        public static LevelSetUp GetLevelByLevelId(int _levelId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.LevelSetUp.Where(x => x.LevelId == _levelId).FirstOrDefault();
        }

        public static DesignationSetUp GetDesignationByDesignationId(int? _designationId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.DesignationSetUp.Where(x => x.DesignationId == _designationId).FirstOrDefault();
        }

        public static EmployeeInfo GetEmployeeInfoByEmnployeeId(int? _employeeId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.EmployeeInfo.Where(x => x.EmployeeId == _employeeId).FirstOrDefault();
        }

        public static VisitTypeSetUp GetSetupVisitTypeIdName(int _VisitTypeId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.VistTypeSetUp.Where(x => x.VisitTypeId == _VisitTypeId).FirstOrDefault();
        }



        public static IEnumerable<SelectListItem> ApprovedStatusListWithAll()
        {
            return new SelectList(new[] { new {
        Id = "0",
        Value = "सबै"
    }, new {
        Id = "1",
        Value = "अपूर्ण"
    }, new {
        Id = "2",
        Value = "स्वीकृत"
    }, new {
        Id = "3",
        Value = "अस्वीकृत"
    } }, "id", "Value");

        }

        public static IEnumerable<SelectListItem> GetEmployeeList(int? officeId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var dd = db.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId).ToList();


            List<SelectListItem> list = new List<SelectListItem>();

            if (dd.Count > 0)
            {
                foreach (var item in dd)
                {
                    list.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp });
                }
            }
            return list;
        }



        public static IEnumerable<SelectListItem> GetEmployeeListBYUserSuper(int? officeId, IPrincipal User)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var dd = db.EmployeeOfficeDetail.ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            if (User.IsInRole("SuperAdmin"))
            {

                if (dd.Count > 0)
                {
                    foreach (var item in dd)
                    {
                        list.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp });
                    }
                }
            }
            else
            {
                dd = dd.Where(x => x.OfficeId == officeId).ToList();

                if (dd.Count > 0)
                {
                    foreach (var item in dd)
                    {
                        list.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().EmployeeNameNp });
                    }
                }
            }



            return list;
        }

        internal static string GetShiftNameById(int SetupShiftId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var data = db.Shift.Where(x => x.ShiftId == SetupShiftId).FirstOrDefault();
            if (data != null)
                return data.ShiftName;
            return "";
        }


        public static SelectList GetStatus(string statusid = "0")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "छान्नुहोस्", Value = "o" });
            list.Add(new SelectListItem() { Text = "सक्रिय", Value = "1" });
            list.Add(new SelectListItem() { Text = "निष्क्रिय", Value = "2" });

            return new SelectList(list, "Value", "Text", statusid);
        }

        public static SelectList GetActiveStatus(string statusid = "0")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "सक्रिय", Value = "True" });
            list.Add(new SelectListItem() { Text = "निष्क्रिय", Value = "False" });

            return new SelectList(list, "Value", "Text", statusid);
        }




        internal static int GetOfficeIdByEmployeeId(int p)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var data = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == p).FirstOrDefault();
            if (data != null)
                return Convert.ToInt32(data.OfficeId);
            return 0;
        }


        public static IEnumerable<SelectListItem> GetSetupVisitTypeIdNameByWithAllOrWithSelect()
        {

            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                return new SelectList(entities.VistTypeSetUp.ToList(), "VisitTypeId", "VisitTypeName");
            }

        }


        public static int GetBranchIdByEmployeeId(int EmployeeId)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                var source = entities.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                if (source.BranchId != null)
                {
                    return Convert.ToInt32(source.BranchId);
                }
                return 0;
            }

        }

        public static int GetServiceIdByEmployeeId(int EmployeeId)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                var source = entities.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                if (source.ServiceId != null)
                {
                    return Convert.ToInt32(source.ServiceId);
                }
                return 0;
            }
        }

        public static int GetLevelIdNameByEmployeeId(int EmployeeId)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                var source = entities.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                if (source.LevelId != null)
                {
                    return Convert.ToInt32(source.LevelId);
                }
                return 0;
            }
        }

        public static int GetDesignationIdByEmployeeId(int EmployeeId)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                var source = entities.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                if (source.DesignationId != null)
                {
                    return Convert.ToInt32(source.DesignationId);
                }
                return 0;
            }
        }




        //internal static IEnumerable<SelectListItem> GetOfficeIdNameByEmployeeId(int EmployeeId)
        //{
        //    IEnumerable<SelectListItem> officeIdNameByEmployeeId = Utility.GetOfficeIdNameByEmployeeId(EmployeeId);
        //    return base.Json(officeIdNameByEmployeeId, 0);

        //}


    }
}




