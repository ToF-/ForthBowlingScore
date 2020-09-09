VARIABLE SCORE

: START
    0 SCORE ! ;

: ROLL+ ( #pins -- )
    SCORE @ 10 = IF DUP SCORE +! THEN
    SCORE +! ;
