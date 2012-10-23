using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace PIConnect2.Services
{
    [ServiceContract(Namespace="PIService")]
    interface IPIService
    {
        [OperationContract]
        string Version(string ClientID);
        [OperationContract]
        List<PIValue> GetPlotValues(string Tag, DateTime StartTime, DateTime EndTime, int Interval);
        [OperationContract]
        List<PIAsset> GetAssetsByLocation(double Longitude, double Latidtude, double Radius);

    }

    interface IPIServiceChannel : IPIService, IClientChannel {}

    [DataContract]
    public class PIValue
    {
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
    }

    [DataContract]
    public class PIAsset
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Guid { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public double Latitude { get; set; }
    }

}
