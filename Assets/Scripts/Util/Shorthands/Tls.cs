﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Timers;
using GameLogic;
using Interfaces;
using System.Collections;
using Assets.Scripts.Util.Shorthands;
using Util.Shorthands;

namespace Util
{
    /** singletone for doing stuff */
    public class Tls
    {
        public readonly Timeout timeout;

        private static Tls instance;
        private GameObject dullGameObject = new GameObject ("_Timeout");
        private readonly AudioSource audioSource;
        private IHero hero;

        private Tls ()
        {
            dullGameObject.AddComponent (typeof(Timeout));
            timeout = dullGameObject.GetComponent<Timeout>();

            var audioSourceEl = new GameObject ("_staticAudio", typeof(AudioSource));
            audioSource = audioSourceEl.GetComponent<AudioSource> ();
        }

        public static Tls Inst()
        {
            return instance
                ?? (instance = new Tls());
        }

        public void PlayAudio(AudioClip audio)
        {
            audioSource.PlayOneShot (audio);
        }

        /*
         * get transform of a fake game object for access to
         * methods like "lookAt" to get rotation between two dots
        */
        public Transform DullTransform(Vector3 pos)
        {
            dullGameObject = dullGameObject ?? new GameObject ("_Timeout");
            dullGameObject.transform.position = pos;
            dullGameObject.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
            return dullGameObject.transform;
        }

        public D.Cb Pause()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            return () => {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            };
        }

        /**
         * @return DCallback - call it to trigger the action before timeout
         */
        public D.Cb SetGameTimeout(float seconds, D.Cb callback)
        {
            return timeout.Game (seconds, callback);
        }

        public TimeoutResult Timeout(float seconds)
        {
            return new TimeoutResult { seconds = seconds, timeout = timeout };
        }

        /**
         * be carefull when using this - object can be destroyed when animation
         * is still not don't - you should check `if obj == null return;` in your
         * closure, or you may get a null pointer exception
         */
        public Animated Animate(int steps, float seconds, D.Cu<float> doStep)
        {
            return Animate(steps, seconds, (prog, i) => doStep(prog));
        }

        public Animated Animate(int steps, float seconds, D.Cu2<float, int> doStep)
        {
            var animation = new Animated();
            var startTime = Time.fixedTime;
            var i = 0;
            D.Cb doNext = null;
            doNext = () => {
                if (animation.done) return;
                var progress = (Time.fixedTime - startTime) / seconds;
                progress = Math.Min(progress, 1);
                doStep(progress, i);
                if (i++ < steps) {
                    var nextProgress = i / 1.0f / steps;
                    var tillNext = seconds * Math.Max(nextProgress - progress, 0.001f);
                    SetGameTimeout(tillNext, doNext);
                } else {
                    animation.done = true;
                    animation.thens.each = (cb,_) => cb();
                }
            };
            doNext();
            return animation;
        }

        public bool IsPaused()
        {
            return Time.timeScale == 0;
        }

        public IHero GetHero()
        {
            return hero ?? (hero = UnityEngine.Object.FindObjectOfType<IHeroMb>());
        }

        public class TimeoutResult
        {
            public float seconds;
            public Timeout timeout;

            public D.Cb game {
                set { timeout.Game(seconds, value); }
            }
            public D.Cb real {
                set { timeout.Real(seconds, value); }
            }

            public D.Cb Game(D.Cb value)
            {
                return timeout.Game(seconds, value);
            }

            public D.Cb Real(D.Cb value)
            {
                return timeout.Real(seconds, value);
            }
        }

        public class Animated
        {
            internal bool done = false;
            internal L<D.Cb> thens = new L<D.Cb>();

            internal Animated()
            {
            }

            public D.Cb thn { set {
                if (done) {
                    value();
                } else {
                    thens.s.Add(value);
                }
            } }

            /**
             * stop animation without execution
             * of function requested on finish
             */
            public void Stp()
            {
                done = true;
            }
        }
    }
}

