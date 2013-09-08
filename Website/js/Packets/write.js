WritePacket = (function()
{
	function WritePacket()
	{

	}

	WritePacket.prototype.handle = function(client, message)
	{
		var text = message.text;
		var frontColor = message.fcolor;
		var backColor = message.bcolor;

		if (message.endline)
			webconsole.writeLine(text, frontColor, backColor);
		else
			webconsole.write(text, frontColor, backColor);
	};

	return WritePacket;
})();