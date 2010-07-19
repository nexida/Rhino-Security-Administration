/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";

Ext.namespace('Rhino.Security');

Rhino.Security.Operation = {
	toString: function (o) {
		if (o) {
			return o.Name || o.Description || o.StringId || '[some value]';
		}
		return '';
	}
};