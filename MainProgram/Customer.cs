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
        const double discountPercentage = 0.3;
        const double membershipPrice = 100;
        const double taxRate = 0.08;
        static (string Name, bool HasMembership, bool WantToPayMembership) CustomerInfo = ("", false, false);
        public static string membershipFile = "membership.txt";
        public static string customerCartFile = "customerCart.txt";
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
                    CustomerInfo = ("", false, false);
                    CustomerMenu();
                }
                else if (buyMembership.ToLower() == "yes")
                {
                    WriteLine("Please sign up for a new membership");
                    Write("Username: ");
                    string userName = ReadLine();
                    Write("Password: ");
                    string password = ReadLine();

                    try
                    {
                        WriteUserTheFile(userName, password);
                    }
                    catch (System.Exception)
                    {
                        WriteLine("This username already exists, please try again");
                        CustomerMembership();
                    }

                    WriteLine("Successfully signed up for new membership! Please sign back in!");
                    CustomerInfo = (userName, true, true);
                    CustomerMembership();
                }
                else
                {
                    WriteLine("Invalid answer! Please write yes or no!");
                }
            }
            else if (membership.ToLower() == "yes")
            {
                while (true)
                {
                    WriteLine("Please sign in to your account");
                    Write("Username: ");
                    var username = ReadLine();
                    Write("Password: ");
                    var password = ReadLine();
                    var signInSuccess = SignInUser(username, password);
                    if (signInSuccess == true)
                    {
                        CustomerInfo = (username, true, false);
                        WriteLine("Successfully signed in!");
                        break;
                    }
                    else
                    {
                        WriteLine("Your username or password are incorrect. Please try again!");
                    }
                }
                CustomerMenu();
            }
        }
        public static void WriteUserTheFile(string userName, string password)
        {
            //before writing, check if the user already exists
            var users = File.ReadAllLines(membershipFile);
            foreach (var user in users)
            {
                if (user.Contains("#"))
                {
                    continue;
                }
                var credentials = user.Split(',');
                if (userName == credentials[0])
                {
                    throw new Exception("User already exists");
                }
            }

            File.AppendAllText(membershipFile, $"{userName},{password}" + "\n");
        }
        public static bool SignInUser(string userName, string password)
        {
            bool signInSuccess = false;
            string[] Members;
            Members = File.ReadAllLines(membershipFile);
            foreach (var member in Members)
            {
                if (member.Contains("#"))
                {
                    continue;
                }
                string[] credentials = member.Split(',');
                if (userName == credentials[0] & password == credentials[1])
                {
                    signInSuccess = true;
                }
            }
            return signInSuccess;
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
                            return;
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
        // can I test this method? I created this method to test the method below.
        public static void AddToCart(string item, int quantity, string qtyLabel, double price)
        {
            string[] customerCart;
            customerCart = File.ReadAllLines(customerCartFile);
            bool alreadyInCart = false;
            double totalPrice = price * quantity;

            //check if the item is already in the cart
            foreach (var cartItem in customerCart)
            {
                string[] cartItemInfo = cartItem.Split(',');
                if (item == cartItemInfo[0])
                {
                    alreadyInCart = true;
                    
                    //item quatity looks like 2lb, so we need to split it to get the number
                    string[] qtyInfo = cartItemInfo[1].Split(qtyLabel);
                    int oldQuantity = Convert.ToInt32(qtyInfo[0]);
                    int newQuantity = oldQuantity + quantity;
                    double oldTotalPrice = Convert.ToDouble(cartItemInfo[2]);
                    double newTotalPrice = oldTotalPrice + totalPrice;

                    //find the index of the item in the cart
                    int index = Array.IndexOf(customerCart, cartItem);

                    //replace the old item with the new item
                    customerCart[index] = $"{item},{newQuantity}{qtyLabel},{price},{newTotalPrice}";
                    return;
                }
            }

            if (!alreadyInCart)
            {
                Array.Resize(ref customerCart, customerCart.Length + 1);
                customerCart[customerCart.Length - 1] = item + "," + quantity.ToString() + qtyLabel + "," + price.ToString() + "," + totalPrice.ToString();
            }

            File.WriteAllLines(customerCartFile, customerCart);
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
               
                if (userCart.ToLower() == "yes")
                {
                    
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
            //write a pseudocode for this method
            //1. ask the user what they want to buy
            //2. check if the item is in the inventory
            //3. if it is, ask how many lb or pieces they want to buy
            //4. calculate the total cost
            //5. ask the user if they want to put it in their cart
            //6. if yes, add the item to the cart
            //7. if no, go back to the main menu


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
        // test
        public static void ChekoutTheItem()
        {
            ShowCart();
            string[] customerCart = File.ReadAllLines("customerCart.txt");
            double sum = 0;

            for (int i = 0; i < customerCart.Length; i++)
            {
                string[] itemInfo = customerCart[i].Split(',');
                sum += Convert.ToDouble(itemInfo[2]);
            }

            if (CustomerInfo.WantToPayMembership)
            {
                sum = sum + membershipPrice;
                WriteLine($"{membershipPrice} $ membership price has been applied!");
            }
            // test if discount is applied

            if (CustomerInfo.HasMembership)
            {
                sum = sum - sum * discountPercentage;
                WriteLine($"Your membership {discountPercentage} % discount has been applied!");
            }
            sum = sum + sum * taxRate;
            WriteLine("The total price of your products is $ " + String.Format("{0:0.00}", sum));
            WriteLine("Thank you for shopping!");
            File.WriteAllText("customerCart.txt", "");
            return;

            // pseduocode for this method

            //1. show the items in the cart
            //2. calculate the total price
            //3. apply membership discount if the customer has a membership
            //4. apply membership price if the customer wants to buy a membership
            //5. apply tax
            //6. show the total price
            //7. clear the cart
            //8. return to the main menu
        }
    }
}
/*
    {"label": "Tests: At least four of your methods should each have at least two corresponding tests (3 points each, include a screenshot and summary of what that test accomplishes)", "points": 24},
    {"label": "Implementation: Completed a project of size and scope roughly 4-5 times the size of a 1405 lab assignment", "points": 90},
    {"label": "Implementation: Meaningful identifier names", "points": 10},
    {"label": "Implementation: Good vertical whitespace (blank lines between methods and between logically related chunks of code)", "points": 10},
    {"label": "Implementation: Good horizontal whitespace (space around assignments, between parameters, etc.) (just pressing Alt+Shift+F should take care of this for you)", "points": 10},
    {"label": "Concepts (pick any 10): List (or other collections) (include a screenshot and commentary of how/why you used that)", "points": 10},
    {"label": "Concepts (pick any 10): parallel arrays or lists (include a screenshot and commentary of how/why you used that)", "points": 10},
    */