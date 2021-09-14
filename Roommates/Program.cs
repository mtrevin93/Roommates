using System;
using System.Collections.Generic;
using System.Linq;
using Roommates.Repositories;
using Roommates.Models;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"{roommate.Id} - {roommate.FirstName} {roommate.LastName} Rent Portion - {roommate.RentPortion} Living In - {roommate.Room.Name} ");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a room"):
                        List<Room> deleteRoomOptions = roomRepo.GetAll();
                        foreach (Room r in deleteRoomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to delete? ");
                        int deleteRoomId = int.Parse(Console.ReadLine());

                        roomRepo.Delete(deleteRoomId);

                        Console.WriteLine("Room has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    // Do stuff
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show chore count"):
                        List<ChoreCount> choreCounts = choreRepo.GetChoreCount();
                        foreach(ChoreCount c in choreCounts)
                        {
                            Console.WriteLine($"{c.Roommate} has {c.NumberOfChores} chores assigned.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a chore"):
                        List<Chore> deleteChoreOptions = choreRepo.GetAll();
                        foreach (Chore c in deleteChoreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to delete? ");
                        int deleteChoreId = int.Parse(Console.ReadLine());

                        choreRepo.Delete(deleteChoreId);

                        Console.WriteLine("Chore has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign a chore to a roommate"):
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"Chore # {c.Id}: {c.Name}");
                        }
                        Console.WriteLine("Please enter the number of a chore to assign it.");
                        int choreId = int.Parse(Console.ReadLine());
                        Chore assignedChore = allChores.FirstOrDefault(c => c.Id == choreId);
                        List<Roommate> allRoommates = roommateRepo.GetAllRoommates();
                        foreach (Roommate r in allRoommates)
                        {
                            Console.WriteLine($"Roommate # {r.Id}: {r.FirstName} {r.LastName}");
                        }
                        Console.WriteLine($"Please enter the number of a roommate to assign {assignedChore.Name}");
                        int assignedRoommateId = int.Parse(Console.ReadLine());
                        Roommate assignedRoommate = allRoommates.FirstOrDefault(r => r.Id == assignedRoommateId);
                        roommateRepo.AssignChore(assignedChore, assignedRoommate);
                        Console.WriteLine($"{assignedRoommate.FirstName} {assignedRoommate.LastName} has been assigned: {assignedChore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Reassign chore"):
                        List<Chore> assignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in assignedChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Select a chore to reassign");
                        int assignedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedAssignedChore = assignedChores.FirstOrDefault(c => c.Id == assignedChoreId);
                        Console.WriteLine($"This chore is currently assigned to {selectedAssignedChore.Assignee}. Who would you like to assign it to?");
                        List<Roommate> assignees = roommateRepo.GetAllRoommates();
                        foreach (Roommate r in assignees)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName}");
                        }
                        int newRoommateId = int.Parse(Console.ReadLine());
                        choreRepo.ReassignChore(selectedAssignedChore.Id, assignees.FirstOrDefault(c => c.Id == assignedChoreId).Id,
                            newRoommateId);
                        Console.WriteLine($"{assignees.FirstOrDefault(r => r.Id == newRoommateId).FirstName} has been assigned {selectedAssignedChore.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                          runProgram = false;
                          break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Search for roommate",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Show chore count",
                "Update a chore",
                "Delete a chore",
                "Show unassigned chores",
                "Assign a chore to a roommate",
                "Reassign chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}