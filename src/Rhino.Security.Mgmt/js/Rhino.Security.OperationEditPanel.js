/*jslint white: true, browser: true, devel: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.OperationEditPanel = Ext.extend(Ext.Panel, {
	initComponent: function () {
		var _this = this,
		_fireItemUpdated = function (item) {
			_this.fireEvent('itemupdated', item);
		},
		_formPanel = new Rhino.Security.OperationFormPanel(),
		_saveItemButtonHandler = function () {
			_this.el.mask('Saving...', 'x-mask-loading');
			Rpc.call({
				url: 'Operation/Save',
				params: { item: _formPanel.getForm().getFieldValues() },
				success: function (result) {
					if (result.success) {
						Ext.MessageBox.show({ msg: 'Changes saved successfully.', icon: Ext.MessageBox.INFO, buttons: Ext.MessageBox.OK });
						if (result.item) {
							_formPanel.getForm().setValues(result.item);
						}
						_formPanel.setUpFormForEditItem();
						_fireItemUpdated(_formPanel.getForm().getFieldValues());
					} else {
						_formPanel.getForm().markInvalid(result.errors.item);
						Ext.MessageBox.show({ msg: 'Error saving data. Correct errors and retry.', icon: Ext.MessageBox.ERROR, buttons: Ext.MessageBox.OK });
					}
				},
				callback: function () {
					_this.el.unmask();
				}
			});
		},
		_refreshItemButtonHandler = function () {
			Ext.MessageBox.confirm('Refresh', 'All modifications will be lost, continue?', function (buttonId) {
				if (buttonId === 'yes') {
					var stringId = _formPanel.getForm().getFieldValues().StringId;
					if (Ext.isEmpty(stringId)) {
						_formPanel.getForm().reset();
					} else {
						_this.loadItem(stringId);
					}
				}
			});
		};

		Ext.apply(_this, {
			border: false,
			layout: 'fit',
			items: _formPanel,
			tbar: [
				{ text: 'Save', handler: _saveItemButtonHandler, icon: 'images/disk.png', cls: 'x-btn-text-icon' },
				{ text: 'Refresh', handler: _refreshItemButtonHandler, icon: 'images/arrow_refresh.png', cls: 'x-btn-text-icon' }
			],
			loadItem: function (stringId) {
				if (!stringId || stringId === null) {
					_formPanel.setUpFormForNewItem();
				}
				else {
					_this.el.mask('Loading...', 'x-mask-loading');
					_formPanel.setUpFormForEditItem();
					Rpc.call({
						url: 'Operation/Load',
						params: { stringId: stringId },
						success: function (item) {
							_this.el.unmask();
							_formPanel.getForm().setValues(item);
						}
					});
				}
			}
		});

		Rhino.Security.OperationEditPanel.superclass.initComponent.apply(_this, arguments);

		_this.addEvents('itemupdated');
	}
});