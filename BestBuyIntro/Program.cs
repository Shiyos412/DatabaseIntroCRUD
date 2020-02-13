using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestBuyIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            var depart = GetAllDepartments();

            foreach(var dept in depart)
            {
                Console.WriteLine(dept);
            }
        }

        public static IEnumerable GetAllDepartments()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("ConnectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT name FROM departments";

            using (conn)
            {
                conn.Open();
                List<string> allDepartments = new List<string>();

                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read() == true)
                {
                    var currentDepartment = reader.GetString("name");
                    allDepartments.Add(currentDepartment);
                }
                return allDepartments;
            }


        }
    }
}
