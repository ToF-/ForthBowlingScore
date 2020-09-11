# The bowling score kata

Write a program which, given a series of rolls delivered by a Ten Pin Bowling player, computes the score of this player. The roll values will be consistent with the game rules: no entry will contain values that would not be possible in a real game (e.g 11, or 7 then 6 in the same frame, or more than ten frames of rolls). In test cases where not all rolls have been played, the resulting value should be the minimum score obtained (i.e the score value if all the subsequent rolls were 0).

## The Problem to Solve
Here is an excerpt of the [game rules](https://www.playerssports.net/page/bowling-rules#:~:text=Rules%20of%20play,before%20the%20next%20frame%20begins.):
> A game of bowling consists of ten frames. In each frame, the bowler will have two chances to knock down as many pins as possible with their bowling ball. In games with more than one bowler, as is common, every bowler will take their frame in a predetermined order before the next frame begins. If a bowler is able to knock down all ten pins with their first ball, he is awarded a strike. If the bowler is able to knock down all 10 pins with the two balls of a frame, it is known as a spare. Bonus points are awarded for both of these, depending on what is scored in the next 2 balls (for a strike) or 1 ball (for a spare). If the bowler knocks down all 10 pins in the tenth frame, the bowler is allowed to throw 3 balls for that frame. This allows for a potential of 12 strikes in a single game, and a maximum score of 300 points, a perfect game.


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
    10 7 3 5
    12
    10 10 10 10 10 10 10 10 10 10 10 10

### Output

    10
    40
    300

## Design Ideas
### Main Routine
Let's pretend we have one of these bowling score sheets and use them to mark the points made as we receive the numbers.

- *10* : we mark a strike `X` and are ready to use the next frame column on the sheet. The score for the frame is still not calculable given that the next frame throws are not known yet.

![score_sheet](/images/score_sheet1.png | width=75%)
- *7* : we mark a 7 on the first column of the frame. The score for frame 1 is still not known.

![score_sheet](/images/score_sheet2.png)
- *3* : we mark a spare '/' and are ready to use the next frame column. The score for the first frame is 20, and the score for this frame is not known yet, as we are expecting a bonus from the next throw.

![score_sheet](/images/score_sheet3.png)
- *5* : we mark a 5 on the first column of the frame. The score for frame 1 + frame 2 is 35.

![score_sheet](/images/score_sheet4.png)
So what we routinely do to keep the score is:
- mark the points made for the current frame
- keep track of the bonus generated by a strike or a spare
- collect the extra points when the roll corresponding to a bonus is being delivered and mark the score on the corresponding frame column
- decide, depending on the roll delivered, if we have to "close the frame", meaning go to a new frame column or play again in the current frame ("open frame")
- keep track of how many frame were already played in order to count the last supplementary rolls as part of the 10th frame, and not the beginning of a new frame.
- and of course, add the points (roll + extra) to the current score

This is basically what our program has to do when receiving a value from the input stream. The only difference is that our program will not need to mark the points and the score for each specific frame, just to keep the score updated. Here is some pseudo code:
```forth
    : ROLL+ ( #pins -- )
        ( collect extra point from previous bonus applied to the #pins value )
        ( frame count is between 0 and 9) IF
            ( add the roll value to the score )
            ( check for bonus with this value given the current frame state )
            ( close the frame or make it open depending on previous step )
            ( increase frame count if the frame is closed )
        THEN ;
```
### Bonus
The bonus mechanism is like a dispenser: we feed it with bonus points gained from a strike or a spare, and these bonus points get used as a factor for extra score each time a new roll is added to the game. Once the bonus for a roll is consumed, the bonus dispenser prepares the next value to be used. Here is the pseudo code, followed by the expected effect on the stack.
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
How can we know when adding a roll to the game if that roll is part of an open frame or if it starts a new frame? We have to keep track of the current frame state. If the state is open, then we should be able to retrieve the first roll value from this frame.

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
While keeping track of the frame count we have to follow 2 rules:
- the frame number cannot exceed 10
- closing a frame increases the frame count by one

```forth
    VARIABLE FRAME#
    : FRAME> ( increases the frame by one, capped to 10 ) … ;
    : START ( -- ) ( should also initialize the frame count to 0 ) … ;
    : CLOSE-FRAME! ( -- ) FRAME> … ( mark the frame as closed ) … ;

    START FRAME# ? ⏎
    0 ok
    FRAME> FRAME# ? ⏎
    1 ok
    FRAME> FRAME> FRAME> FRAME> FRAME> FRAME> FRAME> FRAME>
    FRAME> FRAME> FRAME> FRAME> FRAME# ? ⏎
    10 ok
```
Now that we have identified some of the definitions we'll need, we can start to implement them, and once this is done, try to assemble them in our `ROLL+` definition.
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
There are 2 factors to consider: the bonus factor (to be applied to the next roll) and the next bonus factor (to be applied to the roll following the next). These two tiny values can be stored in the same variable, which makes switching from bonus to next bonus very simple. The bonus part will occupy bits 0 to 1, while the next bonus part will be represented by bit 2.

A spare creates a bonus factor of 1 and sets the next bonus factor to 0:
```forth
: SPARE! 1 BONUS ! ;
```
A strike increments the current bonus factor, and sets the next bonus factor (bit 3) to 1 with a bitwise `OR` operation:
```forth
: STRIKE! BONUS @ 1+ 4 OR BONUS ! ;
```
Consuming the bonus consists in isolating the bonus value (bits 0 and 1) with a bitwise `AND`, leaving that value on the stack and then right-shifting the bonus value by 2 position on the right to make the next bonus (bit 2) the current bonus.
```forth
: BONUS> ( -- factor )
    BONUS @ DUP 3 AND
    SWAP 2 RSHIFT BONUS ! ;
```
### Collecting Bonus
This is done by multiply the roll value by the bonus factor, and adding that to the score.
```forth
: COLLECT-BONUS ( #pins -- )
    BONUS> * SCORE +! ;
```
### Frame Count
The frame count can be incremented by one every time the player closes a frame, but cannot exceed 10, hence the use of `MIN` here: 
```forth
: FRAME>
    FRAME# @ 1+ 10 MIN FRAME# ! ;
```
### Frame State
A frame is either a new frame, meaning the player hasn't thrown a roll yet, or an open frame, meaning the player has already thrown one roll. If the value of the frame state is zero, we consider the frame to be new.
```forth
: OPEN-FRAME? ( -- flag )
    FRAME-STATE @ ;

: NEW-FRAME? ( -- flag )
    OPEN-FRAME? 0= ;
```
Marking the frame as closed can be done by setting the frame state to zero, and advancing the frame count.
```forth
: CLOSE-FRAME!
    0 FRAME-STATE ! FRAME> ;
```
Opening the frame can be done by setting its value to the last roll + 1,since the value of the last roll can be comprised between 0 and 9. Symmetrically, the last roll can be retrieved by subtracting 1 from the frame state value.
```forth
: OPEN-FRAME! ( #pins -- )
    1+ FRAME-STATE ! ;

: LAST-ROLL ( -- #pins )
    FRAME-STATE @ 1- ;
```
### Checking for Bonus
We have a strike if the frame is a new frame and the roll is a 10. In that case, add points to the bonus and close the frame.

If the frame is new but we don't have a strike then open the frame, saving the roll value.

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
We are almost done. Adding a roll to the game will execute these tasks of collecting bonus, adding the roll points to the score, checking for new bonus, and advancing the frame count.
```forth
: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    FRAME# @ 0 10 WITHIN IF
        SCORE +!
        DUP CHECK-BONUS
    THEN ;
```
Let's try our word!
```forth
START 4 ROLL+ SCORE ?  ⏎
4  ok
6 ROLL+ SCORE ?  ⏎
10  ok
3 ROLL+ 2 ROLL+ SCORE ?  ⏎
18  ok
10 ROLL+ SCORE ?  ⏎
28  ok
4 ROLL+ 2 ROLL+ SCORE ?  ⏎
40  ok
10 ROLL+ SCORE ?  ⏎
50  ok
10 ROLL+ SCORE ?  ⏎
70  ok
6 ROLL+ 4 ROLL+ SCORE ?  ⏎
96  ok
3 ROLL+ 5 ROLL+ SCORE ?  ⏎
107  ok
10 ROLL+ SCORE ?  ⏎
117  ok
8 ROLL+ 2 ROLL+ 7 ROLL+ SCORE ?  ⏎
144  ok
: PERFECT START 12 0 DO 10 ROLL+ LOOP SCORE ? ; PERFECT ⏎
300  ok
```
Success!
## Getting Numbers From the Input Stream
To get a number, we have to read the input stream character by character, skipping them until we find a digit, then reading characters while these are digits, accumulating them into the resulting number.

The standard word `DIGIT? ( char -- n,-1|0 )` returns *false* if the character on the stack is not a digit, or *true*, preceded with the matching digit otherwise.
```forth
32 DIGIT? . ⏎
0 ok
50 DIGIT? . .  ⏎
-1 2 ok
```
Here's a word to skip all input characters until a digit is met:
```forth
: SKIP-NON-DIGIT ( -- n )
    BEGIN KEY DIGIT? 0= WHILE REPEAT ;
```
And here's a word to get a number:
```forth
: GET-NUMBER ( -- n )
    0 SKIP-NON-DIGIT
    BEGIN
        SWAP 10 * +
        KEY DIGIT?
    0= UNTIL ;

GET-NUMBER . ⏎
foo4807  ⏎
4807 ok
```
## Main program
Armed with this word, we can now add the top definition to our program, and finally call that word before leaving the Forth environment.
```forth
: BOWLING
    GET-NUMBER ?DUP 0 DO
        START
        GET-NUMBER ?DUP 0 DO
            GET-NUMBER ROLL+
        LOOP
        SCORE ? CR
    LOOP ;

BOWLING
BYE
```
# Testing

Given the this input file:
```
# input.dat: a test file for Bowling.fs
5
4
3 5 2 7
6
10 5 4 10 5 2
12
10 10 10 10 10 10 10 10 10 10 10 10
20
3 5 3 5 3 5 3 5 3 5 3 5 3 5 3 5 3 5 3 5
3
10 10 10
```
The following result is obtained:
```
gforth Bowling.fs <input.dat ⏎
17
52
300
80
60
```
Et voilà!

# The Program
```forth
VARIABLE SCORE VARIABLE BONUS VARIABLE FRAME-STATE VARIABLE FRAME#

: START 0 SCORE ! 0 BONUS ! 0 FRAME-STATE ! 0 FRAME# ! ;

: SPARE! 1 BONUS ! ;
: STRIKE! BONUS @ 1+ 4 OR BONUS ! ;

: BONUS> ( -- factor )
    BONUS @ DUP 3 AND
    SWAP 2 RSHIFT BONUS ! ;

: COLLECT-BONUS ( #pins -- ) BONUS> * SCORE +! ;

: CLOSE-FRAME!  0 FRAME-STATE ! ;
: OPEN-FRAME! ( #pins -- ) 1+ FRAME-STATE ! ;
: OPEN-FRAME? ( -- flag ) FRAME-STATE @ ;
: NEW-FRAME? ( -- flag ) OPEN-FRAME? 0= ;
: LAST-ROLL ( -- #pins ) FRAME-STATE @ 1 - ;

: FRAME>
    NEW-FRAME? IF FRAME# @ 1+ 10 MIN FRAME# ! THEN ;

: CHECK-STRIKE ( #pins -- )
    DUP 10 = IF STRIKE! CLOSE-FRAME! ELSE OPEN-FRAME! THEN ;

: CHECK-SPARE ( #pins -- )
    LAST-ROLL + 10 = IF SPARE! THEN CLOSE-FRAME! ;

: CHECK-BONUS ( #pins -- )
    NEW-FRAME? IF CHECK-STRIKE ELSE CHECK-SPARE THEN ;

: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    FRAME# @ 0 10 WITHIN IF
        DUP CHECK-BONUS
        SCORE +!
        FRAME>
    THEN ;

: SKIP-NON-DIGIT ( -- d )
    BEGIN KEY DIGIT? 0= WHILE REPEAT ;

: GET-NUMBER ( -- n )
    0 SKIP-NON-DIGIT
    BEGIN
        SWAP 10 * +
        KEY DIGIT?
    0= UNTIL ;

: BOWLING
    GET-NUMBER ?DUP 0 DO
        START
        GET-NUMBER ?DUP 0 DO
            GET-NUMBER ROLL+
        LOOP
        SCORE ? CR
    LOOP ;

BOWLING
BYE

```
