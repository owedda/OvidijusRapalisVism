using VismaOvidijusRapalis.Controllers;
using VismaOvidijusRapalis.Models;

namespace VismaOvidijusRapalis.Views
{
    public class ConsoleDisplay
    {
        private IMeetingsController allMeetings;
        private string user;
        private bool isMenuCommandsStillWorking;
        private bool isMenuFiltersStillWorking;
        public ConsoleDisplay(IMeetingsController manageMeetings)
        {
            allMeetings = manageMeetings;
            isMenuCommandsStillWorking = true;
            isMenuFiltersStillWorking = true;
        }
        public void InOutMeetings()
        {
            user = InOutGetUser();         

            while (isMenuCommandsStillWorking)
            {
                PrintCommandMenu();
                var input = Console.ReadKey();
                Console.Clear();
                switch (input.Key)
                {
                    case ConsoleKey.A:
                        InOutEnterNewMeeting();
                        break;
                    case ConsoleKey.B:
                        InOutDeleteMeeting();
                        break;
                    case ConsoleKey.C:
                        InOutAddPersonToMeeting();
                        break;
                    case ConsoleKey.D:
                        InOutRemovePersonFromMeeting();
                        break;
                    case ConsoleKey.E:
                        InOutFilter();
                        break;
                    case ConsoleKey.Q:
                        isMenuCommandsStillWorking = false;
                        break;
                    default:
                        HandleInvalidInput();
                        break;
                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        private void InOutFilter()
        {
            while (isMenuFiltersStillWorking)
            {
                PrintFilterMenu();
                var input = Console.ReadKey();
                Console.Clear();

                switch (input.Key)
                {
                    case ConsoleKey.A:
                        OutAll();
                        break;
                    case ConsoleKey.D:
                        InOutFilterByDesc();
                        break;
                    case ConsoleKey.R:
                        InOutFilterByRes();
                        break;
                    case ConsoleKey.C:
                        InOutFilterByCat();
                        break;
                    case ConsoleKey.T:
                        InOutFilterByType();
                        break;
                    case ConsoleKey.B:
                        InOutFilterByDate();
                        break;
                    case ConsoleKey.N:
                        InOutFilterByNumb();
                        break;
                    case ConsoleKey.Q:
                        isMenuFiltersStillWorking = false;
                        break;
                    default:
                        HandleInvalidInput();
                        break;
                }
            }
        }
        private string InOutGetUser()
        {
            Console.Write("User name: ");
            return Console.ReadLine();
        }
        private void InOutEnterNewMeeting()
        {
            Console.WriteLine("(A) Command to create a new meeting");
            Console.Write("Enter a meeting name: ");
            string name = Console.ReadLine();
            Console.Write("Enter a responsible person: ");
            string responsiblePers = Console.ReadLine();
            Console.Write("Enter a description: ");
            string description = Console.ReadLine();
            Console.Write("Enter a category: ");
            Category category;
            if (Enum.TryParse<Category>(Console.ReadLine(), true, out category))
            {
                Console.Write("Enter a type: ");
                TypeValue type;
                if (Enum.TryParse<TypeValue>(Console.ReadLine(), true, out type))
                {
                    Console.Write("Enter start date (e.g. 1987-10-02): ");
                    DateTime startDate;
                    if (DateTime.TryParse(Console.ReadLine(), out startDate))
                    {
                        Console.Write("Enter end date (e.g. 1987-10-02) (Start date has to be older then end date): ");
                        DateTime endDate;
                        if (DateTime.TryParse(Console.ReadLine(), out endDate))
                        {
                            if (endDate>startDate)
                            {
                                Meeting meeting = new Meeting(Guid.NewGuid(), name, responsiblePers,
                                description, category, type, startDate, endDate);
                                allMeetings.CreateMeeting(meeting);
                                Console.WriteLine("Meeting Successfuly created!");
                            }
                            else
                                HandleInvalidDate();
                        }
                        else
                            HandleInvalidDate();
                    }
                    else
                        HandleInvalidDate();
                }
                else
                    HandleInvalidType();
            }
            else
                HandleInvalidCategory();
        }
        private void InOutDeleteMeeting()
        {
            Console.WriteLine("(B) Command to delete a meeting");
            Console.Write("Enter id of meeting: ");
            string line = Console.ReadLine();
            Guid id = new Guid();
            if (Guid.TryParse(line, out id))
            {
                if (allMeetings.DeleteMeeting(id, user))
                    Console.WriteLine("operation success");
                else Console.WriteLine("operation fail, id is incorrect or person trying to delete isnt responsible for that meeting");
            }
            else HandleInvalidId();
        }

        private void InOutAddPersonToMeeting()
        {
            Console.WriteLine("(C) Command to add a person to the meeting");
            Console.Write("Write meeting id youre working: ");
            string line = Console.ReadLine();
            Guid id = new Guid();
            if (Guid.TryParse(line, out id))
            {
                Console.Write("Write person's name you want to add: ");
                string name = Console.ReadLine();
                if (allMeetings.AddPersonToMeeting(id, name))
                    Console.WriteLine("operation success");
                else Console.WriteLine("operation fail, id is incorrect or" +
                    " person intersects with other meeting or is being added twice to the same meeting");
            }
            else
                HandleInvalidId();
        }
        private void InOutRemovePersonFromMeeting()
        {
            Console.WriteLine("(D) Command to remove a person from the meeting");
            Console.Write("Write meeting id youre working: ");
            string line = Console.ReadLine();
            Guid id = new Guid();
            if (Guid.TryParse(line, out id))
            {
                Console.Write("Write person's name you want to remove: ");
                string name = Console.ReadLine();
                if (allMeetings.RemovePersonFromMeeting(id, name))
                    Console.WriteLine("operation success");
                else Console.WriteLine("operation fail, id is incorrect or " +
                    "person is responsible for the meeting, he can not be removed.");
            }
            else
                HandleInvalidId();
        }
        private void HandleInvalidInput()
        {
            Console.WriteLine("Wrong input");
        }
        private void HandleInvalidId()
        {
            Console.WriteLine("Wrong ID");
        }
        private void HandleInvalidNumber()
        {
            Console.WriteLine("Invalid number");
        }
        private void HandleInvalidCategory()
        {
            Console.WriteLine("Invalid category");
        }
        private void HandleInvalidType()
        {
            Console.WriteLine("Invalid Type");
        }
        private void HandleInvalidDate()
        {
            Console.WriteLine("Invalid Date");
        }
        private void OutAll()
        {
            Console.WriteLine("(A) list all the meetings");
            foreach (var item in allMeetings.AllMeetings().Values)
            {
                Console.WriteLine(item.ToString());
            }
        }
        private void InOutFilterByDesc()
        {
            Console.WriteLine("(D) Filter by description (if the description is “Jono .NET meetas”," +
               " searching for .NET should return this entry)");
            Console.WriteLine("Enter descripttion: ");
            string line = Console.ReadLine();
            foreach (var item in allMeetings.FilterByDescription(line).Values)
            {
                Console.WriteLine(item.ToString());
            }
        }
        private void InOutFilterByRes()
        {
            Console.WriteLine("(R) Filter by responsible person");
            Console.WriteLine("Enter responsible person: ");
            string line = Console.ReadLine();
            foreach (var item in allMeetings.FilterByResponsiblePerson(line).Values)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private void InOutFilterByCat()
        {
            Console.WriteLine("(C) Filter by category");
            Console.WriteLine("Enter category: ");
            Category cat;
            if (Enum.TryParse<Category>(Console.ReadLine(), true, out cat))
            {
                foreach (var item in allMeetings.FilterByCategory(cat).Values)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else
                HandleInvalidCategory();
        }
        private void InOutFilterByType()
        {
            Console.WriteLine("(T) Filter by type");
            Console.WriteLine("Enter type: ");
            TypeValue type;
            if (Enum.TryParse<TypeValue>(Console.ReadLine(), true, out type))
            {
                foreach (var item in allMeetings.FilterByType(type).Values)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else
                HandleInvalidType();
        }

        private void InOutFilterByDate()
        {
            Console.WriteLine("(B) Filter by dates (e.g meetings that will happen starting from" +
                 " 2022-01-01 meetings that will happen between 2022 - 01 - 01 / and 2022 - 02 - 01)");
            Console.Write("Enter start date (e.g. 1987-10-02): ");

            Console.Write("Enter start date (e.g. 1987-10-02): ");
            DateTime startDate;
            if (DateTime.TryParse(Console.ReadLine(), out startDate))
            {
                Console.Write("Enter end date (e.g. 1987-10-02) or / : ");
                string endDateValue = Console.ReadLine();

                if (endDateValue.Equals("/"))
                {
                    PrintAllDates(startDate);
                }
                else
                {
                    DateTime endDate;
                    if (DateTime.TryParse(endDateValue, out endDate) && endDate>startDate)
                    {
                        PrintAllDates(startDate, endDate);
                    }
                    else
                        HandleInvalidDate();
                }
            }
            else
                HandleInvalidDate();
        }
        private void PrintAllDates(DateTime startDate)
        {
            foreach (var item in allMeetings.FilterByDates(startDate).Values)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private void PrintAllDates(DateTime startDate, DateTime endDate)
        {
            foreach (var item in allMeetings.FilterByDates(startDate, endDate).Values)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private void InOutFilterByNumb()
        {
            Console.WriteLine("(N) Filter by the number of attendees (e.g show meetings that have" +
               " over 10 people attending)");
            Console.WriteLine("Enter number of attendees: ");
            string line = Console.ReadLine();
            int number;
            if (int.TryParse(line, out number))
            {
                foreach (var item in allMeetings.FilterByAttendeesCount(number).Values)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else
                HandleInvalidNumber();
        }

        private void PrintCommandMenu()
        {
            Console.WriteLine("Select option");
            Console.WriteLine("(A) Command to create a new meeting");
            Console.WriteLine("(B) Command to delete a meeting");
            Console.WriteLine("(C) Command to add a person to the meeting");
            Console.WriteLine("(D) Command to remove a person from the meeting");
            Console.WriteLine("(E) Command to list all the meetings");
            Console.WriteLine("(Q) Command to quit");
        }
        private void PrintFilterMenu()
        {
            Console.WriteLine("Select option");
            Console.WriteLine("(A) list all the meetings");
            Console.WriteLine("(D) Filter by description (if the description is “Jono .NET meetas”," +
                " searching for .NET should return this entry)");
            Console.WriteLine("(R) Filter by responsible person");
            Console.WriteLine("(C) Filter by category");
            Console.WriteLine("(T) Filter by type");
            Console.WriteLine("(B) Filter by dates (e.g meetings that will happen starting from" +
                " 2022-01-01 meetings that will happen between 2022 - 01 - 01 and 2022 - 02 - 01)");
            Console.WriteLine("(N) Filter by the number of attendees (e.g show meetings that have" +
                " over 10 people attending)");
            Console.WriteLine("(Q) Command to quit");
        }
    }
}
