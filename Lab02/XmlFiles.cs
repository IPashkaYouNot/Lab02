using Data;
using System.Xml;

namespace Lab02
{
    public static class XmlFiles
    {
        private static readonly XmlWriterSettings _settings = new() { Indent = true };
        public static void InitialXmlFilesCreation(Assets data)
        {
            CreateBusesXml(data);
            CreateCompaniesXml(data);
            CreateRoutesXml(data);
            CreatBusRouteXml(data);
        }

        public static void CreatBusRouteXml(Assets data)
        {
            using (XmlWriter writer = XmlWriter.Create("busroute.xml", _settings))
            {
                writer.WriteStartElement("busRoutes");
                foreach (var br in data.BusRoutes)
                {
                    writer.WriteStartElement("busRoute");
                    writer.WriteStartElement("bus");
                    writer.WriteElementString("id", br.Bus.Id.ToString());
                    writer.WriteElementString("number", br.Bus.Number);
                    writer.WriteEndElement();

                    writer.WriteStartElement("route");
                    writer.WriteElementString("id", br.Route.RouteId.ToString());
                    writer.WriteElementString("routeName", br.Route.Name);
                    writer.WriteStartElement("startPoint");
                    writer.WriteElementString("id", br.Route.StartPoint.Id.ToString());
                    writer.WriteElementString("name", br.Route.StartPoint.Name);
                    writer.WriteEndElement();
                    writer.WriteElementString("busAmount", br.Route.BusAmount.ToString());
                    writer.WriteStartElement("endPoint");
                    writer.WriteElementString("id", br.Route.EndPoint.Id.ToString());
                    writer.WriteElementString("name", br.Route.EndPoint.Name);
                    writer.WriteEndElement();
                    writer.WriteElementString("busAmount", br.Route.BusAmount.ToString());

                    writer.WriteStartElement("company");
                    writer.WriteElementString("id", br.Route.Company.Id.ToString());
                    writer.WriteElementString("companyName", br.Route.Company.Name);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndElement();

                }
                writer.WriteEndElement();
            }
        }

        public static void CreateBusesXml(Assets data)
        {
            using (XmlWriter writer = XmlWriter.Create("buses.xml", _settings))
            {
                writer.WriteStartElement("buses");
                foreach (var bus in data.Buses)
                {
                    writer.WriteStartElement("bus");
                    writer.WriteElementString("id", bus.Id.ToString());
                    writer.WriteElementString("number", bus.Number);

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        public static void CreateRoutesXml(Assets data)
        {
            using (XmlWriter writer = XmlWriter.Create("routes.xml", _settings))
            {
                writer.WriteStartElement("routes");
                foreach (var route in data.Routes)
                {
                    writer.WriteStartElement("route");
                    writer.WriteElementString("id", route.RouteId.ToString());
                    writer.WriteElementString("routeName", route.Name);
                    writer.WriteStartElement("startPoint");
                    writer.WriteElementString("id", route.StartPoint.Id.ToString());
                    writer.WriteElementString("name", route.StartPoint.Name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("endPoint");
                    writer.WriteElementString("id", route.EndPoint.Id.ToString());
                    writer.WriteElementString("name", route.EndPoint.Name);
                    writer.WriteEndElement();
                    writer.WriteElementString("busAmount", route.BusAmount.ToString());

                    writer.WriteStartElement("company");
                    writer.WriteElementString("id", route.Company.Id.ToString());
                    writer.WriteElementString("companyName", route.Company.Name);
                    writer.WriteEndElement(); //company

                    writer.WriteStartElement("busesNumbers");
                    foreach (var busNumber in route.BusesNumbers)
                    {
                        writer.WriteElementString("busNumber", busNumber);
                    }
                    writer.WriteEndElement(); //buses numbers

                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //routes
            }
        }

        public static void CreateCompaniesXml(Assets data)
        {
            using (XmlWriter writer = XmlWriter.Create("companies.xml", _settings))
            {
                writer.WriteStartElement("companies");

                foreach (var company in data.Companies)
                {
                    writer.WriteStartElement("company");
                    writer.WriteElementString("id", company.Id.ToString());
                    writer.WriteElementString("companyName", company.Name);

                    writer.WriteStartElement("routes");
                    foreach (var route in company.Routes)
                    {
                        writer.WriteStartElement("route");
                        writer.WriteElementString("id", route.RouteId.ToString());
                        writer.WriteElementString("routeName", route.Name);
                        writer.WriteStartElement("startPoint");
                        writer.WriteElementString("id", route.StartPoint.Id.ToString());
                        writer.WriteElementString("name", route.StartPoint.Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("endPoint");
                        writer.WriteElementString("id", route.EndPoint.Id.ToString());
                        writer.WriteElementString("name", route.EndPoint.Name);
                        writer.WriteEndElement();

                        writer.WriteElementString("busAmount", route.BusAmount.ToString());

                        writer.WriteStartElement("company");
                        writer.WriteElementString("id", company.Id.ToString());
                        writer.WriteElementString("companyName", route.Company.Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("busesNumbers");
                        foreach (var busNumber in route.BusesNumbers)
                        {
                            writer.WriteElementString("busNumber", busNumber);
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();

                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }
    }
}
