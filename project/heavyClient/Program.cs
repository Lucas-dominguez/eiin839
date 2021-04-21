using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using heavyClient.ServiceReferenceRouting;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace heavyClient {
    class Program
    {
        static void Main(string[] args)
        {
            RoutingClient routing = new RoutingClient("SoapRouting");
            Console.WriteLine("Welcome to Let's go Biking ! \n Here you can search for the fastest route using the bicycle of JCDecaux");
            Console.WriteLine("Please press enter to begin");
            Console.ReadLine();
            string res;
            do
            {
                Console.WriteLine("Select what do you want to do :");
                Console.WriteLine("     Search for a route -----> 1");
                Console.WriteLine("     View uses of stations -----> 2");
                Console.WriteLine("     Exit -----> 3");
                res = Console.ReadLine();
                if (res == "1")
                {
                    RoutingResult r = searchRouting(routing);
                    displayResult(r);
                }
                else if (res == "2")
                {
                    Console.WriteLine("Stats have been wrote to StationUsage.xls (at the root of the .exe)");
                }
                else if (res != "3")
                {
                    Console.WriteLine("Error bad input try again");
                }
            }
            while (res != "3");
            Console.WriteLine("Goodbye\nApplication closing...");
            //Console.ReadLine();
        }
        static RoutingResult searchRouting(RoutingClient routing)
        {
            Console.WriteLine("Enter your starting location : ");
            string start = Console.ReadLine();
            Console.WriteLine("Enter your destination : ");
            string end = Console.ReadLine();
            Console.WriteLine("Waiting ....\n\n ");
            return routing.GetRoutingMap(start, end);
        }

        static void displayResult(RoutingResult r)
        {
            if (r.infos != "OK")
            {
                Console.WriteLine("Sorry there have been an error with your request please try again or later");
                return;
            }
            List<Route> routes = new List<Route>();
            List<Station> stations = r.infosStations.ToList();
            foreach (string s in r.routes.ToList())
            {
                routes.Add(JsonConvert.DeserializeObject<RouteResult>(s).routes[0]);
            }
            if (routes.Count > 2)
            {
                Console.WriteLine("You have to go to take the bike at the station " + stations[0].name + " located at " + stations[0].address);
                Console.WriteLine("Distance : " + routes[0].distance + " km (estimate time " + Math.Round(float.Parse(routes[0].duration, CultureInfo.InvariantCulture) / 60) + " min )\n");
                Console.WriteLine("Then you will have to drive to the station " + stations[1].name + " located at " + stations[1].address + " to leave the bike.");
                Console.WriteLine("Distance : " + routes[1].distance + " km (estimate time " + Math.Round(float.Parse(routes[1].duration, CultureInfo.InvariantCulture) / 60) + " min )\n");
                Console.WriteLine("Finally you can walk to your destination");
                Console.WriteLine("Distance : " + routes[2].distance + " km (estimate time " + Math.Round(float.Parse(routes[2].duration, CultureInfo.InvariantCulture) / 60) + " min )\n");
                writeToExcel(stations);
            }
            else
            {
                Console.WriteLine("Sorry you have to do the trip by walking");

            }
            string res;
            do
            {
                Console.WriteLine("\n\nDo you want to ..");
                Console.WriteLine("See details on the path ------> 1");
                Console.WriteLine("See infos on the stations ----> 2 ");
                Console.WriteLine("Go back ----> 3 ");
                res = Console.ReadLine();
                if (res == "1")
                {
                    displayPath(routes);
                }
                else if (res == "2")
                {
                    displayStation(stations);
                }
                else if (res != "3")
                {
                    Console.WriteLine("Error bad input try again");
                }
            } while (res != "3");
        }


        static void displayPath(List<Route> routes)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                if (routes.Count < 2)
                {
                    Console.WriteLine("Trip : \n");
                }
                else
                {
                    if (i == 0) Console.WriteLine("Trip to the nearest station :\n");
                    if (i == 1) Console.WriteLine("Trip in bike : \n");
                    if (i == 2) Console.WriteLine("Trip to destination :\n");
                }
                List<Step> steps = routes[i].legs[0].steps;
                for (var j = 0; j < steps.Count; j++)
                {
                    Console.WriteLine("     * " + steps[j].maneuver.instruction);
                    Console.ReadLine();
                }
                Console.WriteLine("\n\n");
            }
        }

        static void displayStation(List<Station> stations)
        {
            if (stations != null && stations.Count > 1 && stations[0] != null && stations[1] != null)
            {
                for (int i = 0; i < stations.Count; i++)
                {
                    Console.WriteLine("\n\nStation " + i + " : \n");
                    Console.WriteLine("Address : " + stations[i].address);
                    Console.WriteLine("Name : " + stations[i].name);
                    Console.WriteLine("Bikes available : " + stations[i].mainStands.availabilities.bikes + "/" + stations[i].mainStands.capacity);
                }
            }
        }

        static void writeToExcel(List<Station> stations)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                if (excelApp != null)
                {
                    excelApp.DisplayAlerts = false;
                    var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                    var iconPath = Path.Combine(outPutDirectory, "StationsUsage.xls");
                    string path = new Uri(iconPath).LocalPath;
                    FileInfo fi = new FileInfo(path);
                    Excel.Workbook excelWorkbook;
                    Excel.Worksheet excelWorksheet;
                    if (!fi.Exists)
                    {
                        excelWorkbook = excelApp.Workbooks.Add();
                        excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();
                    }
                    else
                    {
                        excelWorkbook = excelApp.Workbooks.Open(path,Type.Missing, false, Type.Missing, Type.Missing, Type.Missing, true);
                        excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.get_Item(1);
                    }
                    try
                    {
                        Excel.Range range = excelWorksheet.UsedRange;
                        int rw = range.Rows.Count;
                        foreach (Station s in stations)
                        {
                            int i;
                            for (i = 1; i < rw+1; i++)
                            {
                                string cellValue = (string)(range.Cells[i, 1] as Excel.Range).Value2;
                                if (cellValue!= null && cellValue.Equals(s.name))
                                {
                                    double value = ((double)(range.Cells[i, 2] as Excel.Range).Value2 + 1);
                                    excelWorksheet.Cells[i, 2] = value;
                                    break;
                                }
                            }
                            if (i > rw)
                            {
                                excelWorksheet.Cells[rw, 1] = s.name;
                                excelWorksheet.Cells[rw, 2] = 1;
                                rw++;
                            }
                        }
                        excelWorkbook.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);

                        excelWorkbook.Close();
                        excelApp.Quit();

                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                        excelWorkbook.Close();
                        excelApp.Quit();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Si il y a une erreur de cast .. Il faut réparer son office 365 sur le pc (personne sait pourquoi). Programmes -> Office 365 -> modifier -> réparation rapide");
            
            }
        }
    }
}
