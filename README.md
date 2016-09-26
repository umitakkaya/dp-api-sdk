# Unofficial Docplanner Integration API SDK

Install [DpApiSDK through NuGet](https://www.nuget.org/packages/DpApiSDK):
```
PM> Install-Package DpApiSDK
```

Or you can include the project into your solution and use it directly by referencing it.

## Usage example:
```
DpRestClient client = new DpRestClient(
    "{{clientId}}", 
    "{{clientSecret}}", 
    Locale.PL // One of the Locales
);

var facility =  client.GetFacilities().First();
var doctors = client.GetDoctors(facility.Id);

foreach (var doctor in doctors)
{
    Console.WriteLine($"Doctor: {doctor.Fullname}");
    Console.WriteLine($"Addresses:");

    foreach (var address in client.GetAddresses(facility.Id, doctor.Id))
    {
        Console.WriteLine($"\tAddress: {address.Name}");
    }
}
```
