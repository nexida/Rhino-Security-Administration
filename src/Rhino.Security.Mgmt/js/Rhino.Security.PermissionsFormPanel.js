/*jslint white: true, browser: true, onevar: true, undef: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.PermissionsFormPanel = Ext.extend(Ext.Panel, {
	layout: 'border',
	initComponent: function () {
		var _this = this,
		_onNodeAppended,
		_onOperationClick,
		_operationsTreePanel,
		_onOperationRefresh,
		_permissionsBuilderPanel = new Rhino.Security.PermissionsBuilderPanel({
			region: 'center'
		});

		_onOperationClick = function (sender, item) {
			_permissionsBuilderPanel.loadPermissions(sender.id);
		};

		_onNodeAppended = function (tree, parent, node, index) {
			node.on('click', _onOperationClick);
		};

		_onOperationRefresh = function () {
			_operationsTreePanel.getLoader().load(_operationsTreePanel.getRootNode());
		};

		_operationsTreePanel = new Ext.tree.TreePanel({
			border: false,
			region: 'west',
			split: true,
			collapsible: false,
			autoScroll: true,
			animCollapse: false,
			animate: false,
			collapseMode: 'mini',
			width: 200,
			rootVisible: false,
			lines: false,
			listeners: {
				append: _onNodeAppended
			},
			dataUrl: 'Operation/GetAllAsTree',
			root: {
				text: 'All',
				id: 'id'
			},
			tbar: {
				xtype: 'toolbar',
				items: [
					{ xtype: 'tbtext', text: 'Operations', cls: 'x-btn-text-icon' },
					{ handler: _onOperationRefresh, icon: 'images/arrow_refresh.png' }
				]
			}
		});

		_this.items = [_operationsTreePanel, _permissionsBuilderPanel];

		Rhino.Security.PermissionsFormPanel.superclass.initComponent.apply(_this, arguments);
	}
});