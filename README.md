# The bowling score kata

## The Problem to Solve
> A game of bowling consists of ten frames. In each frame, the bowler will have two chances to knock down as many pins as possible with their bowling ball. In games with more than one bowler, as is common, every bowler will take their frame in a predetermined order before the next frame begins. If a bowler is able to knock down all ten pins with their first ball, he is awarded a strike. If the bowler is able to knock down all 10 pins with the two balls of a frame, it is known as a spare. Bonus points are awarded for both of these, depending on what is scored in the next 2 balls (for a strike) or 1 ball (for a spare). If the bowler knocks down all 10 pins in the tenth frame, the bowler is allowed to throw 3 balls for that frame. This allows for a potential of 12 strikes in a single game, and a maximum score of 300 points, a perfect game.

(source:[https://www.playerssports.net/page/bowling-rules#:~:text=Rules%20of%20play,before%20the%20next%20frame%20begins.])

Write a program which, given a series of rolls delivered by a player, computes the score of this player. The roll values will be consistent with the game rules: no entry will contain values that would not be possible in a real game (e.g 11, or 7 then 6 in the same frame, or more than ten frames of rolls). In test cases where not all rolls have been played, the resulting value should be the minimum score obtained (the score value if all the subsequent rolls were 0).

## Specifications
### Input

- *t* – the number of test cases, then t test cases follows.
- each test case consists in 2 lines:
    - *n* - the number of rolls delivered, ( 0 < *n* ≤ 21 )
    - *r1*,..*rn* - the rolls delivered    ( 0 ≤ *r* ≤ 10 ) 

### Output

For each test case output one integer: the score made by the player after they played all the rolls in the test case.

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

## Design Ideas
### Main Routine
Let's pretend we have one of these bowling score sheets and use them to mark the points made as we receive the numbers.

    *10* : we mark a strike `X` and are ready to use the next frame column on the sheet. The score for the frame is still not calculable given that the next frame throws are not known yet.

    *3* : we mark a 3 on the first column of the frame. The score for frame 1 is still not known.

    *7* : we mark a spare '/' and are ready to use the next frame column. The score for the first frame is 20, and the score for this frame is not known yet, as we are expecting a bonus from the next throw.

    *5* : we mark a 5 on the first column of the frame. The score for frame 1 + frame 2 is 35.

What we do is:
    - keep track of the bonus generated by a strike or a spare
    - collect the extra points when the roll corresponding to a bonus is being delivered
    - decide, depending on the roll delivered, if we have to "close the frame", meaning go to a new frame column or play again in the current frame ("open frame")
    - keep track of the frame number in order to count the last supplementary rolls as part of the last frame, and not the beginning of a new frame.
    - and of course, add the points (roll + extra) to the current score

This is basically what our program has to do when receiving a value from the input stream. Since the spec is not asking us to keep track of the different score values for each frame, the program should update the score in the same time the roll is treated. Hence the pseudo code:
```forth
    : ROLL+ ( #pins -- )
        ( collect extra point from previous bonus applied to the #pins value )
        ( frame# is between 0 and 9) IF
            ( check for bonus with this value given the current frame state )
            ( close the frame or make it open depending on previous step )
            ( add the roll value to the score ) 
            ( increase frame count if the frame is closed )
        THEN ;
```
### Bonus
The bonus mechanism is like a dispenser: we feed it with bonus points gained from a strike or a spare, and these bonus points get used each time as a factor for extra score when a new roll is added to the game. Once the bonus for a roll is consumed, the bonus dispenser prepares the next value to be used. Here is the pseudo code, followed by the expected effect on the stack.
```forth
    : STRIKE! ( feeds the bonus dispenser with new points ) … ;
    : SPARE!  ( feeds the bonus dispenser with a point ) … ;

    : BONUS>  ( -- factor ) ( dispenses the factor and advance the bonus ) … ;

    : START   ( starts the game, emptying the dispenser ) … ;

    START BONUS> .  ⏎
    0 ok
    START SPARE! BONUS> . BONUS> . ⏎
    1 0 ok
    START STRIKE! BONUS> . BONUS> . ⏎
    1 1 ok
    START STRIKE! BONUS> . STRIKE! BONUS> . BONUS> . ⏎
    1 2 1
```
### Frame State
How can we know, when adding a roll to the game if that roll is part of an open frame or if it starts a new frame? We have to keep track of the current frame state. If the state is open, then we should be able to retrieve the first roll value from this frame.

```forth
    : OPEN-FRAME! ( #pins -- ) ( marks the frame as open and keeps the roll value ) … ;
    : OPEN-FRAME? ( -- flag ) ( trues if the frame is open ) … ;
    : NEW-FRAME? ( -- flag ) OPEN-FRAME? 0= ;
    : LAST-ROLL ( -- #pins ) ( returns the last roll value assuming the frame is open ) … ;
    : CLOSE-FRAME! ( -- ) ( marks the frame as closed ) … ;
    : START ( -- ) ( should also initialize the frame as new ) … ;

    START NEW-FRAME? .  ⏎
    -1 ok
    7 OPEN-FRAME! NEW-FRAME? . OPEN-FRAME? . LAST-ROLL .  ⏎
    0 -1 7 ok
    3 OPEN-FRAME! CLOSE-FRAME! NEW-FRAME? .  ⏎
    -1 ok
```
### Frame Count
While keeping track of the frame count we have to follow 3 rules:
- closing a frame increases the frame count by one
- the frame number cannot exceed 10
- after 10 frames have been played, supplementary rolls are only counted as bonus 

```forth
    VARIABLE FRAME#
    : FRAME> ( increases the frame by one, capped to 10 ) … ;
    : START ( -- ) ( should also initialize the frame count to 0 ) … ;

    START FRAME> FRAME# ? ⏎
    0 ok
    FRAME> FRAME> FRAME> FRAME> FRAME> FRAME> FRAME> FRAME>
    FRAME> FRAME> FRAME> FRAME> FRAME# ? ⏎
    10 ok
```
## Implementation
### Initialization
Let's begin with creating some variables, and a word to start the game:

```forth
VARIABLE SCORE
VARIABLE BONUS
VARIABLE FRAME-STATE
VARIABLE FRAME#

: START
    0 SCORE !
    0 BONUS !
    0 FRAME-STATE !
    0 FRAME# ! ;
```
### Bonus Factors
There are 2 factors to consider: the bonus factor (to be applied to the next roll) and the next bonus factor (to be applied to the roll following the next). These two tiny values can be stored in the same variable, witch makes switching from bonus to next bonus very simple. The bonus part will occupy bits 0 to 1, while the next bonus part will be represented by bit 2.

A spare creates a bonus factor of 1 and sets the next bonus factor to 0:
```forth
: SPARE! 1 BONUS ! ;
```
A strike increments the current bonus factor, and sets the next bonus factor (bit 3) to 1 with a bitwise `OR` operation:
```forth
: STRIKE! BONUS @ 1+ 4 OR BONUS ! ;
```
Consuming the bonus consists in getting the bonus value (bits 0 to 1) on the stack and then shifting the bonus value to make the next bonus the current bonus.
```forth
: BONUS> ( -- factor )
    BONUS @ DUP 3 AND 
    SWAP 2 RSHIFT BONUS ! ;
```
### Collecting Bonus
Given a roll value, multiply it by the bonus factor, and add that to the current score.
```forth
: COLLECT-BONUS ( #pins -- )
    BONUS> * SCORE +! ;
```
### Frame State
Marking the frame as closed can be done by setting the frame state to zero. Opening it can be done by setting its value to the last roll + 1 (since the value of the last roll can be comprised between 0 and 9).
```forth
: CLOSE-FRAME!
    0 FRAME-STATE ! ;

: OPEN-FRAME! ( #pins -- )
    1+ FRAME-STATE ! ;

: OPEN-FRAME? ( -- flag )
    FRAME-STATE @ ;

: NEW-FRAME? ( -- flag )
    OPEN-FRAME? 0= ;

: LAST-ROLL ( -- #pins )
    FRAME-STATE @ 1 - ;
```
### Frame Count
Advancing the frame count is only possible when the frame is not open, and to a max of 10.
```forth
: FRAME>
    NEW-FRAME? IF FRAME# @ 1+ 10 MIN FRAME# ! THEN ;
```
### Checking for Bonus
We have a strike if the frame is a new frame and the roll is a 10. In that case, add points to the bonus and close the frame. 

If the frame is new but we don't have a strike then open the frame.

If the frame is open and the roll added to the previous one makes a 10, we have a spare. 

If the frame is open, wether we have a spare or not we have to close the frame.
```forth
: CHECK-STRIKE ( #pins -- )
    DUP 10 = IF STRIKE! CLOSE-FRAME! ELSE OPEN-FRAME! THEN ;

: CHECK-SPARE ( #pins -- )
    LAST-ROLL + 10 = IF SPARE! THEN CLOSE-FRAME! ;

: CHECK-BONUS ( #pins -- )
    NEW-FRAME? IF CHECK-STRIKE ELSE CHECK-SPARE THEN ;
```
### Adding a Roll to the Current Score
We are almost done. Adding a roll to the game will execute these task of collecting bonus, checking for new bonus, increasing the score and advancing the frame count.
```forth
: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    FRAME# @ 0 10 WITHIN IF
        DUP CHECK-BONUS
        SCORE +!
        FRAME>
    THEN ;


