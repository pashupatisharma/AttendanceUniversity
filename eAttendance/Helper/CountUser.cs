using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.Helper
{
    public static class CountUser
    {
        public static int UserCount = 0;
        public static int increasercount()
        {
            UserCount++;
            return UserCount;
        }
        public static int Decreasecount()
        {
            UserCount--;
            return UserCount;
        }
        public static int GetCurrentNumberofuser()
        {
            return UserCount;
        }
    }
}