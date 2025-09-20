using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.BankAccountApp.GrainInterfaces;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Configure and start the Orleans silo.
        var host = Host.CreateDefaultBuilder(args)
            .UseOrleans(siloBuilder =>
            {
                siloBuilder.UseDashboard()
                    .UseLocalhostClustering()  // Single-node cluster for local development.
                    .AddMemoryGrainStorage("MemoryStore")  // In-memory storage for grain state.
                    .ConfigureLogging(logging => logging.AddConsole());
                    })
            .ConfigureLogging(logging => logging.AddConsole())
            .UseConsoleLifetime()
            .Build();

        await host.StartAsync();  // Start the Orleans silo.
        Console.WriteLine("Orleans Silo is running.");

        // Create an Orleans client (in the same process, we can just use Host.Services).
        var client = host.Services.GetRequiredService<IClusterClient>();
        Console.WriteLine("Orleans Client connected.");

        //Interact with the IAccountGrain.
        var accountId = Guid.NewGuid();
        var account = client.GetGrain<IAccountGrain>(accountId);

        Console.WriteLine($"Using Account Grain with ID: {accountId}");

        // Perform some sample operations:
        await account.Deposit(100m);
        Console.WriteLine("Deposited 100.00");

        await account.Withdraw(30m);
        Console.WriteLine("Withdrew 30.00");

        // Attempt an overdraft
        try
        {
            await account.Withdraw(1000m);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Expected error on overdraft: {ex.Message}");
        }

        // Check balance and history
        var balance = await account.GetBalance();
        Console.WriteLine($"Current balance: {balance:C}");

        var history = await account.GetTransactionHistory();
        Console.WriteLine("Transaction history:");
        foreach (var tx in history)
        {
            Console.WriteLine($"  [{tx.Timestamp:u}] {tx.Type} {tx.Amount:C}");
        }

        Console.WriteLine("Press Enter to terminate...");
        Console.ReadLine();

        //await host.StopAsync();
    }
}
