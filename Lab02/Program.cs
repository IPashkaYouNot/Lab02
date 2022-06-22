using Data;
using Lab02;
using Model;
using System.Text.RegularExpressions;

namespace Lab2Example
{
    class Program
    {
        static void Main()
        {
            var data = DataSeeding.GetData();
            XmlFiles.InitialXmlFilesCreation(data);

            RealiseOptions(data);
        }

        static void PrintAllOptions()
        {
            Console.WriteLine("Choose from 1 to 15 or 0 to quit.");
            Console.WriteLine("1. Print start point and amount of buses starting on this point.");
            Console.WriteLine("2. Print number of buses that run on routes with more than three buses.");
            Console.WriteLine("3. Print companies with the biggest amount of routes.");
            Console.WriteLine("4. Print first and last three buses.");
            Console.WriteLine("5. Print routes with it\'s companies.");
            Console.WriteLine("6. Print common routes between first and second buses.");
            Console.WriteLine("7. Print list of the same amount of routes for buses.");
            Console.WriteLine("8. Print car city codes with their amount.");
            Console.WriteLine("9. Print all start and end points.");
            Console.WriteLine("10. Print routes with the same amount of buses.");
            Console.WriteLine("11. Print avarage amount of buses among routes.");
            Console.WriteLine("12. Print all buses except buses on first and last route.");
            Console.WriteLine("13. Print amount of symbols in routes.");
            Console.WriteLine("14. Print all routes of companies.");
            Console.WriteLine("15. Print all ordered bus numbers.");
            Console.WriteLine("16. Input new bus.");
            Console.WriteLine("0. Quit.");
        }

        static void RealiseOptions(Assets data)
        {
            var queues = new Queues();
            var finished = false;
            while (!finished)
            {
                PrintAllOptions();
                string? opt = Console.ReadLine();
                if (opt is null || !int.TryParse(opt, out int result) || result < 0 || result > 16)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong option. Try again.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }
                switch (result)
                {
                    case 0:
                        Console.WriteLine("Program finished.");
                        finished = true;
                        break;
                    case 1:
                        {
                            var queue = queues.GroupByStartPointAndCount();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 2:
                        {
                            var queue = queues.SelectBusesOnRoutesWithMoreThanGivenAmountOfBuses(3);
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 3:
                        {
                            var queue = queues.GetCompaniesWithTheBiggestAmountOfRoutes();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 4:
                        {
                            var queue = queues.GetFirstAndLastThreeBuses();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 5:
                        {
                            var queue = queues.CompanyRouteJoin();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item.Name1 + " " + item.Name2);
                            }
                            break;
                        }
                    case 6:
                        {
                            var queue = queues.CommonRoutesBetweenFirstAndSecondBus();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 7:
                        {
                            var queue = queues.GroupingBusesByAmountOfRoutesOnEach();
                            foreach (var item in queue)
                            {
                                Console.WriteLine("Amount: " + item.Name1 + ", buses: " + item.Name2);
                            }
                            break;
                        }
                    case 8:
                        {
                            var queue = queues.GroupingByCarCodes();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item.Name1 + " - " + item.Name2);
                            }
                            break;
                        }
                    case 9:
                        {
                            var queue = queues.GetAllEndAndStartPoints();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 10:
                        {
                            var queue = queues.SortRoutesByAmountOfBuses();
                            foreach (var item in queue)
                            {
                                Console.WriteLine("Routes: " + item.Name2 + " with " + item.Name1);
                            }
                            break;
                        }
                    case 11:
                        {
                            var queue = queues.AvarageAmountOfBusesAmongRoutes();
                            Console.WriteLine("Avarage amount: " + queue);
                            break;
                        }
                    case 12:
                        {
                            var queue = queues.AllBusesExceptFirstAndLastRoute();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 13:
                        {
                            var queue = queues.GroupRoutesByAmountOfSymbolsInName();
                            foreach (var item in queue)
                            {
                                Console.WriteLine("Number of symbols: " + item.Name1 + ", routes: " + item.Name2);
                            }
                            break;
                        }
                    case 14:
                        {
                            var queue = queues.GetAllRoutesOfCompanies();
                            foreach (var item in queue)
                            {
                                Console.WriteLine("Company \"" + item.Name1 + "\" has routes: " + item.Name2);
                            }
                            break;
                        }
                    case 15:
                        {
                            var queue = queues.OrderAllBusNumbers();
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case 16:
                        AddingNewItemsToBuses(data);
                        queues.ReadBuses();
                        queues.ReadBusRoutes();
                        break;
                }

                Console.Write("Press any button to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void AddingNewItemsToBuses(Assets data)
        {
            while (true)
            {
                Console.WriteLine("Type bus number:");
                var number = Console.ReadLine();
                if (data.Buses.Any(b => b.Number == number))
                {
                    Console.WriteLine("Wrong number.");
                    continue;
                }

                Console.WriteLine("Choose routes separated by spaces");
                Console.WriteLine(data.Routes.Select(r => r.Name).Aggregate((s1, s2) => s1 + ", " + s2));

                var routes = Console.ReadLine();
                if (routes is null)
                {
                    Console.WriteLine("Type any route");
                    continue;
                }

                var separatedRoutes = routes.Split(' ');

                var routesObj = new List<Route>();
                var isCorrectRoutes = true;
                foreach (var route in separatedRoutes)
                {
                    var rr = data.Routes.FirstOrDefault(r => r.Name == route);
                    if (rr is null)
                    {
                        Console.WriteLine("Route " + route + " was not found");
                        isCorrectRoutes = false;
                        break;
                    }
                    routesObj.Add(rr);
                }
                if (!isCorrectRoutes) continue;

                var bus = new Bus { Number = number, Routes = routesObj };

                foreach (var route in routesObj)
                {
                    route.Buses.Add(bus);
                }

                data.Buses = data.Buses.Append(bus);

                XmlFiles.CreateBusesXml(data);
                Console.WriteLine("done");
                break;
            }
        }
    }
}