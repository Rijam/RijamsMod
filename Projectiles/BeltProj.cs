using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace RijamsMod.Projectiles
{
    //Adapted from Whips & More
    public class BeltProj : ModProjectile
    {
        public Vector2 chainHeadPosition;
        public float firingSpeed;
        public float firingAnimation;
        public float firingTime;
        //public int summonTagCrit;

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            //summonTagCrit = 12;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            // Face the projectile towards its movement direction, offset by 90 degrees counterclockwise because the sprite faces downward.
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);

            // Constantly set the chain's timeLeft to 2 so that it doesn't die.
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

            // Makes some dust and light.
            /*for (int i = 0; i < 3; i++)
            {
                // This velocity vector sends the smoke out at a speed of 1 upward, then rotated randomly across all 360 degrees.
                Vector2 velocity = new Vector2(0f, -1f).RotatedByRandom(MathHelper.ToRadians(360f));
                int selectRand = Utils.SelectRandom(Main.rand, DustID.GreenTorch, DustID.RedTorch);
                Dust dust1 = Dust.NewDustPerfect(chainHeadPosition, selectRand, velocity, 150, Color.White, 1.5f);
                dust1.noGravity = true;
            }*/
            // Like with the explosion projectile's lighting, Color.White can be any color you want, and 0.4f at the end is a radius multiplier.
            //Lighting.AddLight(chainHeadPosition, Color.White.ToVector3() * 0.2f);

            // Use one of the projectile's localAI slot as a cooldown timer for spawning explosions. When an explosion is spawned, this gets set to 4, so it takes 4 ticks to reach 0 again.
            if (projectile.localAI[1] > 0f)
            {
                projectile.localAI[1] -= 1f;
            }

            // The projectile's swerving motion.

            // If this localAI slot is 0, meaning it doesn't have an assigned value, then set it to the projectile's rotation so that we can get the rotation it had on its first tick of being spawned.
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = projectile.rotation;
            }

            // If localAI[0] (the localAI slot we use to store initial rotation)'s X value is greater than 0, then direction is 1. Otherwise, -1.
            float direction = (projectile.localAI[0].ToRotationVector2().X >= 0f).ToDirectionInt();

            // Use a sine calculation to rotate the Solar Eruption around to form an ovular motion.
            Vector2 rotation = (direction * (projectile.ai[0] / firingAnimation * MathHelper.ToRadians(360f) + MathHelper.ToRadians(-90f))).ToRotationVector2();
            rotation.Y *= (float)Math.Sin(projectile.ai[1]);

            rotation = rotation.RotatedBy(projectile.localAI[0]);

            // Use the ai[0] slot as a timer to increment how long the projectile has been alive.
            projectile.ai[0] += 1f;
            if (projectile.ai[0] < firingTime)
            {
                projectile.velocity += (firingSpeed * rotation).RotatedBy(MathHelper.ToRadians(90f));
            }
            else
            {
                // If past the firingTime variable we set in the item's Shoot() hook, kill it.
                projectile.Kill();
            }

            // Manages the positioning for the chain's handle.
            Vector2 offset = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;

            // Flip the offset horizontally if the player is facing left instead of right.
            if (player.direction == -1)
            {
                offset.X = player.bodyFrame.Width - offset.X;
            }
            // Flip the offset vertically if the player is using gravity (such as a Gravity Globe or Gravitation Potion.)
            if (player.gravDir == -1f)
            {
                offset.Y = player.bodyFrame.Height - offset.Y;
            }
            // This line is a custom offset that you can change to move the handle around. Default is 0f, 0f. This projectile uses 4f, -6f.
            offset += new Vector2(0f, -3f) * new Vector2(player.direction, player.gravDir);
            offset -= new Vector2(player.bodyFrame.Width - projectile.width, player.bodyFrame.Height - 42) * 0.5f;
            projectile.Center = player.RotatedRelativePoint(player.position + offset) - projectile.velocity;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            player.MinionAttackTargetNPC = target.whoAmI;
            //player.AddBuff(ModContent.BuffType<Buffs.LuckyStrike>(), 4 * 60);
            //player.GetModPlayer<WhipPlayer>().summonTagCrit = summonTagCrit;
            //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Whipcrack"), (int)projectile.Center.X, (int)projectile.Center.Y);
            if ((!projectile.usesLocalNPCImmunity || projectile.minion) && target.immune[projectile.owner] == 10)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCImmunity[target.whoAmI] = 10;
                target.immune[projectile.owner] = 0;
            }
            //Solar Eruption source
            /*
            target.AddBuff(189, 300);
            if (projectile.owner == Main.myPlayer)
            {
                if (projectile.damage > 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        if (projectile.localAI[1] <= 0f)
                        {
                            //Solar Explosion
                            //Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, 0f, 0f, 612, projectile.damage, 10f, projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                        }
                        projectile.localAI[1] = 4f;
                        projectile.localNPCImmunity[i] = 6;
                        Main.npc[i].immune[projectile.owner] = 4;
                    }
                }
            }*/
        }
        // Set to true so the projectile can break tiles like grass, pots, vines, etc.
        public override bool? CanCutTiles()
        {
            return true;
        }
        // Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity, (float)projectile.width * projectile.scale, DelegateMethods.CutTiles);
        }
        // Plot a line from the start of the Solar Eruption to the end of it, and check if any hitboxes are intersected by it for the entity collision logic. (Don't change this.)
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Custom collision so all chains across the flail can cause impact.
            // Add addition line of sight check so you can't hit enemies through walls.
            float collisionPoint7 = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity, 16f * projectile.scale, ref collisionPoint7)
                && Collision.CanHitLine(new Vector2(targetHitbox.X, targetHitbox.Y), targetHitbox.Width, targetHitbox.Height, Main.player[projectile.owner].position, Main.player[projectile.owner].width, Main.player[projectile.owner].height))
            {
                return true;
            }
            return false;
        }

        //If you want full bright
        /*public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 200);
        }*/

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = lightColor;

            // Some rectangle presets for different parts of the chain.
            Rectangle chainHandle = new Rectangle(0, 2, texture.Width, 40);
            Rectangle chainLinkEnd = new Rectangle(0, 68, texture.Width, 18);
            Rectangle chainLink = new Rectangle(0, 46, texture.Width, 18);
            Rectangle chainHead = new Rectangle(0, 90, texture.Width, 48);

            // If the chain isn't moving, stop drawing all of its components.
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            // These fields / pre-draw logic have been taken from the vanilla source code for the Solar Eruption.
            // They setup distances, directions, offsets, and rotations all so the chain faces correctly.
            float chainDistance = projectile.velocity.Length() + 16f;
            bool distanceCheck = chainDistance < 100f;
            Vector2 direction = Vector2.Normalize(projectile.velocity);
            Rectangle rectangle = chainHandle;
            Vector2 yOffset = new Vector2(0f, Main.player[projectile.owner].gfxOffY);
            float rotation = direction.ToRotation() + MathHelper.ToRadians(-90f);
            // Draw the chain handle. This is the first piece in the sprite.
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + yOffset, rectangle, color, rotation, rectangle.Size() / 2f - Vector2.UnitY * 4f, projectile.scale, SpriteEffects.None, 0f);
            chainDistance -= 40f * projectile.scale;
            Vector2 position = projectile.Center;
            position += direction * projectile.scale * 24f;
            rectangle = chainLinkEnd;
            if (chainDistance > 0f)
            {
                float chains = 0f;
                while (chains + 1f < chainDistance)
                {
                    if (chainDistance - chains < rectangle.Height)
                    {
                        rectangle.Height = (int)(chainDistance - chains);
                    }
                    // Draws the chain links between the handle and the head. This is the "line," or the third piece in the sprite.
                    spriteBatch.Draw(texture, position - Main.screenPosition + yOffset, rectangle, Lighting.GetColor((int)position.X / 16, (int)position.Y / 16), rotation, new Vector2(rectangle.Width / 2, 0f), projectile.scale, SpriteEffects.None, 0f);
                    chains += rectangle.Height * projectile.scale;
                    position += direction * rectangle.Height * projectile.scale;
                }
            }
            Vector2 chainEnd = position;
            position = projectile.Center;
            position += direction * projectile.scale * 24f;
            rectangle = chainLink;
            int offset = distanceCheck ? 9 : 18;
            float chainLinkDistance = chainDistance;
            if (chainDistance > 0f)
            {
                float chains = 0f;
                float increment = chainLinkDistance / offset;
                chains += increment * 0.25f;
                position += direction * increment * 0.25f;
                for (int i = 0; i < offset; i++)
                {
                    float spacing = increment;
                    if (i == 0)
                    {
                        spacing *= 0.75f;
                    }
                    // Draws the actual chain link spikes between the handle and the head. These are the "spikes," or the second piece in the sprite.
                    spriteBatch.Draw(texture, position - Main.screenPosition + yOffset, rectangle, Lighting.GetColor((int)position.X / 16, (int)position.Y / 16), rotation, new Vector2(rectangle.Width / 2, 0f), projectile.scale, SpriteEffects.None, 0f);
                    chains += spacing;
                    position += direction * spacing;
                }
            }
            rectangle = chainHead;
            // Draw the chain head. This is the fourth piece in the sprite.
            spriteBatch.Draw(texture, chainEnd - Main.screenPosition + yOffset, rectangle, Lighting.GetColor((int)chainEnd.X / 16, (int)chainEnd.Y / 16), rotation, texture.Frame().Top(), projectile.scale, SpriteEffects.None, 0f);
            // Because the chain head's draw position isn't determined in AI, it is set in PreDraw.
            // This is so the smoke-spawning dust and white light are at the proper location.
            chainHeadPosition = chainEnd;

            return false;
        }
    }
}