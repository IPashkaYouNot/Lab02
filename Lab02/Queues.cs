using System.Xml.Linq;

namespace Lab02
{
    public class Queues
    {
        private const string _busesFile = "buses.xml";
        private const string _routesFile = "routes.xml";
        private const string _companiesFile = "companies.xml";
        private const string _busRoutesFile = "busroute.xml";

        private XElement _buses;
        private XElement _routes;
        private XElement _companies;
        private XElement _busRoutes;

        public void ReadBuses()
        {
            var xBusesDoc = XDocument.Load(_busesFile);
            _buses = xBusesDoc.Element("buses");
        }

        public void ReadRoutes()
        {
            var xRoutesDoc = XDocument.Load(_routesFile);
            _routes = xRoutesDoc.Element("routes");
        }

        public void ReadCompanies()
        {
            var xCompaniesDoc = XDocument.Load(_companiesFile);
            _companies = xCompaniesDoc.Element("companies");
        }

        public void ReadBusRoutes()
        {
            var xCompaniesDoc = XDocument.Load(_busRoutesFile);
            _busRoutes = xCompaniesDoc.Element("busRoutes");
        }


        public Queues()
        {
            ReadBuses();
            ReadRoutes();
            ReadCompanies();
            ReadBusRoutes();
        }

        public IEnumerable<string> GroupByStartPointAndCount()
        {
            var queue = from route in _routes?.Elements("route")
                        group route by route?.Element("startPoint")?.Element("id")?.Value into routeGroup
                        select new String
                        (
                            routeGroup.Select(x => x.Element("routeName").Value).First() + " - " + routeGroup.Count()
                        );
            return queue;
        }

        public IOrderedEnumerable<string> SelectBusesOnRoutesWithMoreThanGivenAmountOfBuses(int number)
        {
            var queue = _busRoutes.Elements("busRoute")?
                .Where(br => int.Parse(br?.Element("route")?.Element("busAmount")?.Value ?? "-1") >= number)
                .Select(br => br.Element("bus").Element("number").Value)
                .OrderBy(num => num);
            return queue;
        }

        public IEnumerable<string> GetCompaniesWithTheBiggestAmountOfRoutes()
        {
            var queue = (from companies in _companies.Elements("company")
                         where companies.Element("routes").Elements("route").Count() ==
                                (from max in _companies.Elements("company")
                                 select max.Element("routes").Elements("route").Count()).Max()
                         select companies.Element("companyName").Value);

            return queue;
        }

        public IEnumerable<string> GetFirstAndLastThreeBuses()
        {
            var queue = _buses.Elements("bus")
                .Take(3)
                .Concat(_buses.Elements("bus")
                    .TakeLast(3)).Select(bus => bus.Element("number").Value);

            return queue;
        }

        public IEnumerable<StringString> CompanyRouteJoin()
        {
            var queue = from routes in _routes.Elements("route")
                        join companies in _companies.Elements("company")
                        on routes.Element("company").Element("id").Value equals companies.Element("id").Value
                        select new StringString
                        {
                            Name1 = routes.Element("routeName").Value,
                            Name2 = companies.Element("companyName").Value
                        };
            return queue;
        }

        public IEnumerable<string> CommonRoutesBetweenFirstAndSecondBus()
        {
            var queue = _busRoutes.Elements("busRoute")
                .Where(br => br.Element("bus").Element("id").Value == _buses?.Elements("bus")?.ElementAt(1)
                    .Element("id").Value)
                .Select(br => br.Element("route").Element("routeName").Value)
                .Intersect(_busRoutes.Elements("busRoute")
                .Where(br => br.Element("bus").Element("id").Value == _buses?.Elements("bus")?.ElementAt(2)
                    .Element("id").Value)
                .Select(br => br.Element("route").Element("routeName").Value));

            return queue;
        }

        public IEnumerable<StringString> GroupingBusesByAmountOfRoutesOnEach()
        {
            var queue = _busRoutes.Elements("busRoute").GroupBy(br => br.Element("bus").Element("id").Value)
                .Select(br => new
                {
                    Name = br.FirstOrDefault()?.Element("bus"),
                    Count = br.Count()
                })
                .GroupBy(br => br.Count)
                .Select(b => new StringString
                {
                    Name1 = b.Key.ToString(),
                    Name2 = b.Select(n => n.Name.Element("number").Value)
                    .Aggregate((n1, n2) => n1 + ", " + n2)
                });

            return queue;
        }

        public IEnumerable<StringString> GroupingByCarCodes()
        {
            var queue = from res in (from buses in _buses.Elements("bus")
                                     group buses by buses.Element("number").Value[..2] into busesGroups
                                     select new StringString
                                     {
                                         Name1 = busesGroups.Key,
                                         Name2 = busesGroups.Count().ToString()
                                     })
                        orderby res.Name2 descending
                        select res;

            return queue;
        }

        public IOrderedEnumerable<string> GetAllEndAndStartPoints()
        {
            var queue = _routes.Elements("route").Select(r => r.Element("startPoint").Element("name").Value)
                .Union(_routes.Elements("route").Select(r => r.Element("endPoint").Element("name").Value))
                .OrderBy(r => r.Length);

            return queue;
        }

        public IEnumerable<StringString> SortRoutesByAmountOfBuses()
        {
            var queue = from res in (from routes in _routes.Elements("route")
                                     group routes by int.Parse(routes.Element("busAmount").Value) into routeGroups
                                     select new StringString
                                     {
                                         Name1 = routeGroups.Key + " buses",
                                         Name2 = (from r in routeGroups
                                                   select r.Element("routeName").Value).Aggregate((s1, s2) => s1 + ", " + s2)
                                     })
                        orderby res.Name1 descending
                        select res;
            return queue;

        }

        public double AvarageAmountOfBusesAmongRoutes()
        {
            var queue = (from amount in _routes.Elements("route")
                         select double.Parse(amount.Element("busAmount").Value)).Average();

            return queue;
        }

        public IEnumerable<string> AllBusesExceptFirstAndLastRoute()
        {
            var queue = (from buses in _buses.Elements("bus")
                         select buses.Element("number").Value).Except((
                            from buses1 in _routes.Elements("route").First().Element("busesNumbers").Elements("busNumber")
                            select buses1.Value)
                            .Union(
                                from buses2 in _routes.Elements("route").Last().Element("busesNumbers").Elements("busNumber")
                                select buses2.Value)
                            );

            return queue;
        }

        public IEnumerable<StringString> GroupRoutesByAmountOfSymbolsInName()
        {
            var queue = _routes.Elements("route").GroupBy(r =>
                r.Element("routeName").Value.Length)
                .Select(r => new StringString
                {
                    Name1 = r.Key.ToString(),
                    Name2 = r.Select(rr => rr.Element("routeName").Value)
                        .Aggregate((s1, s2) => s1 + ", " + s2)
                });
            return queue;
        }

        public IEnumerable<StringString> GetAllRoutesOfCompanies()
        {
            var queue = _companies.Elements("company").Select(c =>
                new StringString
                {
                    Name1 = c.Element("companyName").Value,
                    Name2 = c.Element("routes").Elements("route").Select(r => r.Element("routeName").Value)
                        .Aggregate((n1, n2) => n1 + ", " + n2)
                }
            );
            return queue;
        }

        public IEnumerable<string> OrderAllBusNumbers()
        {
            var queue = from buses in _buses.Elements("bus")
                        orderby buses.Element("id").Value
                        select buses.Element("number").Value;

            return queue;
        }
    }
    public class StringString
    {
        public string Name1 { get; init; }
        public string Name2 { get; init; }
    }
}
