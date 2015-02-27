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
using Microsoft.Xna.Framework;

namespace TrainJam2015
{
	public class CloudChamber : CCLayer
	{
		//for some reason starting faster than 100 breaks stuff
		const float startSpeed = 100f;
		const float fieldScaleRate = 0.5f; // field units per second
		const float fieldMax = 10f;
		
		b2World world;
		CCSize screenSize;
		CCKeyboardState? lastKeyboardState;
		float fieldStrength;

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

			Color = new CCColor3B (220, 230, 255);

			AddEventListener (new CCEventListenerKeyboard {
				OnKeyPressed = OnKeyEvent,
				OnKeyReleased = OnKeyEvent
			});

			var p = new Particle (world, new CCPoint (screenSize.Center), new b2Vec2 (100, 0));
			AddChild (p);

			Schedule (Tick);
		}

		void Tick (float dt)
		{
			UpdateField (dt);

			world.Step (dt, 8, 1);

			for (var b = world.BodyList; b != null; b = b.Next) {
				if (b.UserData != null) {
					var node = ((CCNode)b.UserData);
					node.Position = new CCPoint (b.Position.x, b.Position.y);
					node.Rotation = -1 * CCMacros.CCRadiansToDegrees(b.Angle);
					UpdateMagneticForce (b);
				}
			}

		}

		void UpdateMagneticForce (b2Body body)
		{
			var particle = body.UserData as Particle;
			if (particle == null) {
				return;
			}
			body.Force = b2Vec2.Zero;
			var force = fieldStrength * body.LinearVelocity.UnitCross ();
			body.ApplyForceToCenter (force);
		}

		void OnKeyEvent (CCEventKeyboard evt)
		{
			lastKeyboardState = evt.KeyboardState;
		}

		void UpdateField (float dt)
		{
			if (!lastKeyboardState.HasValue)
				return;

			var kb = lastKeyboardState.Value;
			if (kb.IsKeyDown (CCKeys.Down)) {
				fieldStrength = MathHelper.Max (-fieldMax, fieldStrength - fieldScaleRate * dt);
			} else if (kb.IsKeyDown (CCKeys.Up)) {
				fieldStrength = MathHelper.Min (fieldMax, fieldStrength + fieldScaleRate * dt);
			}
		}
	}
}

