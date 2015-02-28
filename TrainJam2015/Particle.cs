//
// Particle.cs
//
// Author:
//       Michael Hutchinson <m.j.hutchinson@gmail.com>
//
// Copyright (c) 2015 Michael Hutchinson
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using CocosSharp;
using Box2D.Dynamics;
using Box2D.Common;
using Box2D.Collision.Shapes;
using System;

namespace TrainJam2015
{
	class Particle : CCSprite
	{
		b2Vec2 initialVelocity;
		readonly b2World world;

		public Particle (b2World world, ParticleData data, CCPoint position, b2Vec2 initialVelocity = default (b2Vec2))
		{
			Data = data;
			this.world = world;
			this.initialVelocity = initialVelocity;
			Position = position;
			AnchorPoint = CCPoint.AnchorMiddle;
		}

		public ParticleData Data { get; private set; }

		public float Age { get; set; }

		public b2Body Body { get; private set; }

		public float Charge { get { return Data.Charge; } }
		public float Mass { get { return Data.Mass; } }
		public bool IsUnstable { get { return Data.IsUnstable; } }

		protected override void AddedToScene ()
		{
			var imageName = Data.Charge < 0? "particle-negative" : "particle";
			Texture = CCTextureCache.SharedTextureCache.AddImage (imageName);
			ContentSize = Texture.ContentSizeInPixels;

			var scale = Consts.BaseParticleSize / ContentSize.Width;

			Scale = scale;

			DisplayedColor = Data.Color;
			UpdateColor ();

			var bodyDef = new b2BodyDef {
				type = b2BodyType.b2_dynamicBody,
				position = new b2Vec2 (Position.X, Position.Y) / Consts.PhysicsScale,
				linearVelocity = initialVelocity / Consts.PhysicsScale
			};

			var fixtureDef = new b2FixtureDef {
				shape = new b2CircleShape {
					Radius = (ScaledContentSize.Width / Consts.PhysicsScale) * 0.5f * 0.68f
				},
				density = 0.0f,
				friction = 0.0f,
				restitution =  1.0f,
			};

			Body = world.CreateBody (bodyDef);
			Body.GravityScale = 0;
			Body.UserData = this;
			Body.CreateFixture (fixtureDef);

			// override mass, we don't care about density
			Body.SetMassData (new b2MassData { mass = Mass });
		}

		public CloudChamber Chamber {
			get { return (CloudChamber) Parent; }
		}

		public void Destroy ()
		{
			RemoveFromParent ();
			world.DestroyBody (Body);
			Body.UserData = null;
			Body = null;
		}

		Bubble lastBubble;

		public override void Update (float dt)
		{
			const float bubbleSpacing = Consts.BaseParticleSize * 0.7f;

			Age += dt;

			if (lastBubble == null || (lastBubble.Position - Position).Length > bubbleSpacing) {
				if (Chamber.KeyParticle == this || Bubble.BubblesCount < Bubble.BubblesMax) {
					lastBubble = new Bubble (Position);
					Parent.AddChild (lastBubble, -10);
				}
			}

			UpdateMagneticField (Chamber.FieldStrength);

			ApplySpeedLimit ();
		}

		void UpdateMagneticField (float field)
		{
			if (Body == null) {
				return;
			}

			Body.Force = b2Vec2.Zero;
			var force = field * Body.LinearVelocity.UnitCross () * Charge * Consts.FieldScale;

			Body.ApplyForceToCenter (force);
		}

		void ApplySpeedLimit ()
		{
			var speed = Body.LinearVelocity.Length;
			if (speed > Consts.SpeedHardCap) {
				Body.LinearVelocity *= Consts.SpeedSoftCap / speed;
			} else if (speed > Consts.SpeedSoftCap) {
				Body.LinearVelocity *= 0.9f;
			}
		}
	}
}

