using System;
using System.Collections.Generic;

namespace DebuggerViewTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var list = new List<Entity>()
            {
                new DerivedEntity() {Id = 1, Code = "Code1", Name = "Name1"},
                new DerivedEntity() {Id = 2, Code = "Code2", Name = "Name2"},
                new DerivedEntity() {Id = 3, Code = "Code3", Name = "Name3"},
                new DerivedEntity() {Id = 4, Code = "Code4", Name = "Name4"},
                new DerivedEntity() {Id = 5, Code = "Code5", Name = "Name5"}
            };

            foreach (var entity in list)
            {
                Console.WriteLine(entity);
            }
        }

        private void Example1()
        {
            var list = new List<Entity>()
            {
                new Entity() {Id = 1, Code = "Code1", Name = "Name1"},
                new Entity() {Id = 2, Code = "Code2", Name = "Name2"},
                new Entity() {Id = 3, Code = "Code3", Name = "Name3"},
                new Entity() {Id = 4, Code = "Code4", Name = "Name4"},
                new Entity() {Id = 5, Code = "Code5", Name = "Name5"}
            };

            foreach (var entity in list)
            {
                Console.WriteLine(entity);
            }
        }
    }
}