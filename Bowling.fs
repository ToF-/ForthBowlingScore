VARIABLE _SCORE

: START-GAME 
    _SCORE OFF ;

: SCORE 
    _SCORE @ ;

: ADD-ROLL ( pins -- )
    _SCORE @ 10 = IF
        DUP _SCORE +!
    THEN
    _SCORE +! ;
