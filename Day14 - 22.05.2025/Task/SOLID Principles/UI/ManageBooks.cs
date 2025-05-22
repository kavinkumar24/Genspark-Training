using System;
using SOLID_Principles.Helper;
using SOLID_Principles.Interfaces;
using SOLID_Principles.Models;

namespace SOLID_Principles.UI
{
    public class ManageBooks
    {
        private readonly IInventoryService _inventoryService;
        private readonly INotificationService _notificationService;
        private readonly INotificationService _smsNotificationService;

        public ManageBooks(IInventoryService inventoryService, INotificationService e_notificationService, INotificationService s_notificationService)
        {
            _inventoryService = inventoryService;
            _notificationService = e_notificationService;
            _smsNotificationService = s_notificationService;
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("Welcome to the Bookstore Management System");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. View available books");
                Console.WriteLine("3. Purchase a book");
                Console.WriteLine("4. Exit");
                Console.Write("\nChoose an option: ");

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.Write("Invalid input. Please enter a number: ");
                }

                switch (choice)
                {
                    case 1:
                        AddBook();
                        break;

                    case 2:
                        ViewAvailableBooks();
                        break;

                    case 3:
                        PurchaseBook();
                        break;

                    case 4:
                        isRunning = false;
                        Console.WriteLine("Exiting system...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public (string title, string author, int quantity, double totalValue) GetInputDetails()
        {
            string title;
            do
            {
                Console.Write("Enter Book Title: ");
                title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                    Console.WriteLine("Title is required. Please try again.");
            } while (string.IsNullOrWhiteSpace(title));

            string author;
            do
            {
                Console.Write("Enter Author Name: ");
                author = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(author))
                    Console.WriteLine("Author name is required. Please try again.");
            } while (string.IsNullOrWhiteSpace(author));

            int quantity;
            while (true)
            {
                Console.Write("Enter Quantity: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
                    break;

                Console.WriteLine("Invalid quantity. Please enter a non-negative number.");
            }

            double price;
            while (true)
            {
                Console.Write("Enter Price: ");
                if (double.TryParse(Console.ReadLine(), out price) && price >= 0)
                    break;

                Console.WriteLine("Invalid price. Please enter a non-negative number.");
            }
            double totalValue = quantity * price;

            return (title, author, quantity, totalValue);
        }

        private void AddBook()
        {
            Console.WriteLine("\nAdding a new book:");

            var (title, author, quantity, totalValue) = GetInputDetails();

            Book newBook = new Book(title, author, quantity, totalValue);
            _inventoryService.AddBook(newBook);

            Console.WriteLine($"\n----New book '{title}' by {author} has been added successfully!----");
            _notificationService.SendNotification($"----'{title}' by {author} added to inventory with {quantity} copies.----", 
                NotifyEmails.GetAdminEmail());
            _smsNotificationService.SendNotification($"----'{title}' by {author} added to inventory with {quantity} copies.----", 
                NotifyEmails.GetAdminPhoneNumber());
        }

        private void ViewAvailableBooks()
        {
            Console.WriteLine("\nAvailable Books:");
            var availableBooks = _inventoryService.GetAvailableBooks();
            foreach (var book in availableBooks)
            {
                Console.WriteLine($"{book.Id}: {book.Title} by {book.Author}, {book.Quantity}, in stock, - Rs. {book.Price}");
            }
            if(availableBooks.Count == 0)
            {
                Console.WriteLine("----------No books available.---------");
                return;
            }
           

            _notificationService.SendNotification("User viewed available books.", NotifyEmails.GetAdminEmail());
        }

        private void PurchaseBook()
        {
            Console.WriteLine("\nPurchasing a book:");
            Console.Write("Enter Book ID: ");
            int bookId = int.Parse(Console.ReadLine());
            var book = _inventoryService.GetAvailableBooks().Find(b => b.Id == bookId);
            if (book == null)
            {
                Console.WriteLine("------------Book not found.-----------------");
                return;
            }
            Console.Write("Enter Quantity to purchase: ");
            int quantityToPurchase = int.Parse(Console.ReadLine());

            bool purchaseSuccessful = _inventoryService.PurchaseBook(bookId, quantityToPurchase);

            if (purchaseSuccessful)
            {
                Console.WriteLine("\n-----Purchase successful!-----");
                _notificationService.SendNotification($"----Purchase: Book ID {bookId}, Quantity {quantityToPurchase} ----", NotifyEmails.GetAdminEmail());
            }
            else
            {
                Console.WriteLine("\n----Not enough stock.----");
                _notificationService.SendNotification($"----Failed Purchase: Book ID {bookId}, insufficient stock.----", NotifyEmails.GetAdminEmail());
            }
        }
    }
}
