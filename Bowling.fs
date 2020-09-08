HEX
1 CONSTANT SPARE
5 CONSTANT STRIKE

: FRAME# ( frame -- n )
    4 RSHIFT ;

: FRAME++ ( frame -- frame' )
    F0 AND 10 + A0 MIN ;

: OPEN? ( frame -- flag )
    0F AND ;

: ROLL>FRAME ( pins,frame -- frame' )
    F0 AND SWAP 1+ OR ;

: FRAME>ROLL ( frame -- pins )
    0F AND 1- ;

: SPARE? ( pins,frame -- flag )
    FRAME>ROLL + 0A = ;

: STRIKE? ( pins,frame -- flag )
    DROP 0A = ;

: BONUS ( pins,frame -- bonus )
    DUP OPEN? IF
        SPARE? IF SPARE ELSE 0 THEN
    ELSE
        STRIKE? IF STRIKE ELSE 0 THEN
    THEN ;

: FACTOR ( bonus -- factor )
    3 AND ;

: ADVANCE-BONUS ( bonus -- bonus' )
    2 RSHIFT ;

: FRAME>GAME ( frame,game -- game' )
    FFF00 AND OR ;

: GAME>FRAME ( game -- frame )
    000FF AND ;
    
: BONUS>GAME ( bonus,game -- game' )
    FF8FF AND SWAP 8 LSHIFT OR ;

: GAME>BONUS ( game -- bonus )
    00700 AND 8 RSHIFT ;


DECIMAL
