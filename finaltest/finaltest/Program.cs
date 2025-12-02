using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace FinalProject
{
    public class Library
    {
        private List<LibraryItem> catalogItem = new List<LibraryItem>();
        private List<CheckOutItem> checkoutList = new List<CheckOutItem>();
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "catalog.txt");
        string checkoutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "checkout.txt");
        Boolean discountUsed = false;

        public void CurrentCatalog()
        {
            if (!File.Exists(path))
            {
                string pathContent = "301|Natural Disastors in American History|DVD|0.50\n302|The Count of Monte Cristo|Book|0.25\n303|The Three Musketeers|Audiobook|0.25";
                File.WriteAllText(path, pathContent);
            }

            try
            {
                this.catalogItem.Clear();
                string toLoad = File.ReadAllText(path);
                string[] loadLines = toLoad.Split('\n');
                foreach (string loadLine in loadLines)
                {
                    string[] parts = loadLine.Split("|");
                    int Id = int.Parse(parts[0].Trim());
                    string title = parts[1].Trim();
                    string type = parts[2].Trim();
                    double dailyLateFee = double.Parse(parts[3].Trim());

                    LibraryItem item = new LibraryItem(Id, title, type, dailyLateFee);
                    this.catalogItem.Add(item);
                }
                foreach (LibraryItem item in this.catalogItem)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public int mainMenu()
        {
            while (true)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("1. Add Items to Catalog");
                Console.WriteLine("2. View Catalog");
                Console.WriteLine("3. Remove Items from the Catalog");
                Console.WriteLine("4. Checkout an Item");
                Console.WriteLine("5. Return an Item");
                Console.WriteLine("6. View Checkout Reciept");
                Console.WriteLine("7. Save Checkout List");
                Console.WriteLine("8. Load Checkout List");
                Console.WriteLine("9. Quit");
                try
                {
                    int menuChoice = int.Parse(Console.ReadLine());
                    Console.WriteLine("-------------------------------------------");
                    if (menuChoice > 9 || menuChoice < 1)
                    {
                        Console.WriteLine("Sorry, please try again.");
                        continue;
                    }
                    return menuChoice;
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, please try again.");
                }
            }
        }


        public void AddItem()
        {
            while (true)
               {
                    Boolean IDExists = false;
                    Console.WriteLine("Press ENTER (when entry is blank) to go back.");
                    Console.Write("ID of item: ");
                    string IDInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(IDInput))
                    {
                        break;
                    }
                    if (!int.TryParse(IDInput, out int Id))
                    {
                        Console.WriteLine("Invalid ID. Please try again.");
                        continue;
                    }
                    foreach (LibraryItem item in this.catalogItem)
                    {
                        if (item.ID == Id)
                        {
                            Console.WriteLine("ID already exists in catalog.");
                            IDExists = true;
                            continue;
                        }
                    }
                    if (IDExists)
                    {
                        continue;
                    }

                    string title;
                    while (true)
                    {
                        Console.Write("Title of item: ");
                        string titleInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(titleInput))
                        {
                            Console.WriteLine("Sorry, you must enter an item.");
                            continue;
                        }
                        title = titleInput;
                        break;
                    }

                    string type;
                    while (true)
                    {
                        Console.WriteLine("Type of item: ");
                        Console.WriteLine("Available types are Book, DVD, Audiobook, and EBook.");
                        string typeInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(typeInput))
                        {
                            Console.WriteLine("Sorry, you must enter an item.");
                            continue;
                        }
                        if (typeInput != "Book" && typeInput != "DVD" && typeInput != "Audiobook" && typeInput != "EBook")
                        {
                            Console.WriteLine("Sorry, that type is not available. Please try again.");
                            continue;
                        }
                        type = typeInput;
                        break;
                    }

                    double dailyLateFee;
                    while (true)
                    {
                        Console.Write("Daily late fee of item: ");
                        string Input2 = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(Input2))
                        {
                            Console.WriteLine("Sorry, you must enter an item.");
                            continue;
                        }
                        if (!double.TryParse(Input2, out double DailyLateFee))
                        {
                            Console.WriteLine("Invalid fee amount. Please try again.");
                            continue;
                        }
                        dailyLateFee = DailyLateFee;
                        break;
                    }
                    
                    LibraryItem addedItem = new LibraryItem(Id, title, type, dailyLateFee);
                    this.catalogItem.Add(addedItem);
                    saveCatalog();
                    Console.WriteLine($"{addedItem.Title} has been added to the catalog.");
                }
        }


        public void saveCatalog()
        {
            List<string> catalogLines = new List<string>();
            foreach (LibraryItem item in this.catalogItem)
            {
                catalogLines.Add($"{item.ID}|{item.Title}|{item.Type}|{item.DailyLateFee}");
            }
            File.WriteAllText(path, string.Join("\n", catalogLines));
        }

        public void ViewCatalog()
        {
            Console.WriteLine("---CURRENT CATALOG---");
            CurrentCatalog();
            Console.WriteLine("---------------------");
            Console.Write("Enter anything to go back.");
            string leavecart = Console.ReadLine();
        }
        public void RemoveItem()
        {
            Console.WriteLine("---CURRENT CATALOG---");
            int n = 1;
            foreach (LibraryItem item in this.catalogItem)
            {
                Console.WriteLine($"{n}. {item.ToString()}");
                n++;
            }
            Console.WriteLine("------------------------");
            Console.Write("Type the number of the item you wish to delete.");
            string removeChoice = Console.ReadLine();
            if (removeChoice == "")
            {
                return;
            }
            try
            {
                int index = int.Parse(removeChoice) - 1;
                if (0 <= index && index < this.catalogItem.Count())
                {
                    LibraryItem removedItem = this.catalogItem[index];
                    this.catalogItem.RemoveAt(index);
                    saveCatalog();
                    Console.WriteLine($"{removedItem.Title} has been removed.");
                }
                else
                {
                    Console.WriteLine("Sorry, please enter a valid number.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, please enter a valid number.");
            }
        }

        public void CheckoutAnItem()
        {
            Console.WriteLine("---CURRENT CATALOG---");
            int n = 1;
            foreach (LibraryItem item in this.catalogItem)
            {
                Console.WriteLine($"{n}. {item.ToString()}");
                n++;
            }
            Console.WriteLine("------------------------");
            Console.Write("Type the number of the item you wish to checkout.");
            string checkoutChoice = Console.ReadLine();
            if (checkoutChoice == "")
            {
                return;
            }
            try
            {
                int index = int.Parse(checkoutChoice) - 1;
                if (0 <= index && index < this.catalogItem.Count())
                {
                    LibraryItem checkoutItem = this.catalogItem[index];
                    foreach (CheckOutItem existingCheckout in this.checkoutList)
                    {
                        if (existingCheckout.Item.ID == checkoutItem.ID)
                        {
                            Console.WriteLine("Sorry, this item is already checked out.");
                            return;
                        }
                    }
                    CheckOutItem checkout = new CheckOutItem(checkoutItem, 0, 0);
                    this.checkoutList.Add(checkout);
                }
                else
                {
                    Console.WriteLine("Sorry, please enter a valid number.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, please enter a valid number.");
            }
        }
        public void returnItem()
        {
            Console.WriteLine("CURRENT CHECKOUT LIST");
            int n = 1;
            foreach(CheckOutItem checkout in this.checkoutList)
            {
                Console.WriteLine($"{n}. {checkout.Item.ToString()}");
                n++;
            }
            Console.Write("Type the number of the item you wish to return.");
            string returnChoice = Console.ReadLine();
            if (returnChoice == "")
            {
                return;
            }
            try
            {
                int index = int.Parse(returnChoice) - 1;
                if (0 <= index && index < this.checkoutList.Count())
                {
                    CheckOutItem removedReturnItem = this.checkoutList[index];
                    LibraryItem returnItem = removedReturnItem.Item;
                    this.checkoutList.RemoveAt(index);
                    Console.WriteLine($"{returnItem.Title} has been returned.");
                }
                else
                {
                    Console.WriteLine("Sorry, please enter a valid number.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, please enter a valid number.");
            }
        }
        public void CheckoutReciept()
        {
            double totalFee = 0;
            Console.WriteLine("=== CHECKOUT RECIEPT ===");
            foreach (CheckOutItem item in this.checkoutList)
            {
                Console.WriteLine($"How many days did you hold {item.Item.Title}? ");
                int daysHeld = int.Parse(Console.ReadLine());
                int daysLate = daysHeld - item.DaysDue;
                item.DaysLate = daysLate;
                if (item.DaysDue > daysHeld)
                {
                    item.DaysLate = 0;
                }
                totalFee += item.LateFee();
            }
            foreach (CheckOutItem item in this.checkoutList)
            {
                Console.WriteLine(item.ToString());
            }
            totalFee = applyDiscount(totalFee);
            Console.WriteLine($"TOTAL COST: ${totalFee:F2}");
        }
        public double applyDiscount(double totalFee)
        {
            Console.Write("Enter discount code: (Or type anything to skip.) ");
            string discount = Console.ReadLine();
            if (discountUsed)
            {
                Console.WriteLine("Code already used!");
                return totalFee;
            }
            if (discount == "LIBRARIESAREC00L")
            {
                double discountedSubtotal = totalFee * 0.80;
                discountUsed = true;
                return discountedSubtotal;
            }
            else
            {
                return totalFee;
            }
        }
        public void saveCheckout()
        {
            Console.WriteLine("Save current checkout list? (Type Y if yes.)");
            string saveChoice = Console.ReadLine()?.Trim().ToUpper();
            if (saveChoice != "Y")
            {
                Console.WriteLine("Save cancelled.");
                return;
            }
            List<string> checkoutLines = new List<string>();
            foreach (CheckOutItem item in this.checkoutList)
            {
                checkoutLines.Add($"{item.Item.ID}|{item.Item.Title}|{item.Item.Type}|{item.DaysDue}|{item.Item.DailyLateFee}");
            }
            File.WriteAllText(checkoutPath, string.Join("\n", checkoutLines));
            Console.WriteLine("Checkout list saved.");
        }
        public void loadCheckout()
        {
            if (!File.Exists(checkoutPath))
            {
                Console.WriteLine("No saved checkout list can be found");
                return;
            }
            Console.WriteLine("Load stored checkout list? (Type Y if yes.)");
            string loadChoice = Console.ReadLine()?.Trim().ToUpper();
            if (loadChoice != "Y")
            {
                Console.WriteLine("Load cancelled.");
                return;
            }
            string[] loadLines = File.ReadAllLines(checkoutPath);
            this.checkoutList.Clear();
            foreach (string line in loadLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                string[] parts = line.Split('|');
                int id = int.Parse(parts[0].Trim());
                string title = parts[1].Trim();
                string type = parts[2].Trim();
                int daysdue = int.Parse(parts[3].Trim());
                double dailylatefee = double.Parse(parts[4].Trim());

                Boolean insideCatalog = false;
                foreach (LibraryItem catalogItem in this.catalogItem)
                {
                    if (catalogItem.ID == id)
                    {
                        insideCatalog = true;
                        break;
                    }
                }
                if (insideCatalog)
                {
                    LibraryItem item = new LibraryItem(id, title, type, dailylatefee);
                    CheckOutItem checkoutItem = new CheckOutItem(item, daysdue, 0);
                    this.checkoutList.Add(checkoutItem);
                }
                else
                {
                    Console.WriteLine($"{title} is no longer in the catalog, and has been removed from the loaded list.");
                }
            }
            Console.WriteLine("List loaded successfully!");
        }
        public static void Main(string[] args)
        {
            Library LibraryRun = new Library();
            LibraryRun.CurrentCatalog();
            int menuChoice = 0;
            while (menuChoice != 9)
            {
                menuChoice = LibraryRun.mainMenu();
                if (menuChoice == 1)
                {
                    LibraryRun.AddItem();
                }
                if (menuChoice == 2)
                {
                    LibraryRun.ViewCatalog();
                }
                if (menuChoice == 3)
                {
                    LibraryRun.RemoveItem();
                }
                if (menuChoice == 4)
                {
                    LibraryRun.CheckoutAnItem();
                }
                if (menuChoice == 5)
                {
                    LibraryRun.returnItem();
                }
                if (menuChoice == 6)
                {
                    LibraryRun.CheckoutReciept();
                }
                if (menuChoice == 7)
                {
                    LibraryRun.saveCheckout();
                }
                if (menuChoice == 8)
                {
                    LibraryRun.loadCheckout();
                }
            }
            Console.WriteLine("Thank you for using the program. Goodbye!");
        }
    }
}
