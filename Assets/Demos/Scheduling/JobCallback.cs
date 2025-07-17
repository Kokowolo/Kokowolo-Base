/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 16, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Base.Demos.SchedulingDemo
{
    public delegate void JobCallback<T>(T job) where T : Job;
    public delegate void JobCallback();
}