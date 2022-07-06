namespace VismaOvidijusRapalis.Models
{
    public enum Category
    {
        CodeMonkey,
        Hub,
        Short,
        TeamBuilding
    }
    public enum TypeValue
    {
        Live,
        InPerson
    }
    public class Meeting
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? Description { get; set; }
        public Category Category { get; set; }
        public TypeValue Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<string, DateTime> ParticipantsDic = new Dictionary<string, DateTime>();
        public Meeting(Guid id, string? name, string? responsiblePerson, string? description,
            Category category, TypeValue type, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override string ToString()
        {
            return string.Format("| {0, 10} | {1, 20} | {2, 10} |" +
                    " {3, 30} | {4, 15} | {5, 10} | {6, 10} | {7, 10} | {8, 10}",
                    Id, Name,
                    ResponsiblePerson, Description, Category,
                    Type, StartDate,
                    EndDate, ParticipantsDic.Count);
        }
    }
}
