using System;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions
{
    public class Item
    {
        public string Name { get; set; }
        
        public InnerItem InnerItem { get; set; }
    }

    public class InnerItem
    {
        public string Name { get; set; }
    }
    
    internal class Program
    {
        public static Expression<Func<InnerItem, bool>> GetFilterExpr()
        {
            return item => item.Name.EndsWith("2");
        }

        public static void Main(string[] args)
        {
            Expression<Func<InnerItem, bool>> filterExpr = item => item.Name.EndsWith("2");

            var items = new[]
            {
                new Item() {Name = "Name1", InnerItem = new InnerItem() {Name = "InnerName1"}},
                new Item() {Name = "Name2", InnerItem = new InnerItem() {Name = "InnerName2"}},
                new Item() {Name = "Name3", InnerItem = new InnerItem() {Name = "InnerName3"}},
                new Item() {Name = "Name4", InnerItem = new InnerItem() {Name = "InnerName4"}},
                new Item() {Name = "Name5", InnerItem = new InnerItem() {Name = "InnerName5"}},
                new Item() {Name = "Name6", InnerItem = new InnerItem() {Name = "InnerName6"}},
                new Item() {Name = "Name7", InnerItem = new InnerItem() {Name = "InnerName7"}},
                new Item() {Name = "Name8", InnerItem = new InnerItem() {Name = "InnerName8"}},
                new Item() {Name = "Name9", InnerItem = new InnerItem() {Name = "InnerName9"}}
            };

            var query = items.AsQueryable();
            
            var gv = new GrowerVisitor();

            
            var fiteredQ = query.Where(item => item.InnerItem.Name.EndsWith("4"));
            var newExpr = gv.Visit(fiteredQ.Expression);
            var newQuery = (IQueryable<Item>)fiteredQ.Provider.CreateQuery(newExpr);
            foreach (var item in newQuery)
            {
                Console.WriteLine(item.Name);
            }

        }
        
    }

    public class GrowerVisitor : ExpressionVisitor
    {
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type != typeof(string))
            {
                return base.VisitConstant(node);
            }
            
            return Expression.Constant(node.Value + "modified");
        }
    }
}