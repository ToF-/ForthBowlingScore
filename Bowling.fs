VARIABLE _SCORE
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE LAST-ROLL
VARIABLE OPEN-FRAME

: START-GAME 
    _SCORE     OFF 
    BONUS      OFF 
    NEXT-BONUS OFF
    LAST-ROLL  OFF 
    OPEN-FRAME OFF ;

: SCORE 
    _SCORE @ ;

: COLLECT-BONUS ( pins -- )
    BONUS @ IF
        BONUS @ * _SCORE +!
        NEXT-BONUS @ BONUS !
        NEXT-BONUS OFF 
    ELSE 
        DROP 
    THEN ;

: STRIKE? ( pins -- flag )
    10 = ;

: SPARE? ( pins -- flag )
    LAST-ROLL @ + 10 = 
    OPEN-FRAME @ AND ;

: CALC-BONUS
    DUP STRIKE? IF
        DROP
        1 BONUS +!
        1 NEXT-BONUS !
    ELSE SPARE? IF 
            1 BONUS ! 
        THEN 
    THEN ;

: ADVANCE-FRAME
    OPEN-FRAME @ 0= OPEN-FRAME ! ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    DUP CALC-BONUS
    DUP LAST-ROLL !
    _SCORE +! 
    ADVANCE-FRAME ;
