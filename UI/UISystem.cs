using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RijamsMod.UI
{
	public class UISystem : ModSystem
	{
		internal UserInterface MMInterface;
		internal MatterManipulatorUI MMUI;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				MMUI = new MatterManipulatorUI();
				MMUI.Activate(); // Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
				MMInterface = new UserInterface();
				MMInterface.SetState(MMUI);
			}
		}

		public override void Unload()
		{
			MMUI = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			//if (MyInterface?.CurrentState != null)
			if (MatterManipulatorUI.Visible)
			{
				MMInterface?.Update(gameTime);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"RijamsMod: MMUI",
					delegate
					{
						if (MatterManipulatorUI.Visible)
						//if (_lastUpdateUiGameTime != null && MyInterface?.CurrentState != null)
						{
							MMInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI));
			}
		}
	}
}