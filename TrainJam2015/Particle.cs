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

namespace TrainJam2015
{
	public class Particle : CCNode
	{
		b2Body body;
		CCSprite sprite;
		b2Vec2 initialVelocity;
		b2World world;

		public Particle (b2World world, CCPoint position, b2Vec2 initialVelocity = default (b2Vec2))
		{
			this.world = world;
			this.initialVelocity = initialVelocity;
			Position = position;

			Charge = 1;
			Mass = 1;
		}

		public float Charge { get; set; }
		public float Mass { get; set; }

		protected override void AddedToScene ()
		{
			sprite = new CCSprite ("Particle");
			AddChild (sprite);

			var bodyDef = new b2BodyDef {
				type = b2BodyType.b2_dynamicBody,
				position = new b2Vec2 (Position.X, Position.Y) / Consts.PhysicsScale,
				linearVelocity = initialVelocity / Consts.PhysicsScale
			};

			var fixtureDef = new b2FixtureDef {
				shape = new b2CircleShape {
					Radius = (sprite.ContentSize.Width / Consts.PhysicsScale) * 0.5f * 0.68f
				},
				density = 0.0f,
				friction = 0.0f,
				restitution =  1.0f,
			};

			body = world.CreateBody (bodyDef);
			body.GravityScale = 0;
			body.UserData = this;
			body.CreateFixture (fixtureDef);

			// override mass, we don't care about density
			body.SetMassData (new b2MassData { mass = Mass });
		}

		public void UpdateMagneticField (float field)
		{
			body.Force = b2Vec2.Zero;
			var force = field * body.LinearVelocity.UnitCross () * Charge * Consts.FieldScale;
			body.ApplyForceToCenter (force);
		}
	}
}

