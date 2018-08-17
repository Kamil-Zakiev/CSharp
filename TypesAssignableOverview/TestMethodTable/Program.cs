namespace TestMethodTable
{
    using System;

    class A
    {
        public void Method1()
        {
            
        }
        
        public virtual void Method2()
        {
            
        }
        
        public void Method3()
        {
            
        }
    }

    class B : A
    {
        public void Method1()
        {
            
        }
        
        public override void Method2()
        {
            
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var aMethodInfos = typeof(A).GetMethods();
            foreach (var aMethodInfo in aMethodInfos)
            {
                Console.WriteLine(aMethodInfo.Name + ":\t" + aMethodInfo.DeclaringType );
            }
            /*
                Method1:        TestMethodTable.A
                Method2:        TestMethodTable.A
                Method3:        TestMethodTable.A
                ToString:       System.Object
                Equals:         System.Object
                GetHashCode:    System.Object
                GetType:        System.Object
             */

            Console.WriteLine();
            
            var bMethodInfos = typeof(B).GetMethods();
            foreach (var bMethodInfo in bMethodInfos)
            {
                Console.WriteLine(bMethodInfo.Name + ":\t" + bMethodInfo.DeclaringType);
            }
            /*
                Method1:        TestMethodTable.B
                Method2:        TestMethodTable.B
                Method1:        TestMethodTable.A
                Method3:        TestMethodTable.A
                ToString:       System.Object
                Equals:         System.Object
                GetHashCode:    System.Object
                GetType:        System.Object

             */
        }
    }
}