using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class BloodyArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Arrow");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.UnholyArrow);
			projectile.timeLeft = 1800; //30 seconds
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.damage = (int)(damage * 0.93f);
		}
        public override void PostAI()
        {
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimtaneWeapons, 0f, 0f, 150, default, 1.1f);
			}
		}

        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, projectile.position);
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimtaneWeapons, 0f, 0f, 150, default, 1.1f);
			}
		}
	}
	public class SulfurArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sulfur Arrow");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			projectile.width = 16;               //The width of projectile hitbox
			projectile.height = 16;              //The height of projectile hitbox
			projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.ranged = true;           //Is the projectile shoot by a ranged weapon?
			projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;          //Can the projectile collide with tiles?
			projectile.arrow = true;
			projectile.alpha = 255;
			projectile.timeLeft = 300; //5 seconds
			aiType = ProjectileID.WoodenArrowFriendly;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.SulfuricAcid>(), 150 + Main.rand.Next(0, 120));
			target.netUpdate = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void PostAI()
		{
			if (projectile.alpha > 0)
			{
				Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.2f);
				projectile.alpha -= 20;
			}
			if (projectile.timeLeft % 2 == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.SulfurDust>(), projectile.velocity.X, projectile.velocity.Y, 200, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				if (projectile.timeLeft % 8 == 0)
				{
					Main.dust[dust].noGravity = false;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, ModContent.DustType<Dusts.SulfurDust>());
			Main.PlaySound(SoundID.NPCDeath3, projectile.position);
		}
	}
	public class EvilArrows : ModProjectile
	{
        public override string Texture => "Terraria/Projectile_0";
		public override Color? GetAlpha(Color lightColor) => Color.White * 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil Arrows");     //The English name of the projectile
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.timeLeft = 2;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
        public override void Kill(int timeLeft)
        {
			if (Main.myPlayer == projectile.owner)
			{
				Projectile.NewProjectile(projectile.Center.X - 1, projectile.Center.Y - 4, projectile.velocity.X, projectile.velocity.Y + 0.2f, ProjectileID.UnholyArrow, projectile.damage - 1, projectile.knockBack, projectile.owner);
				Projectile.NewProjectile(projectile.Center.X + 1, projectile.Center.Y + 4, projectile.velocity.X, projectile.velocity.Y - 0.2f, ModContent.ProjectileType<BloodyArrow>(), projectile.damage + 1, projectile.knockBack, projectile.owner);
			}
		}
	}
}