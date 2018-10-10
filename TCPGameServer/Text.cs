using System;
namespace TCPGameServer
{
    public static class Text
    {
		private static string TimeStamp()
		{
			return "[" + DateTime.Now.ToString("hh:mm:ss") + "] ";
		}
		public static void WriteLine(string message, TextType textType, params object[] objects)
		{
			string type = TimeStamp()+ "[" + Enum.GetName(typeof(TextType), textType) + "]";

			switch (textType)
			{
				case TextType.DEBUG:
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(type + message, objects);
					Console.ResetColor();
					break;
				case TextType.INFO:
					Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(type + message, objects);
                    Console.ResetColor();
					break;
				case TextType.ERROR:
					Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(type + message, objects);
                    Console.ResetColor();
					break;
				case TextType.WARNING:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(type + message, objects);
                    Console.ResetColor();
					break;
				default:
					Console.ResetColor();
                   
                    Console.WriteLine(type + message, objects);
					break;
			}
		}
	}

	public enum TextType
	{
		DEBUG,
        INFO,
        ERROR,
        WARNING
	}
}
