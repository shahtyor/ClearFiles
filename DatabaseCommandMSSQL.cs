using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ClearFiles
{
    class DatabaseCommandMSSQL: DatabaseCommand
    {
        public ConnectionStringSettings connection;

        public DatabaseCommandMSSQL(ConnectionStringSettings _connection)
        {
            connection = _connection;
        }

        public override List<FileInfo> FilesForDelete()
        {
            List<FileInfo> fordel = new List<FileInfo>();

            /*MSSQLContext context = new MSSQLContext();
            List<int> listdesc = context.DescFiles.Select(d => d.Id).ToList();
            List<int> listtitle = context.TitleFiles.Select(t => t.Id).ToList();
            List<File> flist = context.Files.Where(x => !listdesc.Contains(x.Id) && !listtitle.Contains(x.Id)).ToList();*/

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                conn.Open();

                string SqlCmd = "SELECT Id, [Path] FROM [File] " +
                    " WHERE Id not in (SELECT FileId FROM DescFile) " +
                    " AND Id not in (SELECT FileId FROM TitleFile)";
                SqlCommand listcmd = new SqlCommand(SqlCmd, conn);
                SqlDataReader reader = listcmd.ExecuteReader();

                while (reader.Read())
                {
                    int Id = (int)reader["Id"];
                    string spath = reader["Path"].ToString();
                    fordel.Add(new FileInfo() { Id = Id, Path = spath });
                }

                conn.Close();
                conn.Dispose();
            }
            return fordel;
        }

        public override string DelFileOnDB(int Id)
        {
            string result = " database: ";
            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                conn.Open();

                try
                {
                    string SqlCmd = "DELETE FROM [File] WHERE Id=@Id";
                    SqlCommand delcmd = new SqlCommand(SqlCmd, conn);
                    delcmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = Id;
                    int n = delcmd.ExecuteNonQuery();
                    result += "deleted!";
                }
                catch (Exception ex)
                {
                    result += "error - " + ex.Message;
                }

                conn.Close();
                conn.Dispose();
            }
            return result;
        }
    }
}
