 //
// Sounds.cs
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
using CocosDenshion;

namespace TrainJam2015
{
	public static class SoundPlayer
	{
		const string musicName = "chip4-2";

		public static void PreloadSounds()
		{
			foreach (var name in Enum.GetNames (typeof (Sound))) {
				CCSimpleAudioEngine.SharedEngine.PreloadEffect (GetPath (name));
			}
			#if MAC
			CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic (musicName);
			#endif
		}

		static string GetPath (string name)
		{
			// Currently build tasks don't place items into subdirectory when they come from a shproj
			#if false
			return System.IO.Path.Combine ("Audio", name);
			#else
			return name;
			#endif
		}

		public static void Play (this Sound sound)
		{
			CCSimpleAudioEngine.SharedEngine.PlayEffect (GetPath (sound.ToString().ToLower()));
		}

		public static void PlayMusic ()
		{
			#if MAC
			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic (GetPath (musicName), true);
			#endif
		}

		public static void StopMusic ()
		{
			#if MAC
			CCSimpleAudioEngine.SharedEngine.StopBackgroundMusic ();
			#endif
		}
	}

	public enum Sound
	{
		a_sharp,
		b_natural,
		c_sharp,
		d_sharp,
		f_sharp
	}
}

