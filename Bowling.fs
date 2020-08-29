VARIABLE _SCORE
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE LAST-ROLL
VARIABLE OPEN-FRAME
VARIABLE FRAME-COUNT

: START-GAME 
    _SCORE     OFF 
    BONUS      OFF 
    NEXT-BONUS OFF
    LAST-ROLL  OFF 
    OPEN-FRAME OFF 
    1 FRAME-COUNT ! ;

: SCORE 
    _SCORE @ ;

: ADVANCE-BONUS 
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: COLLECT-BONUS ( pins -- )
    BONUS @ IF
        BONUS @ * _SCORE +!
        ADVANCE-BONUS
    ELSE 
        DROP 
    THEN ;

: STRIKE? ( pins -- flag )
    10 = OPEN-FRAME @ 0= AND ;

: SPARE? ( pins -- flag )
    LAST-ROLL @ + 10 = 
    OPEN-FRAME @ AND ;

: CALC-BONUS
    DUP STRIKE? IF DROP
        1 BONUS +!  1 NEXT-BONUS !
        OPEN-FRAME ON
    ELSE SPARE? IF 
            1 BONUS ! 
        THEN 
    THEN ;

: ADVANCE-FRAME
    OPEN-FRAME @ IF 1 FRAME-COUNT +! THEN
    OPEN-FRAME @ 0= OPEN-FRAME ! ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    DUP CALC-BONUS
    DUP LAST-ROLL !
    FRAME-COUNT @ 10 <= IF
        _SCORE +! 
    ELSE
        DROP
    THEN
    ADVANCE-FRAME ;
