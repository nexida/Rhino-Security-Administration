/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.PermissionJsonReader = Ext.extend(Rpc.JsonReader, {
	constructor: function (meta, recordType) {
		var cfg = {
			root: 'items',
			idProperty: 'StringId',
			fields: [
				'StringId',
				'Id',
				'Description',
				'Type',
				{
					name: '$ref',
					convert: function (v, r) {
						return r;
					}
				}
			]
		};
		Rhino.Security.PermissionJsonReader.superclass.constructor.call(this, Ext.apply(meta || {}, cfg), recordType);
	}
});