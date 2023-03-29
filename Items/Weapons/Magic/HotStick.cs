using RijamsMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RijamsMod.Items.Weapons.Magic
{
	public class HotStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hot Stick");
		}

		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.noMelee = false;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true; //Channel so that you can hold the weapon [Important]
			Item.mana = 1;
			Item.rare = ItemRarityID.White; //1
			Item.width = 22;
			Item.height = 24;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.knockBack = 0.1f;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Thrust;
			//item.shootSpeed = 16f;
			Item.autoReuse = true;
			//item.noUseGraphic = true;
			//item.shoot = ModContent.ProjectileType<HotStickProjectile>();
			//item.shootSpeed = 1f;
			Item.value = Item.sellPrice(copper: 5);
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Mod.Name + "/Items/GlowMasks/" + Name + "_Glow").Value;
			}
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			// 60 frames = 1 second
			if (Main.rand.NextBool(10))
			{
				target.AddBuff(BuffID.OnFire, 60);
			}
			else if (Main.rand.NextBool(10))
			{
				target.AddBuff(BuffID.OnFire, 30);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup("Wood", 3)
				.AddIngredient(ItemID.Torch, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(3, 0);
		}
	}
}
