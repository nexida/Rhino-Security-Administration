using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Rhino.Security.Interfaces;
using Rhino.Security.Mgmt.Infrastructure;
using Rhino.Security.Mgmt.Data;

namespace Rhino.Security.Mgmt.Dtos
{
	public static class MappingEngineBuilder
	{
		public static IMappingEngine Build()
		{
			Mapper.Reset();
			DomainToDto();
			DtoToDomain();
			Mapper.AssertConfigurationIsValid();
			return Mapper.Engine;
		}

		private static void DomainToDto()
		{
			var sl = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

			Mapper.CreateMap<Rhino.Security.Model.Operation, OperationDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Operation>>().ToString(s)));
			Mapper.CreateMap<Rhino.Security.Model.Operation, OperationReferenceDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Operation>>().ToString(s)))
				.ForMember(d => d.Description, o => o.MapFrom(s => s.ToString()));

			Mapper.CreateMap<Rhino.Security.Model.UsersGroup, UsersGroupDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup>>().ToString(s)));
			Mapper.CreateMap<Rhino.Security.Model.UsersGroup, UsersGroupReferenceDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup>>().ToString(s)))
				.ForMember(d => d.Description, o => o.MapFrom(s => s.ToString()));

			Mapper.CreateMap<IUser, UserDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User>>().ToString((Rhino.Security.Model.User)s)))
				.ForMember(d => d.Groups, o => o.Ignore())
				.ForMember(d => d.Id, o => o.MapFrom(s => ((Rhino.Security.Model.User)s).Id))
				.ForMember(d => d.Name, o => o.MapFrom(s => ((Rhino.Security.Model.User)s).Name));

			Mapper.CreateMap<Rhino.Security.Model.User, UserDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User>>().ToString(s)));
			Mapper.CreateMap<Rhino.Security.Model.User, UserReferenceDto>()
				.ForMember(d => d.StringId, o => o.MapFrom(s => sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User>>().ToString(s)))
				.ForMember(d => d.Description, o => o.MapFrom(s => s.ToString()));

		}

		private static void DtoToDomain()
		{
			var sl = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;

			Mapper.CreateMap<OperationDto, Rhino.Security.Model.Operation>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.Operation>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Operation>>().FromString(s.StringId))
				.ForMember(d => d.Parent, o => o.Ignore())
				.ForMember(d => d.Children, o => o.Ignore());

			Mapper.CreateMap<OperationReferenceDto, Rhino.Security.Model.Operation>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.Operation>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Operation>>().FromString(s.StringId))
				.ForAllMembers(o => o.Ignore());

			Mapper.CreateMap<PermissionDto, Rhino.Security.Model.Permission>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.Permission>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.Permission>>().FromString(s.StringId))
				.ForMember(d => d.Operation, o => o.Ignore())
				.ForMember(d => d.UsersGroup, o => o.Ignore())
				.ForMember(d => d.User, o => o.Ignore())
				.ForMember(d => d.EntitiesGroup, o=> o.Ignore())
				.AfterMap((permissionDto, permission) => {
					var authorizationRepository = sl.GetInstance<AuthorizationRepositoryFactory>().Create();
					permission.Operation = authorizationRepository.GetOperationByName(permissionDto.OperationName);
					if (!string.IsNullOrEmpty(permissionDto.UserStringId))
					{
						permission.User = sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User>>().FromString(permissionDto.UserStringId);
					}
					if (!string.IsNullOrEmpty(permissionDto.UsersGroupStringId))
					{
						permission.UsersGroup = sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup>>().FromString(permissionDto.UsersGroupStringId);
					}
				});

			Mapper.CreateMap<UsersGroupDto, Rhino.Security.Model.UsersGroup>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.UsersGroup>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup>>().FromString(s.StringId))
				.ForMember(d => d.Parent, o => o.Ignore())
				.ForMember(d => d.AllParents, o => o.Ignore())
				.ForMember(d => d.Users, o => o.Ignore())
				.ForMember(d => d.AllChildren, o => o.Ignore())
				.ForMember(d => d.DirectChildren, o => o.Ignore())
				.AfterMap((usersGroupDto, usersGroup) => {
					var synchronizer = sl.GetInstance<SecurityUsersToUsersGroupsAssociationSynchronizer>();
					foreach (var u in usersGroup.Users.ToArray())
					{
						synchronizer.Disassociate((Rhino.Security.Model.User)u, usersGroup);
					}
					var items = Mapper.Map<Rhino.Security.Mgmt.Dtos.UserDto[], Rhino.Security.Model.User[]>(usersGroupDto.Users);
					foreach (var u in items)
					{
						synchronizer.Associate((Rhino.Security.Model.User)u, usersGroup);
					}
				});

			Mapper.CreateMap<UsersGroupReferenceDto, Rhino.Security.Model.UsersGroup>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.UsersGroup>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.UsersGroup>>().FromString(s.StringId))
				.ForAllMembers(o => o.Ignore());

			Mapper.CreateMap<UserDto, Rhino.Security.Model.User>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.User>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User>>().FromString(s.StringId))
				.ForMember(d => d.Groups, o => o.Ignore())
				.AfterMap((userDto, user) => {
					var synchronizer = sl.GetInstance<SecurityUsersToUsersGroupsAssociationSynchronizer>();
					foreach (var g in user.Groups.ToArray())
					{
						synchronizer.Disassociate(user, g);
					}
					var items = Mapper.Map<Rhino.Security.Mgmt.Dtos.UsersGroupDto[], Rhino.Security.Model.UsersGroup[]>(userDto.Groups);
					foreach (var g in items)
					{
						synchronizer.Associate(user, g);
					}
				});

			Mapper.CreateMap<UserReferenceDto, Rhino.Security.Model.User>()
				.ConstructUsing(s => string.IsNullOrEmpty(s.StringId) ? sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IFactory<Rhino.Security.Model.User>>().Create() : sl.GetInstance<Rhino.Security.Mgmt.Infrastructure.IStringConverter<Rhino.Security.Model.User>>().FromString(s.StringId))
				.ForAllMembers(o => o.Ignore());
		}
	}
}