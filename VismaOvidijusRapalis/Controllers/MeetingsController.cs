using VismaOvidijusRapalis.Models;
using VismaOvidijusRapalis.Repositories;
using VismaOvidijusRapalis.Utils;

namespace VismaOvidijusRapalis.Controllers
{
    public class MeetingsController : IMeetingsController
    {
        private readonly IDictionary<Guid, Meeting> _meetingsDictionary;
        private readonly IMeetingsRepository _meetingsRepository;
        public MeetingsController(IMeetingsRepository meetingsRepository)
        {
            if (meetingsRepository is null)
                throw new ArgumentException(nameof(meetingsRepository));
            _meetingsRepository = meetingsRepository;
            _meetingsDictionary = _meetingsRepository.ReadAll();
        }

        public bool AddPersonToMeeting(Guid id, string personName)
        {
            if (_meetingsDictionary.ContainsKey(id) && !_meetingsDictionary[id].ParticipantsDic.ContainsKey(personName))
            {
                if (checkIfPersonAlreadyInMeeting(id, personName))
                    return false;
                else
                {
                    _meetingsDictionary[id].ParticipantsDic.Add(personName, DateTime.Today);
                    _meetingsRepository.Save(_meetingsDictionary);
                    return true;
                }
            }
            return false;
        }

        private bool checkIfPersonAlreadyInMeeting(Guid id, string personName)
        {
            DateTime startDate1 = _meetingsDictionary[id].StartDate;
            DateTime endDate1 = _meetingsDictionary[id].EndDate;
            foreach (var item in _meetingsDictionary.Where(i => i.Value.ParticipantsDic.ContainsKey(personName)))
            {
                DateTime startDate2 = item.Value.StartDate;
                DateTime endDate2 = item.Value.EndDate;
                if (IntervalUtils.DoesIntersect(startDate1, endDate1, startDate2, endDate2))
                    return true;
            }
            return false;
        }
        public IDictionary<Guid, Meeting> AllMeetings()
        {
            return _meetingsDictionary;
        }

        public void CreateMeeting(Meeting meeting)
        {
            _meetingsDictionary.Add(meeting.Id, meeting);
            _meetingsRepository.Save(_meetingsDictionary);
        }

        public bool DeleteMeeting(Guid id, string user)
        {
            if (_meetingsDictionary.ContainsKey(id) && _meetingsDictionary[id].ResponsiblePerson.Equals(user))
            {
                _meetingsDictionary.Remove(id);
                _meetingsRepository.Save(_meetingsDictionary);
                return true;
            }
            return false;
        }

        public IDictionary<Guid, Meeting> FilterByAttendeesCount(int number)
        {
            return _meetingsDictionary
               .Where(i => i.Value.ParticipantsDic.Count() > number)
               .ToDictionary(t => t.Key, t => t.Value);
        }

        public IDictionary<Guid, Meeting> FilterByCategory(Category category)
        {
            return _meetingsDictionary
                .Where(i => i.Value.Category.Equals(category))
                .ToDictionary(t => t.Key, t => t.Value);
        }

        public IDictionary<Guid, Meeting> FilterByDates(DateTime start, DateTime end)
        {
            return _meetingsDictionary
              .Where(i => i.Value.StartDate >= start && i.Value.EndDate <= end)
              .ToDictionary(t => t.Key, t => t.Value);
        }

        public IDictionary<Guid, Meeting> FilterByDates(DateTime start)
        {
            return _meetingsDictionary
               .Where(i => i.Value.StartDate >= start)
               .ToDictionary(t => t.Key, t => t.Value);
        }

        public IDictionary<Guid, Meeting> FilterByDescription(string description)
        {
            return _meetingsDictionary
                .Where(i => i.Value.Description.Contains(description))
                .ToDictionary(t => t.Key, t => t.Value);
        }

        public IDictionary<Guid, Meeting> FilterByResponsiblePerson(string person)
        {
            return _meetingsDictionary
                .Where(i => i.Value.ResponsiblePerson.Equals(person))
                .ToDictionary(t => t.Key, t => t.Value);
        }

        public IDictionary<Guid, Meeting> FilterByType(TypeValue type)
        {
            return _meetingsDictionary
               .Where(i => i.Value.Type.Equals(type))
               .ToDictionary(t => t.Key, t => t.Value);
        }

        public bool RemovePersonFromMeeting(Guid id, string personName)
        {
            if (_meetingsDictionary.ContainsKey(id) &&
                _meetingsDictionary[id].ParticipantsDic.ContainsKey(personName) &&
                !_meetingsDictionary[id].ResponsiblePerson.Equals(personName))
            {
                _meetingsDictionary[id].ParticipantsDic.Remove(personName);
                _meetingsRepository.Save(_meetingsDictionary);
                return true;
            }
            return false;
        }
    }
}
