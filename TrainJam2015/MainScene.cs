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

namespace TrainJam2015
{
	public class MainScene : CCScene
	{
		readonly CloudChamber cloudChamber;
		readonly CCSprite fieldIndicator;

		CCKeyboardState? lastKeyboardState;

		public MainScene (CCWindow mainWindow) : base (mainWindow)
		{
			cloudChamber = new CloudChamber (mainWindow.WindowSizeInPixels);
			AddChild (cloudChamber);

			AddEventListener (new CCEventListenerKeyboard {
				OnKeyPressed = OnKeyEvent,
				OnKeyReleased = OnKeyEvent
			});

			fieldIndicator = new CCSprite ("field-indicator");
			UpdateFieldIndicator ();
			AddChild (fieldIndicator);

			Schedule (Tick);
		}

		public override void OnEnter ()
		{
			base.OnEnter ();
			SoundPlayer.PlayMusic ();
		}

		public override void OnExit ()
		{
			base.OnExit ();
			SoundPlayer.StopMusic ();
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
			if (kb.IsKeyDown (CCKeys.Down) || kb.IsKeyDown (CCKeys.Left)) {
				dt = -dt;
			} else if (!(kb.IsKeyDown (CCKeys.Up) || kb.IsKeyDown (CCKeys.Right))) {
				return;
			}

			var strength = cloudChamber.FieldStrength + Consts.FieldChangeRate * dt;
			cloudChamber.FieldStrength = CCMathHelper.Clamp (strength, -1, 1);

			UpdateFieldIndicator ();
		}

		void UpdateFieldIndicator ()
		{
			bool negative = cloudChamber.FieldStrength < 0;

			const float scale = 4;
			float unitField = (float) Math.Abs (cloudChamber.FieldStrength);
			fieldIndicator.ScaleX = unitField * scale;

			float x = fieldIndicator.ContentSize.Width * scale;
			if (negative) {
				x -= fieldIndicator.ScaledContentSize.Width / 2;
			} else {
				x += fieldIndicator.ScaledContentSize.Width / 2;
			}

			fieldIndicator.Position = new CCPoint (x, 0);
		}
	}
}

