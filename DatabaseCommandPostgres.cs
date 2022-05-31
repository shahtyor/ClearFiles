using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ClearFiles
{
    class DatabaseCommandPostgres: DatabaseCommand
    {
        public ConnectionStringSettings connection;

        public DatabaseCommandPostgres(ConnectionStringSettings _connection)
        {
            connection = _connection;
        }

        public override List<FileInfo> FilesForDelete()
        {
            List<FileInfo> fordel = new List<FileInfo>();

            using (NpgsqlConnection conn = new NpgsqlConnection(connection.ConnectionString))
            {
                conn.Open();

                string SqlCmd = "SELECT \"Id\", \"Path\" FROM \"File\" " +
                    " WHERE \"Id\" not in (SELECT \"FileId\" FROM \"DescFile\") " +
                    " AND \"Id\" not in (SELECT \"FileId\" FROM \"TitleFile\")";
                NpgsqlCommand listcmd = new NpgsqlCommand(SqlCmd, conn);
                NpgsqlDataReader reader = listcmd.ExecuteReader();

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
            using (NpgsqlConnection conn = new NpgsqlConnection(connection.ConnectionString))
            {
                conn.Open();

                try
                {
                    string SqlCmd = "DELETE FROM \"File\" WHERE \"Id\"=@Id";
                    NpgsqlCommand delcmd = new NpgsqlCommand(SqlCmd, conn);
                    delcmd.Parameters.Add("@Id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Id;
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
