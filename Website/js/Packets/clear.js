ClearPacket = (function()
{
	function ClearPacket()
	{

	}

	ClearPacket.prototype.handle = function(client, message)
	{
		webconsole.clear();
	};

	return ClearPacket;
})();