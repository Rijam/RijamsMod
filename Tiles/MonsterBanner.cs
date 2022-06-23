using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RijamsMod.Tiles
{
	public class MonsterBanners : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			DustType = -1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			int style = frameX / 18;
			string item;
			switch (style) {
				case 0:
					item = "DarkSoldierBanner";
					break;
				case 1:
					item = "SkeletonCrossbowerBanner";
					break;
				case 2:
					item = "SkeletonGunnerBanner";
					break;
				case 3:
					item = "DungeonBatBanner";
					break;
				case 4:
					item = "SnowmanMuscleBanner";
					break;
				case 5:
					item = "SirSlushBanner";
					break;
				default:
					return;
			}
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, Mod.Find<ModItem>(item).Type);
		}

		public override void NearbyEffects(int i, int j, bool closer) {
			if (closer) {
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].TileFrameX / 18;
				string type;
				switch (style) {
					case 0:
						type = "DarkSoldier";
						break;
					case 1:
						type = "SkeletonCrossbower";
						break;
					case 2:
						type = "SkeletonGunner";
						break;
					case 3:
						type = "DungeonBat";
						break;
					case 4:
						type = "SnowmanMuscle";
						break;
					case 5:
						type = "SirSlush";
						break;
					default:
						return;
				}
				//player.NPCBannerBuff[Mod.Find<ModNPC>(type).Type] = true;
				//player.hasBanner = true;
				player.HasNPCBannerBuff(Mod.Find<ModNPC>(type).Type);
			}
		}
	}
}
