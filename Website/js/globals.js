﻿window.globals = {};

globals.textColor = "#c8c8c8";
globals.serverIp = "home.skipcast.net";
globals.serverPort = "45654";

globals.fatalColor = "rgb(255, 75, 75)";

globals.applyValues = function ()
{
	$("#input").css("color", this.textColor);
	$("#inputArrow").css("color", this.textColor);
};