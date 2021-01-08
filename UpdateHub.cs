using System;
using System.Collections;
using UnityEngine;

namespace BAStudio.UpdateHub
{
    public class UpdateHub : IDisposable
    {
        public MonoBehaviour Host { get; }
        Coroutine handle;
        public UpdateHub(MonoBehaviour host)
        {
            Host = host;
            Host.StartCoroutine(Update());
        }
        public Action OnUpdate { get; private set; }
        public Action OnFixedUpdate { get; private set; }
        IEnumerator Update ()
        {
            while (true)
            {
                OnUpdate?.Invoke();
                yield return null;
            }
        }

        IEnumerator FixedUpdate ()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                OnFixedUpdate?.Invoke();
                yield return waitForFixedUpdate;
            }
        }

        public void Dispose()
        {
            Host.StopCoroutine(handle);
        }

        public class UpdateHandle : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }

            public UpdateHandle (UpdateHub hub, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnUpdate += action;
            }


            public virtual void Dispose()
            {
                Hub = null;
                Hub.OnUpdate -= Action;
            }
        }

        public class FixedUpdateHandle : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }

            public FixedUpdateHandle (UpdateHub hub, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnFixedUpdate += action;
            }
            public virtual void Dispose()
            {
                Hub = null;
                Hub.OnFixedUpdate -= Action;
            }
        }

        public class DelayByFrames : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            int countdown;
            public DelayByFrames (UpdateHub hub, int frames, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnUpdate += Next;
                countdown = frames;
            }

            void Next ()
            {
                if (countdown-- <= 0)
                {
                    Action.Invoke();
                    Dispose();
                }
            }

            public void Dispose()
            {
                Hub = null;
                Hub.OnUpdate -= Action;
            }
        }

        public class DelayByFixedTime : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            float start, end;
            public DelayByFixedTime (UpdateHub hub, float delay, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnFixedUpdate += Next;
                this.start = Time.fixedTime;
                this.end = Time.fixedTime + delay;
            }

            void Next ()
            {
                if (Time.fixedTime >= end)
                {
                    Action.Invoke();
                    Dispose();
                }
            }

            public void Dispose()
            {
                Hub = null;
                Hub.OnFixedUpdate -= Action;
            }
        }

        public class IntervalByFrames : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            int tick, interval;
            public IntervalByFrames (UpdateHub hub, int frames, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnUpdate += Next;
                interval = tick = frames;
            }

            void Next ()
            {
                if (tick-- <= 0)
                {
                    tick = interval;
                    Action.Invoke();
                }
            }

            public void Dispose()
            {
                Hub = null;
                Hub.OnUpdate -= Action;
            }
        }
        
        public class IntervalByFixedTime : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            float last, interval;
            public IntervalByFixedTime (UpdateHub hub, float interval, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnFixedUpdate += Next;
                this.interval = interval;
                this.last = Time.fixedTime;
            }

            void Next ()
            {
                if (last + interval >= Time.fixedTime)
                {
                    last = Time.fixedTime;
                    Action.Invoke();
                }
            }

            public void Dispose()
            {
                Hub = null;
                Hub.OnFixedUpdate -= Action;
            }
        }
    }
}