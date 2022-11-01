using RijamsMod.UI;
using Terraria.ModLoader;

namespace RijamsMod.UI
{
	public class MyUICommandOpen : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "myui";

		public override string Description
			=> "Show the custom ui";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MatterManipulatorUI.Visible = true;
		}
	}
	public class MyUICommandClose : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "myui_close";

		public override string Description
			=> "Closes the custom ui";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MatterManipulatorUI.Visible = false;
		}
	}
}