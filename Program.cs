using System;
using System.IO;
using Manager;
using Customer;
using static System.Console;

namespace MoneySave
{
    public class Program
    {
        public static void Main(string[] path)
        {
            WriteLine(" ----------Welcome to the MoneySave Store! ----------");
            Write("Are you a customer or a manager? ");
            string userResp = ReadLine();
            while (true)
            {
                if (userResp.ToLower() == "customer")
                {
                    CustomerProfile.CustomerMembership();
                }
                else if (userResp.ToLower() == "manager")
                {
                    ManagerProfile.ManagerMenu();
                }
                else
                {
                    WriteLine("I can't recognize what you said. Please type manager or customer! Thank you!");
                }
            }
        }
    }
}

/*
- Load inventory from a file, save to a file.   

- List products and quantities available.     Done!

- Allow customer to add one or more items to cart.

- Show customer items and cost breakdown.   Done!

- Calculate and add tax (8%).

- Calculate and apply discounts (10% for $100 or more).

- Track users who have membership with a file. (username, password)   Done!

- Allow user to buy membership ($100) or sign into existing one.    Done!

- Give 30% discount to those who have membership

- Allow manager to log into the system for extra options.

- Manager can buy/enter more inventory.

- Automatically calculate retail markup (30%) based on wholesale price.

- Filter inventory by type/category?
*/