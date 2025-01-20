using System.Net;
using uhppoted;

uhppoted.Options OPTIONS = new uhppoted.OptionsBuilder()
                                       .WithBind(new IPEndPoint(IPAddress.Any, 0))
                                       .WithBroadcast(new IPEndPoint(IPAddress.Broadcast, 60000))
                                       .WithTimeout(1000)
                                       .WithDebug(true)
                                       .Build();

Console.WriteLine("uhppoted-lib-dotnet");

var result = Uhppoted.FindControllers(OPTIONS);

if (result.IsOk)
{
    var controllers = result.ResultValue;

    Console.WriteLine("find-controllers: {0}", controllers.Length);
    foreach (var controller in controllers)
    {
        Console.WriteLine("  controller {0}", controller.Controller);
        Console.WriteLine("    address  {0}", controller.Address);
        Console.WriteLine("    netmask  {0}", controller.Netmask);
        Console.WriteLine("    gateway  {0}", controller.Gateway);
        Console.WriteLine("    MAC      {0}", controller.MAC);
        Console.WriteLine("    version  {0}", controller.Version);
        Console.WriteLine("    date     {0}", controller.Date);
        Console.WriteLine("");
    }
}
else if (result.IsError)
{
    throw new Exception(Uhppoted.Translate(result.ErrorValue));
}

