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

        Queue<Job> pendingJobs;
        Queue<Job> scheduledJobs;
        List<Job> activeJobs;
        
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
            pendingJobs = null;
            scheduledJobs = null;
            ListPool.Release(ref activeJobs);
        }

        protected override void Singleton_Awake()
        {
            pendingJobs = new Queue<Job>();
            scheduledJobs = new Queue<Job>();
            activeJobs = ListPool.Get<Job>();
        }

        void LateUpdate()
        {
            // End LateUpdate refresh
            enabled = false;

            // Prevent newly created jobs from intefering with this update
            // var pendingJobs = new Queue<Job>();
            // foreach (var i in this.pendingJobs)
            // {
            //     pendingJobs.Enqueue(i);
            // }
            // this.pendingJobs.Clear();

            // Handle/start pending jobs
            while (pendingJobs.Count > 0)
            {
                Job job = pendingJobs.Dequeue();
                if (job.IsPending) 
                {
                    job.IsPending = false;
                    if (job.IsScheduled)
                    {
                        Schedule(job);
                    }
                    else
                    {
                        StartJob(job);
                    }
                }
            }
        }

        internal void PendJob(Job job)
        {
            job.IsPending = true;
            pendingJobs.Enqueue(job);
            enabled = true;
        }
        
        void StartJob(Job job)
        {
            // job.IsScheduled = false;
            job.OnDispose += Handle_Job_OnDispose;
            activeJobs.Add(job);
            job.Start();
        }

        void Schedule(Job job)
        {   
            scheduledJobs.Enqueue(job);
            job = scheduledJobs.Peek();
            if (!job.IsRunning)
            {
                StartJob(job);
            }
        }

        void Handle_Job_OnDispose(Job job)
        {
            // Handle if active job
            if (!activeJobs.Remove(job)) throw new Exception("impossible state reached");

            // Handle if running scheduled job
            if (!job.IsScheduled) return;

            // Handle next scheduled job
            if (scheduledJobs.Count == 0) throw new Exception("impossible state reached");
            scheduledJobs.Dequeue();
            if (scheduledJobs.Count == 0) return;
            StartJob(scheduledJobs.Peek());
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