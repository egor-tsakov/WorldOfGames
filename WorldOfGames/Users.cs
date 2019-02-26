using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorldOfGames
{
    [Table(Name = "Users")]
    public class Users
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string username { get; set; }
        [Column]
        public string password { get; set; }
        [Column(CanBeNull = true)]
        public string email { get; set; }
        [Column(CanBeNull = true)]
        public string country { get; set; }
        [Column(CanBeNull = true)]
        public string birthdate { get; set; }
        [Column(CanBeNull = true)]
        public string labrary { get; set; }
        [Column(CanBeNull = true)]
        public string basket { get; set; }
        [Column(CanBeNull = true)]
        public int money { get; set; }

        private static string strConnect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\учеба\Лабы\C#\Магазин игр\Магазин игр\WorldOfGames\WorldOfGames\Database.mdf;Integrated Security=True";

        public static Users Search(string login, string password)
        {
            DataContext db = new DataContext(strConnect);
            Table<Users> users = db.GetTable<Users>();
            foreach (var user in users)
            {
                if (user.username.ToString().Trim() == login.ToString() &&
                    user.password.ToString().Trim() == password.ToString())
                {
                    return user;
                }
            }
            return null;
        }

        public static void DeleteUser(Users deleteUsers)
        {
            DataContext db = new DataContext(strConnect);
            Table<Users> users = db.GetTable<Users>();
            foreach (var user in users)
            {
                if (user.Id == deleteUsers.Id)
                {
                    users.DeleteOnSubmit(user);
                    db.SubmitChanges();
                    return;
                }
            }
        }

        public static void Update(Users updateUser)
        {
            DataContext db = new DataContext(strConnect);
            Table<Users> users = db.GetTable<Users>();
            foreach (var user in users)
            {
                if (user.Id == updateUser.Id)
                {
                    user.username = updateUser.username;
                    user.password = updateUser.password;
                    user.email = updateUser.email;
                    user.country = updateUser.country;
                    user.birthdate = updateUser.birthdate;
                    user.labrary = updateUser.labrary;
                    user.basket = updateUser.basket;
                    user.money = updateUser.money;
                    db.SubmitChanges();
                }
            }
        }
        public static void Add(Users user)
        {
            DataContext db = new DataContext(strConnect);
            db.GetTable<Users>().InsertOnSubmit(user);
            db.SubmitChanges();
        }
    }
}
