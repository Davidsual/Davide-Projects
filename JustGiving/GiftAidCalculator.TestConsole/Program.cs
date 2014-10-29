using Autofac;
using GiftAidCalculator.Domain.Donor;
using GiftAidCalculator.Domain.Donor.Entities;
using GiftAidCalculator.Domain.Management;
using GiftAidCalculator.Domain.Management.Entities;
using GiftAidCalculator.Domain.Promoters;
using GiftAidCalculator.Domain.Promoters.Entities;
using GiftAidCalculator.Domain.Repository;
using System;

namespace GiftAidCalculator.TestConsole
{
	class Program
	{
        private static IContainer Container { get; set; }

		static void Main(string[] args)
		{
		    Bootstrap();
			// Calc Gift Aid Based on Previous
            Console.WriteLine("Story 1 - calculate amount donation tax/charity");

			Console.WriteLine("Please Enter donation amount:");
		    decimal amount = Decimal.Parse(Console.ReadLine());

		    var res = Container.Resolve<IDonorService>().GetGiftAidAmount(new GiftAidDto {DonationAmount = amount});

			Console.WriteLine("Donation: {0}", res.DonationAmount);
            Console.WriteLine("Tax rate: {0}%", res.TaxRate);
            Console.WriteLine("Taxed amount: {0}", res.TaxedAmount);
            Console.WriteLine("Charity amount: {0}", res.CharityAmount);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Story 2 - change tax rate and store");
            Console.WriteLine("Please enter new tax rate (1-100):");
            decimal taxRate = Decimal.Parse(Console.ReadLine());
		    Container.Resolve<IManagementService>().UpdateSettings(new SettingDto {TaxRatePercentage = taxRate});
            Console.WriteLine("Tax rate updated!");
            Console.WriteLine("");
            Console.WriteLine("Please Enter donation amount:");
            amount = Decimal.Parse(Console.ReadLine());

            res = Container.Resolve<IDonorService>().GetGiftAidAmount(new GiftAidDto { DonationAmount = amount });

            Console.WriteLine("Donation: {0}", res.DonationAmount);
            Console.WriteLine("Tax rate: {0}%", res.TaxRate);
            Console.WriteLine("Taxed amount: {0}", res.TaxedAmount);
            Console.WriteLine("Charity amount: {0}", res.CharityAmount);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Story 3 - rounded decimals");
            Console.WriteLine("Please Enter donation amount:");
            amount = Decimal.Parse(Console.ReadLine());

            res = Container.Resolve<IDonorService>().GetGiftAidAmount(new GiftAidDto { DonationAmount = amount });
            
            Console.WriteLine("Donation: {0}", res.DonationAmount);
            Console.WriteLine("Tax rate: {0}%", res.TaxRate);
            Console.WriteLine("Taxed amount: {0}", res.TaxedAmount);
            Console.WriteLine("Charity amount: {0}", res.CharityAmount);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Story 4 - Get additional supplement by event type");
            Console.WriteLine("Please Enter the event type (0-2) (0: Running / 1: Swimming / 2: Other)");
            int eventType = Convert.ToInt32(Console.ReadLine());

            var promoterService = Container.Resolve<IPromoterService>().GetTemplateEvent((EventType)eventType);
            Console.WriteLine("Event Type: {0}", promoterService.EventType);
            Console.WriteLine("Event Additional Supplement: {0}%", promoterService.EventAdditionalSupplement);

			Console.WriteLine("Press any key to exit.");

			Console.ReadLine();
		}

	    private static void Bootstrap()
	    {
            var builder = new ContainerBuilder();

            builder.RegisterType<SettingRepository>().As<ISettingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerLifetimeScope();

            builder.RegisterType<PromoterService>().As<IPromoterService>().InstancePerLifetimeScope();
            builder.RegisterType<DonorService>().As<IDonorService>().InstancePerLifetimeScope();
            builder.RegisterType<ManagementService>().As<IManagementService>().InstancePerLifetimeScope();
            
            Container = builder.Build();

    }
	}
}
