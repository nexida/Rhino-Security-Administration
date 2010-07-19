namespace Rhino.Security.Mgmt.Dtos
{
	public class UserDto
	{
		public string StringId { get; set; }

		public System.Int64 Id { get; set; }
				
		public string Name { get; set; }

		public Rhino.Security.Mgmt.Dtos.UsersGroupDto[] Groups { get; set; }
				
	}
}