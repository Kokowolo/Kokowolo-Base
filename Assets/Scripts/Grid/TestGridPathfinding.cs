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

public class TestGridPathfinding : IPathfinding
{
    /************************************************************/
    #region Fields

    #endregion
	/************************************************************/
    #region Properties

    public static TestGridPathfinding _instance;
    public static TestGridPathfinding Instance     
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = new TestGridPathfinding();
            }
            return _instance;
        }
    }

    public bool CanCreatePathsWithRepeatNodes { get; private set; } = false;

    #endregion
    /************************************************************/
    #region Functions

    public List<Node> GetNeighborsFromNode(Node node)
    {
        return node.GetNeighbors();
    }

    public bool IsValidMoveBetweenNodes(Node start, Node end)
    {  
        int height = 2;
        bool isValid = true;
        // Check for Blocking Obstacle
        isValid &= !GridManager.Map.HasBlockingObstacleTowardsCell(start.GetCell(), end.GetCell(), height);

        // TODO: Check for Enough Vertical Space
        // GridManager.Map.GetCellsBelowCoordinates

        return isValid;
    }

    public int GetDistanceBetweenNodes(Node start, Node end)
    {
        return start.GetCell().Coordinates.GetDistanceTo(end.GetCell().Coordinates, ignoreFallDistance: false);
    }

    public int GetMoveCostBetweenNodes(Node start, Node end)
    {
        return 1;
    }

    public bool IsPathOutsideMovementRange(NodePath path)
    {
        return false;
    }

    #endregion
    /************************************************************/
}