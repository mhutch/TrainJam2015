//
// CloudChamber.cs
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
using System.Collections.Generic;

namespace TrainJam2015
{
	class CloudChamber : CCLayer
	{
		b2World world;
		CCSize screenSize;
		readonly Queue<Particle> particlesToAdd = new Queue<Particle> ();
		Particle keyParticle;

		public CloudChamber (CCSize size) : base (size)
		{
			screenSize = size;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			world = new b2World (b2Vec2.Zero) {
				AllowSleep = false,
			};
			world.SetContinuousPhysics (true);

			world.SetContactListener (new Exploder ());
			world.SetContactFilter (new ContactFilter ());

			//TEST PARTICLES
			//for some reason starting faster than 100 physics units per second breaks stuff

			var center = screenSize.Center;

			AddParticle (ParticleData.A, center, new b2Vec2 (50, 0));
			keyParticle = AddParticle (ParticleData.B, center + new CCPoint (-300, 0), new b2Vec2 (200, 0));

			Schedule (Tick);
		}

		public Particle AddParticle (ParticleData data, CCPoint position, b2Vec2 velocity)
		{
			var p = new Particle (world, data, position, velocity);

			if (world.IsLocked) {
				particlesToAdd.Enqueue (p);
			} else {
				AddChild (p);
			}
			return p;
		}

		void Tick (float dt)
		{
			//update physics world
			world.Step (dt, 8, 1);

			//add any objects created during physics update
			while (particlesToAdd.Count > 0) {
				var child = particlesToAdd.Dequeue ();
				AddChild (child);
			}

			//update sprite locations from physics world
			for (var body = world.BodyList; body != null; body = body.Next) {
				var particle = (Particle)body.UserData;
				if (particle == null)
					continue;
				var physicsPos = particle.Body.Position;
				particle.Position = new CCPoint (physicsPos.x, physicsPos.y) * Consts.PhysicsScale;
			}

			//center
			var centerX = keyParticle.Position.X;
			var centerY = keyParticle.Position.Y;

			//recenter camera by transforming layer
			this.AdditionalTransform = new CCAffineTransform (1, 0, 0, 1,
				-centerX + screenSize.Width / 2f,
				-centerY + screenSize.Height / 2f
			);

			//cull children that got too far away
			var cullMask = new CCRect (
				centerX -screenSize.Width, centerY -screenSize.Height,
				screenSize.Width * 2,
				screenSize.Height * 2
			);
			for (int i = 0; i < Children.Count; ) {
				if (cullMask.ContainsPoint (Children [i].Position)) {
					i++;
				} else {
					RemoveChild (Children [i]);
				}
			}

			foreach (var child in Children) {
				child.Update (dt);
			}

			//spawn


		}

		public float FieldStrength { get; set; }
	}
}

