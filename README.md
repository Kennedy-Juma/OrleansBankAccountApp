# Orleans.BankAccountApp

A simple .NET 8 console application demonstrating a bank account system using Microsoft Orleans virtual actors (grains). The app allows deposits, withdrawals, balance checks, and transaction history, all persisted in-memory.

## Features

- Orleans-based actor model for account management
- In-memory persistence (no external database required)
- Console UI for basic banking operations
- Transaction history tracking
- Orleans Dashboard integration

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 or later

## Getting Started

1. **Clone the repository**
git clone https://github.com/Kennedy-Juma/OrleansBankAccountApp.git 

2. **Restore dependencies**
dotnet restore


3. **Build the project**
dotnet build


4. **Run the application**
dotnet run