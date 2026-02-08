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

public class TestGridPathfinding : IPathfinding, IPathfinder
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

    IEnumerable<INode> IPathfinder.GetNeighborsFromNode(INode iNode)
    {
        // Get one cell above (unless it's null)
        GridCell start = (iNode.ToCell().Coordinates + GridCoordinates.Up).ToCell() ?? iNode.ToCell();

        // Search around this "up" cell
        foreach (var direction in GridDirectionExtensions.GetDirections())
        {
            // Continue if border
            if (!start.TryGetInDirection(direction, out GridCell neighbor)) continue;

            // If HasSurface, this is a step
            if (neighbor.HasSurface && neighbor.IsExplorable) 
            {
                yield return neighbor;
            }
            else // Drill down until first Explorable or Ground
            {
                if (GridManager.TargetMapObject.Map.Structure.TraverseWhile(neighbor.Coordinates, GridCoordinates.Down,  out neighbor, _Predicate))
                {
                    if (neighbor.IsExplorable) yield return neighbor;
                }
            }
        }

        static bool _Predicate(GridCell cell)
        {
            return !cell.IsExplorable && !cell.HasSurface;
        }
    }

    bool IPathfinder.IsValidMoveBetweenNodes(INode start, INode end) => IsValidMoveBetweenNodes(start.ToCell(), end.ToCell());
    private bool IsValidMoveBetweenNodes(GridCell start, GridCell end)
    {
        const int height = 2;
        GridDirection direction = start.Coordinates.GetDirectionToCoordinates(end.Coordinates);

        // HACK: [LUTRO-238] Dynamically Sized Units - clean this up
        bool lowerOption = !start.HasBlockingObstacle(direction, fromRelativeHeight: 0, height);
        bool upperOption = !start.HasBlockingObstacle(direction, fromRelativeHeight: 1, height + 1);
        return lowerOption || upperOption; 
        
        // TODO: Check for Enough Vertical Space ?
        // GridManager.Map.GetCellsBelowCoordinates
    }

    int IPathfinder.GetHeuristicCostBetweenNodes(INode start, INode end)
    {
        return start.ToCell().Coordinates.DistanceTo(end.ToCell().Coordinates, ignoreDistanceY: true, allowFastFall: false);
    }

    int IPathfinder.GetMoveCostBetweenNodes(INode start, INode end)
    {
        return 1;
    }

    bool IPathfinder.IsPathOutsideMovementRange(NodePath path)
    {
        // return path.Distance > maxDistance;
        return false;
    }
    

    #endregion
    /************************************************************/
}