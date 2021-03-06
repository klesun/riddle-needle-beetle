﻿using System.Collections.Generic;
using Util;
using Util.Shorthands;

namespace Assets.Scripts.Util.Shorthands
{
    /**
     * S stands for the "Shortcut"
     * i may lack knowledge of how to code in C#, but writing
     *     var list = new List<Abracadabra<Piu.Piu.Ololo, DumDum<Yachatta>>>(someInferencableArray);
     * instead of just say
     *     var list = new List<>(someInferencableArray);
     * pissed me off enough to make me write this class that
     * contains shortcuts to Dict, List and maybe more constructors
     */
    public static class S
    {
        public static L<Tf> L<Tf>(IEnumerable<Tf> subj)
        {
            return new L<Tf> (subj);
        }

        public static List<T> List<T>(IEnumerable<T> someArray) {
            return new List<T> (someArray);
        }

        public static Queue<T> Queue<T>(IEnumerable<T> someArray) {
            return new Queue<T> (someArray);
        }

        public static T4<T> T4<T>(T a, T b, T c, T d) {
            return new T4<T>(a,b,c,d);
        }

        public static Opt<T> Opt<T>(T value = default(T))
        {
            return value == null
                ? new Opt<T>(false, default(T))
                : new Opt<T>(true, value);
        }
    }
}

