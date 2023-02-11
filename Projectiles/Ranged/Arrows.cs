using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Ranged
{
	public class BloodyArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bloody Arrow");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.UnholyArrow);
			Projectile.timeLeft = 1800; //30 seconds
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.damage = (int)(damage * 0.93f);
		}
        public override void PostAI()
        {
			if (Main.rand.NextBool(5))
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CrimtaneWeapons, 0f, 0f, 150, default, 1.1f);
			}
		}

        public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CrimtaneWeapons, 0f, 0f, 150, default, 1.1f);
			}
		}
	}
	public class SulfurArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sulfur Arrow");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;               //The width of projectile hitbox
			Projectile.height = 16;              //The height of projectile hitbox
			Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = true;         //Can the projectile deal damage to enemies?
			Projectile.hostile = false;         //Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged;           //Is the projectile shoot by a ranged weapon?
			Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;          //Can the projectile collide with tiles?
			Projectile.arrow = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300; //5 seconds
			AIType = ProjectileID.WoodenArrowFriendly;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			target.netUpdate = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void PostAI()
		{
			if (Projectile.alpha > 0)
			{
				Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.2f);
				Projectile.alpha -= 20;
			}
			if (Projectile.timeLeft % 2 == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.SulfurDust>(), Projectile.velocity.X, Projectile.velocity.Y, 200, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				if (Projectile.timeLeft % 8 == 0)
				{
					Main.dust[dust].noGravity = false;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, ModContent.DustType<Dusts.SulfurDust>());
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.position);
		}
	}
	public class EvilArrows : ModProjectile
	{
        public override string Texture => "Terraria/Images/Projectile_0";
		public override Color? GetAlpha(Color lightColor) => Color.White * 0f;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Evil Arrows");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.timeLeft = 2;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
        public override void Kill(int timeLeft)
        {
			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Entity.GetSource_Death(), Projectile.Center.X - 1, Projectile.Center.Y - 4, Projectile.velocity.X, Projectile.velocity.Y + 0.2f, ProjectileID.UnholyArrow, Projectile.damage - 1, Projectile.knockBack, Projectile.owner);
				Projectile.NewProjectile(Entity.GetSource_Death(), Projectile.Center.X + 1, Projectile.Center.Y + 4, Projectile.velocity.X, Projectile.velocity.Y - 0.2f, ModContent.ProjectileType<BloodyArrow>(), Projectile.damage + 1, Projectile.knockBack, Projectile.owner);
			}
		}
	}
}