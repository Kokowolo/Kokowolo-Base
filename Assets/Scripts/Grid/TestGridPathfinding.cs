/*
 * File Name: TestGridPathfinding.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 26, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Grid;
using Kokowolo.Pathfinding;

public class TestGridPathfinding : MonoBehaviour, IPathfinding
{
    /************************************************************/
    #region Fields

    #endregion
	/************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    public List<Node> GetNeighborsFromNode(Node node)
    {
        return node.GetNeighbors();
    }

    public bool IsValidMoveBetweenNodes(Node start, Node end)
    {
        return true;
    }

    public int GetMoveCostBetweenNodes(Node start, Node end)
    {
        return 1;
    }

    public bool IsPathTrimmable(NodePath path)
    {
        return false;
    }
    
    #endregion
    /************************************************************/
}