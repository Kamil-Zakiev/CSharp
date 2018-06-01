namespace ProxyWithoutTarget
{
    public class Calculator
    {
        public int Process(ICalcInterface calcInterface, int a, int b)
        {
            return calcInterface.CalcExpression(a, b);
        }
    }
}