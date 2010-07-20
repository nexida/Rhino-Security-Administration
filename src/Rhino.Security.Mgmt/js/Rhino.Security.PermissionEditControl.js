/*jslint white: true, browser: true, devel: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.PermissionEditControl = Ext.extend(Ext.Panel, {
	initComponent: function () {
		var _this = this,
		_selectedOperation,
		_usersWindow,
		_groupsWindow,

		_store = new Ext.data.Store({
			autoDestroy: true,
			remoteSort: false,
			proxy: new Ext.data.MemoryProxy({ items: [] }),
			reader: new Rhino.Security.PermissionJsonReader()
		}),

		_listView = new Ext.list.ListView({
			flex: 1,
			store: _store,
			multiSelect: true,
			hideHeaders: true,
			columns: [
				{
					width: 0.5,
					dataIndex: 'Description'
				}
			]
		}),

		_editEnded = function (window, item, itemType) {
			if (item !== null) {
				var currentItems = _this.getValue(),
			found = false,
			i,
			count = currentItems.length,
			newPermission;
				for (i = 0; i < count; i += 1) {
					if (currentItems[i] && currentItems[i].Description === item.Name) {
						found = true;
						break;
					}
				}

				if (!found) {
					newPermission = {
						Allow: _this.name === 'allowed' ? true : false,
						UserStringId: itemType === 'user' ? item.StringId : null,
						UsersGroupStringId: itemType === 'group' ? item.StringId : null,
						OperationName: _selectedOperation
					};

					_this.el.mask('Saving...', 'x-mask-loading');

					Rpc.call({
						url: 'Permission/Create',
						params: { item: newPermission },
						success: function (actionResult) {
							if (actionResult && actionResult.item) {
								currentItems.push({
									StringId: actionResult.item.Id,
									Id: actionResult.item.Id,
									Description: actionResult.item.Description,
									Type: actionResult.item.Type
								});
								_listView.getStore().load();
							}
						},
						callback: function () {
							_this.el.unmask();
						}
					});


				}
			}

			window.hide();
		},

		_onUserEditEnded = function (window, item) {
			_editEnded(window, item, 'user');
		},

		_onGroupEditEnded = function (window, item) {
			_editEnded(window, item, 'group');
		},

		_buildWindow = function (action) {
			if (action === 'user') {
				return new Rhino.Security.UserEditWindow({
					closeAction: 'hide',
					listeners: {
						editended: _onUserEditEnded
					}
				});
			}
			if (action === 'group') {
				return new Rhino.Security.UsersGroupEditWindow({
					closeAction: 'hide',
					listeners: {
						editended: _onGroupEditEnded
					}
				});
			}
		},

		_onAddUserButtonClick = function (button) {
			if (!_selectedOperation) {
				Ext.MessageBox.show({ msg: 'Please select an operation on the left first.', icon: Ext.MessageBox.WARNING, buttons: Ext.MessageBox.OK });
				return;
			}
			_usersWindow = _usersWindow || _buildWindow('user');
			_usersWindow.show(button.getEl());
		},

		_onAddUsersGroupButtonClick = function (button) {
			if (!_selectedOperation) {
				Ext.MessageBox.show({ msg: 'Please select an operation on the left first.', icon: Ext.MessageBox.WARNING, buttons: Ext.MessageBox.OK });
				return;
			}
			_groupsWindow = _groupsWindow || _buildWindow('group');
			_groupsWindow.show(button.getEl());
		},

		_getSelectedItems = function () {
			var selectedRecords = _listView.getSelectedRecords(),
			selectedItems = [];
			if (selectedRecords && selectedRecords.length > 0) {
				Ext.each(selectedRecords, function (item) {
					selectedItems.push(item.data);
				});
				return selectedItems;
			}
			return null;
		},

		_getSelectedStringIds = function (items) {
			if (items && items.length > 0) {
				var selectedStringIds = [];
				Ext.each(items, function (item) {
					selectedStringIds.push(item.StringId);
				});
				return selectedStringIds;
			}
			return null;
		},

		_onDeleteButtonClick = function () {
			var selectedItems = _getSelectedItems();
			if (!selectedItems || selectedItems.length === 0) {
				return;
			}
			Ext.MessageBox.confirm('Delete', 'Are you sure?', function (buttonId) {
				if (buttonId !== 'yes') {
					return;
				}

				_this.el.mask('Saving...', 'x-mask-loading');

				Rpc.call({
					url: 'Permission/Delete',
					params: { stringIds: _getSelectedStringIds(selectedItems) },
					success: function () {
						var i;
						for (i = 0; i < selectedItems.length; i += 1) {
							_this.getValue().remove(selectedItems[i].$ref);
						}
						_listView.getStore().load();
					},
					callback: function () {
						_this.el.unmask();
					}
				});
			});
		};

		Ext.apply(_this, {
			layout: 'fit',
			items: [_listView],
			fbar: {
				xtype: 'toolbar',
				buttonAlign: 'center',
				items: [
					{
						xtype: 'button',
						text: 'Add User',
						listeners: {
							click: _onAddUserButtonClick
						}
					},
					{
						xtype: 'button',
						text: 'Add Group',
						listeners: {
							click: _onAddUsersGroupButtonClick
						}
					},
					{
						xtype: 'button',
						text: 'Delete',
						listeners: {
							click: _onDeleteButtonClick
						}
					}
				]
			},
			beforeDestroy: function () {
				if (_usersWindow) {
					_usersWindow.close();
				}
				if (_groupsWindow) {
					_groupsWindow.close();
				}
				Rhino.Security.PermissionEditControl.superclass.beforeDestroy.apply(_this, arguments);
			},
			setValue: function (v, operation) {
				_selectedOperation = operation;
				_listView.getStore().proxy.data.items = v;
				_listView.getStore().load();
			},
			getValue: function () {
				return _listView.getStore().proxy.data.items;
			}
		});

		Rhino.Security.PermissionEditControl.superclass.initComponent.apply(_this, arguments);
	}
});

Ext.reg('Rhino.Security.PermissionEditControl', Rhino.Security.PermissionEditControl);