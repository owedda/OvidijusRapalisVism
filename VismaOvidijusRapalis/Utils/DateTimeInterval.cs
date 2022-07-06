using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaOvidijusRapalis.Utils
{
    public static class IntervalUtils
    {
        public static bool DoesIntersect(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            if (DoesIntersectsBeggining(startDate1, startDate2, endDate2) ||
                   DoesIntersectsWhole(startDate1, endDate1, startDate2, endDate2) ||
                   DoesIntersectsMiddle(startDate1, endDate1, startDate2, endDate2) ||
                   DoesIntersectsEnd(endDate1, startDate2, endDate2))
                return true;
            return false;
        }
        private static bool DoesIntersectsBeggining(DateTime startDate1, DateTime startDate2, DateTime endDate2)
        {
            if (startDate2 <= startDate1 && startDate1 <= endDate2)
                return true;
            return false;
        }
        private static bool DoesIntersectsWhole(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            if (startDate2 <= startDate1 && endDate1 <= endDate2)
                return true;
            return false;
        }
        private static bool DoesIntersectsMiddle(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            if (startDate1 <= startDate2 && endDate2 <= endDate1)
                return true;
            return false;
        }
        private static bool DoesIntersectsEnd(DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            if (startDate2 <= endDate1 && endDate1 <= endDate2)
                return true;
            return false;
        }
    }
}
