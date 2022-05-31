using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace ClearFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string log = DeleteFiles();

            Console.WriteLine(log);
            SendEmail(log);
            Console.WriteLine("\r\n\r\nEnd of cleaning");

            Console.ReadLine();
        }

        static string DeleteFiles()
        {
            string log = "Delete log:";
            //Получаем коллекцию строк подключений и для каждой БД запускаем поиск лишних файлов и их удаление
            ConnectionStringSettingsCollection collection = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings c in collection)
            {
                log += "\r\n\r\n" + c.ProviderName + ":";
                List<FileInfo> list = GetFilesForOneDatabase(c);
                if (list.Count == 0) log += " files not found!";
                else log += DelFilesForOneDatabase(list, c);
            }
            return log;
        }

        static List<FileInfo> GetFilesForOneDatabase(ConnectionStringSettings connection)
        {
            //Поиск лишних файлов для каждой БД
            DatabaseCommand DataCommand = GetDatabaseCommand(connection);
            List<FileInfo> result = DataCommand.FilesForDelete();
            return result;
        }

        static DatabaseCommand GetDatabaseCommand(ConnectionStringSettings connection)
        {
            //Выбор нужной логики для работы с БД
            DatabaseCommand DataCommand = new DatabaseCommand();
            if (connection.ProviderName == "System.Data.SqlClient")
            {
                DataCommand = new DatabaseCommandMSSQL(connection);
            }
            else if (connection.ProviderName == "Postgresql")
            {
                DataCommand =  new DatabaseCommandPostgres(connection);
            }
            return DataCommand;
        }

        static string DelFilesForOneDatabase(List<FileInfo> list, ConnectionStringSettings connection)
        {
            //Удаление лишних файлов по переданному списку
            string log = "";
            DatabaseCommand DataCommand = GetDatabaseCommand(connection);
            foreach (FileInfo finfo in list)
            {
                log += "\r\n" + finfo.Path.Trim() + " - ";
                //Сначала пробуем удалить файл на диске
                bool success;
                log += DelFileOnDisk(finfo.Path, out success);

                //Если файл удалось удалить, то очищаем БД
                if (success)
                {
                    log += DataCommand.DelFileOnDB(finfo.Id);
                }
            }
            return log;
        }

        static string DelFileOnDisk(string fname, out bool success)
        {
            //Удаление файла с диска
            success = true;
            string result = " disk: ";
            try
            {
                if (System.IO.File.Exists(fname))
                {
                    System.IO.File.Delete(fname);
                    result += "deleted!";
                }
                else result += "not exist!";
            }
            catch (Exception ex)
            {
                success = false;
                result += "error - " + ex.Message;
            }
            return result;
        }

        static void SendEmail(string message)
        {
            //Отправка лога на email
            MailAddress from = new MailAddress("delfiles12345@mail.ru");
            MailAddress to = new MailAddress(ConfigurationManager.AppSettings["email"]);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Лог очистки файлов";
            m.Body = message;
            m.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 25);
            smtp.Credentials = new NetworkCredential("delfiles12345@mail.ru", "u7jncRm2rXyGVzTTPCes");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(m);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\r\nНе удалось отправить сообщение - " + ex.Message);
            }
        }

    }
}
