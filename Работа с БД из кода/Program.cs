using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Работа_с_БД_из_кода.Models;

namespace Работа_с_БД_из_кода
{
    class Program
    {
        private static string connectionString;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Ошибка: Строка подключения не найдена в файле конфигурации.");
                return;
            }

            Console.WriteLine("Тест ADO.NET");
            await TestAdoNetAsync();

            Console.WriteLine("\nТест Enity Framework ");
            await TestEfCoreAsync();
        }

        static async Task TestAdoNetAsync()
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Создание
            string insertSql = "INSERT INTO Employees (FullName, Department, DomainLogin) VALUES (@Name, @Dept, @Login);";
            using (var cmd = new SqlCommand(insertSql, connection))
            {
                cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value = "Сидоров Алексей Викторович";
                cmd.Parameters.Add("@Dept", System.Data.SqlDbType.NVarChar, 50).Value = "Маркетинг";
                cmd.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar, 50).Value = "sidorov_av";
                await cmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine($"Добавлен: Сидоров Алексей Викторович, Маркетинг, sidorov_av");

            // Чтение
            string selectSql = "SELECT FullName, Department FROM Employees WHERE DomainLogin = @Login;";
            using (var cmd = new SqlCommand(selectSql, connection))
            {
                cmd.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar, 50).Value = "sidorov_av";
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Console.WriteLine($"Найдено: {reader["FullName"]}, {reader["Department"]}, sidorov_av");
                }
            }

            // Изменение
            string updateSql = "UPDATE Employees SET Department = @NewDept WHERE DomainLogin = @Login;";
            using (var cmd = new SqlCommand(updateSql, connection))
            {
                cmd.Parameters.Add("@NewDept", System.Data.SqlDbType.NVarChar, 50).Value = "Продажи";
                cmd.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar, 50).Value = "sidorov_av";
                await cmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine($"Изменено: Сидоров Алексей Викторович, Продажи, sidorov_av");

            // Удаление
            string deleteSql = "DELETE FROM Employees WHERE DomainLogin = @Login;";
            using (var cmd = new SqlCommand(deleteSql, connection))
            {
                cmd.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar, 50).Value = "sidorov_av";
                await cmd.ExecuteNonQueryAsync();
            }
            Console.WriteLine($"Удален: Сидоров Алексей Викторович");
        }

        static async Task TestEfCoreAsync()
        {
            using var db = new ItInfrastructureContext();

            // Создание
            var newEmp = new Employee
            {
                FullName = "Васильев Дмитрий Андреевич",
                Department = "Логистика",
                DomainLogin = "vasilyev_da"
            };
            db.Employees.Add(newEmp);
            await db.SaveChangesAsync();
            Console.WriteLine($"Добавлен: {newEmp.FullName}, {newEmp.Department}, {newEmp.DomainLogin}");

            // Чтение
            var employeeFromDb = await db.Employees.FirstOrDefaultAsync(e => e.DomainLogin == "vasilyev_da");
            if (employeeFromDb != null)
            {
                Console.WriteLine($"Найдено: {employeeFromDb.FullName}, {employeeFromDb.Department}, {employeeFromDb.DomainLogin}");

                // Изменение
                employeeFromDb.Department = "Финансы";
                await db.SaveChangesAsync();
                Console.WriteLine($"Изменено: {employeeFromDb.FullName}, {employeeFromDb.Department}, {employeeFromDb.DomainLogin}");

                // Удаление
                db.Employees.Remove(employeeFromDb);
                await db.SaveChangesAsync();
                Console.WriteLine($"Удален: {employeeFromDb.FullName}");
            }
        }
    }
}