/*jslint white: true, browser: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.MainViewport = Ext.extend(Ext.Viewport, {
	layout: 'border',
	initComponent: function () {
		var _this = this,
		_tabPanel;

		function _setTabTitle(tab, title) {
			tab.setTitle(title);
		}
		function _setTabIdentifier(tab, id) {
			tab.tabContentIdentifier = id;
		}
		function _getTabIdentifier(tab) {
			return tab.tabContentIdentifier;
		}

		function _openTab(title, factory, id) {
			var tab;

			Ext.each(_tabPanel.items.items, function (currentTab) {
				if (_getTabIdentifier(currentTab) === id) {
					tab = currentTab;
					return false;
				}
			});

			if (!tab) {
				tab = (function () {
					var wrappedElement = factory();

					return _tabPanel.add(new Ext.Panel({
						layout: 'fit',
						items: wrappedElement,
						title: title,
						closable: true,
						getWrappedElement: function () {
							return wrappedElement;
						}
					}));
				}());
				_setTabIdentifier(tab, id);
			}

			tab.show();
			return tab;
		}

		// Operation
		function _onOperationNewItem(sender) {
			var newTab = _openTab('New Operation', function () {
				return new Rhino.Security.OperationEditPanel({
					listeners: {
						itemupdated: function (updatedItem) {
							_setTabTitle(newTab, 'Operation ' + Rhino.Security.Operation.toString(updatedItem));
							_setTabIdentifier(newTab, 'Operation-' + updatedItem.StringId);
						}
					}
				});
			}, 'Operation-new');
			newTab.getWrappedElement().loadItem(null);
		}
		function _onOperationClick(sender, item) {
			var newSearchPanelFactory = function () {
				return new Rhino.Security.OperationSearchPanel({
					listeners: {
						newitem: _onOperationNewItem
					}
				});
			};
			_openTab('Search Operation', newSearchPanelFactory, 'Operation-search');
		}

		// Permission
		function _onPermissionClick(sender, item) {
			_openTab('Manage Permissions', function () {
				return new Rhino.Security.PermissionsFormPanel();
			}, 'Permissions manager');
		}

		// UsersGroup
		function _onUsersGroupNewItem(sender) {
			var newTab = _openTab('New UsersGroup', function () {
				return new Rhino.Security.UsersGroupEditPanel({
					listeners: {
						itemupdated: function (updatedItem) {
							_setTabTitle(newTab, 'UsersGroup ' + Rhino.Security.UsersGroup.toString(updatedItem));
							_setTabIdentifier(newTab, 'UsersGroup-' + updatedItem.StringId);
						}
					}
				});
			}, 'UsersGroup-new');
			newTab.getWrappedElement().loadItem(null);
		}
		function _onUsersGroupEditItem(sender, item) {
			var editTab = _openTab('UsersGroup ' + Rhino.Security.UsersGroup.toString(item), function () {
				return new Rhino.Security.UsersGroupEditPanel({
					listeners: {
						itemupdated: function (updatedItem) {
							_setTabTitle(editTab, 'UsersGroup ' + Rhino.Security.UsersGroup.toString(updatedItem));
						}
					}
				});
			}, 'UsersGroup-' + item.StringId);
			editTab.getWrappedElement().loadItem(item.StringId);
		}
		function _onUsersGroupClick(sender, item) {
			var searchPanelFactory = function () {
				return new Rhino.Security.UsersGroupSearchPanel({
					listeners: {
						edititem: _onUsersGroupEditItem,
						newitem: _onUsersGroupNewItem
					}
				});
			};
			_openTab('Search UsersGroup', searchPanelFactory, 'UsersGroup-search');
		}

		// User
		function _onUserNewItem(sender) {
			var newTab = _openTab('New User', function () {
				return new Rhino.Security.UserEditPanel({
					listeners: {
						itemupdated: function (updatedItem) {
							_setTabTitle(newTab, 'User ' + Rhino.Security.User.toString(updatedItem));
							_setTabIdentifier(newTab, 'User-' + updatedItem.StringId);
						}
					}
				});
			}, 'User-new');
			newTab.getWrappedElement().loadItem(null);
		}

		function _onUserEditItem(sender, item) {
			var editTab = _openTab('User ' + Rhino.Security.User.toString(item), function () {
				return new Rhino.Security.UserEditPanel({
					listeners: {
						itemupdated: function (updatedItem) {
							_setTabTitle(editTab, 'User ' + Rhino.Security.User.toString(updatedItem));
						}
					}
				});
			}, 'User-' + item.StringId);
			editTab.getWrappedElement().loadItem(item.StringId);
		}
		function _onUserClick(sender, item) {
			var searchPanelFactory = function () {
				return new Rhino.Security.UserSearchPanel({
					listeners: {
						edititem: _onUserEditItem,
						newitem: _onUserNewItem
					}
				});
			};
			_openTab('Search User', searchPanelFactory, 'User-search');
		}

		_tabPanel = new Ext.TabPanel({
			activeTab: 0,
			enableTabScroll: true,
			border: false,
			//plugins: new Ext.ux.TabCloseMenu(),
			items: [{
				xtype: 'panel',
				title: 'Welcome'
			}]
		});

		this.items = [{
			cls: 'header',
			height: 30,
			region: 'north',
			xtype: 'box',
			el: 'header',
			border: false,
			margins: '0 0 5 0'
		}, {
			xtype: 'panel',
			region: 'center',
			layout: 'fit',
			layoutConfig: {
				align: 'stretch'
			},
			items: [_tabPanel],
			tbar: {
				xtype: 'toolbar',
				items: [
					{
						text: 'Manage Users',
						icon: 'images/user_suit.png',
						cls: 'x-btn-text-icon',
						menu: {
							xtype: 'menu',
							items: [
								{ text: 'Create', handler: _onUserNewItem, icon: 'images/add.png', cls: 'x-btn-text-icon' },
								{ text: 'Search', handler: _onUserClick, icon: 'images/zoom.png', cls: 'x-btn-text-icon' }
							]
						}
					},
					{
						text: 'Manage Groups',
						icon: 'images/group.png',
						cls: 'x-btn-text-icon',
						menu: {
							xtype: 'menu',
							items: [
								{ text: 'Create', handler: _onUsersGroupNewItem, icon: 'images/add.png', cls: 'x-btn-text-icon' },
								{ text: 'Search', handler: _onUsersGroupClick, icon: 'images/zoom.png', cls: 'x-btn-text-icon' }
							]
						}
					},
					{
						text: 'Manage Operations',
						icon: 'images/cog.png',
						cls: 'x-btn-text-icon',
						menu: {
							xtype: 'menu',
							items: [
								{ text: 'Create', handler: _onOperationNewItem, icon: 'images/add.png', cls: 'x-btn-text-icon' },
								{ text: 'Search', handler: _onOperationClick, icon: 'images/zoom.png', cls: 'x-btn-text-icon' }
							]
						}
					},
					{
						text: 'Manage Permissions',
						icon: 'images/lock.png',
						handler: _onPermissionClick,
						cls: 'x-btn-text-icon'
					}
				]
			}
		}];

		Rhino.Security.MainViewport.superclass.initComponent.apply(this, arguments);
	}
});