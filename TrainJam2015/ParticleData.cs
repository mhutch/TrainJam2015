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

		public static readonly ParticleData A = new ParticleData (1, 1, CCColor3B.Red, Sound.Tone8);
		public static readonly ParticleData B = new ParticleData (-1, 1, new CCColor3B (80, 120, 255), Sound.Tone4);

		static ParticleData ()
		{
			A.Children = new[] { A, B };
		}
	}
}
