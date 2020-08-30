
The word `START-GAME` will reinitialize the bowling game, setting the score to 0.
 
The word `SCORE ( -- score ) ` will return the current score.

The word `ADD-ROLL ( pins -- ) ` will register knocked down pins and calculate the score.

The game state:

- current score                      ( 0 .. 300)
- current frame 1st throw / nothing  ( 0 .. 10 / 255 )
- bonus on the next throw            ( 0,1,2 )
- bonus on the next next throw       ( 0,1,2 )
- numero of current frame            ( 0..10 )
