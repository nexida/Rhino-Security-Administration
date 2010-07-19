/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rhino */
"use strict";
Ext.namespace('Rhino.Security');

Rhino.Security.OperationColumn = Ext.extend(Ext.grid.Column, {
    constructor: function (cfg) {
        Rhino.Security.OperationColumn.superclass.constructor.apply(this, arguments);
        this.renderer = function (value, metadata, record, rowIndex, colIndex, store) {
            return value.Description;
        };
    }
});

Ext.grid.Column.types['Rhino.Security.OperationColumn'] = Rhino.Security.OperationColumn;