using System;
using System.Collections;
using UnityEngine;

namespace BAStudio.UpdateHub
{
    public class UpdateHub : MonoBehaviour
    {
        public Action OnUpdate { get; private set; }
        public Action OnFixedUpdate { get; private set; }
        void Update ()
        {
            OnUpdate?.Invoke();
        }

        void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        public UpdateHandle      NewUpdate (Action action) => new UpdateHandle(this, action);
        public FixedUpdateHandle NewFixedUpdate (Action action) => new FixedUpdateHandle(this, action);
        public DelayByFrames     NewDelayByFrames (int frames, Action action) => new DelayByFrames(this, frames, action);
        public DelayByTime       NewDelayByTime (float delay, Action action) => new DelayByTime(this, delay, action);
        public DelayByFixedTime  NewDelayByFixedTime (float delay, Action action) => new DelayByFixedTime(this, delay, action);
        public RepeatByFrames    NewRepeatByFrames (int frames, Action action) => new RepeatByFrames(this, frames, action);
        public RepeatByTime      NewRepeatByTime (float interval, Action action) => new RepeatByTime(this, interval, action);
        public RepeatByFixedTime NewRepeatByFixedTime (float interval, Action action) => new RepeatByFixedTime(this, interval, action);

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

        public class DelayByTime : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            float start, end;
            public DelayByTime (UpdateHub hub, float delay, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnUpdate += Next;
                this.start = Time.time;
                this.end = Time.time + delay;
            }

            void Next ()
            {
                if (Time.time >= end)
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

        public class RepeatByFrames : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            int tick, interval;
            public RepeatByFrames (UpdateHub hub, int frames, Action action)
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

        public class RepeatByTime : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            float last, interval;
            public RepeatByTime (UpdateHub hub, float interval, Action action)
            {
                Hub = hub;
                Action = action;
                hub.OnUpdate += Next;
                this.interval = interval;
                this.last = Time.time;
            }

            void Next ()
            {
                if (last + interval >= Time.time)
                {
                    last = Time.time;
                    Action.Invoke();
                }
            }

            public void Dispose()
            {
                Hub = null;
                Hub.OnUpdate -= Action;
            }
        }

        public class RepeatByFixedTime : IDisposable
        {
            public UpdateHub Hub { get; protected set; }
            public Action Action { get; protected set; }
            float last, interval;
            public RepeatByFixedTime (UpdateHub hub, float interval, Action action)
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