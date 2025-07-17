/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 15, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Base.Demos.GCTestDemo
{
    public class DemoController : MonoBehaviour
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public void Start()
        {
            Something s1;
            System.WeakReference reference;
            s1 = new Something();
                reference = new (s1);
            {
                
                Something s2 = s1;
                reference = new(s2);
                Debug.Log($"{s2} and {reference.IsAlive}");
                s2 = null;
                System.GC.Collect();
                Debug.Log($"{s1} and {reference.IsAlive}");
            }
            System.GC.Collect();
            Debug.Log($"{s1} and {reference.IsAlive}");
            s1.Dispose();
            s1 = null;
            System.GC.Collect();
            Debug.Log("a");
            // yield return null;
            System.GC.Collect();
            Debug.Log("b");
            Debug.Log($"{reference.IsAlive}");
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }

    public class Something : System.IDisposable
    {
        int value = 1;

        bool disposed;
        ~Something()
        {
            Debug.Log("finalized");
            Dispose();
        }
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            // value = -1;
        }

        public Something()
        {
            value = 10;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}