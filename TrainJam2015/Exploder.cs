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

			if (a.IsUnstable) {
				a.Explode ();
			}

			if (b.IsUnstable) {
				b.Explode ();
			}
		}

		public override void PreSolve (b2Contact contact, Box2D.Collision.b2Manifold oldManifold)
		{
		}

		public override void PostSolve (b2Contact contact, ref b2ContactImpulse impulse)
		{
		}
	}
}

