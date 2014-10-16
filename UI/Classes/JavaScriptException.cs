using System;

namespace UI.Classes
{
	public class JavaScriptException:Exception
	{
		public JavaScriptException(string message)
			: base(message)
		{
		}
	}
}