//
// ParticleData.cs
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
	class ParticleData
	{
		public ParticleData (float charge, float mass, CCColor3B color, Sound sound)
		{
			if (mass <= 0)
				throw new ArgumentException ("Massless particles not allowed");

			Charge = charge;
			Mass = mass;
			Sound = sound;
			Color = color;
		}

		public float Charge { get; private set; }
		public float Mass { get; private set; }
		public CCColor3B Color { get; private set; }
		public Sound Sound { get; private set; }
		public ParticleData[] Children { get; private set; }

		public bool IsUnstable { get { return Children != null; } }

		public static readonly ParticleData AP = new ParticleData ( 1, 1, new CCColor3B (255, 080, 057), Sound.Tone8);
		public static readonly ParticleData AN = new ParticleData (-1, 1, new CCColor3B (255, 080, 057), Sound.Tone8);
		public static readonly ParticleData BP = new ParticleData ( 2, 1, new CCColor3B (144, 085, 232), Sound.Tone4);
		public static readonly ParticleData BN = new ParticleData (-2, 1, new CCColor3B (144, 085, 232), Sound.Tone4);
		public static readonly ParticleData CP = new ParticleData ( 3, 1, new CCColor3B (022, 236, 255), Sound.Tone5);
		public static readonly ParticleData CN = new ParticleData (-3, 1, new CCColor3B (022, 236, 255), Sound.Tone5);
		public static readonly ParticleData DP = new ParticleData ( 4, 1, new CCColor3B (153, 232, 102), Sound.Tone8);
		public static readonly ParticleData DN = new ParticleData (-4, 1, new CCColor3B (153, 232, 102), Sound.Tone8);
		public static readonly ParticleData EP = new ParticleData ( 1, 2, new CCColor3B (255, 200, 087), Sound.Tone4);
		public static readonly ParticleData EN = new ParticleData (-1, 2, new CCColor3B (255, 200, 087), Sound.Tone4);

		static ParticleData ()
		{
			AP.Children = new[] { CP, DN };
			CP.Children = new[] { EP, AN };
		}

		public static ParticleData GetRandomParticle ()
		{
			switch (CCRandom.GetRandomInt (0, 9)) {
			case 0:
				return AP;
			case 1:
				return AN;
			case 2:
				return BP;
			case 3:
				return BN;
			case 4:
				return CP;
			case 5:
				return CN;
			case 6:
				return DP;
			case 7:
				return DN;
			case 8:
				return EP;
			case 9:
				return EN;
			default:
				throw new InvalidOperationException ();
			}

		}
	}
}
