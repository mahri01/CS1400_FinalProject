using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace Customer
{
    public class CustomerProfile
    {
        //Global variables we want to use
        static string[] Fruits = { };
        static string[] Vegetables = { };
        static string[] Snacks = { };

        public static void CustomerMembership()
        {
            WriteLine("\n Welcome to the Customer Profile!");
            Write("Do you have a membership? Yes? or No? : ");
            string membership = ReadLine();
            if (membership.ToLower() == "no")
            {
                Write("If you have a membership, you will have 30% discount at the end! Do you want to buy a membership for $100: ");
                string buyMembership = ReadLine();
                if (buyMembership.ToLower() == "no")
                {
                    CustomerMenu();
                }
                else
                {
                    WriteLine("Please sign up a new account");
                    Write("Username: ");
                    var response1 = ReadLine();
                    Write("Password: ");
                    var response2 = ReadLine();
                    File.AppendAllText("membership.txt", $"{response1},{response2}" + "\n");
                    WriteLine("Successfully signed up into your new account!");
                    CustomerMenu();
                }
            }
            else if (membership.ToLower() == "yes")
            {
                WriteLine("Please sign in to your account");
                Write("Username: ");
                var username = ReadLine();
                Write("Password: ");
                var password = ReadLine();
                bool signInSuccess = false; //bollean is to keep track if the credentials are true. 
                string[] Members;
                Members = File.ReadAllLines("membership.txt");
                foreach (var member in Members)
                {
                    if (member.Contains("#"))
                    {
                        continue;
                    }
                    string[] credentials = member.Split(',');
                    if (username == credentials[0] & password == credentials[1])
                    {
                        signInSuccess = true;
                    }
                }
                if (signInSuccess == true)
                {
                    WriteLine("Successfully signed in!");
                }
                else
                {
                    WriteLine("Your username or password are incorrect. Please try again!");
                }
                CustomerMenu();
            }
        }
        public static void CustomerMenu()
        {
            ReadAllProductsFromFile();

            while (true)
            {
                WriteLine("1. Show Inventory");
                WriteLine("2. Buy Inventory");
                Write(" What would you like to do? ");
                string choice = ReadLine();
                if (choice == "1")
                {
                    ShowCustomerInventory();
                    CustomerBuyInventory();
                }
                else if (choice == "2")
                {
                    CustomerBuyInventory();
                }
                else
                {
                    WriteLine("Invalid choice, please try again!");
                }
            }
        }

        public static void ReadAllProductsFromFile()
        {
            Fruits = File.ReadAllLines("inventory_Fruits.txt");
            Vegetables = File.ReadAllLines("inventory_Vegetables.txt");
            Snacks = File.ReadAllLines("inventory_Snacks.txt");
        }
        public static void ShowCustomerInventory()
        {
            Write("We have fruits, vegetables and snacks. What do you want to see? ");
            string inventoryType = ReadLine();
            string[] Items = { };

            if (inventoryType.ToLower() == "fruits")
            {
                Items = Fruits;
            }
            else if (inventoryType.ToLower() == "vegetables")
            {
                Items = Vegetables;
            }
            else if (inventoryType.ToLower() == "snacks")
            {
                Items = Snacks;
            }
            else
            {
                WriteLine("I can't recognize what you said. Please type fruits, vegetables or snacks ONLY! Thank you!");
            }

            int indexNum = 1;
            foreach (var item in Items)
            {
                WriteLine($"{indexNum} {item}");
                indexNum++;
            }
        }
        public static void CustomerBuyInventory()
        {
            Write("What do you want to buy: ");
            string itemName = ReadLine();
            bool itemFound = false;
            string[] itemInfo = { };

            foreach (var fruit in Fruits)
            {
                string[] fruitInfo = fruit.Split(',');
                if (itemName.ToLower() == fruitInfo[0].ToLower())
                {
                    itemFound = true;
                    itemInfo = fruitInfo;
                }
            }
            foreach (var vegetable in Vegetables)
            {
                string[] vegetableInfo = vegetable.Split(',');
                if (itemName.ToLower() == vegetableInfo[0].ToLower())
                {
                    itemFound = true;
                    itemInfo = vegetableInfo;
                }
            }
            foreach (var snack in Snacks)
            {
                string[] snackInfo = snack.Split(',');
                if (itemName.ToLower() == snackInfo[0].ToLower())
                {
                    itemFound = true;
                    itemInfo = snackInfo;
                }
            }
            if (itemFound)
            {
                WriteLine($"This fruit  costs ${itemInfo[1]}, how many lb do you want to buy?");
                var lb = ReadLine();
                var totalCost = Convert.ToDouble(lb) * Convert.ToDouble(itemInfo[1]);
                WriteLine("The total cost is $" + String.Format("{0:0.00}", totalCost));
            }
            else
            {
                WriteLine("Sorry, we don't have it right now");
            }
        }
    }
}
// ask to put in to the cart. 3. show cart 4. checkout. 

// try
// {
//     int indexResp = Int32.Parse(indexNum);
//     Console.WriteLine(indexResp);
// }
// catch (FormatException)
// {
//     Console.WriteLine($"Sorry, I can't recognize '{indexNum}'. Please write only an index number of an item.");
// }
//test
// debugging