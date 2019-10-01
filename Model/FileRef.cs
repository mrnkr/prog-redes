using System;

namespace Gestion.Model
{
    public class FileRef : BaseEntity
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public FileRef()
        {
            Id = Guid.NewGuid().ToString();
        }

        public override object Clone()
        {
            return new FileRef()
            {
                Id = Id,
                Name = Name,
                Path = Path
            };
        }
    }
}
