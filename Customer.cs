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
            string membership = ReadLine().ToLower();
            while (membership != "no" && membership != "yes")
            {
                Write("Invalid input, do you have a membership? Yes? or No? : ");
                membership = ReadLine().ToLower();
            }
            if (membership == "no")
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
                bool signInSuccess = false;
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
                try
                {
                    WriteLine("1. Show Inventory");
                    WriteLine("2. Buy Inventory");
                    WriteLine("3. Show Cart");
                    WriteLine("4. Checkout");
                    Write(" What would you like to do? ");
                    int choice = Convert.ToInt32(ReadLine());

                    switch (choice)
                    {
                        case 1:
                            ShowCustomerInventory();
                            CustomerBuyInventory();
                            break;
                        case 2:
                            CustomerBuyInventory();
                            break;
                        case 3:
                            ShowCart();
                            break;
                        case 4:
                            ChekoutTheItem();
                            break;
                        default:
                            WriteLine("Invalid choice, please try again!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid choice, please try again!");
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
            string itemQtyLbl = "";
            string[] itemInfo = { };

            foreach (var fruit in Fruits)
            {
                string[] fruitInfo = fruit.Split(',');
                if (itemName.ToLower() == fruitInfo[0].ToLower())
                {
                    itemFound = true;
                    itemQtyLbl = "lb";
                    itemInfo = fruitInfo;
                }
            }
            foreach (var vegetable in Vegetables)
            {
                string[] vegetableInfo = vegetable.Split(',');
                if (itemName.ToLower() == vegetableInfo[0].ToLower())
                {
                    itemFound = true;
                    itemQtyLbl = "lb";
                    itemInfo = vegetableInfo;
                }
            }
            foreach (var snack in Snacks)
            {
                string[] snackInfo = snack.Split(',');
                if (itemName.ToLower() == snackInfo[0].ToLower())
                {
                    itemFound = true;
                    itemQtyLbl = "pcs";
                    itemInfo = snackInfo;
                }
            }
            if (itemFound)
            {
                WriteLine($"It costs ${itemInfo[1]}, how many lb or pieces do you want to buy?");
                var lb = ReadLine();
                var totalCost = Convert.ToDouble(lb) * Convert.ToDouble(itemInfo[1]);
                WriteLine("The total cost is $" + String.Format("{0:0.00}", totalCost));
                Write("Do you want to put it in your cart? ");
                string userCart = ReadLine();
                string[] customerCart;
                var path = "customerCart.txt";
                customerCart = File.ReadAllLines(path);
                if (userCart.ToLower() == "yes")
                {
                    Array.Resize(ref customerCart, customerCart.Length + 1);
                    customerCart[customerCart.Length - 1] = itemName + "," + lb + itemQtyLbl + "," + totalCost.ToString();

                    File.WriteAllLines(path, customerCart);
                    WriteLine("Your item has been added into your cart!");
                }
                else if (userCart.ToLower() == "no")
                {
                    CustomerMenu();
                }
                else
                {
                    WriteLine("Sorry, I can't recognize what you said! Please, say 'yes' or 'no'");
                }
            }
            else
            {
                WriteLine("Sorry, we don't have it right now");
            }
        }
        public static void ShowCart()
        {
            WriteLine("These are the items you have in your cart:");
            string[] customerCart = File.ReadAllLines("customerCart.txt");
            foreach (string item in customerCart)
            {
                string[] itemInfo = item.Split(',');
                WriteLine(itemInfo[0] + " " + itemInfo[1] + " " + "$" + itemInfo[2]);
            }
        }
        public static void ChekoutTheItem()
        {
            ShowCart();
            string[] customerCart = File.ReadAllLines("customerCart.txt");
            int number = 0;
            // number = Math.Abs(number);
            double sum = 0;

            foreach (var item in customerCart)
            {
                string[] itemInfo = item.Split(',');
                double itemPrice;
                bool isDigit = double.TryParse(itemInfo[2], out itemPrice);
                // if (isDigit)
                // {
                //     sum = sum + (itemPrice);
                // }
                // while (number != 0)
                // {
                //     sum += number % 8;
                //     number /= 8;
                // }
                // Console.WriteLine(sum);
            }
            WriteLine("The total price of your products is $ " + String.Format("{0:0.00}", sum));
            WriteLine("Thank you for shopping!");
            File.WriteAllText("customerCart.txt", "");
        }
    }
}
// why price doesnt show at checkout
// when checkout, user who has membership will have 30% discount.
// if they want to buy membership, add $100 when checkout. 
// add 8% tax when checkout for everyone. 
// use list
// test
// tuples
// parallel arrays
// check if there is any whitespace
// meaningful names
// is my switch correct?