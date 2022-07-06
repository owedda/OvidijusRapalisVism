using VismaOvidijusRapalis.Models;
using VismaOvidijusRapalis.Utils;

namespace Tests
{
    public class UtilsTests
    {
        [Test]
        public void DoesIntersectsBegginingTest()
        {
            DateTime startDate1 = DateTime.Parse("1999-10-10");
            DateTime endDate1 = DateTime.Parse("2030-10-10");
            DateTime startDate2 = DateTime.Parse("1990-10-10");
            DateTime endDate2 = DateTime.Parse("2010-10-10");
            var result = IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoesIntersectsWholeTest()
        {
            DateTime startDate1 = DateTime.Parse("1999-10-10");
            DateTime endDate1 = DateTime.Parse("2000-10-10");
            DateTime startDate2 = DateTime.Parse("1990-10-10");
            DateTime endDate2 = DateTime.Parse("2010-10-10");
            var result = IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoesIntersectsMiddleTest()
        {
            DateTime startDate1 = DateTime.Parse("1999-10-10");
            DateTime endDate1 = DateTime.Parse("2010-10-10");
            DateTime startDate2 = DateTime.Parse("2000-10-10");
            DateTime endDate2 = DateTime.Parse("2008-10-10");
            var result = IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoesIntersectsEndTest()
        {
            DateTime startDate1 = DateTime.Parse("1999-10-10");
            DateTime endDate1 = DateTime.Parse("2010-10-10");
            DateTime startDate2 = DateTime.Parse("2000-10-10");
            DateTime endDate2 = DateTime.Parse("2020-10-10");
            var result = IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoesNotIntersectsBegginingTest()
        {
            DateTime startDate1 = DateTime.Parse("1999-10-10");
            DateTime endDate1 = DateTime.Parse("2002-10-10");
            DateTime startDate2 = DateTime.Parse("2005-10-10");
            DateTime endDate2 = DateTime.Parse("2020-10-10");
            var result = IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2);
            Assert.IsFalse(result);
        }

        [Test]
        public void DoesNotIntersectsEndTest()
        {
            DateTime startDate1 = DateTime.Parse("2005-10-10");
            DateTime endDate1 = DateTime.Parse("2020-10-10");
            DateTime startDate2 = DateTime.Parse("1999-10-10");
            DateTime endDate2 = DateTime.Parse("2002-10-10");

            var result = IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2);
            Assert.IsFalse(result);
        }
    }
}