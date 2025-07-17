/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 15, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kokowolo.Utilities;

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public class JobManager : MonoBehaviourSingleton<JobManager>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        internal List<Job> pendingJobs;
        internal List<Job> scheduledJobs;
        internal List<Job> activeJobs;
        Job runningScheduledJob;
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool IsFree => pendingJobs.Count == 0 && scheduledJobs.Count == 0 && !IsRunning;
        bool IsRunning => activeJobs.Count != 0;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        protected override void Singleton_OnDestroy()
        {
            ListPool.Release(ref pendingJobs);
            ListPool.Release(ref scheduledJobs);
            ListPool.Release(ref activeJobs);
        }

        protected override void Singleton_Awake()
        {
            pendingJobs = ListPool.Get<Job>();
            scheduledJobs = ListPool.Get<Job>();
            activeJobs = ListPool.Get<Job>();
        }

        void LateUpdate()
        {
            // Validate pending jobs
            for (int i = pendingJobs.Count - 1; i >= 0; i--)
            {
                Job job = pendingJobs[i];
                if (job.IsPending)
                {
                    job.OnDispose -= Handle_PendingJob_OnDispose;
                    StartJob(job);
                }
                pendingJobs.RemoveAt(i);
            }
            enabled = false;
        }

        internal static void PendJob(Job job)
        {
            job.OnDispose += Instance.Handle_PendingJob_OnDispose;
            job.IsPending = true;
            Instance.pendingJobs.Add(job);
            Instance.enabled = true;
        }
        
        void Handle_PendingJob_OnDispose(Job job)
        {
            job.OnDispose -= Instance.Handle_PendingJob_OnDispose;
            pendingJobs.Remove(job);
        }
        
        internal static Job StartJob(Job job)
        {
            job.OnDispose += Instance.Handle_Job_OnDispose;
            job.Start();
            return job;
        }

        internal static Job ScheduleJob(Job job)
        {
            job.OnDispose += Instance.Handle_Job_OnDispose;
            if (Instance.runningScheduledJob == null)
            {
                Instance.runningScheduledJob = job;
                job.Start();
            }
            else
            {
                Instance.scheduledJobs.Add(job);
            }
            return job;
        }

        internal void Handle_Job_OnDispose(Job job)
        {
            // Handle if active job
            job.OnDispose -= Handle_Job_OnDispose;
            activeJobs.Remove(job);

            // Handle if non-running scheduled job
            if (scheduledJobs.Remove(job)) return;

            // Handle if running scheduled job
            if (runningScheduledJob == null || runningScheduledJob.instanceId != job.instanceId) return;
            runningScheduledJob = null;

            // Handle next scheduled job
            if (scheduledJobs.Count == 0) return;
            runningScheduledJob = scheduledJobs[0];
            scheduledJobs[0].Start();
            scheduledJobs.RemoveAt(0);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Editor
#if UNITY_EDITOR

        void OnValidate()
        {
            if (Application.isPlaying) return;
            enabled = false;
        }

#endif
        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}