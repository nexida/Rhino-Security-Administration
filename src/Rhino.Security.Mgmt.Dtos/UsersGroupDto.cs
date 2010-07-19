namespace Rhino.Security.Mgmt.Dtos
{
	public class UsersGroupDto
	{
		public string StringId { get; set; }

		public System.Guid Id { get; set; }
				
		public string Name { get; set; }

		// public Rhino.Security.Mgmt.Dtos.UsersGroupReferenceDto Parent { get; set; }
				
		// public Rhino.Security.Mgmt.Dtos.UsersGroupReferenceDto[] AllParent { get; set; }
				
		public Rhino.Security.Mgmt.Dtos.UserDto[] Users { get; set; }
				
		// public Rhino.Security.Mgmt.Dtos.UsersGroupReferenceDto[] AllChildren { get; set; }
				
		// public Rhino.Security.Mgmt.Dtos.UsersGroupReferenceDto[] DirectChildren { get; set; }
				
	}
}