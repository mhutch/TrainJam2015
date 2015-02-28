//
// Bubble.cs
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

namespace TrainJam2015
{
	//tried using a CC particle system but can't get it to behave how I want
	sealed class Bubble : CCSprite
	{
		const float lifetime = 6f;
		const float positionVariance = Consts.BaseParticleSize / 2f;

		static CCTexture2D bubble;

		public Bubble (CCPoint position) : base (GetBubbleTexture ())
		{
			position.X += CCRandom.GetRandomFloat (-positionVariance, positionVariance);
			position.Y += CCRandom.GetRandomFloat (-positionVariance, positionVariance);
			Position = position;
			Scale = 0f;
			BubblesCount++;
		}

		static CCTexture2D GetBubbleTexture ()
		{
			return bubble ?? (bubble = new CCTexture2D ("trail"));
		}

		public const float BubblesMax = 50;
		public static float BubblesCount = 0;

		float age;

		public override void Update (float dt)
		{
			age += dt;
			if (age > lifetime) {
				Parent.RemoveChild (this);
				BubblesCount--;
				return;
			}

			float maxScale = Consts.BaseParticleSize / ContentSize.Width;

			float scale = age / lifetime;

			Scale = scale * maxScale;
			Opacity = (byte) (255 * (1 - scale * scale));
		}
	}
}
