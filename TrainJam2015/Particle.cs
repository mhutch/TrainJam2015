//
// Particle.cs
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

namespace TrainJam2015
{
	public class Particle : CCNode
	{
		b2World world;
		b2Body body;
		CCSprite sprite;

		public Particle (b2World world, CCPoint position)
		{
			Position = position;
			this.world = world;

			sprite = new CCSprite ("Particle");
			AddChild (sprite);

			var bodyDef = new b2BodyDef ();
			bodyDef.type = b2BodyType.b2_staticBody;
			bodyDef.position.x = position.X;
			bodyDef.position.y = position.Y;

			body = world.CreateBody (bodyDef);
			body.UserData = this;
		}
	}
}

