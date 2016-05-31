using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgramApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AddressBook aBook = AddressBook.Instance;
            
            Logger log = new Logger(new PrintToFile());

            aBook.onUserAdd += log.Info;
            aBook.onUserAddFail += log.Eror;
            aBook.onUserListUse += log.Debug;
            aBook.onUserRemove += log.Info;
            aBook.onUserRemoveFail += log.Warning;

            aBook.AddUser("John", "Doe", new DateTime(1989, 9, 17), DateTime.Now, "Krivoy Rog", "Katkova street", "224389", "male", "JohnDoe@gmail.com");
            aBook.AddUser("Jack", "Smith", new DateTime(1998, 7, 12), new DateTime(2016, 3, 24), "Kiev", null, "981234", "male", "JackSmith@gmail.com");
            aBook.AddUser("Anna", "Fedorenko", new DateTime(1998, 1, 1), new DateTime(2016, 5, 27), "Kiev", "Dobraya str.", "4597712", "female", "");
            aBook.AddUser("Jane", "Moore", new DateTime(1990, 8, 20), new DateTime(2016, 5, 28), "Kharkiv", "Rodnikova str.", "4905040", "female", "JaneMoore@yandex.ru");
            aBook.AddUser("Cris", "Liam", new DateTime(1988, 1, 7), new DateTime(2015, 5, 29), "Lviv", "Nova str.", "308712", "male", "Cris.Liam@mail.ru");
            aBook.AddUser("Katya", "Ivanova", new DateTime(2007, 7, 23), new DateTime(2016, 5, 30), "Nikolaev", "Druzhby str.", "", "female", "Katya.Ivanova@gamil.com");
            aBook.AddUser("Ivan", "Garanenko", new DateTime(1999, 1, 14), new DateTime(2016, 4, 12), "Kiev", "Shkolnaya str.", "", "male", "Ivan.Garanenko@yandex.ru");
            aBook.AddUser("Petr", "Naydenko", new DateTime(1975, 3, 7), new DateTime(2014, 9, 26), "Kiev", "", "", "male", "");
            aBook.AddUser("Tatyana", "Remnyova", new DateTime(1965, 12, 24), new DateTime(2016, 3, 12), "Kharkiv", "Druzhby str.", "", "female", "Remnyova.Tanya@mail.ru");
            aBook.AddUser("Igor", "Besedin", new DateTime(2006, 5, 31), new DateTime(2016, 5, 12), "Lviv", "Hutorovka str.", "889012", "male", "Igor.Besedin@gmail.com");

            Console.WriteLine("1. Users with \"gmail.com\" domain:");
            aBook.FirstQuery();

            Console.WriteLine("\n2. Users that older than 18 years and live in Kiev:");
            aBook.SecondQuery();

            Console.WriteLine("\n3. Female users added in last 10 days:");
            aBook.ThirdQuery();

            Console.WriteLine("\n4. Users born in January with filled address and phone number fields:");
            aBook.FourthQuery();

            Console.WriteLine("\n5. Dictationary:");
            aBook.FifthQuery();

            Console.WriteLine("\n6. Input start and end index by enter: \nResult query from range:");
            aBook.SixthQuery(Console.ReadLine(), Console.ReadLine());

            Console.Write("\nEnter name of town: ");
            Console.WriteLine("7. Today {0} person(s) celebrating Birth Day in entered town", aBook.SeventhQuery(Console.ReadLine()));

            Console.WriteLine("\n8. Deffered execution example. Initial set :");
            string[] animals = { "cat", "elephant", "giraffe", "jaguar", "fish", "raccoon", "mouse", "rat", "horse", "bat" };
            foreach (string animal in animals)
            {
                Console.Write(animal + " ");
            }
            var query = animals.Where(x => x.Length <= 3);
                        
            animals[1] = "dog"; animals[5] = "cow";
            Console.Write("\nQuery result: ");
            foreach (string animal in query)
            {
                Console.Write(animal + " ");
            }

            Console.WriteLine("\n");
            aBook.Notifier();

            Console.ReadKey();
        }
    }
}
