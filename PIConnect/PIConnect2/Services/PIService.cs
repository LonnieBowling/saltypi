using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PISDK;
using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace PIConnect2.Services
{
    class PIService: IPIService 
    {
        public string Version(string ClientID)
        {
            string result = "Version 1.0.0 ClientID = " + ClientID;
            Console.WriteLine(result);
            return result;

        }

        public List<PIValue> GetPlotValues(string Tag, DateTime StartTime, DateTime EndTime, int Interval)
        {
            try
            {
                Console.WriteLine("Data request for " + Tag);
                PISDK.PISDK Pisdk = new PISDK.PISDK();
                var Srv = Pisdk.Servers["Eng-Dev03"];

                PIValues PVS = Srv.PIPoints[Tag].Data.PlotValues(StartTime, EndTime, Interval);

                List<PIValue> PlotValues = new List<PIValue>();
                foreach (PISDK.PIValue pp in PVS)
                {
                    if (pp.IsGood())
                    {
                        PIValue oValue = new PIValue();
                        oValue.Value = pp.Value;
                        oValue.TimeStamp = pp.TimeStamp.LocalDate.ToUniversalTime();
                        PlotValues.Add(oValue);
                    }
                }
                Console.WriteLine("Data request for {0} completed.  {1} values returned.", Tag, PlotValues.Count);
                return PlotValues;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data request for {0} completed. Error  {1}", Tag, ex.Message );
                return null;
            }

        }

        public List<PIAsset> GetAssetsByLocation(double Longitude, double Latidtude, double Radius)
        {
            try 
            {	    
                PISystem af = new PISystems()["eng-dev03"];
                if (!af.ConnectionInfo.IsConnected) af.Connect();

                AFDatabase db = af.Databases["AFTest"];

                AFAttributeValueQuery[] GPSIsSet = new AFAttributeValueQuery[1];
                GPSIsSet[0].AttributeValue = 1;
                GPSIsSet[0].AttributeTemplate = db.ElementTemplates["Tank"].AttributeTemplates["GPS Coordinates"];
                GPSIsSet[0].Operator = OSIsoft.AF.Search.AFSearchOperator.Equal;
                //GPSIsSet[1].AttributeValue = 1;
                //GPSIsSet[1].AttributeTemplate = db.ElementTemplates["Pump"].AttributeTemplates["GPS Coordinates"];
                //GPSIsSet[1].Operator = OSIsoft.AF.Search.AFSearchOperator.Equal;

                var AssetsFound = AFElement.FindElementsByAttribute(null, "*",  GPSIsSet, true, AFSortField.Name, AFSortOrder.Ascending, 10000);
                var AssetsCoords = (from eachAsset in AssetsFound 
                                    select new PIAsset() 
                                    { Name = eachAsset.Name,
                                        Description = eachAsset.Description,
                                        Guid = eachAsset.ID.ToString(),
                                        Longitude = (double)eachAsset.Attributes["GPS Coordinates"].Attributes["Longitude"].GetValue().Value, 
                                        Latitude = (double)eachAsset.Attributes["GPS Coordinates"].Attributes["Latitude"].GetValue().Value });

                //28.334604,-81.77536 Orlando
                //38.30472,-122.29889 Napa
                var assetWithinRadius = (from eachAsset in AssetsCoords
                                        where (HaversineInKM(Longitude, Latidtude, eachAsset.Latitude, eachAsset.Longitude) <= Radius)
                                        select eachAsset).ToList();
                Console.WriteLine("Asset request by location({0}, {1}) completed.  {2} values returned.", Longitude, Latidtude, assetWithinRadius.Count);
                return assetWithinRadius;
            }
	        catch (Exception ex)
	        {
                Console.WriteLine("Asset request by location({0}, {1}) completed. Error  {2}.", Longitude, Latidtude, ex.Message);
		        return null;
	        }
        }

        private static double _eQuatorialEarthRadius = 6378.1370D;
        private static double _d2r = (Math.PI / 180D);

        private static  int HaversineInM(double lat1, double long1, double lat2, double long2)
        {
            return (int)(1000D * HaversineInKM(lat1, long1, lat2, long2));
        }

        private static double HaversineInKM(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2D), 2D) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }
    } 
}
