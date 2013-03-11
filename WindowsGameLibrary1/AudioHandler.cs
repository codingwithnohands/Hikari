using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LameChicken
{
    public static class AudioHandler
    {
        #region Fields

        private static AudioEngine _audioEngine;
        private static WaveBank _waveBank;
        private static SoundBank _soundBank;

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static void Initialize()
        {
            _audioEngine = new AudioEngine("Content/Audio/hikari.xgs");
            _waveBank = new WaveBank(_audioEngine, "Content/Audio/waveBank.xwb");
            _soundBank = new SoundBank(_audioEngine, "Content/Audio/soundBank.xsb");
        }

        public static Cue GetCue(string name)
        {
            Cue newCue = _soundBank.GetCue(name);
            return newCue;
        }

        #endregion
    }
}
