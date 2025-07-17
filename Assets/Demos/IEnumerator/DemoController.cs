/* 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: July 17, 2025
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Kokowolo.Base.Demos.IEnumeratorDemo
{
    public class DemoController : MonoBehaviour
    {
        // /*██████████████████████████████████████████████████████████*/
        // #region Fields

        // Item head = new Item();

        // #endregion
        // /*██████████████████████████████████████████████████████████*/
        // #region Properties

        // #endregion
        // /*██████████████████████████████████████████████████████████*/
        // #region Functions

        // void Update()
        // {
        //    if (Input.GetKeyDown(KeyCode.A)) PrependItem();
        //    if (Input.GetKeyDown(KeyCode.D)) AppendItem();
        //    if (Input.GetMouseButtonDown(0)) StartCoroutine(Routine());
        // }

        // void AppendItem() => head.Append();
        // void PrependItem() => head.Prepend();
        
        // IEnumerator Routine()
        // {
        //     Debug.Log("a");
        //     var enumerator = head.GetEnumerator();
        //     while (enumerator.MoveNext())
        //     {
        //         var a = enumerator.Current;
        //         yield return null;
        //     }
        //     Debug.Log("b");
        // }

        // #endregion
        // /*██████████████████████████████████████████████████████████*/
        // #region Subclasses

        // class Item : IEnumerable<int>
        // {
        //     System.Action start;
        //     System.Action complete;


        //     IEnumerator afterStart;
        //     IEnumerator main;
        //     IEnumerator afterComplete;

        //     public void Append(Item item)
        //     {
                
        //     }

        //     public void Prepend(Item item)
        //     {
                
        //     }

        //     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //     public IEnumerator<CustomYieldInstruction> GetEnumerator()
        //     {
        //         start?.Invoke();
        //         yield return afterStart;
        //         yield return main;
        //         complete?.Invoke();
        //         yield return complete;
        //     }
        // }

        // #endregion
        // /*██████████████████████████████████████████████████████████*/
    }
}