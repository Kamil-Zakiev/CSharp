using System.Diagnostics;

namespace DebuggerViewTest
{
    [DebuggerDisplay("Id = {Id}, Code = {Code}, Name = {Name}")]
    internal class Entity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}