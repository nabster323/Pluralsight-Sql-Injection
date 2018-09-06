using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pluralsight_sql_injection
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new CustomerContext())
            {
                if (!ctx.Customers.Any())
                {
                    ctx.Customers.Add(new Customer { FirstName = "Daniel", LastName = "Wood", DOB = new DateTime(1990, 3, 6) });
                    ctx.Customers.Add(new Customer { FirstName = "Robert", LastName = "Martin", DOB = new DateTime(1989, 12, 25) });
                    ctx.Customers.Add(new Customer { FirstName = "Troy", LastName = "Hunt", DOB = new DateTime(1989, 10, 20) });
                    ctx.SaveChanges();
                }

                var keepRunning = true;

                while (keepRunning)
                {

                    try
                    {
                        Console.Write("\nWelcome, to search for a user please enter a search term:");

                        var searchQuery = Console.ReadLine();

                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            //Because we are concatenating raw user input with our intended query, this gives a malicious user the chance to
                            //"inject" malicious code!
                            //The interpreter, SQL server in this case doesn't know the difference between what we intended to do and what might actually
                            //happen. Hence we have introduced an injection flaw
                            var customerSearch = $"select * from Customers where FirstName LIKE '%{searchQuery}%' OR LastName LIKE '%{searchQuery}%'";
                            
                            var results = ctx.Database.SqlQuery<Customer>(customerSearch);

                            if (results.Any())
                            {
                                Console.WriteLine("The following customers have been foud:");

                                foreach (var customer in results)
                                {
                                    Console.WriteLine($"{customer.FirstName} {customer.LastName} born on : {customer.DOB.ToShortDateString()}");
                                }
                            }
                            else
                                Console.WriteLine("Sorry, there were no results for '{0}'", searchQuery);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Console.Write("\n would you like to search again? Y/N: ");

                    var userResponseMenu = Console.ReadKey();

                    if (userResponseMenu.Key == ConsoleKey.N)
                        keepRunning = false;
                }
            }

            Console.ReadKey();
        }
    }
}
