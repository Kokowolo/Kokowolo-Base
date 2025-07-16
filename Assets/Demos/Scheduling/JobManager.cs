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

namespace Kokowolo.Base.Demo.SchedulingDemo
{
    public class JobManager : MonoBehaviourSingleton<JobManager>
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        List<Job> scheduledJobs;
        List<Job> activeJobs;
        Job runningScheduledJob;
        
        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        public bool IsFree => scheduledJobs.Count == 0 && !IsRunning;
        bool IsRunning => activeJobs.Count != 0;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        protected override void Singleton_OnDestroy()
        {
            ListPool.Release(ref scheduledJobs);
            ListPool.Release(ref activeJobs);
        }

        protected override void Singleton_Awake()
        {
            scheduledJobs = ListPool.Get<Job>();
            activeJobs = ListPool.Get<Job>();
            enabled = false;
        }
        
        public static Job StartJob(Action action, float time) => StartJob(Job.Get(action, time));
        public static Job StartJob(IEnumerator routine) => StartJob(Job.Get(routine));
        static Job StartJob(Job job)
        {
            job.OnDispose += Instance.Handle_Job_OnDispose;
            Instance.activeJobs.Add(job);
            job.Start();
            return job;
        }

        public static Job ScheduleJob(Action action, float time) => ScheduleJob(Job.Get(action, time));
        public static Job ScheduleJob(IEnumerator routine) => ScheduleJob(Job.Get(routine));
        static Job ScheduleJob(Job job)
        {
            job.OnDispose += Instance.Handle_Job_OnDispose;
            if (Instance.runningScheduledJob == null)
            {
                Instance.runningScheduledJob = job;
                Instance.activeJobs.Add(job);
                job.Start();
            }
            else
            {
                Instance.scheduledJobs.Add(job);
            }
            return job;
        }

        // public static JobSequence JobSequence()
        // {
        //     return new JobSequence();
        // }

        internal void Handle_Job_OnDispose(object sender, EventArgs e)
        {
            // Handle if active job
            Job job = sender as Job;
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
            activeJobs.Add(scheduledJobs[0]);
            scheduledJobs[0].Start();
            scheduledJobs.RemoveAt(0);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}