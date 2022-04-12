
using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BankingSystem.Data
{
    public static class DbInitializer
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BankingSystemContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BankingSystemContext>>()))
            {

                context.Database.EnsureCreated();

                // Look for any students.
                if (context.Customers.Any())
                {
                    return;   // DB has been seeded
                }
                else
                {
                    var customers = new Customer[]
                    {
                        new Customer{
                            LastName="Wonderland",
                            FirstName="Alice",
                            Birthday=DateTime.Parse("2000-01-01"),
                            Email = "wonderlandalice@example.com"
                        },
                        new Customer{
                            LastName="Thumb",
                            FirstName="Thomas",
                            Birthday=DateTime.Parse("2001-01-01"),
                            Email = "thumbthomas@example.com"
                        },
                        new Customer
                        {
                            LastName="Irons",
                            FirstName="Jake",
                            Birthday=DateTime.Parse("1998-06-18"),
                            Email="ironsj@mail.gvsu.edu"
                        }
                    };
                    foreach (Customer s in customers)
                    {
                        context.Customers.Add(s);
                    }
                    context.SaveChanges();


                    var accounts = new Account[]
                    {
                        new Account {
                             CustomerID = 1,
                             AccountDate=DateTime.Parse("2022-03-01"),
                             Name = "Wonderland",
                             Kind = Account.Kinds.checking,
                             Balance = 0.0M,
                             InterestRate = 0.0M },
                        new Account {
                             CustomerID = 1,
                             AccountDate=DateTime.Parse("2022-01-01"),
                             Name = "Discover",
                             Kind = Account.Kinds.credit,
                             Balance = 0.0M,
                             InterestRate = 0.01M },
                        new Account {
                             CustomerID = 1,
                             AccountDate=DateTime.Parse("2022-01-01"),
                             Name = "Visa",
                             Kind = Account.Kinds.debit,
                             Balance = 0.0M,
                             InterestRate = 0.01M },
                        new Account {
                             CustomerID = 1,
                             AccountDate=DateTime.Parse("2022-01-01"),
                             Name = "Wonderland",
                             Kind = Account.Kinds.savings,
                             Balance = 0.0M,
                             InterestRate = 0.01M },
                        new Account {
                             CustomerID = 2,
                             AccountDate=DateTime.Parse("2022-03-01"),
                             Name = "Thumb",
                             Kind = Account.Kinds.checking,
                             Balance = 0.0M,
                             InterestRate = 0.0M },
                        new Account {
                             CustomerID = 3,
                             AccountDate=DateTime.Parse("2022-04-12"),
                             Name = "Irons",
                             Kind = Account.Kinds.savings,
                             Balance = 500.0M,
                             InterestRate = 0.01M },
                        new Account {
                             CustomerID = 3,
                             AccountDate=DateTime.Parse("2022-04-12"),
                             Name = "Irons",
                             Kind = Account.Kinds.checking,
                             Balance = 312.80M,
                             InterestRate = 0.0M },
                        new Account {
                             CustomerID = 3,
                             AccountDate=DateTime.Parse("2022-04-12"),
                             Name = "Discover",
                             Kind = Account.Kinds.credit,
                             Balance = 0.0M,
                             InterestRate = 0.01M },
                        new Account {
                             CustomerID = 3,
                             AccountDate=DateTime.Parse("2022-04-12"),
                             Name = "Chase Visa",
                             Kind = Account.Kinds.debit,
                             Balance = 0.0M,
                             InterestRate = 0.0M },
                        new Account {
                             CustomerID = 3,
                             AccountDate=DateTime.Parse("2022-04-12"),
                             Name = "Taxes",
                             Kind = Account.Kinds.bill,
                             Balance = 0.0M,
                             InterestRate = 0.01M },
                        new Account {
                             CustomerID = 3,
                             AccountDate=DateTime.Parse("2022-04-12"),
                             Name = "Rent",
                             Kind = Account.Kinds.bill,
                             Balance = 0.0M,
                             InterestRate = 0.01M },
                    };
                    foreach (Account i in accounts)
                    {
                        context.Accounts.Add(i);
                    }
                    context.SaveChanges();


                    var transactions = new Transaction[]
                    {
                        new Transaction {
                            AccountID = 1,
                            TimeSlot=DateTime.Parse("2022-04-01"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 2,
                            TimeSlot=DateTime.Parse("2022-04-01"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                       new Transaction {
                            AccountID = 3,
                            TimeSlot=DateTime.Parse("2022-04-01"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 4,
                            TimeSlot=DateTime.Parse("2022-04-01"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 5,
                            TimeSlot=DateTime.Parse("2022-04-02"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 6,
                            TimeSlot=DateTime.Parse("2022-04-12"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 7,
                            TimeSlot=DateTime.Parse("2022-04-12"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 8,
                            TimeSlot=DateTime.Parse("2022-04-12"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 9,
                            TimeSlot=DateTime.Parse("2022-04-12"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 10,
                            TimeSlot=DateTime.Parse("2022-04-12"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                        new Transaction {
                            AccountID = 11,
                            TimeSlot=DateTime.Parse("2022-04-12"),
                            Action= Transaction.Actions.deposit,
                            Amount= 0.0M,
                            NewBalance=0.0M
                        },
                    };
                    foreach (Transaction d in transactions)
                    {
                        context.Transactions.Add(d);
                    }
                    context.SaveChanges();

                }
            }
        }
    }
}