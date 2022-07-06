using VismaOvidijusRapalis.Models;

namespace VismaOvidijusRapalis.Controllers
{
    public interface IMeetingsController
    {
        void CreateMeeting(Meeting meeting);
        bool DeleteMeeting(Guid id, string user);
        bool AddPersonToMeeting(Guid id, string personName);
        bool RemovePersonFromMeeting(Guid id, string personName);
        IDictionary<Guid, Meeting> FilterByDescription(string description);
        IDictionary<Guid, Meeting> FilterByResponsiblePerson(string person);
        IDictionary<Guid, Meeting> FilterByCategory(Category category);
        IDictionary<Guid, Meeting> FilterByType(TypeValue type);
        IDictionary<Guid, Meeting> FilterByDates(DateTime start, DateTime end);
        IDictionary<Guid, Meeting> FilterByDates(DateTime start);
        IDictionary<Guid, Meeting> FilterByAttendeesCount(int number);
        IDictionary<Guid, Meeting> AllMeetings();
    }
}