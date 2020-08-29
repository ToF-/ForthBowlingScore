VARIABLE _SCORE
VARIABLE BONUS
VARIABLE LAST-ROLL

: START-GAME 
    _SCORE OFF 
    BONUS OFF 
    LAST-ROLL OFF ;

: SCORE 
    _SCORE @ ;

: COLLECT-BONUS ( pins -- )
    BONUS @ IF
        _SCORE +!
        BONUS OFF
    ELSE 
        DROP 
    THEN ;

: CALC-BONUS
    LAST-ROLL @ + 10 = IF
        BONUS ON
    THEN ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    DUP CALC-BONUS
    DUP LAST-ROLL !
    _SCORE +! ;
