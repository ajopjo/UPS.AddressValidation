using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPS.AddressValidation.Service;

namespace UPS.AddressValidation
{
    public class UpsAutoFacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<StreetValidator>().As<IStreetValidator>();
            builder.RegisterType<CityStateZipValidator>().As<ICityStateZipValidator>();
            builder.RegisterType<Repository.StreetValidator>().As<Repository.IStreetValidator>();
            builder.RegisterType<Repository.CityStateStreetValidator>().As<Repository.ICityStateStreetValidator>();
        }
    }
}
