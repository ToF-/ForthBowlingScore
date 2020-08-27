: start-game ( -- game )
    0 ;

: last-throw ( game -- pins,flag )
    16 rshift dup 
    if 1- true then ;

: set-last-throw ( pins -- value )
    1+ 16 lshift ;

: reset-last-throw ( -- value )
    65535 and ;

: last-throw! ( game,pins -- game )
    over last-throw 0= if 
        set-last-throw 
        rot or 
    else 
        drop drop 
        reset-last-throw
    then ;

: knock-down ( game,pins -- game )
    dup -rot
    last-throw!
    + ;

: score ( game -- score ) 
    511 and ;



