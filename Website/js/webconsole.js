window.WebConsole = function()
{
	this.input = "";
	this.output = $("#output");
	this.defaultColor = globals.textColor;
	this.defaultBackgroundColor = "transparent";
	this.readonly = false;
	this.history = [];
	this.historyPosition = 0;
	this.intercept = false;
	this.reading = false;
	
	for (var historyCommand in Cookies.get("history").split(/\n/g))
	{
		this.history.push(historyCommand);
	}
	this.historyPosition = this.history.length - 1;

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
		
		text = text.toString();

		if (text.endsWith("\n"))
		{
			this.writeLine(text.substr(0, text.length - 1), frontColor);
			return;
		}

		if (frontColor == undefined)
			frontColor = this.defaultColor;
		if (backColor == undefined)
			backColor = this.defaultBackgroundColor;
		
		text = text.replace(/\n/, "<br>");
		
		var add = "<span style='color:" + frontColor + "; background-color:" + backColor + ";'>" + text + "</span>";
		
		this.output.append(add);
		
		$(".scrollwrapper").TrackpadScrollEmulator("recalculate");
		this.scrollToBottom();
	};
	
	this.readLine = function(callback, intercept)
	{
		var result = "";
		var onKeyPress = function(event)
		{
			var _this = event.data._this;
			if (event.charCode != 13)
			{
				result += String.fromCharCode(event.charCode);
			}
			else
			{
				$(document).off("keypress", onKeyPress);
				_this.reading = false;
				_this.input = "";
				$("#input").html("");
				
				if (!intercept)
					_this.writeLine(result);
				else
					_this.writeLine();
				_this.intercept = false;
				
				callback(result);
			}
		};
		
		if (intercept)
			this.intercept = true;

		this.reading = true;
		$(document).on("keypress", { _this: this }, onKeyPress);
	};
	
	this.readKey = function(callback, intercept, clear)
	{
		if (clear == undefined)
			clear = true;
		
		var onKeyPress = function(event)
		{
			var _this = event.data._this;
			$(document).off("keypress", onKeyPress);
			this.intercept = false;
			if (clear)
			{
				$("#input").html("");
				_this.input = "";
				_this.reading = false;
			}
			if (!intercept)
				_this.writeLine(String.fromCharCode(event.charCode));
			callback(event);
			_this.intercept = false;
		};

		this.reading = true;
		if (intercept)
			this.intercept = true;
		
		$(document).on("keypress", { _this: this }, onKeyPress);
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
	
	this.setReadonly = function(readonly)
	{
		this.readonly = readonly;

		if (readonly)
		{
			$("#cursor").css("opacity", 0);
			this.input = "";
			$("#input").html("");
		}
		else
			$("#cursor").css("opacity", 1);
	};
};