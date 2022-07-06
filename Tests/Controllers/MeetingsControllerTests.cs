using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaOvidijusRapalis.Controllers;
using VismaOvidijusRapalis.Models;
using VismaOvidijusRapalis.Repositories;

namespace Tests.Controllers
{
    public class MeetingsControllerTests
    {
        private MeetingsController _meetingsController;


        private Meeting CreateAMeeting()
        {
            return new Meeting(Guid.NewGuid(), "Juozas", "Antanas",
                                "Paprastas meetingas", Category.CodeMonkey, TypeValue.InPerson,
                                DateTime.Parse("2000-10-10"), DateTime.Parse("2012-10-10"));
        }
        private Meeting CreateAMeeting(string name, string responsiblePers, string description, Category category,
            TypeValue type, DateTime startDate, DateTime endDate)
        {
            return new Meeting(Guid.NewGuid(), name, responsiblePers,
                description, category, type, startDate, endDate);
        }

        [Test]
        public void CreateMeetingTest()
        {
            // Arrange
            Meeting meeting = CreateAMeeting();
            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            IDictionary<Guid, Meeting> savedMeetings = new Dictionary<Guid, Meeting>();

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting> (storedMeetings));

            moqMeetingsRepository.Setup(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()))
                .Callback<IDictionary<Guid, Meeting>>(m => savedMeetings = m);

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);

            // Act
            _meetingsController.CreateMeeting(meeting);

            // Assert
            moqMeetingsRepository.Verify(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()), Times.Once); 
            Assert.AreEqual(storedMeetings.Count + 1, savedMeetings.Count);
            Assert.IsTrue(savedMeetings.ContainsKey(meeting.Id));
            AssertEqualMeetings(savedMeetings[meeting.Id], meeting);
        }

        private void AssertEqualMeetings(Meeting expected, Meeting actual)
        {
            Assert.AreEqual(expected.Category, actual.Category);
            Assert.AreEqual(expected.StartDate, actual.StartDate);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.EndDate, actual.EndDate);
            Assert.AreEqual(expected.ResponsiblePerson, actual.ResponsiblePerson);
            Assert.AreEqual(expected.ParticipantsDic.Count, actual.ParticipantsDic.Count);
            for (int i = 0; i < expected.ParticipantsDic.Count; i++)
            {
                AssertEqualParticipants(expected.ParticipantsDic.ElementAt(i), actual.ParticipantsDic.ElementAt(i));
            }
        }
        private void AssertEqualParticipants(KeyValuePair<string, DateTime> expected, KeyValuePair<string, DateTime> actual)
        {
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        [Test]
        public void AddPersonToMeetingTest_MeetingDoesNotExist()
        {
            // Arrange
            Guid meetingId = Guid.NewGuid();
            string personName = "testPerson";

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);

            // Act
            var result = _meetingsController.AddPersonToMeeting(meetingId, personName);

            // Assert
            moqMeetingsRepository.Verify(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()), Times.Never);
            Assert.IsFalse(result);
        }
        [Test]
        public void AddPersonToMeetingTest_PersonAlreadyInThisMeeting()
        {
            // Arrange
            string personName = "testPerson";
            Meeting meeting = CreateAMeeting();
            meeting.ParticipantsDic.Add("testPerson", DateTime.UtcNow);

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting.Id, meeting);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            var result = _meetingsController.AddPersonToMeeting(meeting.Id, personName);

            // Assert
            moqMeetingsRepository.Verify(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()), Times.Never);
            Assert.IsFalse(result);
        }

        [Test]
        public void AddPersonToMeetingTest_PersonIsInOtherMeeting()
        {
            // Arrange
            string personName = "testPersonPerson";
            Meeting meeting = CreateAMeeting();
            Meeting meetingWithDifferentDateInterval = CreateAMeeting();
            meetingWithDifferentDateInterval.StartDate = DateTime.Parse("2005-10-10");
            meetingWithDifferentDateInterval.EndDate = DateTime.Parse("2020-10-10");
            meetingWithDifferentDateInterval.Id = Guid.NewGuid();
            meetingWithDifferentDateInterval.ParticipantsDic.Add(personName, DateTime.Now);

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting.Id, meeting);
            storedMeetings.Add(meetingWithDifferentDateInterval.Id, meetingWithDifferentDateInterval);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            var result = _meetingsController.AddPersonToMeeting(meeting.Id, personName);

            // Assert
            moqMeetingsRepository.Verify(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()), Times.Never);
            Assert.IsFalse(result);
        }

        [Test]
        public void AddPersonToMeetingTest_PersonQualifiesAllParameters()
        {
            // Arrange
            string personName = "testPersonPerson";
            Meeting meeting = CreateAMeeting();

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting.Id, meeting);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            var result = _meetingsController.AddPersonToMeeting(meeting.Id, personName);

            // Assert
            //Assert.AreEqual(storedMeetings.Count + 1, );
            moqMeetingsRepository.Verify(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void AllMeetingsTest()
        {
            Meeting meeting1 = CreateAMeeting();

            Meeting meeting2 = CreateAMeeting(".Net meetas", "Ovidijus",
                                "Trumpas", Category.Hub, TypeValue.Live,
                                DateTime.Parse("2023-10-10"), DateTime.Parse("2025-10-10"));

            Meeting meeting3 = CreateAMeeting("Visma", "Dominykas",
                                "Greitai pasibaigsiantis", Category.CodeMonkey, TypeValue.InPerson,
                                DateTime.Parse("1990-10-10"), DateTime.Parse("2012-10-10"));

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting1.Id, meeting1);
            storedMeetings.Add(meeting2.Id, meeting2);
            storedMeetings.Add(meeting3.Id, meeting3);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            IDictionary<Guid, Meeting> result = _meetingsController.AllMeetings();

            // Assert
            Assert.AreEqual(storedMeetings.Count, result.Count);
            AssertEqualStoredMeetings(result, storedMeetings);
        }

        private void AssertEqualStoredMeetings(IDictionary<Guid, Meeting> expected, IDictionary<Guid, Meeting> actual)
        {
            for (int i = 0; i < expected.Count; i++)
            {
                AssertEqualMeetings(expected.ElementAt(i).Value, actual.ElementAt(i).Value);
            }
        }

        [Test]
        public void DeleteMeetingTest_IdsDoNotMatch()
        {
            Meeting meeting = CreateAMeeting();
            string user = "Jonas";
            Guid meetingId = Guid.NewGuid();

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting.Id, meeting);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);

            var result = _meetingsController.DeleteMeeting(meetingId, user);
            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteMeetingTest_ResponsiblePersonDoNotMatch()
        {
            Meeting meeting = CreateAMeeting();
            string user = "Jonas";
            meeting.ResponsiblePerson = "Ovidijus";

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting.Id, meeting);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);

            var result = _meetingsController.DeleteMeeting(meeting.Id, user);
            Assert.IsFalse(result);
        }

        [Test]
        public void DeleteMeetingTest_MeetingParametersQualifies()
        {
            Meeting meeting = CreateAMeeting();
            string user = "Ovidijus";
            meeting.ResponsiblePerson = "Ovidijus";

            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting.Id, meeting);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);

            var result = _meetingsController.DeleteMeeting(meeting.Id, user);

            moqMeetingsRepository.Verify(s => s.Save(It.IsAny<IDictionary<Guid, Meeting>>()), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void FilterByAttendeesCountTest()
        {
            Meeting meeting1 = CreateAMeeting();

            Meeting meeting2 = CreateAMeeting(".Net meetas", "Ovidijus",
                                "Trumpas", Category.Hub, TypeValue.Live,
                                DateTime.Parse("2023-10-10"), DateTime.Parse("2025-10-10"));

            Meeting meeting3 = CreateAMeeting("Visma", "Dominykas",
                                "Greitai pasibaigsiantis", Category.CodeMonkey, TypeValue.InPerson,
                                DateTime.Parse("1990-10-10"), DateTime.Parse("2012-10-10"));

            meeting2.ParticipantsDic.Add("Ovidijus", DateTime.Now);
            meeting2.ParticipantsDic.Add("Jon", DateTime.Now);
            meeting2.ParticipantsDic.Add("Kahn", DateTime.Now);
            meeting1.ParticipantsDic.Add("Lol", DateTime.Now);
            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting1.Id, meeting1);
            storedMeetings.Add(meeting2.Id, meeting2);
            storedMeetings.Add(meeting3.Id, meeting3);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            IDictionary<Guid, Meeting> result1 = _meetingsController.FilterByAttendeesCount(2);
            IDictionary<Guid, Meeting> result2 = _meetingsController.FilterByAttendeesCount(0);
            // Assert
            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual(2, result2.Count);
        }

        [Test]
        public void FilterByCategoryTest()
        {
            //Category.CodeMonkey
            Meeting meeting1 = CreateAMeeting();

            Meeting meeting2 = CreateAMeeting(".Net meetas", "Ovidijus",
                                "Trumpas", Category.Hub, TypeValue.Live,
                                DateTime.Parse("2023-10-10"), DateTime.Parse("2025-10-10"));

            Meeting meeting3 = CreateAMeeting("Visma", "Dominykas",
                                "Greitai pasibaigsiantis", Category.CodeMonkey, TypeValue.InPerson,
                                DateTime.Parse("1990-10-10"), DateTime.Parse("2012-10-10"));
            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting1.Id, meeting1);
            storedMeetings.Add(meeting2.Id, meeting2);
            storedMeetings.Add(meeting3.Id, meeting3);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            IDictionary<Guid, Meeting> result1 = _meetingsController.FilterByCategory(Category.CodeMonkey);
            IDictionary<Guid, Meeting> result2 = _meetingsController.FilterByCategory(Category.TeamBuilding);
            // Assert
            Assert.AreEqual(2, result1.Count);
            Assert.AreEqual(0, result2.Count);
        }

        [Test]
        public void FilterByDatesTest()
        {
            //DateTime.Parse("2000-10-10"), DateTime.Parse("2012-10-10"))
            Meeting meeting1 = CreateAMeeting();

            Meeting meeting2 = CreateAMeeting(".Net meetas", "Ovidijus",
                                "Trumpas", Category.Hub, TypeValue.Live,
                                DateTime.Parse("2023-10-10"), DateTime.Parse("2025-10-10"));

            Meeting meeting3 = CreateAMeeting("Visma", "Dominykas",
                                "Greitai pasibaigsiantis", Category.CodeMonkey, TypeValue.InPerson,
                                DateTime.Parse("1990-10-10"), DateTime.Parse("2012-10-10"));
            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting1.Id, meeting1);
            storedMeetings.Add(meeting2.Id, meeting2);
            storedMeetings.Add(meeting3.Id, meeting3);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            IDictionary<Guid, Meeting> result1 = _meetingsController.FilterByDates(DateTime.Parse("1995-10-10"));
            IDictionary<Guid, Meeting> result2 = _meetingsController.FilterByDates(DateTime.Parse("1995-10-10"), DateTime.Parse("2005-10-10"));
            // Assert
            Assert.AreEqual(2, result1.Count);
            Assert.AreEqual(0, result2.Count);
        }

        [Test]
        public void FilterByDescriptionTest()
        {
            //DateTime.Parse("2000-10-10"), DateTime.Parse("2012-10-10"))
            Meeting meeting1 = CreateAMeeting();

            Meeting meeting2 = CreateAMeeting(".Net meetas", "Ovidijus",
                                "Trumpas", Category.Hub, TypeValue.Live,
                                DateTime.Parse("2023-10-10"), DateTime.Parse("2025-10-10"));

            Meeting meeting3 = CreateAMeeting("Visma", "Dominykas",
                                "Greitai pasibaigsiantis meetas", Category.CodeMonkey, TypeValue.InPerson,
                                DateTime.Parse("1990-10-10"), DateTime.Parse("2012-10-10"));
            IDictionary<Guid, Meeting> storedMeetings = new Dictionary<Guid, Meeting>();
            storedMeetings.Add(meeting1.Id, meeting1);
            storedMeetings.Add(meeting2.Id, meeting2);
            storedMeetings.Add(meeting3.Id, meeting3);

            Mock<IMeetingsRepository> moqMeetingsRepository = new Mock<IMeetingsRepository>();
            moqMeetingsRepository.Setup(s => s.ReadAll()).Returns(new Dictionary<Guid, Meeting>(storedMeetings));

            _meetingsController = new MeetingsController(moqMeetingsRepository.Object);
            // Act
            IDictionary<Guid, Meeting> result1 = _meetingsController.FilterByDescription("Testtas");
            IDictionary<Guid, Meeting> result2 = _meetingsController.FilterByDescription("mee");
            // Assert
            Assert.AreEqual(0, result1.Count);
            Assert.AreEqual(2, result2.Count);
        }
    }
}
