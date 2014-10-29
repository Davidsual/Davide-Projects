using Autofac;
using Autofac.Core;
using GiftAidCalculator.Domain.Donor;
using GiftAidCalculator.Domain.Donor.Entities;
using GiftAidCalculator.Domain.Management;
using GiftAidCalculator.Domain.Management.Entities;
using GiftAidCalculator.Domain.Promoters;
using GiftAidCalculator.Domain.Promoters.Entities;
using GiftAidCalculator.Domain.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace GiftAidCalculator.Tests
{
    [TestFixture]
    public class GiftAidCalculator
    {
        private IContainer Container { get; set; }

        [SetUp]
        public void BeforeTest()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SettingRepository>().As<ISettingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerLifetimeScope();

            builder.RegisterType<PromoterService>().As<IPromoterService>().InstancePerLifetimeScope();
            builder.RegisterType<DonorService>().As<IDonorService>().InstancePerLifetimeScope();
            builder.RegisterType<ManagementService>().As<IManagementService>().InstancePerLifetimeScope();

            Container = builder.Build();

        }

        [TestCase("450.23")]
        [TestCase("450.2321332")]
        [TestCase("450")]
        public void Test_CalculateGiftAid(string amount)
        {
            decimal castedAmount = Decimal.Parse(amount);

             Mock<IDonorService> donorServiceMock = new Mock<IDonorService>();
            donorServiceMock.Setup(
                c => c.GetGiftAidAmount(It.Is<GiftAidDto>(i => i.DonationAmount == castedAmount))).Returns<GiftAidDto>((inputObject)
                     => new GiftAidResultDto
                     {
                         TaxRate = 20,
                         TaxedAmount = Math.Round((inputObject.DonationAmount * (20m / (100 - 20m))),2),
                         CharityAmount = Math.Round((inputObject.DonationAmount - Math.Round((inputObject.DonationAmount * (20m / (100 - 20m))), 2)), 2),
                         DonationAmount = inputObject.DonationAmount
                     });

            var mockResult = donorServiceMock.Object.GetGiftAidAmount(new GiftAidDto {DonationAmount = castedAmount});
            
            var result = Container.Resolve<IDonorService>().GetGiftAidAmount(new GiftAidDto { DonationAmount = castedAmount });

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TaxRate, mockResult.TaxRate);
            Assert.AreEqual(result.DonationAmount, mockResult.DonationAmount);
            Assert.AreEqual(result.CharityAmount, mockResult.CharityAmount);
            Assert.AreEqual(result.TaxedAmount, mockResult.TaxedAmount);
        }

        [TestCase("1000")]
        [TestCase( "1000.25")]
        [TestCase("1000.55585485")]
        public void Test_Rounded_To_TwoDecimalPlaces(string amount)
        {
            decimal castedAmount = Decimal.Parse(amount);

            var result = Container.Resolve<IDonorService>().GetGiftAidAmount(new GiftAidDto { DonationAmount = castedAmount });

            Assert.IsNotNull(result);
            Assert.AreEqual(result.TaxedAmount.ToString(CultureInfo.InvariantCulture).Split('.')[1].Length,2);
            Assert.AreEqual(result.CharityAmount.ToString(CultureInfo.InvariantCulture).Split('.')[1].Length, 2);
        }

        [TestCase("19")]
        [TestCase("23")]
        [TestCase("43")]
        public void Test_Change_TaxRate(string taxRate)
        {
            decimal castedTaxRate = Decimal.Parse(taxRate);

            var setting = Container.Resolve<IManagementService>().GetSettings();

            Assert.IsNotNull(setting);
            Assert.AreNotEqual(setting.TaxRatePercentage, taxRate);

            //changeSetting
            Container.Resolve<IManagementService>()
                .UpdateSettings(new SettingDto {NumberDecimalRounded = 2, TaxRatePercentage = castedTaxRate});
            setting = Container.Resolve<IManagementService>().GetSettings();

            Assert.IsNotNull(setting);
            Assert.AreEqual(setting.TaxRatePercentage, castedTaxRate);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Test_Supplement_GifAid_Payment(int eventType)
        {
            EventType inputParam = (EventType) eventType;
            Mock<IEventRepository> eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(
                c => c.GetSupplementByEventType(It.Is<EventType>(param => param == inputParam))).Returns<EventType>(
                    (inputObject)
                        =>
                    {
                        switch (inputObject)
                        {
                            case EventType.Running:
                                return 5m;
                            case EventType.Swimming:
                                return 3m;
                            case EventType.Other:
                                return 0m;
                            default:
                                throw new ArgumentOutOfRangeException("eventType");
                        }
                    });
            var mockResult = eventRepositoryMock.Object.GetSupplementByEventType(inputParam);

            var result = Container.Resolve<IPromoterService>().GetTemplateEvent(inputParam);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.EventAdditionalSupplement, mockResult);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [ExpectedException( "System.ArgumentOutOfRangeException" ) ]
        public void Test_Exception_Supplement_GifAid_Payment(int eventType)
        {
            EventType inputParam = (EventType)eventType;
            Container.Resolve<IPromoterService>().GetTemplateEvent(inputParam);

        }

        [TearDown]
        public void AfterTest()
        {
            if(Container != null)
                Container.Dispose();
        }
    }
}
