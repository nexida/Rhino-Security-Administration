/*jslint white: true, browser: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";
Ext.namespace('Rhino.Security');

Rhino.Security.UserEditWindow = Ext.extend(Ext.Window, {
	initComponent: function () {
		var _this = this,
		_fireEditEndedEvent = function (item) {
			_this.fireEvent('editended', _this, item);
		},
		_onGridPanelRowDblClick = function (grid, rowIndex, event) {
			var item = grid.getStore().getAt(rowIndex).data;
			_fireEditEndedEvent(item);
		},
		_searchFormPanel = new Rhino.Security.UserSearchFormPanel({
			title: 'Search Filters',
			region: 'north',
			autoHeight: true,
			collapsible: true,
			collapsed: true,
			titleCollapse: true,
			floatable: false
		}),
		_store = new Ext.data.Store({
			autoDestroy: true,
			proxy: new Rpc.JsonPostHttpProxy({
				url: 'User/Search'
			}),
			remoteSort: true,
			reader: new Rhino.Security.UserJsonReader()
		}),
		_pagingToolbar = new Ext.PagingToolbar({
			store: _store,
			displayInfo: true,
			pageSize: 25,
			prependButtons: true
		}),
		_gridPanel = new Rhino.Security.UserGridPanel({
			region: 'center',
			store: _store,
			bbar: _pagingToolbar,
			listeners: {
				rowdblclick: _onGridPanelRowDblClick
			}
		}),
		_onSearchButtonClick = function (b, e) {
			var params = _searchFormPanel.getForm().getFieldValues();
			Ext.apply(_gridPanel.getStore().baseParams, params);
			_gridPanel.getStore().load({
				params: {
					start: 0,
					limit: _pagingToolbar.pageSize
				}
			});
		},
		_onSelectNoneButtonClick = function (button, event) {
			_fireEditEndedEvent(null);
		},
		_onSelectButtonClick = function (button, event) {
			var sm = _gridPanel.getSelectionModel();
			var selectedItem = sm.getCount() > 0 ? sm.getSelected().data : null;
			_fireEditEndedEvent(selectedItem);
		};

		Ext.apply(_this, {
			title: 'Pick a User',
			width: 600,
			height: 300,
			layout: 'border',
			maximizable: true,
			modal: true,
			items: [_searchFormPanel, _gridPanel],
			tbar: [
				{ text: 'Search', handler: _onSearchButtonClick, icon: 'images/zoom.png', cls: 'x-btn-text-icon' }
			],
			buttons: [
				{ text: 'Select None', handler: _onSelectNoneButtonClick },
				{ text: 'Select', handler: _onSelectButtonClick }
			],
			setItem: function (item) {
				// TODO this method is here to match EditWindow API, can be used to set the selected row
			}
		});

		Rhino.Security.UserEditWindow.superclass.initComponent.apply(_this, arguments);

		_this.addEvents('editended');
	}
});