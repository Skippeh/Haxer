ReadKeyPacket = (function()
{
	function ReadKeyPacket()
	{

	}

	ReadKeyPacket.prototype.handle = function(client, message)
	{
		webconsole.readKey(function(event)
		{
			client.send("readKey",
				{
					"character": String.fromCharCode(event.charCode),
					"shift": event.shiftKey,
					"ctrl": event.ctrlKey,
					"alt": event.altKey
				});
		}, message.intercept);
	};

	return ReadKeyPacket;
})();