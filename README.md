# The bowling score kata

A game of bowling consists of ten frames. In each frame, the bowler will have two chances to knock down as many pins as possible with their bowling ball. In games with more than one bowler, as is common, every bowler will take their frame in a predetermined order before the next frame begins. If a bowler is able to knock down all ten pins with their first ball, he is awarded a strike. If the bowler is able to knock down all 10 pins with the two balls of a frame, it is known as a spare. Bonus points are awarded for both of these, depending on what is scored in the next 2 balls (for a strike) or 1 ball (for a spare). If the bowler knocks down all 10 pins in the tenth frame, the bowler is allowed to throw 3 balls for that frame. This allows for a potential of 12 strikes in a single game, and a maximum score of 300 points, a perfect game.

(source:[https://www.playerssports.net/page/bowling-rules#:~:text=Rules%20of%20play,before%20the%20next%20frame%20begins.])

Write a program which, given a series of rolls delivered by a player, computes the score of this player. The roll values will be consistent with the game rules: no entry will contain values that would not be possible in a real game (e.g 11, or 7 then 6 in the same frame, or more than ten frames of rolls). In test cases where not all rolls have been played, the resulting value should be the minimum score obtained (the score value if all the subsequent rolls were 0).

## Input

- *t* – the number of test cases, then t test cases follows.
- each test case consists in 2 lines:
    - *n* - the number of rolls delivered, ( 0 < *n* ≤ 21 )
    - *r1*,..*rn* - the rolls delivered    ( 0 ≤ *r* ≤ 10 ) 

## Output

For each test case output one integer: the score made given the test case rolls.

## Example
### Input

    3
    3
    4 6
    4
    10 3 7 5
    12
    10 10 10 10 10 10 10 10 10 10 10 10

### Output

    10
    40
    300

# Design Ideas 

The main definition will read the number of cases, then for each case read the number of rolls, compute the score obtained from the rolls and print it.

    : MAIN GET-NUMBER ?DUP 0 DO 
        GET-NUMBER START ?DUP 0 DO 
            GET-NUMBER ROLL+ 
        LOOP 
        .SCORE END 
    LOOP ;

- `START` initializes the game
- `ROLL+` adds a roll to the current game, updating the score
- `.SCORE` outputs the current score on the terminal
- `END` cleans the stack

During the game, adding a roll involves: counting the bonus from previous frame, checking for new bonus, and adding the number of pins to the score. After the 10th frame, things are different: we are only counting the rolls as bonus points; new bonus cannot be earned, and the rolls are not added to the score.

    : ROLL+ ( #pins -- )
        DUP COUNT-BONUS
        FRAME @ 0 10 WITHIN IF
            DUP CHECK-BONUS
            SCORE +!
        ELSE DROP THEN ;

The first thing that has to be done with a roll is to count it as a bonuns for the previous frame the player played. The number of pins is multiplied by the bonus count and added to the score. Then the bonus is advanced, meaning that what was kept for the roll following the next roll is now counting for the next roll.

    : COUNT-BONUS ( #pins -- )
        BONUS @ * SCORE +!
        NEXT-BONUS @ BONUS ! 
        0 NEXT-BONUS ! ;

Next, we have to examine the number of pins and compute the bonus earned from this roll. The calculation is very different if we are on a new frame or an open frame.

    : CHECK-BONUS ( #pins -- )
        OPEN-FRAME? IF CHECK-SPARE ELSE CHECK-STRIKE THEN ;

If the player is playing a new frame, delivering a strike will set the bonus for the next two rolls, and the game is on a new frame again. Delivering less than 10 will not affect the bonus, and the player will have another roll in the turn: the number of pins must be saved in order to detect a spare on the next roll.

    : CHECK-STRIKE ( #pins -- ) 
        DUP 10 = IF 
            DROP STRIKE! FRAME++ 
        ELSE
            LAST-ROLL !  
        THEN ;

If the player is playing their second roll in the frame, delivering a spare will set the bonus for the next roll, and the game is on a new frame again. The last roll is checked to see if we have a spare.

: CHECK-SPARE ( #pins -- )
    LAST-ROLL @ + 10 = IF SPARE! THEN
    FRAME++ ;

We can tell if the game is on a new frame or an open frame by checking the `LAST-ROLL` variable and using a convention: if its value is within 0 and 9, we have an open frame, if it is anything else the game is on a new frame.

    : OPEN-FRAME? ( -- flag )
        LAST-ROLL @ 0 10 WITHIN ;

Thus, getting on a new frame will set the value of the last roll to 'nil'. Also, there's no more than 10 frames in the game.

    : FRAME++
        -1 LAST-ROLL !
        FRAME @ 1+ 10 MIN FRAME ! ;


Now we know more about what needs to be initialized at the outset of a game:

    : START
        0 FRAME ! -1 LAST-ROLL ! 0 SCORE ! ;

The bonus rule for a strike is : the next roll will have to be counted as bonus once more, and the following roll will have to be counted as bonus once. Adding 1 to the bonus makes it possible to have a bonus of 2, in the case when the players delivers several strikes in a row.

    : STRIKE!
        1 BONUS +!  1 NEXT-BONUS ! ;
