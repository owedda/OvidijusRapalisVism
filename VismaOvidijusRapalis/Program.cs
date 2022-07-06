using VismaOvidijusRapalis.Controllers;
using VismaOvidijusRapalis.Repositories;
using VismaOvidijusRapalis.Views;

namespace VismaOvidijusRapalis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "Meetings.json";
            ConsoleDisplay consoleNavi = new ConsoleDisplay
                (new MeetingsController
                (new MeetingsRepository(fileName)));
            consoleNavi.InOutMeetings();
        }
    }
}