//
// Consts.cs
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

namespace TrainJam2015
{
	public static class Consts
	{
		//how much bigger the real world is than the physics world
		public const float PhysicsScale = 100;

		//how strong the magnetic field is
		public const float FieldScale = 1;

		//what proportion of range field can change per second
		public const float FieldChangeRate = 0.5f;

		//how long after creation particles are immune to collisions and exploding
		public const float ExplosionImmunityTime = 1.0f;

		//size in pixels of a "normal" particle
		public const float BaseParticleSize = 48f;

		//caps to prevent velocity getting too high too fast
		public const float SpeedSoftCap = 80f;
		public const float SpeedHardCap = 100f;

		public const int MinParticles = 15;
		public const int MaxParticles = 50;
	}
}
