
USE [master]
GO

IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'RhinoSecurityAdmin')
DROP DATABASE [RhinoSecurityAdmin]
GO

USE [master]
GO

CREATE DATABASE [RhinoSecurityAdmin]
GO

USE [RhinoSecurityAdmin]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO

CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKBBE4029387CC6C80]') AND parent_object_id = OBJECT_ID('security_EntityReferences'))
alter table security_EntityReferences  drop constraint FKBBE4029387CC6C80


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKD0398EC71CF0F16E]') AND parent_object_id = OBJECT_ID('security_EntitiesGroups'))
alter table security_EntitiesGroups  drop constraint FKD0398EC71CF0F16E


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK17A323D6DDB11ADF]') AND parent_object_id = OBJECT_ID('security_EntityReferencesToEntitiesGroups'))
alter table security_EntityReferencesToEntitiesGroups  drop constraint FK17A323D6DDB11ADF


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK17A323D6DE03A26A]') AND parent_object_id = OBJECT_ID('security_EntityReferencesToEntitiesGroups'))
alter table security_EntityReferencesToEntitiesGroups  drop constraint FK17A323D6DE03A26A


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK76531C74645BDDCE]') AND parent_object_id = OBJECT_ID('security_EntityGroupsHierarchy'))
alter table security_EntityGroupsHierarchy  drop constraint FK76531C74645BDDCE


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK76531C746440D8EE]') AND parent_object_id = OBJECT_ID('security_EntityGroupsHierarchy'))
alter table security_EntityGroupsHierarchy  drop constraint FK76531C746440D8EE


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE58BBFF82B7CDCD3]') AND parent_object_id = OBJECT_ID('security_Operations'))
alter table security_Operations  drop constraint FKE58BBFF82B7CDCD3


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKEA223C4C71C937C7]') AND parent_object_id = OBJECT_ID('security_Permissions'))
alter table security_Permissions  drop constraint FKEA223C4C71C937C7


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKEA223C4CFC8C2B95]') AND parent_object_id = OBJECT_ID('security_Permissions'))
alter table security_Permissions  drop constraint FKEA223C4CFC8C2B95


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKEA223C4C2EE8F612]') AND parent_object_id = OBJECT_ID('security_Permissions'))
alter table security_Permissions  drop constraint FKEA223C4C2EE8F612


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKEA223C4C6C8EC3A5]') AND parent_object_id = OBJECT_ID('security_Permissions'))
alter table security_Permissions  drop constraint FKEA223C4C6C8EC3A5


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKEC3AF233D0CB87D0]') AND parent_object_id = OBJECT_ID('security_UsersGroups'))
alter table security_UsersGroups  drop constraint FKEC3AF233D0CB87D0


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7817F27AA6C99102]') AND parent_object_id = OBJECT_ID('security_UsersToUsersGroups'))
alter table security_UsersToUsersGroups  drop constraint FK7817F27AA6C99102


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7817F27A1238D4D4]') AND parent_object_id = OBJECT_ID('security_UsersToUsersGroups'))
alter table security_UsersToUsersGroups  drop constraint FK7817F27A1238D4D4


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK69A3B61FA860AB70]') AND parent_object_id = OBJECT_ID('security_UsersGroupsHierarchy'))
alter table security_UsersGroupsHierarchy  drop constraint FK69A3B61FA860AB70


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK69A3B61FA87BAE50]') AND parent_object_id = OBJECT_ID('security_UsersGroupsHierarchy'))
alter table security_UsersGroupsHierarchy  drop constraint FK69A3B61FA87BAE50



    if exists (select * from dbo.sysobjects where id = object_id(N'security_EntityReferences') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_EntityReferences

    if exists (select * from dbo.sysobjects where id = object_id(N'security_EntitiesGroups') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_EntitiesGroups

    if exists (select * from dbo.sysobjects where id = object_id(N'security_EntityReferencesToEntitiesGroups') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_EntityReferencesToEntitiesGroups

    if exists (select * from dbo.sysobjects where id = object_id(N'security_EntityGroupsHierarchy') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_EntityGroupsHierarchy

    if exists (select * from dbo.sysobjects where id = object_id(N'security_Operations') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_Operations

    if exists (select * from dbo.sysobjects where id = object_id(N'security_Permissions') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_Permissions

    if exists (select * from dbo.sysobjects where id = object_id(N'security_EntityTypes') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_EntityTypes

    if exists (select * from dbo.sysobjects where id = object_id(N'security_UsersGroups') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_UsersGroups

    if exists (select * from dbo.sysobjects where id = object_id(N'security_UsersToUsersGroups') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_UsersToUsersGroups

    if exists (select * from dbo.sysobjects where id = object_id(N'security_UsersGroupsHierarchy') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table security_UsersGroupsHierarchy

    if exists (select * from dbo.sysobjects where id = object_id(N'Users') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Users

    create table security_EntityReferences (
        Id UNIQUEIDENTIFIER not null,
       EntitySecurityKey UNIQUEIDENTIFIER not null unique,
       Type UNIQUEIDENTIFIER not null,
       primary key (Id)
    )

    create table security_EntitiesGroups (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       Parent UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table security_EntityReferencesToEntitiesGroups (
        GroupId UNIQUEIDENTIFIER not null,
       EntityReferenceId UNIQUEIDENTIFIER not null,
       primary key (GroupId, EntityReferenceId)
    )

    create table security_EntityGroupsHierarchy (
        ParentGroup UNIQUEIDENTIFIER not null,
       ChildGroup UNIQUEIDENTIFIER not null,
       primary key (ChildGroup, ParentGroup)
    )

    create table security_Operations (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       Comment NVARCHAR(255) null,
       Parent UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table security_Permissions (
        Id UNIQUEIDENTIFIER not null,
       EntitySecurityKey UNIQUEIDENTIFIER null,
       Allow BIT not null,
       Level INT not null,
       EntityTypeName NVARCHAR(255) null,
       Operation UNIQUEIDENTIFIER not null,
       [User] BIGINT null,
       UsersGroup UNIQUEIDENTIFIER null,
       EntitiesGroup UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table security_EntityTypes (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       primary key (Id)
    )

    create table security_UsersGroups (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       Parent UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table security_UsersToUsersGroups (
        GroupId UNIQUEIDENTIFIER not null,
       UserId BIGINT not null,
       primary key (GroupId, UserId)
    )

    create table security_UsersGroupsHierarchy (
        ParentGroup UNIQUEIDENTIFIER not null,
       ChildGroup UNIQUEIDENTIFIER not null,
       primary key (ChildGroup, ParentGroup)
    )
    create table Users (
        Id BIGINT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       primary key (Id)
    )


    alter table security_EntityReferences 
        add constraint FKBBE4029387CC6C80 
        foreign key (Type) 
        references security_EntityTypes

    alter table security_EntitiesGroups 
        add constraint FKD0398EC71CF0F16E 
        foreign key (Parent) 
        references security_EntitiesGroups

    alter table security_EntityReferencesToEntitiesGroups 
        add constraint FK17A323D6DDB11ADF 
        foreign key (EntityReferenceId) 
        references security_EntityReferences

    alter table security_EntityReferencesToEntitiesGroups 
        add constraint FK17A323D6DE03A26A 
        foreign key (GroupId) 
        references security_EntitiesGroups

    alter table security_EntityGroupsHierarchy 
        add constraint FK76531C74645BDDCE 
        foreign key (ChildGroup) 
        references security_EntitiesGroups

    alter table security_EntityGroupsHierarchy 
        add constraint FK76531C746440D8EE 
        foreign key (ParentGroup) 
        references security_EntitiesGroups

    alter table security_Operations 
        add constraint FKE58BBFF82B7CDCD3 
        foreign key (Parent) 
        references security_Operations

    alter table security_Permissions 
        add constraint FKEA223C4C71C937C7 
        foreign key (Operation) 
        references security_Operations

    alter table security_Permissions 
        add constraint FKEA223C4CFC8C2B95 
        foreign key ([User]) 
        references Users

    alter table security_Permissions 
        add constraint FKEA223C4C2EE8F612 
        foreign key (UsersGroup) 
        references security_UsersGroups

    alter table security_Permissions 
        add constraint FKEA223C4C6C8EC3A5 
        foreign key (EntitiesGroup) 
        references security_EntitiesGroups

    alter table security_UsersGroups 
        add constraint FKEC3AF233D0CB87D0 
        foreign key (Parent) 
        references security_UsersGroups

    alter table security_UsersToUsersGroups 
        add constraint FK7817F27AA6C99102 
        foreign key (UserId) 
        references Users

    alter table security_UsersToUsersGroups 
        add constraint FK7817F27A1238D4D4 
        foreign key (GroupId) 
        references security_UsersGroups

    alter table security_UsersGroupsHierarchy 
        add constraint FK69A3B61FA860AB70 
        foreign key (ChildGroup) 
        references security_UsersGroups

    alter table security_UsersGroupsHierarchy 
        add constraint FK69A3B61FA87BAE50 
        foreign key (ParentGroup) 
        references security_UsersGroups

