using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class AStar
    {
        public List<node> visitedNodes = new List<node>(), availableNodesToExpand = new List<node>();
        public Stack<int> solutionSteps = new Stack<int>();
        public string finalState = "123456780";
        public bool reachedGoal = false;
        public node initialNode;

        public void solve(node currentNode)
        {
            if (currentNode.state == finalState)
            {
                while (currentNode.parent != null)
                {
                    solutionSteps.Push(currentNode.neededStepToCreateFromParent);
                    currentNode = visitedNodes.Where(x => x == currentNode.parent).FirstOrDefault();
                }
                reachedGoal = true;
            }
            else
            {
                visitedNodes.Add(currentNode);

                List<node> childrenStates = currentNode.getChildrenStates();
                foreach (var child in childrenStates)
                {
                    if (!checkForDuplicateOrHasHigherCost(child))
                        availableNodesToExpand.Add(child);
                }

                availableNodesToExpand.Remove(currentNode);

                node minimumNode = availableNodesToExpand.OrderByDescending(x => x.cost).Last();
                solve(minimumNode);
            }
        }
        public bool checkForDuplicateOrHasHigherCost(node aNode)
        {
            foreach (var item in visitedNodes)
            {
                if (aNode.state == item.state && aNode.cost >= item.cost)
                    return true;
            }
            return false;
        }
        public AStar(string input)
        {
            initialNode = new node(input, 0, 0, null, -1);
            initialNode.cost = initialNode.getF_n();
        }
    }
    public class node
    {
        public int depthFoundAt = 0;
        public node parent;
        public string state = "";
        public int cost = 0;
        public int neededStepToCreateFromParent;
        public node(string _state, int _depthFoundAt, int _sum, node _parent, int stepToCreateFromParent)
        {
            state = _state;
            depthFoundAt = _depthFoundAt;
            cost = _sum;
            parent = _parent;
            neededStepToCreateFromParent = stepToCreateFromParent;
        }
        public List<node> getChildrenStates()
        {
            List<node> nextStates = new List<node>();
            var legalMovements = _8Puzzle.getLegalMovementsInString(this.state);
            var zeroPositionInString = this.state.IndexOf('0');

            foreach (var step in legalMovements)
            {
                string childState = _8Puzzle.swapTwoCharInAString(this.state, zeroPositionInString, zeroPositionInString + step);
                node child = new node(childState, this.depthFoundAt + 1, this.getF_n(), this, step);
                nextStates.Add(child);
            }

            return nextStates;
        }
        public int getF_n() { return getHeuristicValue() + this.depthFoundAt; }
        public int getHeuristicValue() { return getHammingDistance() + getManhattanDistance() + getChebishevDistance(); }
        public int getManhattanDistance()
        {
            int manhattanDistance = 0;
            for (int i = 0; i < this.state.Length; i++)
                manhattanDistance += getColumnPositionDifference(i) + getRowPositionDifference(i);
            return manhattanDistance;
        }
        public int getHammingDistance()
        {
            int misplacedTiles = 0;
            for (int i = 0; i < this.state.Length; i++)
            {
                if (Char.GetNumericValue(this.state[i]) != (i + 1) % 9)
                    misplacedTiles++;
            }
            return misplacedTiles;
        }
        public int getChebishevDistance() 
        {
            int chebishevDistance = 0;
            for (int i = 0; i < this.state.Length; i++)
                chebishevDistance += Math.Max(getRowPositionDifference(i), getColumnPositionDifference(i));
            return chebishevDistance;
        }
        public int getRowPositionDifference(int indexOfChar)
        {
            int rowPositionDifference = 0;
            int valueAtThisCell = (int)Char.GetNumericValue(this.state[indexOfChar]);
            if (valueAtThisCell != (indexOfChar + 1) % 9) 
            {
                var trueRowPosition = (valueAtThisCell) - 1 / 3;
                var currentRowPosition = indexOfChar / 3;

                if (valueAtThisCell == 0)
                    trueRowPosition = 2;

                rowPositionDifference = Math.Abs(trueRowPosition - currentRowPosition);
            }
            return rowPositionDifference;
        }
        public int getColumnPositionDifference(int indexOfChar)
        {
            int columnPositionDifference = 0;
            int valueAtThisCell = (int)Char.GetNumericValue(this.state[indexOfChar]);
            if (valueAtThisCell != (indexOfChar + 1) % 9)
            {
                var trueColumnPosition = (valueAtThisCell) - 1 % 3;
                var currentColumnPosition = indexOfChar % 3;

                if (valueAtThisCell == 0)
                    trueColumnPosition = 2;

                columnPositionDifference = Math.Abs(trueColumnPosition - currentColumnPosition);
            }
            return columnPositionDifference;
        }
    }
}
