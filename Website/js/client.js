Client = (function()
{
	function Client()
	{
		this.socket = null;
		this.ip = null;
		this.port = null;
		this.connected = false;
		this.authenticated = false;
		this.packetHandlers = {};
		this.addPackets();
	}
	
	Client.prototype.addPackets = function()
	{
		this.packetHandlers["auth"] = new AuthenticatePacket();
		this.packetHandlers["write"] = new WritePacket();
	};

	Client.prototype.connect = function(ip, port)
	{
		this.ip = ip;
		this.port = port;
		var socket = new WebSocket("ws://" + ip + ":" + port);
		this.socket = socket;
		
		var client = this;
		socket.onopen = function() { client.onopen(this); };
		socket.onclose = function() { client.onclose(this); };
		socket.onerror = function() { client.onerror(this); };
		socket.onmessage = function(message) { client.onmessage(this, message); };
		
		webconsole.writeLine("Connecting to remote server...");
	};
	
	Client.prototype.send = function(id, data)
	{
		if (data == undefined)
			data = {};
		
		data.id = id;
		this.socket.send(JSON.stringify(data));
		console.log("Sent:");
		console.log(data);
	};
	
	Client.prototype.disconnect = function()
	{
		this.socket.close();
	};
	
	Client.prototype.onopen = function(socket)
	{
		console.log("on open!");
		webconsole.writeLine("Connected!");
		webconsole.writeLine("\nAuthenticating...");
		this.connected = true;
		this.send("auth");
	};
	
	Client.prototype.onmessage = function(socket, message)
	{
		console.log("on message: " + message.data);
		
		message = JSON.parse(message.data);
		
		if (this.packetHandlers[message.id] == undefined)
		{
			console.error("UNHANDLED PACKET: " + message.id);
			this.disconnect();
			return;
		}
		
		console.log("Handling packet: " + message.id);
		this.packetHandlers[message.id].handle(this, message);
	};
	
	Client.prototype.onerror = function(socket)
	{
		console.log("on error!");
	};
	
	Client.prototype.onclose = function(socket)
	{
		console.log("on close!");
		this.connected = false;
		
		if (this.authenticated)
		{
			webconsole.writeLine("Lost connection to the server!", globals.fatalColor);
		}
		else
		{
			webconsole.writeLine("Could not connect to the server!", globals.fatalColor);
		}
	};
	
	return Client;
})();