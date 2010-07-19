namespace Rhino.Security.Mgmt.Infrastructure
{
	public interface IFactory<out T>
	{
		T Create();
	}
}