using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.Service;

namespace UPS.AddressValidation.Test.Address
{
    [TestFixture]
    public class CityState
    {
        private IContainer _container;

        [TestFixtureSetUp]
        public void Initialize()
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new UpsAccountConfig()
            {
                StreetValidationUri = new Uri("https://onlinetools.ups.com/webservices/XAV"),
                CityStateZipValidationUri = new Uri("https://wwwcie.ups.com/ups.app/xml/AV"),
                CountryCode = "US",
                //Enter user name and password
                Password = "",
                UserName = "",
                AccessLicenseNumber = "1CB80525F98C5155"
            });
            builder.RegisterModule(new UpsAutoFacModule());

            _container = builder.Build();
        }
        [Test]
        public void TestAddressUsingCustom()
        {

            var streetValidator = _container.Resolve<IStreetValidator>();

            var response = streetValidator.ValidateAddress(new DTO.Address() { AddressLine = new[] { "1800 W Central Rd" }, State = "IL", Zip = "60056" });
        }


        [TestFixtureTearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

    }
}
