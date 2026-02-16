/*
 * Author(s): Kokowolo, Will Lacey
 * Date Created: June 12, 2023
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
    [Obsolete("ScheduledEventManager has been marked as Obsolete, please use new Scheduling.Job framework instead")]
    public class ScheduledEventManager : MonoBehaviourSingleton<ScheduledEventManager>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        List<ScheduledEvent> queuedEvents = new List<ScheduledEvent>();
        List<ScheduledEvent> activeEvents = new List<ScheduledEvent>();

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public static bool IsApplicationQuitting { get; private set; }

        public static bool IsRunning => Instance.activeEvents.Count > 0;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        protected override void Singleton_Awake()
        {
            Application.quitting += Handle_Application_Quitting;
        }

        protected override void Singleton_OnDestroy()
        {
            Application.quitting -= Handle_Application_Quitting;
        }

        public static IEnumerator WaitWhileIsRunning()
        {
            yield return new WaitWhile(_IsRunning);

            bool _IsRunning()
            {
                return IsRunning;
            }
        }

        public static void StopEvent(ScheduledEvent scheduledEvent)
        {
            if (scheduledEvent == null || scheduledEvent.HasStopped) return;

            if (scheduledEvent.IsAlive)
            {
                (scheduledEvent as IScheduledEvent).Stop();
            }
            else
            {
                Instance.Handle_ScheduledEvent_OnStopped(scheduledEvent, EventArgs.Empty);
            }
        }

        public static ScheduledEvent StartEvent(Action function, float time)
        {
            ScheduledEvent scheduledEvent = new ScheduledEvent(function, time);
            StartEvent(scheduledEvent);
            return scheduledEvent;
        }

        public static ScheduledEvent StartEvent(IEnumerator routine)
        {
            ScheduledEvent scheduledEvent = new ScheduledEvent(routine);
            StartEvent(scheduledEvent);
            return scheduledEvent;
        }

        static void StartEvent(ScheduledEvent scheduledEvent)
        {
            Instance.queuedEvents.Remove(scheduledEvent);
            Instance.activeEvents.Add(scheduledEvent);
            scheduledEvent.OnStopped += Instance.Handle_ScheduledEvent_OnStopped;
            (scheduledEvent as IScheduledEvent).Start();
        }

        public static ScheduledEvent ScheduleEvent(Action function, float time)
        {
            ScheduledEvent scheduledEvent = new ScheduledEvent(function, time);
            ScheduleEvent(scheduledEvent);
            return scheduledEvent;
        }

        public static ScheduledEvent ScheduleEvent(IEnumerator routine)
        {
            ScheduledEvent scheduledEvent = new ScheduledEvent(routine);
            ScheduleEvent(scheduledEvent);
            return scheduledEvent;
        }

        static void ScheduleEvent(ScheduledEvent scheduledEvent)
        {
            (scheduledEvent as IScheduledEvent).IsScheduled = true;
            Instance.queuedEvents.Add(scheduledEvent);

            for (int i = 0; i < Instance.activeEvents.Count; i++)
            {
                if ((Instance.activeEvents[i] as IScheduledEvent).IsScheduled)
                {
                    return;
                }
            }
    
            StartEvent(Instance.queuedEvents[0]);
        }
        
        void Handle_ScheduledEvent_OnStopped(object sender, EventArgs e)
        {
            ScheduledEvent scheduledEvent = sender as ScheduledEvent;
            scheduledEvent.OnStopped -= Handle_ScheduledEvent_OnStopped;
            Instance.activeEvents.Remove(scheduledEvent);

            bool scheduleNext = (scheduledEvent as IScheduledEvent).IsScheduled && !Instance.queuedEvents.Remove(scheduledEvent);
            if (scheduleNext && queuedEvents.Count > 0) 
            {
                StartEvent(queuedEvents[0]);
            }
        }

        void Handle_Application_Quitting()
        {
            IsApplicationQuitting = true;
        }
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}