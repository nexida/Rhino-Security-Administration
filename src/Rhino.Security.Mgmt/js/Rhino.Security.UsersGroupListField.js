/*jslint white: true, browser: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";

Ext.namespace('Rhino.Security');
Rhino.Security.UsersGroupListField = Ext.extend(Ext.form.Field, {
	initComponent: function () {
		var _this = this,
		_gridPanel,
		_store,
		_window,
		_selectedItem = null,
		_onEditEnded = function (window, item) {
			if (item !== null) {
				if (_selectedItem) {
					Ext.apply(_selectedItem, item);
				} else {
					var currentItems = _this.getValue(),
					found = false,
					i,
					count = currentItems.length;
					for (i = 0; i < count; i += 1) {
						if (currentItems[i] && currentItems[i].StringId && currentItems[i].StringId === item.StringId) {
							found = true;
							break;
						}
					}

					if (!found) {
						currentItems.push(item);
					}
					_gridPanel.getStore().load();
				}
			}
			window.hide();
		},
		_buildWindow = function () {
			return new Rhino.Security.UsersGroupEditWindow({
				closeAction: 'hide',
				listeners: {
					editended: _onEditEnded
				}
			});
		},
		_onNewButtonClick = function (button) {
			_selectedItem = null;
			_window = _window || _buildWindow();
			_window.show(button.getEl());
		},
		_onDeleteButtonClick = function () {
			var sm = _gridPanel.getSelectionModel(),
			selectedItems = sm.getSelections(),
			count = sm.getCount(),
			i;
			if (count > 0) {
				for (i = 0; i < count; i += 1) {
					_this.getValue().remove(selectedItems[i].data.$ref);
				}
				_gridPanel.getStore().load();
			}
		};

		_store = new Ext.data.Store({
			autoDestroy: true,
			remoteSort: false,
			proxy: new Ext.data.MemoryProxy({ items: [] }),
			reader: new Rhino.Security.UsersGroupJsonReader()
		});

		_gridPanel = new Rhino.Security.UsersGroupGridPanel(Ext.copyTo({
			id: _this.id + '-gridpanel',
			store: _store,
			tbar: [
				{ text: 'Add', handler: _onNewButtonClick, icon: 'images/add.png', cls: 'x-btn-text-icon' },
				{ text: 'Delete', handler: _onDeleteButtonClick, icon: 'images/delete.png', cls: 'x-btn-text-icon' }
			]
		}, _this.initialConfig, []));

		Ext.apply(_this, {
			onRender: function (ct, position) {
				// TODO This creates a hidden field above the grid. Check if this is good or not
				this.autoCreate = {
					id: _this.id,
					name: _this.name,
					type: 'hidden',
					tag: 'input'
				};
				Rhino.Security.UsersGroupListField.superclass.onRender.call(_this, ct, position);
				_this.wrap = _this.el.wrap({ cls: 'x-form-field-wrap' });
				_this.resizeEl = _this.positionEl = _this.wrap;
				_gridPanel.render(_this.wrap);
			},
			onResize: function (w, h, aw, ah) {
				Rhino.Security.UsersGroupListField.superclass.onResize.apply(_this, arguments);
				_gridPanel.setSize(w, h);
			},
			onEnable: function () {
				Rhino.Security.UsersGroupListField.superclass.onEnable.apply(_this, arguments);
				_gridPanel.enable();
			},
			onDisable: function () {
				Rhino.Security.UsersGroupListField.superclass.onDisable.apply(_this, arguments);
				_gridPanel.disable();
			},
			beforeDestroy: function () {
				if (_window) {
					_window.close();
				}
				Ext.destroy(_gridPanel);
				Rhino.Security.UsersGroupListField.superclass.beforeDestroy.apply(_this, arguments);
			},
			setValue: function (v) {
				_gridPanel.getStore().proxy.data.items = v;
				_gridPanel.getStore().load();
				return Rhino.Security.UsersGroupListField.superclass.setValue.apply(_this, arguments);
			},
			getValue: function () {
				return _gridPanel.getStore().proxy.data.items;
			}
		});

		Rhino.Security.UsersGroupListField.superclass.initComponent.apply(_this, arguments);
	}
});

Ext.reg('Rhino.Security.UsersGroupListField', Rhino.Security.UsersGroupListField);