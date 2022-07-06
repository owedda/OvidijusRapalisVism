using Newtonsoft.Json;
using VismaOvidijusRapalis.Models;

namespace VismaOvidijusRapalis.Repositories
{
    public class MeetingsRepository : IMeetingsRepository
    {
        private readonly string _fileName;
        public MeetingsRepository(string fileName)
        {
            if (fileName is null)
                throw new ArgumentException(nameof(fileName));
            _fileName = fileName;
        }

        public IDictionary<Guid, Meeting> ReadAll()
        {
            var data = JsonConvert.DeserializeObject<IDictionary<Guid, Meeting>>(File.ReadAllText(_fileName));
            if (data is null)
                return new Dictionary<Guid, Meeting>();
            return data;
        }

        public void Save(IDictionary<Guid, Meeting> data)
        {
            string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_fileName, jsonString);
        }
    }
}
