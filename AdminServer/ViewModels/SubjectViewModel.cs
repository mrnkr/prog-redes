using Gestion.Model;

namespace AdminServer.ViewModels
{
    public class SubjectViewModel
    {
        public string id { get; set; }
        public string name { get; set; }

        public static SubjectViewModel FromEntity(Subject s)
        {
            return new SubjectViewModel()
            {
                id = s.Id,
                name = s.Name
            };
        }
    }
}