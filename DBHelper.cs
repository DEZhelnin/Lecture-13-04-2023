using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector; //подключаем библиотеку
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace _13._04._2023_lec_dataBinding
{
    
    public class DBHelper //класс-помощник по работе с БД
    {
        private static MySqlConnection? conn = null;
        private DBHelper( // private чтобы вызывать только внутри класса 
            string host,
            int port,
            string User,
            string password ,
            string database
            ) 
        {
            var connStr = $"Server={host};port={port};database={database};User Id={User};password={password}";// строка подключения
            conn = new MySqlConnection(connStr);// создаем подключение
            conn?.Open();// открываем подключение
        }
        private static DBHelper instance = null; //та самая "сущность"
                                                 //которая будет единственной для класса
         public static DBHelper GetInstance( //мы хотим создавать DBHelper только в ед.
                                              //экземпляре
             string host = "localhost",
            int port = 0,
            string User = "root",
            string password ="",
            string database="")
        {
            if (instance == null) // если не было, то создаем
            {
                instance = new DBHelper (host, port, User, password, database);
            }
            return instance;
        }
        public BindingList<Group> GetGroups() // метод будет делать выборку из таблицы и записывать в список
        {
            BindingList<Group> groups = new BindingList<Group>();
            var queryStr = "SELECT * FROM StudentGroup";// строка запроса на выборку
            var cmd= conn?.CreateCommand();// непосредственно создаем запрос на выборку
            cmd.CommandText = queryStr;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group()
                        {
                            Num = reader.GetString(reader.GetOrdinal(nameof(Group.Num))),
                            Year = reader.GetInt32(reader.GetOrdinal(nameof(Group.Year))),
                            Spec = reader.GetString(reader.GetOrdinal(nameof(Group.Spec))),
                            Department = reader.GetString(reader.GetOrdinal(nameof(Group.Department))),
                            Level = reader.GetString(reader.GetOrdinal(nameof(Group.Level)))
                            // здесь мы передаем в reader не номер столбца как в пред лекции
                            // а имя (это другая возможная перегрузка)
                            // nameof дает строку с названием поля, указанного в качестве параметра
                        });
                    }
                }
            }
            return groups; // возвращаем список полученных групп
        }

        public void InsertNew(Group newGr)// метод на вставку данных в БД из графического интерфейса
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO `StudentGroup`(Num, Year, Spec, Department, Level)" +
                "VALUES (@num, @year, @spec, @department,@level);";// параметризованный запрос
            cmd.Parameters.Add(new MySqlParameter("@num", newGr.Num));// добавляем параметр
            cmd.Parameters.Add(new MySqlParameter("@year", newGr.Year));
            cmd.Parameters.Add(new MySqlParameter("@spec", newGr.Spec));
            cmd.Parameters.Add(new MySqlParameter("@department", newGr.Department));
            cmd.Parameters.Add(new MySqlParameter("@level", newGr.Level));
            cmd.ExecuteNonQuery();
        }
    }
    
}
