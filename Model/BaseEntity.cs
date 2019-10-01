using System;

namespace Gestion.Model
{
    public abstract class BaseEntity : ICloneable
    {
        public string Id { get; set; }

        public abstract object Clone();
    }
}
