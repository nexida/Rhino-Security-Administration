/*jslint white: true, browser: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";
Ext.namespace('Rhino.Security');

Rhino.Security.UserField = Ext.extend(Ext.form.TriggerField, {
	editable: false,
	hideTrigger: true,
	initComponent: function () {
		var _this = this,
		_window,
		_selectedItem = null,
		_onEditEnded = function (sender, item) {
			_this.setValue(item);
			_window.hide();
		};

		Ext.apply(_this, {
			onTriggerClick: function () {
				_window = _window || new Rhino.Security.UserEditWindow({
					closeAction: 'hide',
					listeners: {
						editended: _onEditEnded
					}
				});
				_window.setItem(_selectedItem);
				_window.show(this.getEl());
			},
			beforeDestroy: function () {
				if (_window) {
					_window.close();
				}
				return Rhino.Security.UserField.superclass.beforeDestroy.apply(_this, arguments);
			},
			setValue: function (v) {
				_selectedItem = v;
				return Rhino.Security.UserField.superclass.setValue.call(_this, Rhino.Security.User.toString(v));
			},
			getValue: function () {
				return _selectedItem;
			}
		});

		Rhino.Security.UserField.superclass.initComponent.apply(_this, arguments);
	}
});

Ext.reg('Rhino.Security.UserField', Rhino.Security.UserField);