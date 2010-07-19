using Rhino.Security.Model;
namespace Rhino.Security.Mgmt.Model

{
	public class OperationFactory : Rhino.Security.Mgmt.Infrastructure.IFactory<Operation>
	{
		public Operation Create()
		{
			return new Operation();
		}
	}
}