1 CONSTANT SPARE
5 CONSTANT STRIKE

: FRAME# ( frame -- n )
    4 RSHIFT ;

: NEXT-FRAME ( frame -- frame' )
    240 AND 16 + 160 MIN ;

: OPEN-FRAME? ( frame -- flag )
    15 AND ;

: ROLL>FRAME ( pins,frame -- frame' )
    240 AND SWAP 1+ OR ;

: FRAME>ROLL ( frame -- pins )
    15 AND 1- ;

: BONUS ( pins,frame -- bonus )
    DUP OPEN-FRAME? IF
        FRAME>ROLL + 10 = IF
            SPARE
        ELSE 
            0
        THEN
    ELSE
       DROP 10 = IF 
           STRIKE 
       ELSE 
          0 
       THEN
    THEN ;
