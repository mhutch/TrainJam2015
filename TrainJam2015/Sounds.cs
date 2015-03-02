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
using CocosSharp;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace TrainJam2015
{
	public static class SoundPlayer
	{
		const string musicName = "chip4-2";

		static Song song;
		static SoundEffect[] effects;

		//CCSimpleAudioEngine is broken on Windows
		//and MediaPlayer/SoundEffect are broken on Mac
		//yay
		public static void PreloadSounds()
		{
			#if MAC
			foreach (var name in Enum.GetNames (typeof (Sound))) {
				CCSimpleAudioEngine.SharedEngine.PreloadEffect (GetPath (name));
			}
			CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic (musicName);
			#else
			var cm = new ContentManager (CCContentManager.SharedContentManager.ServiceProvider) { RootDirectory = "Content" };
			song = cm.Load<Song> (GetPath (musicName));

			var effectIds = Enum.GetValues ((typeof(Sound)));
			effects = new SoundEffect[effectIds.Length];
			foreach (Sound val in effectIds) {
				effects [(int)val] = cm.Load<SoundEffect> (GetPath (val.ToString ()));
			}
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
			#if MAC
			CCSimpleAudioEngine.SharedEngine.PlayEffect (GetPath (sound.ToString().ToLower()));
			#else
			effects [(int)sound].Play (0.2f, 0f, 0f);
			#endif
		}

		public static void PlayMusic ()
		{
			#if MAC
			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic (GetPath (musicName), true);
			#else
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play (song);
			#endif
		}

		public static void StopMusic ()
		{
			#if MAC
			CCSimpleAudioEngine.SharedEngine.StopBackgroundMusic ();
			#else
			MediaPlayer.Stop ();
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

