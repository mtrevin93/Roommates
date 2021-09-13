using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, MoveInDate, Roommate.Id, Room.Id AS 'RoomId', Name, MaxOccupancy " +
                                      "FROM Roommate " +
                                      "JOIN Room ON Room.Id = Roommate.RoomId " +
                                      "WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                            }
                        };
                    }
                    return roommate;
                }
            }
        }
        public List<Roommate> GetAllRoommates()
        {
            {
                using (SqlConnection conn = Connection)
                {

                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = "SELECT Roommate.Id AS 'RoommateId', FirstName, LastName, RentPortion, MoveInDate, Room.Id AS 'RoomId', Name, MaxOccupancy FROM Roommate Join Room on Room.Id = Roommate.RoomId";

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("RoommateId");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string firstNameValue = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string lastNameValue = reader.GetString(lastNameColumnPosition);

                            int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                            int rentPortionValue = reader.GetInt32(rentPortionColumnPosition);

                            int moveInDatePosition = reader.GetOrdinal("MoveInDate");
                            DateTime moveInDateValue = reader.GetDateTime(moveInDatePosition);

                            int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                            int roomIdValue = reader.GetInt32(roomIdColumnPosition);

                            int roomNameColumnPosition = reader.GetOrdinal("Name");
                            string roomNameValue = reader.GetString(roomNameColumnPosition);

                            int maxOccupancyColumnPosition = reader.GetOrdinal("MaxOccupancy");
                            int maxOccupancyValue = reader.GetInt32(maxOccupancyColumnPosition);

                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                                RentPortion = rentPortionValue,
                                MovedInDate = moveInDateValue,
                                Room = new Room
                                {
                                    Id = roomIdValue,
                                    Name = roomNameValue,
                                    MaxOccupancy = maxOccupancyValue
                                }
                            };
                            roommates.Add(roommate);
                        }
                        return roommates;
                    }
                }
            }
        }
    }
}

