/*jslint white: true, browser: true, devel: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.UsersGroupFormPanel = Ext.extend(Ext.form.FormPanel, {
	initComponent: function () {
		var _this = this,
		_entityIdFieldContainer = new Ext.Container({
			xtype: 'container',
			layout: 'form',
			autoEl: 'div',
			items: [{ name: 'Id', fieldLabel: 'Id', xtype: 'displayfield'}]
		}),
		_entityNameField = new Ext.form.Field({
			xtype: 'textfield',
			fieldLabel: 'Name',
			name: 'Name'
		}); ;

		Ext.apply(_this, {
			border: false,
			layout: 'vbox',
			layoutConfig: {
				align: 'stretch',
				pack: 'start'
			},
			items: [{
				layout: 'form',
				border: false,
				padding: 10,
				items: [
					{ name: 'StringId', xtype: 'hidden' },
					_entityIdFieldContainer,
					_entityNameField
				]
			},
			{
				flex: 1,
				xtype: 'tabpanel',
				plain: true,
				border: false,
				activeTab: 0,
				deferredRender: false, // IMPORTANT! See http://www.extjs.com/deploy/dev/examples/form/dynamic.js
				items: [
					{ name: 'Users', title: 'Users', xtype: 'Rhino.Security.UserListField' }
				]
			}
			],
			setUpFormForEditItem: function () {
				_entityIdFieldContainer.show();
				_entityNameField.setReadOnly(true);
			},
			setUpFormForNewItem: function () {
				_entityIdFieldContainer.hide();
				_this.getForm().findField('Id').setRawValue('00000000-0000-0000-0000-000000000000');
			}
		});

		Rhino.Security.UsersGroupFormPanel.superclass.initComponent.apply(_this, arguments);
	}
});