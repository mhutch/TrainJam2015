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
using System;

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

		public Particle KeyParticle { get { return keyParticle; } }

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			world = new b2World (b2Vec2.Zero) {
				AllowSleep = false,
			};
			world.SetContinuousPhysics (true);

			world.SetContactListener (new Exploder ());
			world.SetContactFilter (new ContactFilter ());

			keyParticle = AddParticle (ParticleData.BN, screenSize.Center + new CCPoint (-300, 0), new b2Vec2 (200, 0));

			SpawnParticles (Consts.MinParticles, ConstructScreenRect (0.2f));

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
			// how many screen sizes away a particles gets before it's culled
			const float cullRange = 0.5f;
			var cullMask = ConstructScreenRect (cullRange);
			for (int i = 0; i < Children.Count; ) {
				if (cullMask.ContainsPoint (Children [i].Position)) {
					i++;
				} else {
					var child = Children [i];
					if (child == keyParticle)
						throw new Exception ();
					var p = child as Particle;
					if (p != null) {
						p.Destroy ();
					} else {
						RemoveChild (child);
					}
				}
			}

			foreach (var child in Children) {
				child.Update (dt);
			}

			//replace culled particles with random spawn
			var replaceCount = Consts.MinParticles - world.BodyCount;
			if (replaceCount > 0) {
				SpawnParticlesAroundEdges (replaceCount, ConstructScreenRect (0.1f));
			}
		}

		public CCRect ConstructScreenRect (float expandFactor)
		{
			var centerX = keyParticle.Position.X;
			var centerY = keyParticle.Position.Y;

			var width = screenSize.Width * (expandFactor * 2 + 1);
			var height = screenSize.Height * (expandFactor * 2 + 1);

			return new CCRect (
				centerX - width /2f, centerY - height /2f,
				width, height
			);
		}

		//TODO: min distance from existing particles?
		void SpawnParticles (int count, CCRect include)
		{
			for (int i = 0; i < count; i++) {
				var x = CCRandom.GetRandomFloat (include.MinX, include.MaxX);
				var y = CCRandom.GetRandomFloat (include.MinY, include.MaxY);
				var vx = CCRandom.GetRandomFloat (-200f, 200f);
				var vy = CCRandom.GetRandomFloat (-200f, -200f);
				var data = ParticleData.GetRandomParticle ();
				AddParticle (data, new CCPoint (x, y), new b2Vec2 (vx, vy));
			}
		}

		void SpawnParticlesAroundEdges (int count, CCRect include)
		{
			for (int i = 0; i < count; i++) {
				float minx = include.MinX, maxx = include.MaxX, miny = include.MinY, maxy = include.MaxY;
				float minvx = -200f, maxvx = 200f, minvy = -200f, maxvy = 200f;
				switch (CCRandom.GetRandomInt (0, 3)) {
				//LEFT
				case 0:
					maxx = include.MinX;
					minvx = 0f;
					minvy *= 0.2f;
					maxvy *= 0.2f;
					break;
				//RIGHT
				case 1:
					minx = include.MaxX;
					maxvx = 0f;
					minvy *= 0.2f;
					maxvy *= 0.2f;
					break;
				//TOP
				case 2:
					miny = include.MaxX;
					maxvy = 0f;
					minvx *= 0.2f;
					maxvx *= 0.2f;
					break;
				//BOTTOM
				case 3:
					maxy = include.MinY;
					minvy = 0f;
					minvx *= 0.2f;
					maxvx *= 0.2f;
					break;
				default:
					throw new InvalidOperationException ();
				}

				var x = CCRandom.GetRandomFloat (minx, maxx);
				var y = CCRandom.GetRandomFloat (miny, maxy);
				var vx = CCRandom.GetRandomFloat (minvx, maxvx);
				var vy = CCRandom.GetRandomFloat (minvy, maxvy);
				var data = ParticleData.GetRandomParticle ();

				AddParticle (data, new CCPoint (x, y), new b2Vec2 (vx, vy));
			}
		}

		public float FieldStrength { get; set; }
	}
}

