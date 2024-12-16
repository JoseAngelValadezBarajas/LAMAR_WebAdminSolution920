using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerCampus.DataAccess;

namespace PowerCampus.UnitTests
{
    [TestClass]
    public class CreditNoteTest
    {
        [TestMethod]
        public void GetAllChargeCredits()
        {
            var catalogDA = new CatalogDA();
            var chargeCredits = catalogDA.GetAllChargeCredits();
            Assert.IsNotNull(chargeCredits);
        }
    }
}