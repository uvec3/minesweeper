using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper
{
    public static class GameAnalysis
    {
        //returns an array of the same size as source data with probabilities that cell is not a mine
        //sourceData for open cells must contain number of mines nearby and for closed cells must be equal to -1
        public static float[,] calculateProbabilityOfSuccess(int[,] sourceData, int allMinesCount)
        {
            int h = sourceData.GetLength(0);
            int w = sourceData.GetLength(1);

            //resulting probability matrix
            float[,] res = new float[h, w];

            //initially initialized with -1 (undefined)
            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                    res[i, j] = -1;

            //found mines are marked here
            bool[,] mines = new bool[h, w];

            //whether a mine or a free cell found during the previous iteration
            bool changed=true;
            //repeat the algorithm as long as new information can be extracted
            while (changed)
            {
                //reset the flag
                changed = false;

                //guessing algorithm for finding mines
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)//loop over all cells
                        if (sourceData[i, j] > 0)//if the cell is open and non 0 (it is unnecessary to examine the region around 0 cells because they never contact with mines)
                        {
                            //gather coordinates of all unknown cells nearby and count nearby mines found in previous iterations
                            List<int[]> unknownCellsNear = new List<int[]>(0);
                            int knownMinesNear = 0;
                            for (int ii = i - 1; ii <= i + 1; ++ii)
                                for (int jj = j - 1; jj <= j + 1; ++jj)
                                    if (ii >= 0 && jj >= 0 && ii < h && jj < w)
                                    {
                                        if (mines[ii, jj])
                                            ++knownMinesNear;
                                        else if (sourceData[ii, jj] == -1)
                                            unknownCellsNear.Add(new int[] { ii, jj });
                                    }

                            //if there ara still unknown cells nearby
                            if (unknownCellsNear.Count > 0)
                            {
                                //how many mines actually still unknown
                                int unknownMinesNear = sourceData[i, j] - knownMinesNear;

                                //build all possible combinations of mines and check which of them are possible in current context
                                int[,] vars = getCombinations(unknownMinesNear, unknownCellsNear.Count);
                                bool[] variationIsPossible = new bool[vars.GetLength(0)];

                                for (int k = 0; k < vars.GetLength(0); ++k)
                                {
                                    variationIsPossible[k] = true;
                                    for (int z = 0; z < unknownMinesNear; ++z)
                                    {
                                        for (int ii = unknownCellsNear[vars[k, z]][0] - 1; ii <= unknownCellsNear[vars[k, z]][0] + 1; ++ii)
                                            for (int jj = unknownCellsNear[vars[k, z]][1] - 1; jj <= unknownCellsNear[vars[k, z]][1] + 1; ++jj)
                                                if (ii >= 0 && jj >= 0 && ii < h && jj < w)
                                                {
                                                    if (sourceData[ii, jj] > 0)
                                                    {
                                                        int minesCount = 0;
                                                        for (int iii = ii - 1; iii <= ii + 1; ++iii)
                                                            for (int jjj = jj - 1; jjj <= jj + 1; ++jjj)
                                                                if (iii >= 0 && jjj >= 0 && iii < h && jjj < w && mines[iii, jjj])
                                                                    ++minesCount;
                                                        for (int zz = 0; zz < unknownMinesNear; ++zz)
                                                            if (Math.Abs(ii - unknownCellsNear[vars[k, zz]][0]) <= 1 && Math.Abs(jj - unknownCellsNear[vars[k, zz]][1]) <= 1)
                                                                ++minesCount;
                                                        variationIsPossible[k] &= minesCount <= sourceData[ii, jj];

                                                    }
                                                }
                                    }
                                }


                                //go over unknown cells and check if it is mine ina all of possible combinations left
                                //if yes than it must be a mine
                                for (int k = 0; k < unknownCellsNear.Count; ++k)
                                {
                                    bool flag = true;
                                    bool cellIsTrueOnce = false;
                                    for (int z = 0; z < vars.GetLength(0); ++z)
                                        if (variationIsPossible[z])
                                        {
                                            cellIsTrueOnce = true;
                                            bool found = false;
                                            for (int d = 0; d < unknownMinesNear; ++d)
                                                found |= vars[z, d] == k;
                                            flag &= found;

                                        }

                                    //cell contains mine in all possible combinations
                                    if (cellIsTrueOnce && flag)
                                    {
                                        //mark this cell as mine
                                        mines[unknownCellsNear[k][0], unknownCellsNear[k][1]] = true;
                                        //we have some new information which means that at least one more iteration is needed
                                        changed = true;
                                    }
                                }
                            }
                        }
                

                //now updating information on mines we can update cells which are definitely not mines
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                        if (sourceData[i, j] > 0)
                        {
                            //counting mines around this cell
                            int minesCount = 0;
                            for (int ii = i - 1; ii <= i + 1; ++ii)
                                for (int jj = j - 1; jj <= j + 1; ++jj)
                                    if (ii >= 0 && jj >= 0 && ii < h && jj < w && mines[ii, jj])
                                        ++minesCount;
                            //if the number of mines coincided with the number indicated on the cell, then all other cells nearby are NOT mines
                            if (minesCount == sourceData[i, j])
                                for (int ii = i - 1; ii <= i + 1; ++ii)
                                    for (int jj = j - 1; jj <= j + 1; ++jj)
                                        if (ii >= 0 && jj >= 0 && ii < h && jj < w && sourceData[ii, jj] == -1 && !mines[ii, jj])
                                        {
                                            //the probability of success of opening this cell is 1
                                            res[ii, jj] = 1.0f;
                                            //in the next iteration, this cell will be considered open, but since the value of the mines nearby is unknown,
                                            //it is marked as 0, because cells with a value of 0 are not processed directly by this algorithm
                                            sourceData[ii, jj] = 0;
                                            //some new information is found
                                            changed = true;
                                        }

                        }

            }



            //marks all cells in which a mine is found with a success chance of 0
            int foundMines = 0;
            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                    if (mines[i, j])
                    {
                        res[i, j] = 0.0f;
                        ++foundMines;
                    }


            //if all mines are found, then all still closed cells have a success of 1
            if (foundMines == allMinesCount)
            {
                for (int i = 0; i < h; ++i)
                    for (int j = 0; j < w; ++j)
                        if (sourceData[i, j] == -1 && !mines[i, j])
                            res[i, j] = 1.0f;
                return res;
            }


            //calculate still unknown cells
            int unknownCells = 0;
            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                    if (sourceData[i, j] == -1 && !mines[i, j])
                        ++unknownCells;

            //for all other unknown cells which are neither mines nor a definitely a free cell calculate probability
            //of success when opening it in diapason (0;1)
            float[,] probabilityNearCell = new float[h, w];
            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                    if (sourceData[i, j] > 0)
                    {
                        int foundMinesNear = 0;
                        int unknownCellsNear = 0;
                        for (int ii = i - 1; ii <= i + 1; ++ii)
                            for (int jj = j - 1; jj <= j + 1; ++jj)
                                if (ii >= 0 && jj >= 0 && ii < h && jj < w)
                                {
                                    if (mines[ii, jj])
                                        ++foundMinesNear;
                                    else if (sourceData[ii, jj] == -1)
                                        ++unknownCellsNear;
                                }

                        if (unknownCellsNear > 0)
                            probabilityNearCell[i, j] = 1 - (sourceData[i, j] - foundMinesNear) / (float)unknownCellsNear;
                    }



            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                    if (sourceData[i, j] == -1 && !mines[i, j])
                    {
                        int count = 0;
                        float sum = 0.0f;
                        for (int ii = i - 1; ii <= i + 1; ++ii)
                            for (int jj = j - 1; jj <= j + 1; ++jj)
                                if (ii >= 0 && jj >= 0 && ii < h && jj < w && sourceData[ii, jj] > 0)
                                {
                                    sum += probabilityNearCell[ii, jj];
                                    ++count;
                                }
                        if (count > 0)
                            res[i, j] = (sum / count);
                        else
                            res[i, j] = 1.0f - (allMinesCount - foundMines) / (float)unknownCells;
                    }


            return res;
        }


        //Build array of all possible combinations for choosing n elements out of m
        //each element is named by its index from  0 to n-1
        private static int[,] getCombinations(int n, int m)
        {
            //return empty array for 0 elements
            if (n == 0)
                return new int[,] { { } };

            //Allocate results array
            //There are C(n,m) combinations with n elements each
            int[,] res = new int[C(n, m), n];

            //current combination index
            int i = 0;



            //imitate multilevel for loop of depth n
            //k[kid] is a loop index for kid depth
            int[] k = new int[n];
            k[0] = -1;
            int kid = 0;

            //it is easier to count until all combinations are filled in this case
            while (i < res.GetLength(0))
            {
                ++k[kid];

                if (k[kid] == m)
                {
                    --kid;
                    continue;
                }

                if (kid < n - 1)
                {
                    ++kid;
                    k[kid] = k[kid - 1];
                }
                else
                {
                    for (int z = 0; z < n; ++z)
                        res[i, z] = k[z];
                    ++i;
                }
            }
            return res;
        }


        private static int C(int n, int m)
        {
            int res = 1;
            for (int i = n + 1; i <= m; ++i)
                res *= i;
            for (int i = 1; i <= m - n; ++i)
                res /= i;
            return res;
        }
    }
}
