public class FirstRareClass
{
	public int IntField;
	
	public void SomeMethod(int a)
	{
		System.Console.Write(a);
	}
}

public class SecondRareClass
{
	public FirstRareClass FirstRare;
	
	public void SomeMethod(FirstRareClass a)
	{
		System.Console.Write(a.GetType());
	}
}