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

namespace TrainJam2015
{
	public class CloudChamber : CCLayer
	{
		b2World world;
		CCSize screenSize;

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

			//TEST PARTICLES
			//for some reason starting faster than 100 physics units per second breaks stuff

			var p = new Particle (world, new CCPoint (screenSize.Center), new b2Vec2 (20, 0));
			AddChild (p);

			var p2 = new Particle (world, new CCPoint (screenSize.Center.X - 300, screenSize.Center.Y), new b2Vec2 (100, 0));
			p2.Charge = 3f;
			AddChild (p2);

			var p3 = new Particle (world, new CCPoint (screenSize.Center.X - 400, screenSize.Center.Y), new b2Vec2 (100, 0));
			p3.Charge = -2f;
			AddChild (p3);

			Schedule (Tick);
		}

		void Tick (float dt)
		{
			world.Step (dt, 8, 1);

			for (var b = world.BodyList; b != null; b = b.Next) {
				if (b.UserData != null) {
					var node = ((CCNode)b.UserData);
					node.Position = new CCPoint (b.Position.x, b.Position.y) * Consts.PhysicsScale;
					node.Rotation = -1 * CCMacros.CCRadiansToDegrees(b.Angle);
					var p = node as Particle;
					if (p != null) {
						p.UpdateMagneticField (FieldStrength);
					}
				}
			}
		}

		public float FieldStrength { get; set; }
	}
}

