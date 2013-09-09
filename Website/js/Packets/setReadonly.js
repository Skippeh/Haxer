SetReadonlyPacket = (function()
{
	function SetReadonlyPacket()
	{

	}

	SetReadonlyPacket.prototype.handle = function(client, message)
	{
		webconsole.setReadonly(message.cond);
	};

	return SetReadonlyPacket;
})();