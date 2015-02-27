//
// Exploder.cs
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

using System;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;
using Box2D.Collision;
using CocosSharp;
using Box2D.Common;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TrainJam2015
{
	public class Exploder : b2ContactListener
	{
		public override void BeginContact (b2Contact contact)
		{
			var a = (Particle) contact.FixtureA.Body.UserData;
			var b = (Particle) contact.FixtureB.Body.UserData;

			if (a == null || b == null) {
				return;
			}

			// sanity check to stop world from exploding
			if (contact.FixtureA.Body.World.BodyCount > 100) {
				return;
			}

			if (a.IsUnstable || b.IsUnstable) {
				Explode (a, b);
			}
		}

		void Explode (Particle a, Particle b)
		{
			if (a.IsUnstable) {
				CreateChildren (a, b);
			}

			if (b.IsUnstable) {
				CreateChildren (b, a);
			}

			if (a.IsUnstable) {
				a.Destroy ();
			}

			if (b.IsUnstable) {
				b.Destroy ();
			}
		}

		void CreateChildren (Particle a, Particle b)
		{
			var v = a.Body.LinearVelocity;
			var children = a.Data.Children;

			var momentum = a.Body.LinearVelocity * a.Mass;

			var combinedVel = (a.Body.LinearVelocity * a.Mass + b.Body.LinearVelocity * b.Mass) / (a.Mass + b.Mass);
			combinedVel.Normalize ();

			const float halfPi = CCMathHelper.Pi / 2f;
			var direction = CCVector2.AngleOf (new CCVector2 (combinedVel.x, combinedVel.y));
			direction += halfPi;

			float rotationPerChild = CCMathHelper.TwoPi / (float)children.Length;

			var combinedChildMass = children.Sum (c => c.Mass);
			var momentumPerUnitChildMass = momentum / combinedChildMass;

			foreach (var c in children) {
				var dirVector = new b2Vec2 ((float)Math.Cos (direction), (float)Math.Sin (direction));
				dirVector.Normalize ();

				//TODO: consistent energy and mass conservation for children
				const float childSpeed = 200f;

				a.Chamber.AddParticle (c, a.Position, momentumPerUnitChildMass * c.Mass + dirVector * childSpeed);
				direction += rotationPerChild;
			}
		}

		public override void PreSolve (b2Contact contact, b2Manifold oldManifold)
		{
		}

		public override void PostSolve (b2Contact contact, ref b2ContactImpulse impulse)
		{
		}
	}
}

