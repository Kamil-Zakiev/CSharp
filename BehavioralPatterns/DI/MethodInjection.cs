namespace DI.MethodInjection
{
    public interface ISomeContext
    {

    }

    public class SomeClass
    {
        public void DoStuff(ISomeContext context)
        {
            // do something based on context
        }
    }
}
