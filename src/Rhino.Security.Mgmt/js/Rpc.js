/*jslint white: true, browser: true, devel: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, strict: true, newcap: true, immed: true */
/*global Ext, Rpc: true */
"use strict";

Rpc = {
	init: function () {
		Ext.data.DataProxy.on('exception', function (proxy, type, action) {
			Ext.MessageBox.show({ msg: 'An error has occured while trying to communicate with the server.', icon: Ext.MessageBox.ERROR, buttons: Ext.MessageBox.OK });
		});
	},

	call: function (opts) {
		opts = Ext.apply({
			params: {},
			success: Ext.emptyFn,
			failure: function () {
				Ext.MessageBox.show({ msg: 'An error has occured while trying to communicate with the server.', icon: Ext.MessageBox.ERROR, buttons: Ext.MessageBox.OK });
			},
			callback: Ext.emptyFn,
			scope: this
		}, opts);

		Ext.Ajax.request({
			url: opts.url,
			method: 'POST',
			jsonData: JSON.stringify(opts.params),
			success: function (response, options) {
				opts.success.call(opts.scope, Rpc.parseResponse(response));
			},
			failure: function (response, options) {
				opts.failure.call(opts.scope);
			},
			callback: function (options, success, response) {
				opts.callback.call(opts.scope);
			}
		});
	},

	JsonPostHttpProxy: Ext.extend(Ext.data.HttpProxy, {
		method: 'POST',
		doRequest: function (action, rs, params, reader, cb, scope, arg) {
			params = {
				jsonData: JSON.stringify(params)
			};
			Rpc.JsonPostHttpProxy.superclass.doRequest.call(this, action, rs, params, reader, cb, scope, arg);
		}
	}),

	JsonReader: Ext.extend(Ext.data.JsonReader, {
		read: function (response) {
			var o = Rpc.parseResponse(response);
			if (!o) {
				throw { message: 'Rpc.JsonReader.read: Json object not found' };
			}
			return this.readRecords(o);
		}
	}),

	LoadableValue: function (config) {
		var _this = this,
		_valueWhileLoading = null,
		_setValueOnLoadComplete = false,
		_loaded = false,
		_getValue = config.getValue,
		_setValue = config.setValue;

		Ext.apply(_this, {
			getValue: function () {
				if (_loaded) {
					return _getValue();
				} else {
					return _valueWhileLoading;
				}
			},
			setValue: function (v) {
				if (_loaded) {
					_setValue(v);
				} else {
					_valueWhileLoading = v;
					_setValueOnLoadComplete = true;
				}
			},
			notifyLoadComplete: function () {
				_loaded = true;
				if (_setValueOnLoadComplete) {
					_setValue(_valueWhileLoading);
				}
			}
		});
	},

	parseResponse: function (response) {
		var reviver, isJson;

		reviver = function (key, value) {
			if (typeof value === 'string') {
				var re, r;
				re = new RegExp('\\/Date\\(([-+])?(\\d+)(?:[+-]\\d{4})?\\)\\/');
				r = value.match(re);
				if (r) {
					return new Date(((r[1] || '') + r[2]) * 1);
				}
			}
			return value;
		};

		isJson = (response.getResponseHeader('content-type') || '').toLowerCase().indexOf('application/json') !== -1;
		return isJson ? JSON.parse(response.responseText, reviver) : null;
	}
};

