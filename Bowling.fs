VARIABLE _SCORE

: START-GAME 
    _SCORE OFF ;

: SCORE 
    _SCORE @ ;

: ADD-ROLL ( pins -- )
    _SCORE +! ;
