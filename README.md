# Minesweeper Game and Analytics

This is a classic Minesweeper game, developed as coursework for my first programming course using Windows Forms. 

It includes all standard expected functionality, such as flagging, saving and loading games, a mine counter, and auto-opening. It also allows for custom field sizes and mine counts. Additionally, the game features an optional assistant that analyzes the current board state and calculates safe choices.

<img width="985" height="576" alt="base" src="https://github.com/user-attachments/assets/661f9713-5645-4977-9a2f-e7bf9db84c7d" />

<img width="409" height="274" alt="game_mode" src="https://github.com/user-attachments/assets/e9cda538-01b9-4bbc-9261-4bb1957cda8e" />
*Selecting game mode*

<img width="928" height="581" alt="load_game" src="https://github.com/user-attachments/assets/c9a9278f-9afc-4db6-81ef-a894d4e7dccf" />
*Loading game menu*

---

## Probability Analysis

On top of the core game, there is an assistant that displays the probability that an unknown cell is a safe choice. This algorithm uses only the information available to the player and has no hidden knowledge of the actual mine placements.

<img width="985" height="576" alt="assistant2" src="https://github.com/user-attachments/assets/db0cfdb3-0dc0-40e8-80a8-d10ca5a424ab" />
<img width="985" height="576" alt="assistant" src="https://github.com/user-attachments/assets/4da7b6eb-1b21-440e-b7f4-e8dd453d6804" />

---

## Algorithm Description

### Searching for Mines

Here is a baseline description of the probability calculator:

We consider each open cell that has mines next to it. Let there be $n$ mines. Then, looking at the closed cells next to it—the number of which is $m$ (where $m \ge n$)—all possible combinations of $n$ elements out of $m$ are considered.

<img width="179" height="147" alt="c35" src="https://github.com/user-attachments/assets/92a915e3-3f9a-4dbd-9279-29397fc0174a" />

For example, let $m = 3$ and $n = 5$. If we name the adjacent closed cells (highlighted in green) with the checkable indices (highlighted in blue) from $0$ to $n-1$, we obtain the following combinations of neighboring cells in which mines can be located (highlighted in red):

1. **0, 1, 2** <img width="109" height="89" alt="1" src="https://github.com/user-attachments/assets/4c173064-3443-4d65-81b9-7d5da8a8db13" />
2. **0, 1, 3** <img width="119" height="94" alt="2" src="https://github.com/user-attachments/assets/41855e95-d1ab-4ba7-81d9-cc31c0b4a28b" />
3. **0, 1, 4** <img width="117" height="89" alt="3" src="https://github.com/user-attachments/assets/1d0afdc2-1f38-4b2c-a7a8-62af2325af5b" />
4. **0, 2, 3** <img width="119" height="95" alt="4" src="https://github.com/user-attachments/assets/ca7ab73f-3735-49bf-8f71-57699c0d485b" />
5. **0, 2, 4** <img width="120" height="96" alt="5" src="https://github.com/user-attachments/assets/956098d5-dc6c-42f0-8ebe-52ff31cbe152" />
6. **0, 3, 4** <img width="121" height="96" alt="6" src="https://github.com/user-attachments/assets/1bf2b886-4a61-4366-afea-e3a900ea634b" />
7. **1, 2, 3** <img width="123" height="96" alt="7" src="https://github.com/user-attachments/assets/584cddbe-7915-41fd-a74a-50387db53c31" />
8. **1, 2, 4** <img width="122" height="94" alt="8" src="https://github.com/user-attachments/assets/1578a3fe-83af-4247-89ce-de0eeb1ae9b2" />
9. **1, 3, 4** <img width="120" height="95" alt="9" src="https://github.com/user-attachments/assets/df9cebc1-771f-4e25-ac97-e7e7fc2759cd" />
10. **2, 3, 4**<img width="122" height="93" alt="10" src="https://github.com/user-attachments/assets/38f739c8-a4f9-4946-bdc2-a781005251de" />


It is worth noting that the total number of these combinations is equal to the mathematical combination of $m$ items taken $n$ at a time, denoted as $C(m,n)$.

Having generated a list of such combinations, a check is performed to see if each one is actually possible. Specifically, all cells that "touch" these combinations (cells adjacent to those in the combination) are checked against the number of mines marked on them. This factors in the sum of nearby mines within the combination or those already found in a previous iteration. If the number of adjacent mines for every neighboring cell does not exceed the number indicated on it, the combination is considered *possible*.

After this filtering process, only a list of valid mine combinations remains. If a specific cell contains a mine across *all* possible combinations, then it is mathematically guaranteed to be a mine. In the visual example given above, all combinations are possible and $m < n$, which implies that no guaranteed mines can be determined from this specific situation alone.

Here is another, simpler example that demonstrates how this logic is used to find a mine:

<img width="295" height="195" alt="{5C1B8740-E776-44DA-BDD2-93A70A580856}" src="https://github.com/user-attachments/assets/ff91f127-4a6f-4c3d-a3d2-7c212222ef0c" />

The cell around which the analysis is being carried out is highlighted in blue. The available free cells nearby are marked in green and designated **0, 1,** and **2**. 

Here, only three combinations for the location of the mines can be considered:
* **0, 1** – Possible
* **0, 2** – Possible
* **1, 2** – Not possible

Of these, only two permissible combinations remain (**0, 1** and **0, 2**). From this, it is clear that cell **0** is definitely a mine (because it appears as a mine in all possible combinations). The second mine is located in either cell **1** or **2**, which no longer has any significance at this stage of the algorithm.

### Determining Safe Cells

Once the mine-searching algorithm records all found mines into the matrix (based on the exclusion method in the main `while` loop), we can then try to determine which cells are definitely *not* mines.

This is done as follows: For each open cell that has mines nearby, the number of adjacent mines is counted. If this count matches the number marked on the cell, then obviously, all other neighboring closed cells (that are not marked as mines) are completely safe. 

On the next iteration, the mine search algorithm will assume these safe cells are open, which further shrinks the lists of valid combinations.

### Uncertain Cells

Finally, after extracting definitive information about certain mines and certain free cells, the approximate probabilities for the remaining uncertain cells are calculated within a range of $(0, 1)$.

For each cell that remains uncertain, we check if it touches any opened cells:

**1. Cells not touching opened cells:** If a closed cell does not touch any opened cell, we only know it is one of many remaining closed cells. Its probability is calculated globally as:

$$P = \frac{\text{Total Mines} - \sum \text{Mines Marked on Opened Cells}}{\text{Number of Cells Not Touching Opened Cells}}$$

**2. Cells touching opened cells:** For cells that *do* touch opened cells, we can compute a more precise local probability. We find the ratio of unknown mines (the number drawn on the opened cell minus the number of known mines around it) to the unknown cells nearby. We then average this ratio across all opened cells that touch the target cell:

$$P_{\text{local}} \approx \text{Average of } \left( \frac{\text{Number on Opened Cell} - \text{Known Mines Nearby}}{\text{Unknown Cells Nearby}} \right)$$

*(Note: This approximation, which is based on averaging probabilities across different sources of evidence, is not perfectly mathematically rigorous and could be improved in the future with more precise calculations.)*
