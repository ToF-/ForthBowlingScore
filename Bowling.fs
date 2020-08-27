: start-game ( -- game )
    0 ;

: last-throw! ( game,pins -- game )
    1+ 16 lshift rot or ;

: knock-down ( game,pins -- game )
    dup -rot
    last-throw!
    + ;

: score ( game -- score ) 
    511 and ;

: last-throw ( game -- pins,flag )
    16 rshift dup 
    if 1- true then ;

    

