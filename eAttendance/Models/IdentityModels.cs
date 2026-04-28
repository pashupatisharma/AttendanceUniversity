using eAttendance.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace eAttendance.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {


        }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<OfficeSetUp> OfficeSetUp { get; set; }
        public DbSet<EmployeeOfficeDetail> EmployeeOfficeDetail { get; set; }
        public DbSet<EmployeeInfo> EmployeeInfo { get; set; }
        public DbSet<EmployeeImage> EmployeeImage { get; set; }


        public DbSet<DepartmentSetup> DepartmentSetup { get; set; }
        public DbSet<DesignationSetUp> DesignationSetUp { get; set; }
        public DbSet<DeviceData> DeviceData { get; set; }

        public DbSet<EmployeeShiftTime> EmployeeShiftTime { get; set; }



        public DbSet<FiscalYearSetUp> FiscalYearSetUp { get; set; }
        public DbSet<LeaveApplication> LeaveApplication { get; set; }
        public DbSet<LeaveTypeSetUp> LeaveTypeSetUp { get; set; }

        
        public DbSet<LevelSetUp> LevelSetUp { get; set; }
        public DbSet<OfficeDeviceSetUp> OfficeDeviceSetUp { get; set; }
        public DbSet<ServiceSetUp> ServiceSetUp { get; set; }


        public DbSet<SetupShiftTime> SetupShiftTime { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<ShiftType> ShiftType { get; set; }
        public DbSet<WeekDaySetUp> WeekDaySetUp { get; set; }
        public DbSet<YearlyShift> YearlyShift { get; set; }

        public DbSet<AssignEmployeeLeave> AssignEmployeeLeave { get; set; }
        public DbSet<BranchSetUp> BranchSetUp { get; set; }
       
     
        public DbSet<HolidayCalender> HolidayCalender { get; set; }
        public DbSet<VisitApplication> VisitApplication { get; set; }
        public DbSet<VisitTypeSetUp> VistTypeSetUp { get; set; }
        public DbSet<DistrictSetUp> DistrictSetUp { get; set; }
        public DbSet<ForgotEntry> ForgotEntry { get; set; }
        public DbSet<ZoneSetUp> ZoneSetUp { get; set; }
        public DbSet<AttendanceLog> AttendanceLog { get; set; }
        public DbSet<TransferModel> TransferModel { get; set; }
        public System.Data.Entity.DbSet<eAttendance.Models.DocumentCatogory> DocumentCatogory { get; set; }
        public System.Data.Entity.DbSet<eAttendance.Models.Document> Documents { get; set; }


    }
}