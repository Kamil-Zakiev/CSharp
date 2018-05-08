public class FirstFreqClass
{
	public int IntField;
	
	public void SomeMethod(int a)
	{
		System.Console.Write(a);
	}
}

public class SecondFreqClass
{
	public FirstFreqClass FirstFreq;
	
	public void SomeMethod(FirstFreqClass a)
	{
		System.Console.Write(a.GetType());
	}
}