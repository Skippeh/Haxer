AuthenticatePacket = (function()
{
	function AuthenticatePacket()
	{

	}

	AuthenticatePacket.prototype.handle = function(client, message)
	{
		client.authenticated = message.success;
		
		if (!message.success)
		{
			webconsole.writeLine("Could not authenticate with remote server: " + message.reason, globals.fatalColor);
			return;
		}
		
		webconsole.writeLine("Authenticated with remote server!");
		webconsole.clear();
		webconsole.writeLine(message.motd + "\n");
		webconsole.setReadonly(false);
	};
	
	return AuthenticatePacket;
})();