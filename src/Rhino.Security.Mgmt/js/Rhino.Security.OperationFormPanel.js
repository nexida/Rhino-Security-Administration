/*jslint white: true, browser: true, devel: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.OperationFormPanel = Ext.extend(Ext.form.FormPanel, {
	initComponent: function () {
		var _this = this,
		_entityIdFieldContainer = new Ext.Container({
			xtype: 'container',
			layout: 'form',
			autoEl: 'div',
			items: [{ name: 'Id', fieldLabel: 'Id', xtype: 'displayfield'}]
		});

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
					{ name: 'Name', fieldLabel: 'Name', xtype: 'textfield' }
				]
			}],
			setUpFormForEditItem: function () {
				_entityIdFieldContainer.show();
			},
			setUpFormForNewItem: function () {
				_entityIdFieldContainer.hide();
				_this.getForm().findField('Id').setRawValue('00000000-0000-0000-0000-000000000000');
			}
		});

		Rhino.Security.OperationFormPanel.superclass.initComponent.apply(_this, arguments);
	}
});