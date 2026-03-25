using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RijamsMod.Items.Dyes;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.RGB;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Buffs.Debuffs
{
	public class Tagged : ModBuff
	{
		/// <summary> 0.1f </summary>
		public static readonly float TagDamage = 0.1f;
		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsATagBuff[Type] = true;
			Main.debuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<TaggedDebuffNPC>().markedByTAG = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<TaggedDebuffPlayer>().markedByTAG = true;
		}
	}

	public class TaggedDebuffNPC : GlobalNPC
	{
		// This is required to store information on entities that isn't shared between them.
		public override bool InstancePerEntity => true;

		public bool markedByTAG;

		public override void ResetEffects(NPC npc)
		{
			markedByTAG = false;
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (markedByTAG)
			{
				modifiers.ScalingBonusDamage += Tagged.TagDamage;
			}
		}

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (markedByTAG)
			{
				modifiers.ScalingBonusDamage += Tagged.TagDamage;
			}
		}

		/*
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (markedByTAG && npc.active)
			{
				// drawColor.R = Math.Max(drawColor.R, (byte)200);
				// drawColor.G = Math.Max(drawColor.G, (byte)200);
				// drawColor.B = Math.Max(drawColor.B, (byte)255);
				drawColor.R = 200;
				drawColor.G = 200;
				drawColor.B = 255;
			}
		}
		*/

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (markedByTAG && npc.active)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				// Retrieve reference to shader
				// var outlineShader = GameShaders.Armor.GetShaderFromItemId(ModContent.ItemType<OutlineDye>());
				if (GameShaders.Misc.TryGetValue("RijamsMod:Inline", out MiscShaderData shader))
				{
					// Reset back to default value.
					//outlineShader.UseOpacity(1f);
					shader.UseColor(new Color(0.9f, 0.9f, 1f));
					// Call Apply to apply the shader to the SpriteBatch. Only 1 shader can be active at a time.
					shader.Apply(null);
				}
				return true;
			}
			return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (markedByTAG && npc.active)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			}
			base.PostDraw(npc, spriteBatch, screenPos, drawColor);
		}
	}
	public class TaggedDebuffPlayer : ModPlayer
	{
		public bool markedByTAG;

		public override void ResetEffects()
		{
			markedByTAG = false;
		}

		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (markedByTAG)
			{
				modifiers.FinalDamage += Tagged.TagDamage;
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (markedByTAG && Player.active)
			{
				// r = Math.Max(r, 0.782f);
				// g = Math.Max(g, 0.782f);
				// b = Math.Max(b, 1f);
				// fullBright = true;
				r = 0.782f;
				g = 0.782f;
				b = 1f;
			}
		}
	}
}