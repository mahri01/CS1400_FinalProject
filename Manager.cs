using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace Manager
{
    public class ManagerProfile
    {
        public static void ManagerMenu()
        {
            WriteLine("\n Welcome to the Manager Profile!");
            while (true)
            {
                WriteLine("1. Show Inventory");
                WriteLine("2. Add Inventory");
                Write(" What would you like to do? ");
                string choice = ReadLine();
                if (choice == "1")
                {
                    ShowInventory();
                }
                else if (choice == "2")
                {
                    ManagerAddInventory();
                }
                else
                {
                    WriteLine("Invalid choice, please try again!");
                }
            }
        }
        public static void ShowInventory()
        {
            Write("We have fruits, vegetables and snacks. What do you want to see? ");
            string inventoryType = ReadLine();
            while (true)
            {
                if (inventoryType.ToLower() == "fruits")
                {
                    string FruitStrings = File.ReadAllText("inventory_Fruits.txt");
                    WriteLine(FruitStrings);
                }
                else if (inventoryType.ToLower() == "vegetables")
                {
                    string VegetablesStrings = File.ReadAllText("inventory_Vegetables.txt");
                    WriteLine(VegetablesStrings);
                }
                else if (inventoryType.ToLower() == "snacks")
                {
                    string SnackStrings = File.ReadAllText("inventory_Snacks.txt");
                    WriteLine(SnackStrings);
                }
                else
                {
                    WriteLine("I can't recognize what you said. Please type fruits, vegetables or snacks ONLY! Thank you!");
                }
            }
        }
        public static void ManagerAddInventory()
        {
            Write("Please, write an index what item you want to buy: ");

        }
    }
}

// Write("Do you have a membership? ");
//             string membership = ReadLine();
//             WriteLine("1. Show Inventory");
//             WriteLine("2. Buy Inventory");

//             Write(" \n What would you like to do? ");
//             string choice = ReadLine();

//             if (choice == "1")
//             {

//             }
//             else if (choice == "2")
//             {

//             }
//             else
//             {
//                 WriteLine("Invalid choice, please try again!");
//             }

//  var path = "inventory.txt";
//             string[] Items;

//             Write(" Do you want to see what we have? To see the list of item, please enter 'yes' : ");
//             string customerResp = ReadLine();
//             //tuple each method. 
//             // List<string> groceryNames = new List<string>();
//             // List<decimal> groceryPrice = new List<decimal>();
//             List<List<string>> groceryItemsList = new List<List<string>>();
//             // use array to seperate the list. 

//             // groceryItems.Add({"Oranges" , "fruits" , "quantity"});
//             // groceryPrices.Add("3.99");

//             // Console.WriteLine($"{groceryNames[0]}: ${groceryPrices[0]}");
//             if (customerResp.ToLower() == "yes")
//             {
//                 if (File.Exists(path))
//                 {
//                     Items = File.ReadAllLines(path);
//                     int index = 1;
//                     foreach (var item in Items)
//                     {
//                         // if(item.Contains("----")){
//                         //     continue;
//                         // }
//                         groceryItemsList.Add(Items.ToList());
//                         Console.WriteLine(index + ". " + item);
//                         index++;
//                     }
//                 }

//                 Console.WriteLine("Grocery Items");
//                 foreach (var lineItem in groceryItemsList)
//                 {
//                     foreach (var item in lineItem)
//                     {
//                         Console.WriteLine(item);
//                     }
//                 }

//                 // groceryItemsList
//             }
//             else
//             {
//                 WriteLine("Please enter only 'yes'! ");

//             }
//             WriteLine("\n");
//             Write("What do you want to buy?: ");
//             string itemsBought = ReadLine();
//             Write("How much do you want to buy?: ");
//             string itemsAmount = ReadLine();
//             Write($"In your cart, you have {itemsAmount} {itemsBought}");

//             if (customerResp == "2")
//             {
//                 Console.WriteLine("\n\n");
//                 Console.WriteLine("Enter the index of the value you want to buy: ");
//                 string indexResp = Console.ReadLine();
//                 int indexNum;
//                 while (!Int32.TryParse(indexResp, out indexNum))
//                 {
//                     Console.WriteLine("Enter the index of the value you want to buy: ");
//                     indexResp = Console.ReadLine();
//                 }
//             }
//         }
//         public static void GetInventory(List<string> groceryNames,
//                                         List<string> groceryTypes,
//                                         List<string> groceryUnits,
//                                         List<int> groceryQuantities,
//                                         List<double> groceryPrices)
//         {

//         }

//     }
// }