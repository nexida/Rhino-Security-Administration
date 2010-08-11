<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<link rel="stylesheet" type="text/css" href="ext/resources/css/ext-all.css" />
	<link rel="stylesheet" type="text/css" href="styles/Styles.css" />
	<script type="text/javascript" src="js/json2.js"></script>
	<script type="text/javascript" src="ext/adapter/ext/ext-base-debug-w-comments.js"></script>
	<script type="text/javascript" src="ext/ext-all-debug-w-comments.js"></script>
	<script type="text/javascript" src="js/Rpc.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationGridPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupGridPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserGridPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationJsonReader.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupJsonReader.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserJsonReader.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationSearchPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupSearchPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserSearchPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationSearchFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupSearchFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserSearchFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationField.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupField.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserField.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserListField.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.MainViewPort.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationColumn.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupColumn.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserColumn.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.Operation.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.Permission.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroup.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.User.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationEditWindow.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupEditWindow.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserEditWindow.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.OperationEditPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupEditPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UserEditPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.UsersGroupListField.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.PermissionsFormPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.PermissionsBuilderPanel.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.PermissionEditControl.js"></script>
	<script type="text/javascript" src="js/Rhino.Security.PermissionJsonReader.js"></script>
		
	<script type="text/javascript">
		"use strict";

		Ext.BLANK_IMAGE_URL = 'ext/resources/images/default/s.gif';
		Ext.USE_NATIVE_JSON = true;

		Ext.onReady(function () {
			Ext.QuickTips.init();
			Rpc.init();
			var mainViewport = new Rhino.Security.MainViewport();
		});
		</script>
	<title></title>
</head>
<body>
  <div id="header">
	<div class="title">Rhino Security Administration (beta)</div>
  </div>
  <div id="welcome">
	<p>
	<b>Rhino Security Administration</b> is a simple implementation of a management UI for <a href="http://github.com/ayende/rhino-security" target="_blank">Rhino Security</a>.
	</p>
	<br />
	It currently supports:<br />
	<ul class="supportedFeatures">
		<li>managing users, groups and their associations</li>
		<li>managing operations (add and search)</li>
		<li>managing permissions for users\groups on operations</li>
	</ul>
	<br />
	<p>It was built with <a href="http://www.nexida.com/smartui" target="_blank">NEXiDA code generation tools</a>, 
	using the <a href="http://www.nexida.com/smartui/Code_Generation_Shared_Templates.asp">MVC + ExtJs template set</a>.</p>
	<br />
	<p>The project is open source and hosted on github - <a href="http://github.com/nexida/Rhino-Security-Administration" target="_blank">see source code</a>.</p>
  </div>
</body>
</html>
