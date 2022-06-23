using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
	public class BasicDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basic Dart");     //The English name of the projectile
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
			Projectile.tileCollide = true;          //Can the projectile collide with tiles?
			Projectile.arrow = false;
			AIType = ProjectileID.WoodenArrowFriendly;           //Act exactly like default Dart
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => true;

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
	public class SulfurDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sulfur Dart");     //The English name of the projectile
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
			Projectile.arrow = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300; //5 seconds
			AIType = ProjectileID.WoodenArrowFriendly;           //Act exactly like default Dart
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
	public class ChlorophyteDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Dart");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;    //The length of old position to be recorded
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
			Projectile.penetrate = 7;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;          //Can the projectile collide with tiles?
			Projectile.arrow = false;
			AIType = ProjectileID.WoodenArrowFriendly;           //Act exactly like default Dart
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.25f, 0.5f, 0.25f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (int)(damage * 0.9f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}

	public class SpectreDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Dart");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;    //The length of old position to be recorded
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
			Projectile.penetrate = 2;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;          //Can the projectile collide with tiles?
			Projectile.alpha = 127;
			Projectile.arrow = false;
			AIType = ProjectileID.WoodenArrowFriendly;           //Act exactly like default Dart
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.timeLeft = 1200;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (int)(damage * 0.70f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.45f, 0.45f, 0.5f);
			//Taken from Ichor Dart
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.ai[1] >= 0f)
				{
					Projectile.penetrate = -1;
				}
				else if (Projectile.penetrate < 0)
				{
					Projectile.penetrate = 1;
				}
				if (Projectile.ai[1] >= 0f)
				{
					Projectile.ai[1] += 1f;
				}
				if (Projectile.ai[1] > 10f)
				{
					Projectile.ai[1] = -1000f;
					float num196 = Projectile.velocity.Length();
					Vector2 vector28 = Projectile.velocity;
					vector28.Normalize();
					int num197 = 5;
					for (int num198 = 0; num198 < num197; num198++)
					{
						Vector2 vector29 = new(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
						vector29.Normalize();
						vector29 += vector28 * 2f;
						vector29.Normalize();
						vector29 *= num196 * 3f;
						Projectile.NewProjectile(Entity.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, vector29.X, vector29.Y, Projectile.type, (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 0f, -1000f);
					}
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
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
	public class ShroomiteDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Dart");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;    //The length of old position to be recorded
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
			Projectile.penetrate = 6;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;          //Can the projectile collide with tiles?
			Projectile.alpha = 0;
			Projectile.arrow = false;
			AIType = ProjectileID.CrystalDart;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.extraUpdates = 7;
			Projectile.timeLeft = 1200;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (int)(damage * 0.9f);
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.325f, 0.325f, 0.5f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
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
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
	public class LuminiteDart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Dart");     //The English name of the projectile
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;               //The width of projectile hitbox
			Projectile.height = 16;              //The height of projectile hitbox
			Projectile.aiStyle = 0;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = true;         //Can the projectile deal damage to enemies?
			Projectile.hostile = false;         //Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged;           //Is the projectile shoot by a ranged weapon?
			Projectile.penetrate = 4;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;          //Can the projectile collide with tiles?
			Projectile.arrow = false;
			AIType = 1;
			//AIType = ProjectileID.ChlorophyteBullet;           //Act exactly like default Dart
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 1200;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (int)(damage * 0.75f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
		public override void AI()
		{
            Lighting.AddLight(Projectile.Center, 0.9f, 1.0f, 0.9f);

			//Copied from Chlorophyte Bullet source code
			if (Projectile.alpha < 170)
			{
				for (int num164 = 0; num164 < 10; num164++)
				{
					float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num164;
					float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num164;
					int newDust = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.Vortex); //Changed cursed flame dust to vortex dust
					Main.dust[newDust].alpha = Projectile.alpha;
					Main.dust[newDust].position.X = x2;
					Main.dust[newDust].position.Y = y2;
					Main.dust[newDust].velocity *= 0f;
					Main.dust[newDust].noGravity = true;
				}
			}
			float num166 = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
			float num167 = Projectile.localAI[0];
			if (num167 == 0f)
			{
				Projectile.localAI[0] = num166;
				num167 = num166;
			}
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			float num168 = Projectile.position.X;
			float num169 = Projectile.position.Y;
			float num170 = 300f;
			bool flag4 = false;
			int num171 = 0;
			if (Projectile.localAI[1] == 0f)
			{
				for (int num172 = 0; num172 < 200; num172++)
				{
					if (Main.npc[num172].CanBeChasedBy(this) && (Projectile.localAI[1] == 0f || Projectile.localAI[1] == (float)(num172 + 1)))
					{
						float num173 = Main.npc[num172].position.X + (float)(Main.npc[num172].width / 2);
						float num174 = Main.npc[num172].position.Y + (float)(Main.npc[num172].height / 2);
						float num175 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num173) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num174);
						if (num175 < num170 && Collision.CanHit(new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)(Projectile.height / 2)), 1, 1, Main.npc[num172].position, Main.npc[num172].width, Main.npc[num172].height))
						{
							num170 = num175;
							num168 = num173;
							num169 = num174;
							flag4 = true;
							num171 = num172;
						}
					}
				}
				if (flag4)
				{
					Projectile.localAI[1] = num171 + 1;
				}
				flag4 = false;
			}
			if (Projectile.localAI[1] > 0f)
			{
				int num176 = (int)(Projectile.localAI[1] - 1f);
				if (Main.npc[num176].active && Main.npc[num176].CanBeChasedBy(this, ignoreDontTakeDamage: true) && !Main.npc[num176].dontTakeDamage)
				{
					float num177 = Main.npc[num176].position.X + (float)(Main.npc[num176].width / 2);
					float num178 = Main.npc[num176].position.Y + (float)(Main.npc[num176].height / 2);
					if (Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num177) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num178) < 1000f)
					{
						flag4 = true;
						num168 = Main.npc[num176].position.X + (float)(Main.npc[num176].width / 2);
						num169 = Main.npc[num176].position.Y + (float)(Main.npc[num176].height / 2);
					}
				}
				else
				{
					Projectile.localAI[1] = 0f;
				}
			}
			if (!Projectile.friendly)
			{
				flag4 = false;
			}
			if (flag4)
			{
				float num179 = num167;
				Vector2 vector25 = new(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num180 = num168 - vector25.X;
				float num181 = num169 - vector25.Y;
				float num182 = (float)Math.Sqrt(num180 * num180 + num181 * num181);
				num182 = num179 / num182;
				num180 *= num182;
				num181 *= num182;
				int num183 = 8;
				Projectile.velocity.X = (Projectile.velocity.X * (float)(num183 - 1) + num180) / (float)num183;
				Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num183 - 1) + num181) / (float)num183;
			}
			//Rotations
			// Set the rotation to face the current trajectory:
			//projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			// Or, this version is easier to read:
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
}
