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
        const double DiscountPercentage = 0.3;
        const double MembershipPrice = 100;
        const double TaxRate = 0.08;
        public static (string Name, bool HasMembership, bool WantToPayMembership) CustomerInfo = ("", false, false);
        public static string membershipFile = "membership.txt";
        public static string customerCartFile = "customerCart.txt";
        public static string paymentInfoFile = "paymentInfo.txt";
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
                        WriteUserToFile(userName, password);
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
        public static void WriteUserToFile(string userName, string password)
        {
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
        public static void AddToCart(string item, int quantity, string qtyLabel, double price)
        {
            string[] customerCart;
            customerCart = File.ReadAllLines(customerCartFile);
            bool alreadyInCart = false;
            double totalPrice = price * quantity;

            foreach (var cartItem in customerCart)
            {
                string[] cartItemInfo = cartItem.Split(',');

                if (item.ToLower() == cartItemInfo[0].ToLower())
                {
                    alreadyInCart = true;
                    string[] qtyInfo = cartItemInfo[1].Split(qtyLabel);
                    int oldQuantity = Convert.ToInt32(qtyInfo[0]);
                    int newQuantity = oldQuantity + quantity;
                    double oldTotalPrice = Convert.ToDouble(cartItemInfo[3]);
                    double newTotalPrice = oldTotalPrice + totalPrice;

                    WriteLine($"You already have {oldQuantity}{qtyLabel} {item} in your cart. You are adding {quantity}{qtyLabel} more {item}. Your new total is {newQuantity}{qtyLabel} {item}.");
                    //find the index of the item in the cart
                    int index = Array.IndexOf(customerCart, cartItem);

                    //replace the old item with the new item
                    customerCart[index] = $"{item},{newQuantity}{qtyLabel},{price},{String.Format("{0:0.00}", newTotalPrice)}";
                }
            }

            if (!alreadyInCart)
            {
                Array.Resize(ref customerCart, customerCart.Length + 1);
                customerCart[customerCart.Length - 1] = item + "," + quantity.ToString() + qtyLabel + "," + price.ToString() + "," + String.Format("{0:0.00}", totalPrice);
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
                AddToCart(itemName, Convert.ToInt32(lb), itemQtyLbl, Convert.ToDouble(itemInfo[1]));
                WriteLine("Item added to cart!");
            }
            else
            {
                WriteLine("Sorry, we don't have it right now");
            }
        }
        public static void ShowCart()
        {
            string[] customerCart = File.ReadAllLines(customerCartFile);

            if(customerCart.Length == 0)
            {
                WriteLine("Your cart is empty!");
                return;
            }else{
                WriteLine("These are the items you have in your cart:");
            }
            foreach (string item in customerCart)
            {
                string[] itemInfo = item.Split(',');
                WriteLine(itemInfo[0] + " " + itemInfo[1] + " " + "$" + itemInfo[2]);
            }
        }
        public static void ChekoutTheItem()
        {
            ShowCart();
            string[] customerCart = File.ReadAllLines(customerCartFile);
            double sum = 0;

            for (int i = 0; i < customerCart.Length; i++)
            {
                string[] itemInfo = customerCart[i].Split(',');
                sum += Convert.ToDouble(itemInfo[2]);
            }

            if (CustomerInfo.WantToPayMembership)
            {
                sum = sum + MembershipPrice;
                WriteLine($"{MembershipPrice} $ membership price has been applied!");
            }
            if (CustomerInfo.HasMembership)
            {
                sum = sum - sum * DiscountPercentage;
                WriteLine($"Your membership {DiscountPercentage} % discount has been applied!");
            }
            sum = sum + sum * TaxRate;
            WriteLine("The total price of your products is $ " + String.Format("{0:0.00}", sum));
            MakePayment(sum);
            return;
        }
        public static void MakePayment(double sum)
        {
            Write("Do you want to pay with cash or card? ");
            string paymentMethod = ReadLine();


            if (paymentMethod.ToLower() == "cash")
            {
                Write("How much money do you have? ");
                double cash = Convert.ToDouble(ReadLine());
                if (cash >= sum)
                {
                    WriteLine("Thank you for your purchase!");
                    WriteLine("Your change is $" + String.Format("{0:0.00}", cash - sum));
                    WriteLine("Thank you for shopping!");
                    WriteLine("Have a nice day!");
                    PaymentToFile(sum, paymentMethod);
                    File.WriteAllText(customerCartFile, "");
                    CustomerMenu();
                }
                else
                {
                    WriteLine("Sorry, you don't have enough money!");
                    ChekoutTheItem();
                }
            }
            else if (paymentMethod.ToLower() == "card")
            {
                Write("Please enter your card number: ");
                string cardNumber = ReadLine();
                Write("Please enter your card pin: ");
                string cardPin = ReadLine();
                WriteLine("Thank you for your purchase!");
                WriteLine("Thank you for shopping!");
                WriteLine("Have a nice day!");
                 PaymentToFile(sum, paymentMethod);
                File.WriteAllText(customerCartFile, "");
                CustomerMenu();
            }
            else
            {
                WriteLine("Sorry, I can't recognize what you said! Please, say 'cash' or 'card'");
                ChekoutTheItem();
            }
        }
        public static void PaymentToFile(double TotalPrice, string PaymentType)
        {
            string[] paymentInfo = { CustomerInfo.Name, TotalPrice.ToString(), PaymentType };
            string[] paymentInformation = File.ReadAllLines(paymentInfoFile);
            Array.Resize(ref paymentInformation, paymentInformation.Length + 1);
            paymentInformation[paymentInformation.Length - 1] = $"{CustomerInfo.Name},{TotalPrice},{PaymentType}";
            File.WriteAllLines(paymentInfoFile, paymentInformation);
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