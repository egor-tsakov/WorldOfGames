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
    [Table(Name = "Games")]
    public class Games
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string name { get; set; }
        [Column]
        public string sourse { get; set; }
        [Column]
        public int price { get; set; }
        private static string strConnect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\учеба\Лабы\C#\Магазин игр\Магазин игр\WorldOfGames\WorldOfGames\Database.mdf;Integrated Security=True";

        public static Table<Games> GetTable()
        {
            DataContext db = new DataContext(strConnect);
            return db.GetTable<Games>();
        }
    }
}
