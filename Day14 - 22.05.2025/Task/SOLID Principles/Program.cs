
using System;
using SOLID_Principles.Interfaces;
using SOLID_Principles.Models;
using SOLID_Principles.Repositories;
using SOLID_Principles.Services;
using SOLID_Principles.UI;

namespace BookstoreManagementSystem
{
    class Program
    {
        static void Main()
        {
            IRepository bookRepository = new BookRepository();
            INotificationService e_notificationService = new EmailNotificationService();
            INotificationService s_notificationService = new SMSNotificationService();
            IInventoryService inventoryService = new BookstoreInventoryService(bookRepository);

            var UI = new ManageBooks(inventoryService, e_notificationService, s_notificationService);
            UI.Run();
        }
    }

}

/*
 * SOLID Principles
 - S - Single Responsibility Principle (SRP): A class should have one and only one reason to change, 
        meaning that a class should have only one job or responsibility.
 - O - Open/Closed Principle (OCP): Software entities (classes, modules, functions, etc.) should be open for 
        extension but closed for modification.
 - L - Liskov Substitution Principle (LSP): Objects of a superclass should be replaceable with objects of a 
        subclass without affecting the correctness of the program.
 - I - Interface Segregation Principle (ISP): No client should be forced to depend on methods it does not use. 
        Clients should be able to choose the interfaces they want to implement.
    - D - Dependency Inversion Principle (DIP): High-level modules should not depend on low-level modules.
 */

