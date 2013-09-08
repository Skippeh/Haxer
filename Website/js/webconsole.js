window.WebConsole = function()
{
	this.output = $("#outputDiv");
	this.defaultColor = globals.textColor;
	this.defaultBackgroundColor = "transparent";

	this.writeLine = function (text, frontColor, backColor)
	{
		if (text == undefined)
			text = "";
		
		text = text.toString();

		if (frontColor == undefined)
			frontColor = this.defaultColor;
		if (backColor == undefined)
			backColor = this.defaultBackgroundColor;

		text = text.replace(/\n/, "<br>");
		
		this.output.append("<span style='color:" + frontColor + "; background-color:" + backColor + ";'>" + text + "</span><br>");
		
		$(".scrollwrapper").TrackpadScrollEmulator("recalculate");
		this.scrollToBottom();
	};
	
	this.write = function (text, frontColor, backColor)
	{
		if (text == undefined)
			throw "ArgumentNull error: text is undefined!";

		if (text.endsWith("\n"))
		{
			this.writeLine(text.substr(0, text.length - 1), frontColor);
			return;
		}

		text = text.toString();
		
		if (frontColor == undefined)
			frontColor = this.defaultColor;
		if (backColor == undefined)
			backColor = this.defaultBackgroundColor;
		
		text = text.replace(/\n/, "<br>");
		
		var appendTarget = $("#outputDiv span:last");
		var add = "<span style='color:" + frontColor + "; background-color:" + backColor + ";'>" + text + "</span>";
		
		if (appendTarget.length == 0)
			appendTarget = this.output;
		
		appendTarget.append(add);
		
		$(".scrollwrapper").TrackpadScrollEmulator("recalculate");
		this.scrollToBottom();
	};
	
	this.clear = function ()
	{
		this.output.html("");
	};

	this.scrollToBottom = function ()
	{
		var scrollDiv = $(".tse-scroll-content");
		scrollDiv.scrollTop(scrollDiv[0].scrollHeight - scrollDiv.height());
	};
};