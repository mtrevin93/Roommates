using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roommates.Models;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        String nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        chores.Add(chore);
                    }
                    return chores;
                }
            }
        }
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Chore.Id, Chore.Name FROM Chore LEFT JOIN RoommateChore on Chore.Id = RoommateChore.ChoreId WHERE RoommateChore.RoommateId IS NULL";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };
                        chores.Add(chore);


                    }
                    return chores;
                }
            }
        }
        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                            SET Name = @name
                                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                    using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteReader();
                }
            }
        }
    }
}
