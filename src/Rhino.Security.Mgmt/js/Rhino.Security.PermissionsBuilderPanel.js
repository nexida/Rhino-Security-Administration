/*jslint white: true, browser: true, devel: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.PermissionsBuilderPanel = Ext.extend(Ext.Panel, {
	initComponent: function () {
		var _this = this,
		_allowPermissionEditControl = new Rhino.Security.PermissionEditControl({
			name: 'allowed',
			title: 'Allowed',
			border: true,
			width: 300,
			margins: { 
				top: 0,
				left: 0,
				right: 10,
				bottom: 0
			}
		}),
		_denyPermissionEditControl = new Rhino.Security.PermissionEditControl({
			name: 'forbidden',
			title: 'Forbidden',
			width: 300
		});

		Ext.apply(_this, {
			layout: 'hbox',
			layoutConfig: {
				padding: 10,
				align: 'stretch'
			},
			border: true,
			items: [_allowPermissionEditControl, _denyPermissionEditControl ],
			loadPermissions: function (operationName) {
				_this.el.mask('Loading...', 'x-mask-loading');

				Rpc.call({
					url: 'Permission/LoadByOperationName',
					params: { operationName: operationName },
					success: function (item) {
						_allowPermissionEditControl.setValue(item.allowed, operationName);
						_denyPermissionEditControl.setValue(item.forbidden, operationName);
						_this.el.unmask();
					}
				});
			}
		});

		Rhino.Security.PermissionsBuilderPanel.superclass.initComponent.apply(_this, arguments);
	}
});