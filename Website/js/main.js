function main()
{
	globals.applyValues();
	
	$(".scrollwrapper").TrackpadScrollEmulator({ autoHide: false });
	$(window).resize(function ()
	{
		$(".scrollwrapper").TrackpadScrollEmulator("recalculate");
	});

	window.webconsole = new WebConsole();
	webconsole.setReadonly(true);
	window.client = new Client();
	client.connect(globals.serverIp, globals.serverPort);
	
	var input = $("#input");
	$(document).keypress(function (event)
	{
		if (webconsole.readonly)
			return false;
		
		var character = String.fromCharCode(event.keyCode);
		switch(event.keyCode)
		{
			default:
				{
					webconsole.input += character;
					if (!webconsole.intercept)
					{
						input.html(webconsole.input);
					}
					break;
				}
			case 13: // enter pressed
				{
					if (webconsole.reading)
						break;
					
					var inputText = webconsole.input.trim();
					webconsole.input = "";
					input.html("");
					if (inputText.length != 0)
					{
						client.send("command", { "cmd": inputText });
					}
					
					break;
				}
		}
	});
	
	$(document).keydown(function(event)
	{
		if (event.keyCode == 8)
		{
			var selection = getSelection();
			
			if (selection.focusNode.parentElement == document.getElementById("input"))
			{
				getSelection().removeAllRanges();
			}
			else
			{
				var html = input.html();

				if (webconsole.input.length > 0)
					webconsole.input = webconsole.input.substr(0, webconsole.input.length - 1);

				if (!webconsole.intercept)
					input.html(webconsole.input);
			}
			
			return false;
		}
		
		if (event.ctrlKey)
		{
			// CTRL + ? pressed.

			return false;
		}
	});
	
	focused = true;
	$(window).focus(function()
	{
		if (!webconsole.readonly)
			$("#cursor").css("opacity", 1);
		
		focused = true;
	});
	$(window).blur(function()
	{
		$("#cursor").css("opacity", 0);
		focused = false;
	});
	
	if (navigator.userAgent.toLowerCase().indexOf("mobile") != -1)
	{
		$("*").css("font-size", "115%");
		
		$("body").append("<input type='text' id='mobileInput' style='left:-1000px;position:fixed;'></input>");

		$(document).click(function()
		{
			$("#mobileInput").focus();
		});
		
		$("#mobileInput").keypress(function(event)
		{
			$(document).keypress(event);
		});
	}
	
	$(document).bind("textselect", function(event, text, element)
	{
		if (text != "")
		{
			clipboardPrompt(text);
		}
	});

	startBlinkingCursor();
	$("body").css("background-color", getRandomColor());
	startFadingBackground();
}

function checkFocus()
{
	var selection = getSelection();
	if (selection.baseOffset - selection.extentOffset != 0)
		return;
	
	$('#input').focus();
}

String.prototype.endsWith = function (suffix)
{
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

function startBlinkingCursor()
{
	var visible = true;
	setInterval(function()
	{
		if (webconsole.readonly)
			return;
		
		if (visible)
		{
			$("#cursor").animate({ opacity: 0 }, 60);
			visible = false;
		}
		else
		{
			if (!focused)
			{
				visible = false;
				return;
			}
			$("#cursor").animate({ opacity: 1 }, 60);
			visible = true;
		}
	}, 530);
}

function startFadingBackground()
{
	//var color = getRandomColor();
	//$("body").animate({ "background-color": color }, 20000);
	//setTimeout(startFadingBackground, 20000);
}

function getRandomColor()
{
	var r = Math.floor(Math.random() * 255 * 0.1);
	var g = Math.floor(Math.random() * 255 * 0.1);
	var b = Math.floor(Math.random() * 255 * 0.1);
	return "rgb(" + r + "," + g + "," + b + ")";
}

function clipboardPrompt(text)
{
	var left = (screen.width / 2) - (500 / 2);
	var top = (screen.height / 2) - (300 / 2);

	var w = window.open("clipboard.html?text=" + encodeURI(text), "Clipboard", "menubar=no,statusbar=no,toolbar=no,location=no,width=500px,height=300px,top=" + top + ",left=" + left);
	w.focus();
}