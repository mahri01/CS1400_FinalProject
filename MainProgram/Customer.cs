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
        static int EncryptionKey=1234567890;
        public static void CustomerMembership()
        {
            WriteLine("\nWelcome to the Customer Profile!");
            Write("\nDo you have a membership? Yes? or No? : ");
            string membership = ReadLine().ToLower();
            while (membership != "no" && membership != "yes")
            {
                Write("\nInvalid input, do you have a membership? Yes? or No? : ");
                membership = ReadLine().ToLower();
            }
            if (membership == "no")
            {
                Write("\nIf you have a membership, you will have 30% discount at the end! Do you want to buy a membership for $100: ");
                string buyMembership = ReadLine();
                if (buyMembership.ToLower() == "no")
                {
                    CustomerInfo = ("", false, false);
                    CustomerMenu();
                }
                else if (buyMembership.ToLower() == "yes")
                {
                    WriteLine("\nPlease sign up for a new membership");
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
                        WriteLine("\nThis username already exists, please try again");
                        CustomerMembership();
                    }

                    WriteLine("\nSuccessfully signed up for new membership! Please sign back in!");
                    CustomerInfo = (userName, true, true);
                    CustomerMembership();
                }
                else
                {
                    WriteLine("\nInvalid answer! Please write yes or no!");
                }
            }
            else if (membership.ToLower() == "yes")
            {
                while (true)
                {
                    WriteLine("\nPlease sign in to your account");
                    Write("Username: ");
                    var username = ReadLine();
                    Write("Password: ");
                    var password = ReadLine();
                    var signInSuccess = SignInUser(username, password);
                    if (signInSuccess == true)
                    {
                        CustomerInfo = (username, true, false);
                        WriteLine("\nSuccessfully signed in!");
                        break;
                    }
                    else
                    {
                        WriteLine("\nYour username or password are incorrect. Please try again!");
                    }
                }
                CustomerMenu();
            }
        }

        static string EncryptDecrypt(string userPassword)  
        {  
            StringBuilder inputStringBuild = new StringBuilder(userPassword);  
            StringBuilder outStringBuild = new StringBuilder(userPassword.Length);  
            char Textch;  
            for (int iCount = 0; iCount < userPassword.Length; iCount++)  
            {  
                Textch = inputStringBuild[iCount];  
                Textch = (char)(Textch ^ EncryptionKey);  
                outStringBuild.Append(Textch);  
            }  
            return outStringBuild.ToString(); 
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

            File.AppendAllText(membershipFile, $"{userName},{EncryptDecrypt(password)}" + "\n");
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
                if (userName == credentials[0] & password == EncryptDecrypt(credentials[1]))
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
                    WriteLine("\n1. Show Inventory");
                    WriteLine("2. Buy Inventory");
                    WriteLine("3. Show Cart");
                    WriteLine("4. Checkout");
                    Write(" \nWhat would you like to do? ");
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
                            WriteLine("\nInvalid choice, please try again!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    return;
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
            Write("\nWe have fruits, vegetables and snacks. What do you want to see? ");
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
                WriteLine("\nI can't recognize what you said. Please type fruits, vegetables or snacks ONLY! Thank you!");
                ShowCustomerInventory();
                return;
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

                    WriteLine($"\n You already have {oldQuantity}{qtyLabel} {item} in your cart. You are adding {quantity}{qtyLabel} more {item}. Your new total is {newQuantity}{qtyLabel} {item}.");
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
            Write("\nPlease write an item name you want to buy: ");
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
                Write($"\nIt costs ${itemInfo[1]}, how many lb or pieces do you want to buy?");
                var lb = ReadLine();
                var totalCost = Convert.ToDouble(lb) * Convert.ToDouble(itemInfo[1]);
                WriteLine("\nThe total cost is $" + String.Format("{0:0.00}", totalCost));
                AddToCart(itemName, Convert.ToInt32(lb), itemQtyLbl, Convert.ToDouble(itemInfo[1]));
                WriteLine("Item is added to your cart!");
            }
            else
            {
                WriteLine("\nSorry, we don't have it right now");
            }
        }
        public static void ShowCart()
        {
            string[] customerCart = File.ReadAllLines(customerCartFile);

            if (customerCart.Length == 0)
            {
                WriteLine("\nYour cart is empty!");
                return;
            }
            else
            {
                WriteLine("\nThese are the items you have in your cart:");
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
                WriteLine($"\n {MembershipPrice} $ membership price has been applied!");
            }

            if (CustomerInfo.HasMembership)
            {
                sum = sum - sum * DiscountPercentage;
                WriteLine("\nYour membership 30 % discount has been applied!");
            }
            sum = sum + sum * TaxRate;
            WriteLine("\nThe total price of your products is $ " + String.Format("{0:0.00}", sum));
            MakePayment(sum);
            return;
        }
        public static void MakePayment(double sum)
        {
            Write("\nDo you want to pay with cash or card? ");
            string paymentMethod = ReadLine();

            if (paymentMethod.ToLower() == "cash")
            {
                Write("How much money do you have? ");
                double cash = Convert.ToDouble(ReadLine());
                if (cash >= sum)
                {
                    WriteLine("Thank you for your purchase!");
                    WriteLine("Your change is $" + String.Format("{0:0.00}", cash - sum));
                    WriteLine("\nThank you for shopping with SaveMoney!");
                    WriteLine("Have a nice day!");
                    PaymentToFile(sum, paymentMethod);
                    File.WriteAllText(customerCartFile, "");
                    CustomerMenu();
                }
                else
                {
                    WriteLine("\nSorry, you don't have enough money!");
                    ChekoutTheItem();
                }
            }
            else if (paymentMethod.ToLower() == "card")
            {
                Write("Please enter your card number: ");
                string cardNumber = ReadLine();
                Write("Please enter your card pin: ");
                string cardPin = ReadLine();
                WriteLine("\nThank you for your purchase!");
                WriteLine("Thank you for shopping with SaveMoney!");
                WriteLine("Have a nice day!");
                PaymentToFile(sum, paymentMethod);
                File.WriteAllText(customerCartFile, "");
                CustomerMenu();
            }
            else
            {
                WriteLine("\nSorry, I can't recognize what you said! Please, say 'cash' or 'card'");
                ChekoutTheItem();
            }
        }
        public static void PaymentToFile(double TotalPrice, string PaymentType)
        {
            string[] paymentInfo = { CustomerInfo.Name, TotalPrice.ToString(), PaymentType };
            string[] paymentInformation = File.ReadAllLines(paymentInfoFile);
            Array.Resize(ref paymentInformation, paymentInformation.Length + 1);
            var customerName = CustomerInfo.Name == "" ? "Guest": CustomerInfo.Name;
            paymentInformation[paymentInformation.Length - 1] = $"{customerName},{String.Format("{0:0.00}",TotalPrice)},{PaymentType}";
            File.WriteAllLines(paymentInfoFile, paymentInformation);
        }
    }
}