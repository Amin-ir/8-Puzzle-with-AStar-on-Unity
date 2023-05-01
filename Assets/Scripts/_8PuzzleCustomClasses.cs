using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public static class _8Puzzle
    {
        public static string generateRandomGameState()
        {
            string state;
            List<int> availableNumbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            do
            {
                state = "";
                while (availableNumbers.Count != 0)
                {
                    bool createdUniqueRandomNumber = false;
                    while (!createdUniqueRandomNumber)
                    {
                        var randomNumber = UnityEngine.Random.Range(0, 9);
                        if (availableNumbers.Contains(randomNumber))
                        {
                            createdUniqueRandomNumber = true;
                            state += randomNumber;
                            availableNumbers.Remove(randomNumber);
                        }
                    }
                }
            } while (!checkSolvability(state));
            return state;
        }
        public static bool hasReachedFinalState(string state)
        {
            bool result = false;
            result = (state == "123456780") ? true : false;
            return result;
        }
        public static List<int> getLegalMovementsInString(string state)
        {
            List<int> legalMovements = new List<int>();

            var zeroPosition = state.IndexOf('0');
            //Checking for legal vertical movement in state string
            switch (zeroPosition / 3)
            {
                case 0:
                    legalMovements.Add(3);
                    break;
                case 1:
                    legalMovements.Add(3);
                    legalMovements.Add(-3);
                    break;
                case 2:
                    legalMovements.Add(-3);
                    break;
            }
            //Checking for legal horizontal movement in state string
            switch (zeroPosition % 3)
            {
                case 0:
                    legalMovements.Add(1);
                    break;
                case 1:
                    legalMovements.Add(1);
                    legalMovements.Add(-1);
                    break;
                case 2:
                    legalMovements.Add(-1);
                    break;
            }

            return legalMovements;
        }
        public static string swapTwoCharInAString(string state, int index1, int index2)
        {
            StringBuilder temp = new StringBuilder(state);
            char tempChar = temp[index1];
            temp[index1] = temp[index2];
            temp[index2] = tempChar;
            state = temp.ToString();
            return state;
        }
        public static int countInversions(string state)
        {
            int numberOfInversions = 0;

            for (int i = 0; i < state.Length - 1; i++)
                for (int j = i + 1; j < state.Length; j++)
                {
                    int iCharIntEquavalent = (int)Char.GetNumericValue(state[i]);
                    int jCharIntEquavalent = (int)Char.GetNumericValue(state[j]);
                    if (iCharIntEquavalent != 0 && jCharIntEquavalent != 0 && iCharIntEquavalent > jCharIntEquavalent)
                        numberOfInversions++;
                }

            return numberOfInversions;
        }
        public static bool checkSolvability(string state)
        {
            int numberOfInversion = 0;
            numberOfInversion = countInversions(state);
            if (numberOfInversion % 2 == 0)
                return true;
            else return false;
        }
    }
}
