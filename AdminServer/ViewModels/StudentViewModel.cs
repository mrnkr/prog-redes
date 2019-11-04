using Gestion.Model;
using System.Linq;

namespace AdminServer.ViewModels
{
    public class StudentViewModel
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public static StudentViewModel FromEntity(Student s)
        {
            return new StudentViewModel
            {
                id = s.Id,
                firstName = s.FirstName,
                lastName = s.LastName,
            };
        }
    }
}