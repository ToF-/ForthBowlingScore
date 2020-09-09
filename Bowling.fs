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
