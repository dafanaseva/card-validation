using CardValidation.Core.Enums;
using CardValidation.Core.Services;
using CardValidation.Core.Services.Interfaces;
using NUnit.Framework;
using System;

namespace CardValidation.Tests
{
    public class Tests
    {
        private ICardValidationService cardValidationService;

        [SetUp]
        public void Setup()
        {
            cardValidationService = new CardValidationService();
        }


        [Test]
        [TestCase("5555555555554444", true, PaymentSystemType.MasterCard)]
        [TestCase("5200828282828210", true, PaymentSystemType.MasterCard)]
        [TestCase("5105105105105100", true, PaymentSystemType.MasterCard)]
        [TestCase("2225105105105100", true, PaymentSystemType.MasterCard)]
        [TestCase("2275105105105100", true, PaymentSystemType.MasterCard)]
        [TestCase("4242424242424242", true, PaymentSystemType.Visa)]
        [TestCase("4000056655665556", true, PaymentSystemType.Visa)]
        [TestCase("4000056655665", true, PaymentSystemType.Visa)]
        [TestCase("378282246310005", true, PaymentSystemType.AmericanExpress)]
        [TestCase("371449635398431", true, PaymentSystemType.AmericanExpress)]
        public void TestValidateNumber(string number, bool isValid, PaymentSystemType type)
        {
            Assert.AreEqual(isValid, cardValidationService.ValidateNumber(number));
            Assert.AreEqual(type, cardValidationService.GetPaymentSystemType(number));
        }



        [Test]
        [TestCase("40000566556655561234", false, PaymentSystemType.Visa)]
        [TestCase("40000566556656", false, PaymentSystemType.Visa)]
        [TestCase("22751051051051006", false, PaymentSystemType.MasterCard)]
        [TestCase("227bbbbbbbbbbbbb", false, PaymentSystemType.MasterCard)]
        [TestCase("aaaa bbbb", false, PaymentSystemType.MasterCard)]
        [TestCase("3782822463100055", false, PaymentSystemType.AmericanExpress)]
        public void TestValidateWrongNumber(string number, bool isValid, PaymentSystemType type)
        {
            Assert.AreEqual(isValid, cardValidationService.ValidateNumber(number));
            Assert.Throws<NotImplementedException>(() => cardValidationService.GetPaymentSystemType(number));
        }

        [Test]
        [TestCase("firstname lastname", true)]
        [TestCase("Firstname Lastname", true)]
        [TestCase("5200828282828210", false)]
        [TestCase("11/22", false)]
        [TestCase("1123", false)]
        [TestCase("Name", true)]
        [TestCase("Lastname", true)]
        [TestCase("", false)]
        public void TestValidateOwner(string number, bool isValid)
        {
            Assert.AreEqual(isValid, cardValidationService.ValidateOwner(number));
        }

        [Test]
        [TestCase("555", true)]
        [TestCase("111", true)]
        [TestCase("222", true)]
        [TestCase("", false)]
        [TestCase("4444", true)]
        [TestCase("55555", false)]
        [TestCase("name", false)]
        [TestCase("ab1", false)]
        public void TestValidateCvc(string number, bool isValid)
        {
            Assert.AreEqual(isValid, cardValidationService.ValidateCvc(number));
        }

        [Test]
        [TestCase("1122", true)]
        [TestCase("1121", false)]
        [TestCase("01/25", true)]
        [TestCase("02/21", false)]
        [TestCase("01/22", false)]
        [TestCase("00/22", false)]
        [TestCase("", false)]
        [TestCase("aa/bb", false)]
        [TestCase("aabb", false)]
        [TestCase("0000", false)]
        public void TestValidateIssueDate(string number, bool isValid)
        {
            Assert.AreEqual(isValid, cardValidationService.ValidateIssueDate(number));
        }
    }
}