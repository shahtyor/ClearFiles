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

            Program p = new Program();
            Console.WriteLine(log);
            p.SendEmail(log);
            Console.ReadLine();
        }

        static string DeleteFiles()
        {
            string log = "Delete log:";
            ConnectionStringSettingsCollection collection = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings c in collection)
            {
                if (c.Name != "LocalSqlServer")
                {
                    log += "\r\n\r\n" + c.ProviderName + ":";
                    List<FileInfo> list = GetFilesForOneDatabase(c);
                    if (list.Count == 0) log += " files not found!";
                    else log += DelFilesForOneDatabase(list, c);
                }
            }
            return log;
        }

        static List<FileInfo> GetFilesForOneDatabase(ConnectionStringSettings connection)
        {
            List<FileInfo> result = new List<FileInfo>();
            if (connection.ProviderName == "System.Data.SqlClient")
            {
                DatabaseCommandMSSQL dbconn1 = new DatabaseCommandMSSQL(connection);
                result = dbconn1.FilesForDelete();
            }
            else if (connection.ProviderName == "Postgresql")
            {
                DatabaseCommandPostgres dbcomm2 = new DatabaseCommandPostgres(connection);
                result = dbcomm2.FilesForDelete();
            }
            return result;
        }

        static string DelFilesForOneDatabase(List<FileInfo> list, ConnectionStringSettings connection)
        {
            string log = "";
            foreach (FileInfo finfo in list)
            {
                log += "\r\n" + finfo.Path.Trim() + " - ";
                if (connection.ProviderName == "System.Data.SqlClient")
                {
                    DatabaseCommandMSSQL dbconn1 = new DatabaseCommandMSSQL(connection);
                    log += dbconn1.DelFileOnDB(finfo.Id);
                }
                else if (connection.ProviderName == "Postgresql")
                {
                    DatabaseCommandPostgres dbcomm2 = new DatabaseCommandPostgres(connection);
                    log += dbcomm2.DelFileOnDB(finfo.Id);
                }

                log += DelFileOnDisk(finfo.Path);
            }
            return log;
        }

        static string DelFileOnDisk(string fname)
        {
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
            catch (IOException ex)
            {
                result += "error - " + ex.Message;
            }
            return result;
        }

        private void SendEmail(string message)
        {
            MailAddress from = new MailAddress("delfiles12345@mail.ru");
            MailAddress to = new MailAddress(ConfigurationManager.AppSettings["email"]);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Лог очистки файлов";
            m.Body = message;
            m.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 25);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("delfiles12345@mail.ru", "u7jncRm2rXyGVzTTPCes");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

    }
}
