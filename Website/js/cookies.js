window.Cookies = {};

Cookies.get = function(key, defaultValue)
{
	if (defaultValue == undefined)
		defaultValue = "";
	
	if (defaultValue == undefined)
		defaultValue = "";
	key = encodeURI(key);

	var cValue = document.cookie;
	var cStart = cValue.indexOf(" " + key + "=");
	if (cStart == -1)
	{
		cStart = cValue.indexOf(key + "=");
	}
	if (cStart == -1)
	{
		cValue = defaultValue;
		Cookies.set(key, encodeURI(defaultValue));
	}
	else
	{
		cStart = cValue.indexOf("=", cStart) + 1;
		var c_end = cValue.indexOf(";", cStart);
		if (c_end == -1)
		{
			c_end = cValue.length;
		}
		cValue = unescape(cValue.substring(cStart, c_end));
	}

	if (decodeURI(cValue) == "")
	{
		Cookies.set(key, encodeURI(defaultValue));
		return defaultValue;
	}

	return decodeURI(cValue);
};

Cookies.set = function(key, value)
{
	key = encodeURI(key);
	value = encodeURI(value);

	document.cookie = key + "=" + value.replace(/=/g, "&#61;")
										.replace(/</g, "&lt;")
										.replace(/>/g, "&gt");
};