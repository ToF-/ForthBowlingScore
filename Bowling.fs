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

: COLLECT-BONUS ( #pins -- )
    BONUS @ ?DUP IF * SCORE +! ELSE DROP THEN
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;

: OPEN-FRAME? ( -- flag )
    NEW-FRAME? 0= ;

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
    FRAME# @ 0 10 WITHIN IF 
        NEW-FRAME? IF DUP CHECK-STRIKE ELSE DUP CHECK-SPARE THEN
        SCORE +! 
        ADVANCE-FRAME 
    ELSE DROP
    THEN ;
