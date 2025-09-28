using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.BankAccountApp.GrainInterfaces;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseOrleans(siloBuilder =>
            {
                siloBuilder.UseDashboard()
                    .UseLocalhostClustering()
                    .AddMemoryGrainStorage("MemoryStore")
                    .ConfigureLogging(logging => logging.AddConsole());
            })
            .ConfigureLogging(logging => logging.AddConsole())
            .UseConsoleLifetime()
            .Build();

        await host.StartAsync();
        Console.WriteLine("Orleans Silo is running.");

        var client = host.Services.GetRequiredService<IClusterClient>();
        Console.WriteLine("Orleans Client connected.");

        var accountId = Guid.NewGuid(); 
        var account = client.GetGrain<IAccountGrain>(accountId);
        Console.WriteLine($"Using Account Grain with ID: {accountId}");

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("==== Account Menu ====");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Check Balance");
            Console.WriteLine("4. Transaction History");
            Console.WriteLine("5. Exit");
            Console.WriteLine("======================");
            Console.Write("Select an option: ");

            if (!int.TryParse(Console.ReadLine(), out int selectedOption))
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                continue;
            }

            decimal balance;
            switch (selectedOption)
            {
                case 1:
                    Console.Write("Enter amount to deposit: ");
                    decimal.TryParse(Console.ReadLine(), out decimal depositAmount);
                    await account.Deposit(depositAmount);
                    Console.WriteLine($"Deposited {depositAmount:C}");
                    balance = await account.GetBalance();
                    Console.WriteLine($"Balance: {balance:C}");
                    break;

                case 2:
                    Console.Write("Enter amount to withdraw: ");
                    decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount);
                    await account.Withdraw(withdrawAmount);
                    Console.WriteLine($"Withdrew {withdrawAmount:C}");
                    balance = await account.GetBalance();
                    Console.WriteLine($"Balance: {balance:C}");
                    break;

                case 3:
                    balance = await account.GetBalance();
                    Console.WriteLine($"Current balance: {balance:C}");
                    break;

                case 4:
                    var history = await account.GetTransactionHistory();
                    Console.WriteLine("Transaction history:");
                    foreach (var tx in history)
                    {
                        Console.WriteLine($"  [{tx.Timestamp:u}] {tx.Type} {tx.Amount:C}");
                    }
                    break;

                case 5:
                    exit = true;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Select a valid option.");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }

        // await host.StopAsync();
    }
}
