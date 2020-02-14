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

            //var userInput = Console.ReadLine();

            //CreateDepartment(userInput);

            depart = GetAllDepartments();

            foreach(var dept in depart)
            {
                
                Console.WriteLine($"{dept.DepartmentID}: {dept.DepartmentName}");
            }

            Console.WriteLine("Do you want to update a Department?");
            string input = Console.ReadLine();
            bool may = (input.ToLower() == "yes" ? true : false);
            while (may)
            {
                Console.WriteLine("Which department do you want to update? Type the number:");
                int deptID = int.Parse(Console.ReadLine());
                Console.WriteLine("What will the new name be?");
                string newName = Console.ReadLine();
                UpdateDepartment(deptID, newName);
                Console.WriteLine("Type yes to update another:");
                may = Console.ReadLine().ToLower() == "yes" ? true : false;
            }
            depart = GetAllDepartments();

            foreach(var dept in depart)
            {
                Console.WriteLine($"{dept.DepartmentID}: {dept.DepartmentName}");
            }
        }

        static Department FindDepartment(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("ConnectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT departmentid, name FROM departments where departmentid = @ID";
            cmd.Parameters.AddWithValue("ID", ID);
            using (conn)
            {
                conn.Open();
                Department myDepartment = new Department();

                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read() == true)
                {
                    myDepartment.DepartmentID = reader.GetInt32("departmentid");
                    myDepartment.DepartmentName = reader.GetString("name");
                }
                return myDepartment;
            }
        }

        public static List<Department> GetAllDepartments()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = System.IO.File.ReadAllText("ConnectionString.txt");

            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT departmentid, name FROM departments";

            using (conn)
            {
                conn.Open();
                List<Department> allDepartments = new List<Department>();

                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read() == true)
                {
                    var currentDepartment = new Department();
                    currentDepartment.DepartmentID = reader.GetInt32("departmentid");
                    currentDepartment.DepartmentName = reader.GetString("name");
                    allDepartments.Add(currentDepartment);


                }
                return allDepartments;
            }


        }

        static void CreateDepartment(string departmentName)
        {
            var connStr = System.IO.File.ReadAllText("ConnectionString.txt");

            //If you adopt initializing the connection inside the using statement then you can't make a mistake //later when reorganizing or refactoring code and accidentally doing something that implicitly //opens a connection that isn't closed
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                // parameterized query to prevent SQL Injection
                cmd.CommandText = "INSERT INTO departments (Name) VALUES (@departmentName);";
                cmd.Parameters.AddWithValue("departmentName", departmentName);
                cmd.ExecuteNonQuery();
            }
        }

        static void DeleteDepartment(string departmentName)
        {
            var connStr = System.IO.File.ReadAllText("ConnectionString.txt");

            using(var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "DELETE FROM departments WHERE name = @departmentName;";
                cmd.Parameters.AddWithValue("departmentName", departmentName);
                cmd.ExecuteNonQuery();
            }
        }

        

        static void UpdateDepartment(int departmentID, string newName)
        {
            Department updated = FindDepartment(departmentID);
            var connStr = System.IO.File.ReadAllText("ConnectionString.txt");

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "UPDATE departments SET name = @newName WHERE departmentID = @departmentID";
                cmd.Parameters.AddWithValue("newName", newName);
                cmd.Parameters.AddWithValue("departmentID", updated.DepartmentID);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
