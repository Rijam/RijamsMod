using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles.Magic
{
	public class MiniGusterProj : ModProjectile
	{
		private int mouseDirection = 1;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 8;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return false;
		}

		private class NPCDistanceByIndexComparator : IComparer<Tuple<int, float>>
		{
			public int Compare(Tuple<int, float> npcIndex1, Tuple<int, float> npcIndex2) => npcIndex1.Item2.CompareTo(npcIndex2.Item2);
		}

		private static List<Tuple<int, float>> _miniGusterTargetList = new();
		private static NPCDistanceByIndexComparator _miniGusterTargetComparer = new();

		// Adapted from the Medusa Head
		public override void AI()
		{
			float hitDistance = 8f * 16f; // 8 tiles or 128 pixels

			Player player = Main.player[Projectile.owner];
			Vector2 positionOffset = Vector2.Zero;
			positionOffset.X = player.direction == 1f ? -6f : -14f;
			positionOffset.Y = player.gravDir == 1f ? -25f : 0f;
			bool attacking = Projectile.ai[0] > 0f;
			if (!player.dead)
			{
				Projectile.timeLeft = 3;
			}

			if (Projectile.ai[0] > 0f)
			{ 
				Projectile.ai[0] -= 1f;
			}

			Projectile.frameCounter++;
			if (attacking)
			{
				if (Projectile.frame < 4)
				{
					Projectile.frame = 4;
				}

				if (Projectile.frame >= 8)
				{
					Projectile.frame = 4;
				}

				if (Projectile.frameCounter >= 5)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
					if (Projectile.frame >= 8)
					{
						Projectile.frame = 4;
					}
				}
			}
			else if (Projectile.frameCounter >= 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
			if (Main.myPlayer == Projectile.owner && Main.netMode != NetmodeID.Server)
			{
				/*Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - playerCenter;
				if (player.gravDir == -1f)
				{
					mousePos.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - playerCenter.Y;
				}

				Vector2 vectorDirection = new(Math.Sign((mousePos.X == 0f) ? ((float)player.direction) : mousePos.X), 0f);
				if (vectorDirection.X != Projectile.velocity.X || vectorDirection.Y != Projectile.velocity.Y)
				{
					Projectile.netUpdate = true;
				}
				mouseDirection = (int)vectorDirection.X;*/
				mouseDirection = Main.MouseWorld.X > playerCenter.X ? 1 : -1;
			}

			//mouseDirection = Main.MouseWorld.X > playerCenter.X ? 1 : -1;

			Projectile.velocity = new Vector2(mouseDirection, 0);
			Projectile.spriteDirection = Projectile.direction = mouseDirection;
			Projectile.netUpdate = true;

			//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("mouseDirection " + mouseDirection + " Projectile.velocity " + Projectile.velocity + " Projectile.direction " + Projectile.direction + " player.direction " + player.direction), Color.Green);

			if (attacking && Projectile.soundDelay == 0)
			{
				ModContent.GetInstance<RijamsMod>().PlayNetworkSound(SoundID.Item45 with { Pitch = -1f }, Projectile.Center, Main.player[Main.myPlayer]);
				ModContent.GetInstance<RijamsMod>().PlayNetworkSound("Terraria/Sounds/Custom/dd2_dark_mage_attack_0", volume: 1f, pitch: -1f, Projectile.Center, Main.player[Main.myPlayer]);
			}

			Projectile.soundDelay = (attacking ? 2 : 0);
			if (Main.myPlayer == Projectile.owner)
			{
				Vector2 projPosition = player.MountedCenter + new Vector2(player.direction * 4, player.gravDir * 2f);
				if (!player.channel)
				{
					Projectile.Kill();
					return;
				}

				if (!attacking || Projectile.ai[0] % 15f == 0f)
				{
					bool canAttack = false;
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC nPC = Main.npc[i];
						if (nPC.active && Projectile.Distance(nPC.Center) < hitDistance && nPC.CanBeChasedBy(this) && Collision.CanHitLine(nPC.position, nPC.width, nPC.height, projPosition, 0, 0))
						{
							canAttack = true;
							break;
						}
					}

					if (canAttack && player.CheckMana(player.inventory[player.selectedItem], pay: false)) // Pay mana later
					{
						int projDamage = Projectile.damage;
						projPosition = Projectile.Center;
						float gustRotation = 0f;
						_miniGusterTargetList.Clear();
						for (int j = 0; j < Main.maxNPCs; j++)
						{
							NPC searchNPC = Main.npc[j];
							float distanceToNPC = Projectile.Distance(searchNPC.Center);
							if (searchNPC.active && distanceToNPC < hitDistance && searchNPC.CanBeChasedBy(this) && Collision.CanHitLine(searchNPC.position, searchNPC.width, searchNPC.height, projPosition, 0, 0))
								_miniGusterTargetList.Add(Tuple.Create(j, distanceToNPC));
						}

						_miniGusterTargetList.Sort(_miniGusterTargetComparer);
						for (int k = 0; k < _miniGusterTargetList.Count && k < 1; k++) // k < 3
						{
							Tuple<int, float> tuple = _miniGusterTargetList[k];
							NPC nPC3 = Main.npc[tuple.Item1];
							Vector2 distFromNPCToProj = nPC3.Center - projPosition;

							// Only attack in the direction it is facing.
							// Also avoids issues with knockback being in the wrong direction (hopefully).
							if ((distFromNPCToProj.X < 0 && Projectile.direction == 1) || (distFromNPCToProj.X > 0 && Projectile.direction == -1))
							{
								continue;
							}

							// Pay mana here. So that it doesn't drain mana while it can't actually attack.
							if (player.CheckMana(player.inventory[player.selectedItem], pay: true))
							{
								gustRotation += distFromNPCToProj.ToRotation();
								Projectile gust = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), projPosition, distFromNPCToProj, ModContent.ProjectileType<MiniWindGust>(), 0, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
								gust.direction = Projectile.direction;
								gust.Center = nPC3.Center;
								gust.damage = projDamage;
								gust.Damage();
								gust.damage = 0;
								gust.Center = projPosition;
								if (Main.netMode == NetmodeID.MultiplayerClient)
								{
									NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, gust.whoAmI);
								}
								Projectile.ai[0] = 180f;
							}
						}

						Projectile.ai[0] = 60f;
						Projectile.netUpdate = true;
					}
				}

				Lighting.AddLight(Projectile.Center, 0f, 0f, 0.1f);
			}

			Projectile.rotation = player.fullRotation;
			Projectile.timeLeft = 2;
			Vector2 onHandPos = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
			if (player.direction != 1)
			{
				onHandPos.X = (float)player.bodyFrame.Width - onHandPos.X;
			}

			onHandPos -= (player.bodyFrame.Size() - new Vector2(player.width, 42f)) / 2f;
			Projectile.Center = ((player.HandPosition ?? player.position) + onHandPos + positionOffset - Projectile.velocity).Floor();
			//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("mouseDirection " + mouseDirection + " Projectile.velocity " + Projectile.velocity + " Projectile.direction " + Projectile.direction + " player.direction " + player.direction), Color.Gold);
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((sbyte)mouseDirection);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			mouseDirection = reader.ReadSByte();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			if (Main.player[Projectile.owner].gravDir == -1f)
			{
				spriteEffects |= SpriteEffects.FlipVertically;
			}

			// Get texture of projectile
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			Vector2 origin = sourceRectangle.Size() / 2f;

			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition,
				sourceRectangle, lightColor * 0.85f, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			return false;
		}
	}
}