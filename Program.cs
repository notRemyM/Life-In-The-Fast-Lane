using Microsoft.PowerPlatform.Dataverse.Client;
using System.Diagnostics;

namespace LifeInTheFastLane
{
    class MainApplication
    {
        static void Main()
        {
            ServiceClient client = SetupHelper.GetServiceClient();
            if (client.IsReady)
            {
                bool showMenu = true;
                while (showMenu)
                {
                    showMenu = Menu(client);
                }
            }
        }

        // Change to your liking, but these can be changed in-app
        static int entityCountSingleTest = 10000;
        static int entityCountMultipleTest = 1000;
        static int configuredDegreeOfParallelism = 10;


        private static bool Menu(ServiceClient service)
        {
            Stopwatch stopwatch;
            Console.Clear();
            Console.WriteLine("Welcome to LifeInTheFastLane!");
            Console.WriteLine("\r\nEntity count for single test: " + entityCountSingleTest);
            Console.WriteLine("Entity count for multiple test: " + entityCountMultipleTest);
            Console.WriteLine("Configured degree of parallelism (for parallel operations, async): " + configuredDegreeOfParallelism);
            Console.WriteLine("\r\nChoose an option:");
            Console.WriteLine("1. Run Create, single threaded");
            Console.WriteLine("2. Run Create, parallel");
            Console.WriteLine("3. Run CreateMultiple, single threaded");
            Console.WriteLine("4. Run CreateMultiple, parallel");
            Console.WriteLine("5. Run CreateMultipleAsync");
            Console.WriteLine("6. Run all functions one-after-the-other (often inaccurate)");
            Console.WriteLine("7. Set entity count for single test");
            Console.WriteLine("8. Set entity count for multiple test");
            Console.WriteLine("9. Create bulk delete request for test entity");
            Console.WriteLine("D. Set degree of parallelism ");
            Console.WriteLine("0. Quit");
            Console.Write("\r\nSelect an option: ");


            // This is so spaghetti, I don't usually write like this but this is a demo
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Running Create, single threaded test with " + entityCountSingleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateImpl.SingleThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountSingleTest), service);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");
                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Running Create, parallel test with " + entityCountSingleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateImpl.MultiThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountSingleTest), service, configuredDegreeOfParallelism);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");
                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Running CreateMultiple, single threaded test with " + entityCountSingleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateMultipleImpl.SingleThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountSingleTest), service);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");
                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Running CreateMultiple, parallel test with " + entityCountSingleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateMultipleImpl.MultiThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountSingleTest), service, configuredDegreeOfParallelism);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");
                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Running CreateAsync test with " + entityCountSingleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    // VERY ILLEGAL only done because we're benchmarking
                    Task task = CreateMultipleAsyncImpl.CreateAsync(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountSingleTest), service, configuredDegreeOfParallelism);
                    task.Wait();
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");
                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "6":
                    Console.Clear();

                    Console.WriteLine("Running Create, single threaded test with " + entityCountMultipleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateImpl.SingleThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountMultipleTest), service);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");

                    Console.WriteLine("Running Create, parallel test with " + entityCountMultipleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateImpl.MultiThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountMultipleTest), service, configuredDegreeOfParallelism);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");

                    Console.WriteLine("Running CreateMultiple, single threaded test with " + entityCountMultipleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateMultipleImpl.SingleThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountMultipleTest), service);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");

                    Console.WriteLine("Running CreateMultiple, parallel test with " + entityCountMultipleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    CreateMultipleImpl.MultiThreadCreate(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountMultipleTest), service, configuredDegreeOfParallelism);
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");

                    Console.WriteLine("Running CreateAsync test with " + entityCountMultipleTest + " records...");
                    stopwatch = Stopwatch.StartNew();
                    Task task2 = CreateMultipleAsyncImpl.CreateAsync(SetupHelper.GetCollectionOfEntitiesForCreate(entityCountMultipleTest), service, configuredDegreeOfParallelism);
                    task2.Wait();
                    stopwatch.Stop();
                    Console.WriteLine("Writing completed after " + stopwatch.Elapsed.TotalSeconds + " seconds");

                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "7":
                    Console.Clear();
                    Console.Write("Type the amount you want to create for a single test, then press enter: \r\n");
                    if (int.TryParse(Console.ReadLine(), out int y) && y > 0) { entityCountSingleTest = y; }
                    else return true;
                    return true;
                case "8":
                    Console.Clear();
                    Console.Write("Type the amount you want to create for combined tests, then press enter: \r\n");
                    if (int.TryParse(Console.ReadLine(), out int x) && x > 0) { entityCountMultipleTest = x; }
                    else return true;
                    return true;
                case "9":
                    Console.Clear();
                    SetupHelper.CreateBulkDeleteJobForDemoEntity(service);
                    Console.Write("\nPress any key to continue....");
                    Console.ReadKey(true);
                    return true;
                case "D":
                case "d":
                    Console.Clear();
                    Console.Write("Type the amount of threads you would like to test with for parallel operations (ie CreateMultiple Parallel, CreateAsync) \r\n");
                    if (int.TryParse(Console.ReadLine(), out int z) && z > 0 && z < 101) { configuredDegreeOfParallelism = z; }
                    else return true;
                    return true;
                case "0":
                    return false;
                default:
                    return true;
            }
        }
    }
}