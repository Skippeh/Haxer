ReadLinePacket = (function()
{
	function ReadLinePacket()
	{

	}

	ReadLinePacket.prototype.handle = function(client, message)
	{
		webconsole.readLine(function(line)
		{
			client.send("readLine",
				{
					"line": line
				});
		}, message.intercept);
	};

	return ReadLinePacket;
})();