using System.Collections.Generic;

namespace Gestion.Model
{
    public class Subject
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }

        public List<FileRef> Files { get; set; }
    }
}
