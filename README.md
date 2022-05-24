# SudokuSolver
A 9x9 board generator for the game of Sudoku made in Unity. This EPC talk inspired me to study the WFC algorithm: [EPC2018 - Oskar Stalberg - Wave Function Collapse in Bad North](https://www.youtube.com/watch?v=0bcZb-SsnrA&t=27s&ab_channel=BUasGames). 

The speed of my implementation is certainly suboptimal compared to the actual WFC algorithm, but to the human eye it can generate boards instantly:

![X0giHmEFqv](https://user-images.githubusercontent.com/49330163/170133970-4c8b4c71-3009-47ae-80cc-3d8173009c74.gif)

Here is a slowed down process of the algorithm creating a valid Sudoku board, tile by tile:

![Nm7VJWBZfk](https://user-images.githubusercontent.com/49330163/170132395-fedd9b14-0df2-4d29-8ee5-fd47592033d1.gif)

This is not a comprehensive study of the algorithm, so I ended up not investing more time into making it a playable product, and my implementation does not allow for much modularity, my objective was merely to be able to generate sudoku boards.
