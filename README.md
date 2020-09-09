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

## Test List

- average level game
- spare bonus
- rolls totalizing 10 accross a frame are not a spare
- strike bonus
- repeated strikes
- last frame with no bonus throws
- last frame spare
- last frame strike
- maximum score

## Average level game

After starting a game and adding 3 then 5 then the score is 8.


VARIABLE SCORE

```forth
: START
    0 SCORE ! ;

: ROLL+ ( #pins -- )
    SCORE +! ;
```

## spare bonus

After adding 6 then 4 then 2 the score is 14.

```forth
VARIABLE SCORE

: START
    0 SCORE ! ;

: ROLL+ ( #pins -- )
    SCORE @ 10 = IF DUP SCORE +! THEN
    SCORE +! ;
```

After adding 3 and 3 then 5 and 5 then 2 the score is 2O
```forth
VARIABLE SCORE
VARIABLE LAST-ROLL
VARIABLE BONUS

: START
    0 SCORE ! 
    0 LAST-ROLL ! 
    BONUS OFF ;

: ROLL+ ( #pins -- )
    BONUS @ IF DUP SCORE +! BONUS OFF THEN
    DUP LAST-ROLL @ + 10 = IF BONUS ON THEN
    DUP LAST-ROLL !
    SCORE +! ;
```
## Refactoring
```forth
: COLLECT-BONUS ( #pins -- #pins )
    BONUS @ IF 
        DUP SCORE +!
        BONUS OFF
    THEN ;

: CHECK-SPARE ( #pins -- #pins )
    DUP LAST-ROLL @ + 10 = IF BONUS ON THEN ;

: ROLL+ ( #pins -- )
    COLLECT-BONUS
    CHECK-SPARE
    DUP LAST-ROLL !
    SCORE +! ;
```
## Spare happens only within a frame boundary

After adding 4 and 4 and 6 and 2 the score is 16
```forth
VARIABLE SCORE
VARIABLE LAST-ROLL
VARIABLE BONUS
-1 CONSTANT NOTHING

: START
    0 SCORE ! 
    NOTHING LAST-ROLL ! 
    BONUS OFF ;

: COLLECT-BONUS ( #pins -- #pins )
    BONUS @ IF 
        DUP SCORE +!
        BONUS OFF
    THEN ;

: CHECK-SPARE ( #pins -- #pins )
    LAST-ROLL @ DUP NOTHING <> IF
        OVER + 10 = IF BONUS ON THEN
    ELSE DROP THEN ;

: ROLL+ ( #pins -- )
    COLLECT-BONUS
    CHECK-SPARE
    LAST-ROLL @ NOTHING = IF DUP ELSE NOTHING THEN 
    LAST-ROLL ! 
    SCORE +! ;
```
## Refactoring
```forth
: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;

: OPEN-FRAME? ( -- flag )
    NEW-FRAME? 0= ;

: CHECK-SPARE ( #pins -- #pins )
    OPEN-FRAME? IF
        DUP LAST-ROLL @ + 10 = IF BONUS ON THEN
    THEN ;

: ADVANCE-FRAME ( #pins -- #pins )
    NEW-FRAME? IF DUP ELSE NOTHING THEN 
    LAST-ROLL ! ;

: ROLL+ ( #pins -- )
    COLLECT-BONUS
    CHECK-SPARE
    ADVANCE-FRAME
    SCORE +! ;
```
## Strike bonus

After 10 and 4 and 2 the score is 22
```forth
: CHECK-STRIKE ( #pins -- #pins )
    NEW-FRAME? IF
        DUP 10 = IF
            BONUS ON
            NEXT-BONUS ON
            ADVANCE-FRAME
        THEN
    THEN ;

: ADVANCE-FRAME ( #pins -- #pins )
    NEW-FRAME? IF DUP ELSE NOTHING THEN 
    LAST-ROLL ! ;

: ROLL+ ( #pins -- )
    COLLECT-BONUS
    CHECK-SPARE
    CHECK-STRIKE
    ADVANCE-FRAME
    SCORE +! ;
```
## Repeated strikes

After 10 and 10 and 4 and 2 and 1 the score is 47
```forth
: START
    0 SCORE ! 
    NOTHING LAST-ROLL ! 
    0 BONUS !
    0 NEXT-BONUS ! ;

: COLLECT-BONUS ( #pins -- #pins )
    BONUS @ ?DUP IF OVER * SCORE +! THEN
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;

: OPEN-FRAME? ( -- flag )
    NEW-FRAME? 0= ;

: CHECK-SPARE ( #pins -- #pins )
    OPEN-FRAME? IF
        DUP LAST-ROLL @ + 10 = IF 1 BONUS ! NEXT-BONUS OFF THEN
    THEN ;

: ADVANCE-FRAME ( #pins -- #pins )
    NEW-FRAME? IF DUP ELSE NOTHING THEN 
    LAST-ROLL ! ;

: CHECK-STRIKE ( #pins -- #pins )
    NEW-FRAME? IF
        DUP 10 = IF
            1 BONUS +!
            1 NEXT-BONUS !
            ADVANCE-FRAME
        THEN
    THEN ;
```
## Last Frame, no bonus throws

After 10 frames and no bonus, adding throws does not increase score
```forth
VARIABLE SCORE
VARIABLE LAST-ROLL
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE FRAME#
-1 CONSTANT NOTHING

: START
    0 SCORE ! 
    NOTHING LAST-ROLL ! 
    0 BONUS !
    0 NEXT-BONUS ! 
    0 FRAME# ! ;

: COLLECT-BONUS ( #pins -- #pins )
    BONUS @ ?DUP IF OVER * SCORE +! THEN
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;

: OPEN-FRAME? ( -- flag )
    NEW-FRAME? 0= ;

: CHECK-SPARE ( #pins -- #pins )
    OPEN-FRAME? IF
        DUP LAST-ROLL @ + 10 = IF 1 BONUS ! NEXT-BONUS OFF THEN
    THEN ;

: ADVANCE-FRAME ( #pins -- #pins )
    OPEN-FRAME? IF FRAME# @ 1+ 10 MIN FRAME# ! THEN 
    NEW-FRAME? IF DUP ELSE NOTHING THEN LAST-ROLL ! ;

: CHECK-STRIKE ( #pins -- #pins )
    NEW-FRAME? IF
        DUP 10 = IF
            1 BONUS +!
            1 NEXT-BONUS !
            ADVANCE-FRAME
        THEN
    THEN ;

: ROLL+ ( #pins -- )
    COLLECT-BONUS
    CHECK-SPARE
    CHECK-STRIKE
    FRAME# @ 0 10 WITHIN IF DUP SCORE +! THEN 
    ADVANCE-FRAME DROP ;
```
## Refactoring
```forth
: COLLECT-BONUS ( #pins -- )
    BONUS @ ?DUP IF * SCORE +! ELSE DROP THEN
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;

: OPEN-FRAME? ( -- flag )
    NEW-FRAME? 0= ;

: CHECK-SPARE ( #pins -- )
    OPEN-FRAME? IF
        LAST-ROLL @ + 10 = IF 1 BONUS ! NEXT-BONUS OFF THEN
    ELSE DROP THEN ;

: ADVANCE-FRAME ( #pins -- )
    NEW-FRAME? IF
        LAST-ROLL ! 
    ELSE 
        DROP 
        NOTHING LAST-ROLL !
        FRAME# @ 1+ 10 MIN FRAME# !
    THEN ;

: CHECK-STRIKE ( #pins -- )
    NEW-FRAME? IF
        DUP 10 = IF
            1 BONUS +!
            1 NEXT-BONUS !
            DUP ADVANCE-FRAME
        THEN
        DROP
    ELSE
        DROP
    THEN ;

: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    DUP CHECK-SPARE
    DUP CHECK-STRIKE
    FRAME# @ 0 10 WITHIN IF DUP SCORE +! THEN 
    ADVANCE-FRAME ;
```
## Refactoring 
```forth
: CLOSE-FRAME 
    NOTHING LAST-ROLL ! ;

: CHECK-SPARE ( #pins -- )
    LAST-ROLL @ + 10 = IF 
        1 BONUS ! NEXT-BONUS OFF 
    THEN 
    CLOSE-FRAME ;

: ADVANCE-FRAME 
    NEW-FRAME? IF
        FRAME# @ 1+ 10 MIN FRAME# !
    THEN ;

: CHECK-STRIKE ( #pins -- )
    DUP 10 = IF
        DROP
        1 BONUS +!
        1 NEXT-BONUS !
        CLOSE-FRAME 
    ELSE
        LAST-ROLL !
    THEN ;

: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    NEW-FRAME? IF DUP CHECK-STRIKE ELSE DUP CHECK-SPARE THEN
    FRAME# @ 0 10 WITHIN IF SCORE +! ELSE DROP THEN 
    ADVANCE-FRAME ;
```
## Last Frame Spare
After 9 average frame and a spare only the bonus roll is counted

## Last Frame Strike
After 10 strikes only bonus rolls are counted
