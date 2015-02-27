//
// MainScene.cs
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
using CocosSharp;
using Microsoft.Xna.Framework;

namespace TrainJam2015
{
	public class MainScene : CCScene
	{
		//for some reason starting faster than 100 breaks stuff
		const float startSpeed = 100f;
		const float fieldScaleRate = 0.5f; // field units per second
		const float fieldMax = 10f;

		readonly CloudChamber cloudChamber;

		CCKeyboardState? lastKeyboardState;

		public MainScene (CCWindow mainWindow) : base (mainWindow)
		{
			var resolution = new CCSize (
				mainWindow.WindowSizeInPixels.Width,
				mainWindow.WindowSizeInPixels.Height
			);

			cloudChamber = new CloudChamber (resolution);
			AddChild (cloudChamber);

			AddEventListener (new CCEventListenerKeyboard {
				OnKeyPressed = OnKeyEvent,
				OnKeyReleased = OnKeyEvent
			});

			Schedule (Tick);
		}

		void Tick (float dt)
		{
			UpdateField (dt);
		}

		void OnKeyEvent (CCEventKeyboard evt)
		{
			lastKeyboardState = evt.KeyboardState;

			if (evt.KeyboardState.IsKeyDown (CCKeys.Escape)) {
				AppDelegate.Application.ExitGame ();
			}
		}

		void UpdateField (float dt)
		{
			if (!lastKeyboardState.HasValue)
				return;

			var kb = lastKeyboardState.Value;
			if (kb.IsKeyDown (CCKeys.Down)) {
				dt = -dt;
			} else if (!kb.IsKeyDown (CCKeys.Up)) {
				return;
			}

			var strength = cloudChamber.FieldStrength + fieldScaleRate * dt;
			cloudChamber.FieldStrength = CCMathHelper.Clamp (strength, -fieldMax, fieldMax);
		}
	}
}

