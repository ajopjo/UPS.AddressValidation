# USPS.AddressValidation

This is a Proof of Concept project created for validating US Address using UPS APIs using .NET:

- https://www.ups.com/upsdeveloperkit/downloadresource?loc=en_US
- https://www.ups.com/upsdeveloperkit/downloadresource?loc=en_US

## Dependency
Auto fac

## How To

The core project provides an AutoFac module UpsAutoFacModule that will wire up the internal dependencies.

Initialize the autofac container and configure the UPS Account Settings.
```sh
 var builder = new ContainerBuilder();

            builder.Register(c => new UpsAccountConfig()
            {
                StreetValidationUri = new Uri("https://onlinetools.ups.com/webservices/XAV"),
                CityStateZipValidationUri = new Uri("https://wwwcie.ups.com/ups.app/xml/AV"),
                CountryCode = "US",
                //Enter user name and password
                Password = "",
                UserName = "",
                AccessLicenseNumber = ""
            });
            builder.RegisterModule(new UpsAutoFacModule());

            _container = builder.Build();
```
The Core module exposes two interfaces:
 - IStreetValidator
 - ICityStateZipValidator
 

More details coming soon

###License

The MIT License (MIT)
Copyright (c) <year> <copyright holders>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
