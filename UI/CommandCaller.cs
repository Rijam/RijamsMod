using Terraria.ModLoader;

namespace RijamsMod.UI
{
	public class MMUICommandOpen : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "mmui_open";

		public override string Description
			=> "Opens the Matter Manipulator UI";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MatterManipulatorUI.Visible = true;
		}
	}
	public class MMUICommandClose : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "mmui_close";

		public override string Description
			=> "Closes the Matter Manipulator UI";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MatterManipulatorUI.Visible = false;
		}
	}
}