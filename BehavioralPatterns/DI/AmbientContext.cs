namespace DI.AmbientContext
{
    public abstract class Context
    {
        private static Context _default = new DefaultContext();
        private static Context _current;

        public static Context Current
        {
            get
            {
                return _current ?? _default;
            }
            set
            {
                _current = value;
            }
        }
    }

    public class DefaultContext : Context
    {        
    }

    public class SomeClass
    {
        public void Method()
        {
            var context = Context.Current;

            // do something
        }
    }
}
