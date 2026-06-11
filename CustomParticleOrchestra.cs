using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.NetModules;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Net;

namespace RijamsMod
{
	public enum CustomParticleOrchestraType
	{
		/// <summary>
		/// Summons the visuals for the explosive flares explosion. Dusts not included.
		/// <br/>PositionInWorld -> position
		/// <br/>UniqueInfoPiece -> color.PackedValue
		/// <br/>MovementVector -> scale
		/// </summary>
		ExplosiveFlarePop,
		/// <summary>
		/// Copied From 1.4.4 (it was removed in 1.4.5)
		/// <br/>PositionInWorld -> position
		/// <br/>UniqueInfoPiece -> unused
		/// <br/>MovementVector -> unused
		/// </summary>
		AshTreeShake
	}

	/// <summary>
	/// Copied and modified ParticleOrchestrator to allow for custom particle types.
	/// </summary>
	public class CustomParticleOrchestra
	{
		public class CustomNetParticlesModule : NetModule
		{
			public static NetPacket Serialize(CustomParticleOrchestraType particleType, ParticleOrchestraSettings settings)
			{
				NetPacket result = NetModule.CreatePacket<NetParticlesModule>(22);
				result.Writer.Write((byte)particleType);
				settings.Serialize(result.Writer);
				return result;
			}

			public override bool Deserialize(BinaryReader reader, int userId)
			{
				CustomParticleOrchestraType particleOrchestraType = (CustomParticleOrchestraType)reader.ReadByte();
				ParticleOrchestraSettings settings = default;
				settings.DeserializeFrom(reader);
				if (Main.netMode == NetmodeID.Server)
					NetManager.Instance.Broadcast(Serialize(particleOrchestraType, settings), userId);
				else
					CustomParticleOrchestra.SpawnParticlesDirect(particleOrchestraType, settings);

				return true;
			}
		}
		
		/// <summary>
		/// Use this to spawn in a particle.
		/// </summary>
		/// <param name="clientOnly"></param>
		/// <param name="type">Type of the particle</param>
		/// <param name="settings"></param>
		/// <param name="overrideInvokingPlayerIndex"></param>
		public static void RequestParticleSpawn(bool clientOnly, CustomParticleOrchestraType type, ParticleOrchestraSettings settings, int? overrideInvokingPlayerIndex = null)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
			if (overrideInvokingPlayerIndex.HasValue)
				settings.IndexOfPlayerWhoInvokedThis = (byte)overrideInvokingPlayerIndex.Value;

			SpawnParticlesDirect(type, settings);
			if (!clientOnly && Main.netMode == NetmodeID.MultiplayerClient)
				NetManager.Instance.SendToServer(CustomNetParticlesModule.Serialize(type, settings));
		}

		public static void BroadcastParticleSpawn(CustomParticleOrchestraType type, ParticleOrchestraSettings settings)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
			if (!Main.dedServ)
				SpawnParticlesDirect(type, settings);
			else
				NetManager.Instance.Broadcast(CustomNetParticlesModule.Serialize(type, settings));
		}

		public static void BroadcastOrRequestParticleSpawn(CustomParticleOrchestraType type, ParticleOrchestraSettings settings)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
			if (!Main.dedServ)
				SpawnParticlesDirect(type, settings);

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetManager.Instance.SendToServerOrBroadcast(CustomNetParticlesModule.Serialize(type, settings));
		}

		public static void SpawnParticlesDirect(CustomParticleOrchestraType type, ParticleOrchestraSettings settings)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				switch (type)
				{
					case CustomParticleOrchestraType.ExplosiveFlarePop:
						Spawn_ExplosiveFlarePop(settings);
						break;
					case CustomParticleOrchestraType.AshTreeShake:
						Spawn_AshTreeShake(settings);
						break;
					default:
						break;
				}
			}
		}

		private static ParticlePool<DrawAllFramesSequentiallyParticle> _poolDrawAllFramesSequentially = new(200, GetNewDrawAllFramesSequentiallyParticle);
		private static ParticlePool<PrettySparkleParticle> _poolPrettySparkle = new(200, GetNewPrettySparkleParticle);

		private static DrawAllFramesSequentiallyParticle GetNewDrawAllFramesSequentiallyParticle() => new();
		private static PrettySparkleParticle GetNewPrettySparkleParticle() => new();

		/// <summary>
		/// Spawns the particle for the explosive flare pop. See also DrawAllFramesSequentiallyParticle.
		/// </summary>
		private static void Spawn_ExplosiveFlarePop(ParticleOrchestraSettings settings)
		{
			Asset<Texture2D> textureAsset = ModContent.Request<Texture2D>("RijamsMod/Projectiles/Ranged/ExplodingFlarePop");
			DrawAllFramesSequentiallyParticle particle = _poolDrawAllFramesSequentially.RequestParticle();
			particle.SetBasicInfo(textureAsset, null, Vector2.Zero, settings.PositionInWorld);
			particle.SetTypeInfo(10, 5, 50);
			particle.DrawColor = new Color() { PackedValue = (uint)settings.UniqueInfoPiece, A = 0 };
			particle.Scale = settings.MovementVector;
			particle.LocalPosition = settings.PositionInWorld;
			Main.ParticleSystem_World_OverPlayers.Add(particle);
		}

		/// <summary>
		/// Copied from 1.4.4 Terraria (it was removed in 1.4.5)
		/// </summary>
		private static void Spawn_AshTreeShake(ParticleOrchestraSettings settings)
		{
			float num = 10f + 20f * Main.rand.NextFloat();
			float num2 = -(float)Math.PI / 4f;
			float num3 = 0.2f + 0.4f * Main.rand.NextFloat();
			Color colorTint = Main.hslToRgb(Main.rand.NextFloat() * 0.1f + 0.06f, 1f, 0.5f);
			colorTint.A /= 2;
			colorTint *= Main.rand.NextFloat() * 0.3f + 0.7f;
			for (float num4 = 0f; num4 < 2f; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
				Vector2 vector = ((float)Math.PI / 4f + (float)Math.PI * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = colorTint;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f * num3;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
				prettySparkleParticle.Velocity = vector * 0.05f;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num4 == 1f)
				{
					prettySparkleParticle.Scale *= 1.5f;
					prettySparkleParticle.Velocity *= 1.5f;
					prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
				}

				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}

			for (float num5 = 0f; num5 < 2f; num5 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
				Vector2 vector2 = ((float)Math.PI / 4f + (float)Math.PI * num5 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(1f, 0.4f, 0.2f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f * num3;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
				prettySparkleParticle2.Velocity = vector2 * 0.05f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num5 == 1f)
				{
					prettySparkleParticle2.Scale *= 1.5f;
					prettySparkleParticle2.Velocity *= 1.5f;
					prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
				}

				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				for (int i = 0; i < 1; i++)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, DustID.Torch, vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust.noGravity = true;
					dust.scale = 1.4f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, DustID.Torch, -vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
		}
	}

	/// <summary>
	/// A particle type the draws all of the frames in the texture sequentially.
	/// <br/><see cref="ABasicParticle.SetBasicInfo(Asset{Texture2D}, Rectangle?, Vector2, Vector2)"/> to set the texture, velocity, and position. Use null for the frame.
	/// <br/><see cref="SetTypeInfo(int, int, float)"/> to set the frame count, the delay between frames, and the total time the particle should live for.
	/// </summary>
	public class DrawAllFramesSequentiallyParticle : ABasicParticle
	{
		public int AnimationFramesAmount;
		public int GameFramesPerAnimationFrame;
		public Color DrawColor = Color.White;
		private float _timeTolive;
		private float _timeSinceSpawn;
		private int _gameFramesCounted;
		private int _frameNumber;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			AnimationFramesAmount = 0;
			GameFramesPerAnimationFrame = 0;
			DrawColor = Color.White;
			_timeTolive = 0f;
			_timeSinceSpawn = 0f;
			_gameFramesCounted = 0;
			_frameNumber = 0;
		}

		public void SetTypeInfo(int animationFramesAmount, int gameFramesPerAnimationFrame, float timeToLive)
		{
			_timeTolive = timeToLive;
			GameFramesPerAnimationFrame = gameFramesPerAnimationFrame;
			AnimationFramesAmount = animationFramesAmount;
			IncrementFrame();
		}

		private void IncrementFrame()
		{
			_frame = _texture.Frame(1, AnimationFramesAmount, 0, _frameNumber++);
			_origin = _frame.Size() / 2f;
		}

		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			_timeSinceSpawn += 1f;
			if (_timeSinceSpawn >= _timeTolive)
				base.ShouldBeRemovedFromRenderer = true;

			if (++_gameFramesCounted >= GameFramesPerAnimationFrame)
			{
				_gameFramesCounted = 0;
				IncrementFrame();
			}
		}

		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 position = settings.AnchorPosition + LocalPosition;

			spritebatch.Draw(_texture.Value, position, _frame, DrawColor, Rotation, _origin, Scale, SpriteEffects.None, 0f);
		}
	}
}
