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
        public List<Chore> GetAssignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT Chore.Id, Chore.Name, r.FirstName, r.LastName, r.Id
                        FROM Chore
                        JOIN RoommateChore ON Chore.Id = RoommateChore.ChoreId
                        JOIN Roommate r on RoommateChore.RoommateId = r.Id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        int firstNamePosition = reader.GetOrdinal("FirstName");
                        string firstName = reader.GetString(firstNamePosition);

                        int lastNamePosition = reader.GetOrdinal("LastName");
                        string lastName = reader.GetString(lastNamePosition);

                        int roommateIdPosition = reader.GetOrdinal("r.Id");
                        int roommateId = reader.GetInt32(roommateIdPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue,
                            Assignee = new Roommate
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Id = roommateId
                            }
                        };
                        chores.Add(chore);
                    }
                    return chores;
                }
            }
        }
        public void ReassignChore(int c, int or, int nr)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                    cmd.CommandText = @"DELETE FROM RommateChore
                        WHERE RoommateId = @oldRoommateId;
                        INSERT INTO RoommateChore (ChoreId, RoommateId)
                        Values (@choreId, @newRoommateId)";
                    cmd.Parameters.AddWithValue("@oldRoommateId", or);
                    cmd.Parameters.AddWithValue("@newRoommateId", nr);
                    cmd.Parameters.AddWithValue("@choreId", c);
                    cmd.ExecuteNonQuery();
                    }
            }
        }
        public List<ChoreCount> GetChoreCount()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT r.FirstName, r.LastName, COUNT(c.Id) as ChoreCount
                                        FROM Roommate r
                                        JOIN RoommateChore rmc on r.Id = rmc.RoommateId
                                        JOIN Chore c on rmc.ChoreId = c.Id
                                        GROUP BY r.FirstName, r.LastName";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<ChoreCount> choreCounts = new List<ChoreCount>();
                    while (reader.Read())
                    {
                        int choreCountColumnPosition = reader.GetOrdinal("ChoreCount");
                        int choreCountValue = reader.GetInt32(choreCountColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        ChoreCount choreCount = new ChoreCount
                        {
                            Roommate = firstNameValue + " " + lastNameValue,
                            NumberOfChores = choreCountValue
                        };
                        choreCounts.Add(choreCount);
                    }
                    return choreCounts;
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
