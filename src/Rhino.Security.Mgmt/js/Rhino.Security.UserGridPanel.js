/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";

Ext.namespace("Rhino.Security");

Rhino.Security.UserGridPanel = Ext.extend(Ext.grid.GridPanel, {
	border: false,
	initComponent: function () {
		this.colModel = new Ext.grid.ColumnModel({
			defaults: { width: 100, sortable: true },
			columns: [
				{ dataIndex: 'Id', header: 'Id', xtype: 'numbercolumn' }, { dataIndex: 'Name', header: 'Name', xtype: 'gridcolumn', width: 400 }
			]
		});
		Rhino.Security.UserGridPanel.superclass.initComponent.apply(this, arguments);
	}
});