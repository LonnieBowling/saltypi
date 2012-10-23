using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.ServiceBus;
using PIConnect2.Services;
using System.ServiceModel.Dispatcher;

namespace PIConnect2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(PIService));

            sh.AddServiceEndpoint(
               typeof(IPIService), new NetTcpRelayBinding(),
               ServiceBusEnvironment.CreateServiceUri("sb", "PISpark", "service"))
                .Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateSharedSecretTokenProvider("owner", "Pz4nCcvg4obB9TSNLH01a0hAPKz2SeSc0mmGHVA3H0A=")
                });

            sh.Open();

            foreach (ChannelDispatcherBase channelDispatcherBase in sh.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase
                as ChannelDispatcher;
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    Console.WriteLine("Listening at: {0}", endpointDispatcher.EndpointAddress);
                }
            }


            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();

            sh.Close();
        }
    }
}
