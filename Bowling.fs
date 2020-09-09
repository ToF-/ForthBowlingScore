VARIABLE SCORE
VARIABLE LAST-ROLL
VARIABLE BONUS
VARIABLE NEXT-BONUS
-1 CONSTANT NOTHING

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

: ROLL+ ( #pins -- )
    COLLECT-BONUS
    CHECK-SPARE
    CHECK-STRIKE
    ADVANCE-FRAME
    SCORE +! ;
