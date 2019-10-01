using System;

namespace Gestion.Model
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }

        public Subject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public override object Clone()
        {
            return new Subject()
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
