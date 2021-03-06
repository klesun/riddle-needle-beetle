﻿using System;

namespace Util.SoundFontPlayer
{
    /**
     * this class describes the structure of json file generated by
     * @see https://github.com/klesun/shmidusic.lv/blob/master/scripts/sf2parser_adapt.py
    */
    [Serializable]
    public class JsonDefinition
    {
        public Preset[] presets;
    }

    [Serializable]
    public class Preset
    {
        public Instrument instrument;
        // combined with specific instrument values if any
        public Generator generatorApplyToAll;
    }

    [Serializable]
    public class Instrument
    {
        public SampleInfo[] samples;
        public Generator generator;
        // overriden by specific sample values if any
        public Generator generatorApplyToAll;
    }

    [Serializable]
    public class SampleInfo
    {
        public Generator generator;
        public string sampleName;
        // means nothing if "overridingRootKey" is defined in generator
        public int originalPitch;
        // like "44100" or "22500"
        public int sampleRate;
        // divide by sampleRate to get in seconds
        public int startLoop;
        // divide by sampleRate to get in seconds
        public int endLoop;
    }

    [Serializable]
    public class Generator
    {
        // in semitones
        public KeyRange keyRange;
        public int? overridingRootKey;
        // 100 of fineTune = 1 semitone
        public int? fineTune;
        public int? coarseTune;
        // add to sample.startLoop if present
        public int? startloopAddrsOffset;
        // add to sample.endLoop if present
        public int? endloopAddrsOffset;
        // how much volume should be reduced in centibels
        public int? initialAttenuation;
        // BiquadFilterNode::Q * 10
        public int? initialFilterQ;
        // 2 ^ (it / 1200) = BiquadFilterNode::frequency * 0.122322364
        public int? initialFilterFc;
        public int? sampleModes;
    }

    [Serializable]
    public class KeyRange
    {
        public int hi;
        public int lo;
    }
}

