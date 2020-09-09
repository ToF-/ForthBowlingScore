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

It looks like a simple problem of adding rolls to a current *score* variable, but things get complicated very quick, given that the bonuses for a roll will only be counted in the *next* rolls. 

So we need to keep the bonuses in variables as well as score.

```forth
VARIABLE SCORE
VARIABLE BONUS
VARIABLE NEXT-BONUS
```

Bonus detection depends on the frame situation. A 10 means a *strike* only if the player was delivering their first roll on the frame. If a roll is a second roll, we have to add this number to the preceding roll delivered to check for a *spare*. 

```forth
VARIABLE LAST-ROLL
```
We could use this variable to detect a new-frame: if at any the variable contains a number between 0 and 9, we are on an open frame. If the variable contains another value -- let's call it `NOTHING` -- then it's a new frame. 

```forth
-1 CONSTANT NOTHING

: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;
```

The frame count is also important.

```forth
VARIABLE FRAME#
```
The frame will need to advance until it is 10, meaning 10 frames have been played. On a new frame, `LAST-ROLL` has no value.

```forth
: FRAME++
    FRAME# @ 1+ 10 MIN FRAME# ! 
    NOTHING LAST-ROLL ! ;
```
At the beginning of a game, all these variables should be initialized:

```forth
: START 
    0 SCORE !
    0 FRAME# !
    NOTHING LAST-ROLL !
    0 BONUS !
    0 NEXT-BONUS ! ;
```

The first task after a player delivered a roll is to collect the bonus from the previous frame. To collect an existing bonus, we multiply the number of pins with that bonus and add the result to the score. Next, we should advance the bonuses: the next time we collect bonuses, the *next bonus* will be the current bonus, and next-bonus should be set to zero. 

```forth
: COLLECT-BONUS ( #pins -- )
    BONUS @ * SCORE +!
    NEXT-BONUS @ BONUS ! 
    0 NEXT-BONUS ! ;
```

How are these variables `BONUS` and `NEXT-BONUS` set? In case of a strike, the bonus is *increased by one*: that way several strikes in a row will produce a bonus of 2, and the next bonus is one. 

```forth
: STRIKE!
    1 BONUS +!  1 NEXT-BONUS ! ;
```

In case of a spare, the bonus is set to 1 (there's no need to keep track of a current bonus, because a spare can't be determined *just* after a strike roll, only after a first roll in an open frame), and the next bonus is set to 0.

```forth
: SPARE!
    1 BONUS ! 0 NEXT-BONUS ! ;
```

The next task is to check for new bonus. Depending if the player is playing a new frame or an open frame, the program has to check for different things. If on a new frame, look for a strike and if there is one, changes the bonus and advance the frame count. Otherwise, save the roll for the next calculation.

```forth
: NEW-CHECK ( #pins -- ) 
    DUP 10 = IF 
        DROP STRIKE! FRAME++ 
    ELSE
        LAST-ROLL !  
    THEN ;
```
For an open frame, we have to check the last roll, and if this roll + the last roll makes 10, we have a spare. After that the frame count is incremented, since the player just finished an open frame.

```forth
: OPEN-CHECK ( #pins -- )
    LAST-ROLL @ + 10 = IF SPARE! THEN
    FRAME++ ;
```

Thus checking the bonus is done by calling either one or the other of the two words.

```forth
: CHECK-BONUS ( #pins -- )
    NEW-FRAME? IF NEW-CHECK ELSE OPEN-CHECK THEN ;
```

After the last frame strike or open frame was played, supplementary rolls are only added as bonus to the score, not as real throws. Since the score should not exceed the score obtained with the last frame, no new bonus should be detected. Therefore we need to check that the game is not past the 10th frame. 

```forth
: BEFORE-10TH-FRAME? 
    FRAME# @ 0 10 WITHIN ;
```
Here is the last definition:
```forth
: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS 
    BEFORE-10TH-FRAME? IF 
        DUP CHECK-BONUS 
        SCORE +!
    THEN ;
```

