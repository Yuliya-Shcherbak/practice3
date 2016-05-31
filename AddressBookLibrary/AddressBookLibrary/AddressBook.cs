using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

class User
{
    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public DateTime Birthdate { get; private set; }
    public DateTime TimeAdded { get; private set; }
    public string City { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Gender { get; private set; }
    public string Email { get; private set; }

    public User(string name, string lastname, DateTime birthDate, DateTime created, string city, string address, string phone, string gender, string email)
    {
        FirstName = name;
        LastName = lastname;
        Birthdate = birthDate;
        TimeAdded = created;
        City = city;
        Address = address;
        PhoneNumber = phone;
        Gender = gender;
        Email = email;
    }
}

static class AddressBookExtention  
{
    public static IEnumerable<User> GetUsersForSecondQuery(this List<User> userList)
    {
        foreach (var user in userList)
        {
            int age = DateTime.Today.Year - user.Birthdate.Year;
            if (user.City == "Kiev" && age > 18)
            {
                yield return user;
            }
            else if (age == 18)
            {
                if ((DateTime.Today.Month > user.Birthdate.Month) ||
                    (DateTime.Today.Month == user.Birthdate.Month && DateTime.Today.Day >= user.Birthdate.Day))
                    yield return user;
            }
        }
    }
}

public sealed class AddressBook
{
    private static readonly Lazy<AddressBook> lazy = new Lazy<AddressBook>(() => new AddressBook());

    public static AddressBook Instance { get { return lazy.Value; } }

    public delegate void UserAdded(string firstname, string lastname, string act);
    public delegate void UserRemoved(string firstname, string lastname, string act);
    public delegate void UserListUsed(string act);
    public delegate void UserRemoveFail(string firstname, string lastname);
    public delegate void UserAddFail(string firstname, string lastname);

    public event UserAdded onUserAdd;
    public event UserRemoved onUserRemove;
    public event UserListUsed onUserListUse;
    public event UserRemoveFail onUserRemoveFail;
    public event UserAddFail onUserAddFail;

    List<User> UserList = new List<User>();

    public void AddUser(string name, string lastname, DateTime birthDate, DateTime created, string city, string address, string phone, string gender, string email)
    {
        onUserListUse("add");
        try
        {
            UserList.Add(new User(name, lastname, birthDate, created, city, address, phone, gender, email));
            onUserAdd(name, lastname, "added");
        }
        catch (Exception e)
        {
            onUserAddFail(name, lastname);
        }
    }

    public void RemoveUser(string name, string lastname, DateTime bithdate)
    {
        onUserListUse("remove");
        try
        {
            if (UserList.Remove(UserList.SingleOrDefault(x => x.FirstName == name && x.LastName == lastname && x.Birthdate == bithdate)))
            {
                onUserRemove(name, lastname, "removed");
            }
            else
            {
                Exception ex = new Exception();
                throw ex;
            }
        }
        catch (Exception e)
        {
            onUserRemoveFail(name, lastname);
        }
    }

    public void FirstQuery()
    {
        int index = 0;
        var result = UserList.Where(x => x.Email.Contains("gmail.com"));
        foreach (var item in result)
        {
            Console.WriteLine(" {0}. {1} {2} - {3} ", index++, item.FirstName, item.LastName, item.Email);
        }
    }

    public void SecondQuery()
    {
        int index = 0;
        foreach (var user in UserList.GetUsersForSecondQuery())
        {
            Console.WriteLine(" {0}. {1} {2} - {3} ", index++, user.FirstName, user.LastName, user.Birthdate.ToShortDateString());
        }
    }

    public void ThirdQuery()
    {
        var result = from user in UserList
                     where user.Gender == "female" && user.TimeAdded > DateTime.Today.AddDays(-10)
                     select new
                     {
                         FirstName = user.FirstName,
                         LastName = user.LastName,
                         TimeAdded = user.TimeAdded
                     };

        int index = 0;
        foreach (var item in result)
        {
            Console.WriteLine(" {0}. {1} {2} - {3} ", index++, item.FirstName, item.LastName, item.TimeAdded.ToShortDateString());
        }
    }

    public void FourthQuery()
    {
        var result = UserList.Where(x => x.Birthdate.Month == 1 && x.Address.Length > 0 && x.PhoneNumber.Length > 0).OrderByDescending(x => x.LastName);

        int index = 0;
        foreach (var item in result)
        {
            Console.WriteLine(" {0}. {1} {2} - {3} ", index++, item.FirstName, item.LastName, item.Birthdate.ToShortDateString());
        }
    }

    public void FifthQuery()
    {
        Dictionary<string, IEnumerable<User>> result = new Dictionary<string, IEnumerable<User>>();
        var maleList = UserList.Where(x => x.Gender == "male");
        var femaleList = UserList.Where(x => x.Gender == "female");

        result.Add("man", maleList);
        result.Add("woman", femaleList);

        foreach (var pair in result)
        {
            Console.WriteLine(pair.Key + " : ");
            foreach (var user in pair.Value)
            {
                Console.WriteLine("\t" + user.FirstName + " " + user.LastName);
            }
        }
    }

    public void SixthQuery(string start, string end)
    {
        int s = 0;
        int e = 5;
        Int32.TryParse(start, out s);
        Int32.TryParse(end, out e);
        var result = UserList.Where(x => x.PhoneNumber.Length > 0).Skip(s).Take(e - s);
        int index = 0;
        foreach (var item in result)
        {
            Console.WriteLine(" {0}. {1} {2} - {3} ", index++, item.FirstName, item.LastName, item.City);
        }
    }

    public int SeventhQuery(string cityName)
    {
        var result = (from user in UserList
                      where user.City == cityName && user.Birthdate.Day == DateTime.Today.Day && user.Birthdate.Month == DateTime.Today.Month
                      select new
                      {
                          Name = user.FirstName,
                          BirthDate = user.Birthdate
                      }).Count();
        return result;
    }

    public void Notifier()
    {
        string path = "..\\..\\LastDayChecked.txt";
        DateTime dateToCheck;
        using (StreamReader streamReader = new StreamReader(path))
        {
            dateToCheck = Convert.ToDateTime(streamReader.ReadLine());
        }
        if (dateToCheck < DateTime.Today)
        {
            var query = UserList.Where(x => x.Birthdate.Day == DateTime.Today.Day && x.Birthdate.Month == DateTime.Today.Month && x.Email.Length > 0);
            foreach (var user in query)
            {
                Console.WriteLine("Greeting letter sent to {0} {1}!", user.FirstName, user.LastName);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(DateTime.Now);
            }
        }
        else Console.WriteLine("Greeting mails already sent!");
    }

    private AddressBook() { }
}

