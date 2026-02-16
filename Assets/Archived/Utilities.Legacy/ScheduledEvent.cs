/*
 * Author(s): Kokowolo, Will Lacey
 * Date Created: October 28, 2023
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;
namespace Kokowolo.Utilities.Legacy
{
    [Obsolete("IScheduledEvent has been marked as Obsolete, please use new Scheduling.Job framework instead")]
    public interface IScheduledEvent
    {
        public bool IsScheduled { get; set; }
        // public bool IsAlive { get; }

        public void Start();
        public void Stop();
    }

    [Obsolete("ScheduledEvent has been marked as Obsolete, please use new Scheduling.Job framework instead")]
    public class ScheduledEvent : IEquatable<ScheduledEvent>, IScheduledEvent//, IPoolable<ScheduledEvent>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Events

        public event EventHandler OnStopped;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        static int idCount = 0;

        int id;
        IEnumerator routine;
        Coroutine coroutine;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties
            
        bool HasStarted { get; set; } = false;
        public bool HasStopped { get; private set; } = false;

        public bool IsAlive => HasStarted && coroutine != null;

        bool IScheduledEvent.IsScheduled { get; set; }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public ScheduledEvent(IEnumerator routine)
        {
            id = idCount++;
            this.routine = routine;
        }

        public ScheduledEvent(Action function, float time)
        {
            id = idCount++;
            time = Mathf.Max(0, time);
            routine = InvokeFunctionAfterTime(function, time);
        }

        void IScheduledEvent.Start()
        {
            if (HasStarted || HasStopped) return;
#pragma warning disable 0618
            coroutine = ScheduledEventManager.Instance.StartCoroutine(Run());
#pragma warning restore 0618
        }

        void IScheduledEvent.Stop()
        {
            if (HasStopped) return;
            HasStopped = true;
            
            if (coroutine != null)
            {
#pragma warning disable 0618
                ScheduledEventManager.Instance.StopCoroutine(coroutine);
#pragma warning restore 0618
                routine = null;
            }
            OnStopped?.Invoke(this, EventArgs.Empty);
        }

        // public void Stop()
        // {
        //     ScheduledEventManager.StopEvent(this);
        // }

        IEnumerator Run()
        {
            HasStarted = true;
            yield return routine;
            routine = null;
            (this as IScheduledEvent).Stop();
        }

        IEnumerator InvokeFunctionAfterTime(Action function, float time)
        {
            yield return new WaitForSeconds(time);
            function.Invoke();
        }

        /*——————————————————————————————————————————————————————————*/
        #region Interface Functions

        // public static ScheduledEvent Create(params object[] args)
        // {
            
        // }

        // void IPoolable<ScheduledEvent>.OnAddPoolable()
        // {
            
        // }

        // void IPoolable<ScheduledEvent>.OnGetPoolable(params object[] args)
        // {
            
        // }

        public bool Equals(ScheduledEvent other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(this, null)) return false;
            if (ReferenceEquals(other, null)) return false;
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ScheduledEvent);
        }

        public static bool operator ==(ScheduledEvent a, ScheduledEvent b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null)) return false;
            if (ReferenceEquals(b, null)) return false;
            return a.id == b.id;
        }

        public static bool operator !=(ScheduledEvent a, ScheduledEvent b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }

        public override string ToString()
        {
            return $"{nameof(ScheduledEvent)} {id}";
        }

        #endregion
        /*——————————————————————————————————————————————————————————*/
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}