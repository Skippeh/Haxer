function main()
{
	globals.applyValues();
	
	$(".scrollwrapper").TrackpadScrollEmulator({ autoHide: false });
	$(window).resize(function ()
	{
		$(".scrollwrapper").TrackpadScrollEmulator("recalculate");
	});

	window.webconsole = new WebConsole();
	window.client = new Client();
	client.connect(globals.serverIp, globals.serverPort);
	
	var input = $("#input");
	$(document).keypress(function (event)
	{
		var character = String.fromCharCode(event.keyCode);
		switch(event.keyCode)
		{
			default:
				{
					input.html(input.html() + character);
					break;
				}
			case 13: // enter pressed
				{
					var inputText = input.html().trim();
					input.html("");
					if (input.length != 0)
					{
						client.send("command", {"cmd": inputText});
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

				if (html.length > 0)
					html = html.substr(0, html.length - 1);

				input.html(html);
			}
			
			return false;
		}
		
		if (event.ctrlKey && event.keyCode != 67) // control pressed and not c.
		{
			// CTRL + ? pressed.

			return false;
		}
	});
	
	focused = true;
	$(window).focus(function()
	{
		$("#cursor").css("opacity", 1);
		focused = true;
	});
	$(window).blur(function()
	{
		$("#cursor").css("opacity", 0);
		focused = false;
	});
	
	startBlinkingCursor();
	
	if (navigator.userAgent.toLowerCase().indexOf("mobile") != -1)
	{
		webconsole.writeLine("mobile");
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