using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;
using RijamsMod.Projectiles.Summon.Whips;

namespace RijamsMod.Items.Weapons.Summon.Whips
{
	public class TailoThreeCats : WhipItemBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemOriginDesc.itemList.Add(Item.type, ["[c/474747:Dropped by Deerclops]"]);
			ItemID.Sets.UniqueTagEffects[Type] = new WhipTagEffect_TailoThreeCats() { TagDamage = 5 };
		}
		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<TailoThreeCatsProj>(), 30, 2f, 4.25f, 35);

			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
			Item.channel = false;
		}
	}

	public class WhipTagEffect_TailoThreeCats : WhipTagEffect
	{
		public override bool OnProcHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
		{
			//base.OnProcHit(owner, optionalProjectile, npcHit, calcDamage);
			npcHit.AddBuff(BuffID.Confused, 420);
			CombatText.NewText(optionalProjectile.Hitbox, Color.Magenta, "❓");
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.DeadCellsMushroomBoiTargetFound, new ParticleOrchestraSettings
			{
				UniqueInfoPiece = optionalProjectile.whoAmI
			});
			return true;
		}

		public override void OnTaggedHit(Player owner, Projectile optionalProjectile, NPC npcHit, NPC.HitInfo hit)
		{
			//npcHit.AddBuff(BuffID.Confused, 240);
			//CombatText.NewText(optionalProjectile.Hitbox, Color.Purple, "BANG!");
		}
	}
}
