using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIConnect2.Services;
using System.ServiceModel;
using Microsoft.ServiceBus;

namespace PIWebRole.Services
{
    public class PIService
    {
        public static List<PIValue> GetPlotValues(string Tag, DateTime StartTime, DateTime EndTime, int Interval)
        {
            try
            {            
                var cf = new ChannelFactory<IPIService>(
                    new NetTcpRelayBinding(),
                    new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", "PISpark", "service")));

                cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior { TokenProvider = TokenProvider.CreateSharedSecretTokenProvider("owner", "Pz4nCcvg4obB9TSNLH01a0hAPKz2SeSc0mmGHVA3H0A=") });

                var ch = cf.CreateChannel();

                return ch.GetPlotValues(Tag, StartTime, EndTime, Interval);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<PIAsset> GetAssetsByLocation(double Longitude, double Latidtude, double Radius)
        {
            try
            {
                var cf = new ChannelFactory<IPIService>(
                    new NetTcpRelayBinding(),
                    new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", "PISpark", "service")));

                cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior { TokenProvider = TokenProvider.CreateSharedSecretTokenProvider("owner", "Pz4nCcvg4obB9TSNLH01a0hAPKz2SeSc0mmGHVA3H0A=") });

                var ch = cf.CreateChannel();

                return ch.GetAssetsByLocation(Longitude,Latidtude,Radius);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}