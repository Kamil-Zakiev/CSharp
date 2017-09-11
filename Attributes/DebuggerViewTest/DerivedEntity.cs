using System.Diagnostics;

namespace DebuggerViewTest
{
    [DebuggerDisplay("Name = {Name}")]
    [DebuggerDisplay("Id = {Id}, Code = {Code}, ParentEntityId = {ParentEntity?.Id}")]
    internal class DerivedEntity : Entity
    {
        public Entity ParentEntity { get; set; }
    }
}