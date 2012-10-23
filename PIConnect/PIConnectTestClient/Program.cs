using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIConnect2.Services;
using System.ServiceModel;
using Microsoft.ServiceBus;

namespace PIConnectTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var cf = new ChannelFactory<IPIService>(
            new NetTcpRelayBinding(),
            new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", "PISpark", "service")));

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior { TokenProvider = TokenProvider.CreateSharedSecretTokenProvider("owner", "Pz4nCcvg4obB9TSNLH01a0hAPKz2SeSc0mmGHVA3H0A=") });
            
            var ch = cf.CreateChannel();

            var l = ch.GetPlotValues("cdt158", DateTime.Now.AddHours(-4), DateTime.Now, 100);

            Console.WriteLine( "Data returned: " + l.Count);
           
            Console.ReadLine();

            //28.334604,-81.77536 Orlando
            //38.30472,-122.29889 Napa
            var m = ch.GetAssetsByLocation(28.33, -81.77, 100);
            Console.WriteLine("Assets returned: " + m.Count);
            


            Console.ReadLine();
        }
    }
}
