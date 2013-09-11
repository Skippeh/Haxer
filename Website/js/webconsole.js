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
	
	var cookieHistory = Cookies.get("history").split(/\n/g).reverse();
	for (var index in cookieHistory)
	{
		this.history.push(cookieHistory[index]);
	}
	this.historyPosition = -1;
	
	$(document).on("keydown", { _this: this }, function(event)
	{
		var _this = event.data._this;

		if (event.keyCode == 38) // up arrow
		{
			var history = _this.getHistory(-1);
			$("#input").html(history);
			_this.input = history;

			return false;
		}

		if (event.keyCode == 40) // down arrow
		{
			var history = _this.getHistory(1);
			$("#input").html(history);
			_this.input = history;

			return false;
		}
	});
	
	this.getHistory = function(direction)
	{
		if (this.history.length == 0)
			return "";
		
		if (direction == -1)
		{
			if (this.historyPosition + 1 < this.history.length)
				this.historyPosition += 1;
		}
		else if (direction == 1)
		{
			if (this.historyPosition - 1 >= 0)
				this.historyPosition -= 1;
			else
			{
				this.historyPosition = -1;
				return "";
			}
		}
		
		return this.history.slice()[this.historyPosition];
	};
	
	this.addHistory = function(text)
	{
		this.history.unshift(text);
		Cookies.set("history", Cookies.get("history") + "\n" + text);
	};

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
				$(document).off("keydown", onKeyDown);
				_this.reading = false;
				_this.input = "";
				$("#input").html("");
				
				if (!intercept)
					_this.writeLine(result);
				else
					_this.writeLine();
				_this.intercept = false;

				if (!_this.readonly)
					$("#inputPrefix").show();
				
				callback(result);
			}
		};
		
		var onKeyDown = function(event)
		{
			if (event.keyCode == 8)
			{
				if (result.length > 0)
				{
					result = result.substr(0, result.length - 1);
				}
			}
		};
		
		if (intercept)
			this.intercept = true;

		this.reading = true;
		$("#inputPrefix").hide();
		$(document).on("keypress", { _this: this }, onKeyPress);
		$(document).on("keydown", { _this: this }, onKeyDown);
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
			$("#inputPrefix").hide();
			this.input = "";
			$("#input").html("");
		}
		else
		{
			$("#cursor").css("opacity", 1);
			$("#inputPrefix").show();
		}
	};
};