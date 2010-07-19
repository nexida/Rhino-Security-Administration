namespace Rhino.Security.Mgmt.Dtos
{
	public class PermissionDto
	{
		public string StringId { get; set; }

		public System.Guid Id { get; set; }
				
		public System.Guid? EntitySecurityKey { get; set; }
				
		public bool Allow { get; set; }
				
		public int Level { get; set; }
				
		public string EntityTypeName { get; set; }
				
		public string OperationName { get; set; }
				
		public string UsersGroupStringId { get; set; }

		public string UserStringId { get; set; }
	}
}