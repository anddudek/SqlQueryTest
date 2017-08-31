using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlQuery
{
    public class Categories
    {
        public static Category Kosmetyki = new Category("Kosmetyki", "60F25B6E-E3CB-411D-A235-429F194EB60A");

        public static Category Wplata = new Category("Wpłata", "C041805D-5CEA-4043-B349-554ABB638EA4");

        public static Category Ubrania = new Category("Ubrania", "AAA8E4DC-4CDC-4A79-9CEE-6ECE0FABEF48");

        public static Category Lunch = new Category("Lunch", "72854A02-C1EB-48FD-99A7-7347DE77B7BE");

        public static Category NieplanowaneWydatki = new Category("Nieplanowane wydatki", "C902CD38-6F34-4FF8-8F44-BC1223B9942F");

        public static Category Rozrywka = new Category("Rozrywka", "6781E6AD-71A0-4469-89B5-E529BFFB3970");

        public static Category ZakupyDoDomu = new Category("Zakupy do domu", "F08AB977-107C-4F2C-8102-F191F5CA475E");

        public static string GetCategoryName(Guid catId)
        {
            List<Category> categories = new List<Category> { Kosmetyki, Wplata, Ubrania, Lunch, NieplanowaneWydatki, Rozrywka, ZakupyDoDomu };
            foreach (var cat in categories)
            {
                if (cat.CatGuid.ToUpper().Equals(catId.ToString().ToUpper()))
                {
                    return cat.CatName;
                }
            }
            return null;
        }
    }

    public class Category
    {
        public string CatName;
        public string CatGuid;

        public Category(string _name, string _guid)
        {
            CatName = _name;
            CatGuid = _guid;
        }
    }

    public class Users
    {
        public static User Klaudia = new User("Klaudia", "DBDC7D4B-F935-41FC-80C9-5D94E4463853");
        public static User Andrzej = new User("Andrzej", "CA802E61-947A-48D7-A56B-ED588F025B0C");

        public static string GetUserName(Guid userId)
        {
            List<User> users = new List<User> { Klaudia, Andrzej };
            foreach (var u in users)
            {
                if (u.UGuid.ToUpper().Equals(userId.ToString().ToUpper()))
                {
                    return u.UName;
                }
            }
            return null;
        }
    }

    public class User
    {
        public string UName;
        public string UGuid;

        public User(string _name, string _guid)
        {
            UName = _name;
            UGuid = _guid;
        }
    }

    public class MessageRecord
    {
        public string Message { get; set; }
        public string Creator { get; set; }
    }

    public class TransactionRecord
    {
        public DateTime Date { get; set; }
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

   
}
