\ Bowling.fs

VARIABLE SCORE
VARIABLE PREV-ROLL
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE OPEN-FRAME

: START-GAME 
    0 SCORE ! 0 PREV-ROLL ! 0 BONUS ! 
    0 NEXT-BONUS ! FALSE OPEN-FRAME ! ;

: COLLECT-BONUS ( pins -- )
    BONUS @ ?DUP IF * SCORE +! ELSE DROP THEN 
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: STRIKE? ( pins -- flag )
    10 = OPEN-FRAME @ 0= AND ;

: SPARE? ( pins -- flag )
    PREV-ROLL @ + 10 =
    OPEN-FRAME @ AND ;

: MARK-STRIKE 
    1 BONUS +!
    1 NEXT-BONUS !
    TRUE OPEN-FRAME ! ;

: MARK-SPARE 
    1 BONUS ! ;

: CALC-BONUS ( pins -- )
    DUP STRIKE? IF
        DROP MARK-STRIKE
    ELSE SPARE? IF 
        MARK-SPARE THEN 
    THEN ;

: ADD-PINS
    SCORE +! ;

: ADVANCE-FRAME 
    OPEN-FRAME @ 0= OPEN-FRAME ! ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    DUP CALC-BONUS
    DUP PREV-ROLL !
    ADD-PINS
    ADVANCE-FRAME ;


