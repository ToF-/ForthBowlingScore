: start-game ( -- game )
    0 ;

: last-throw ( game -- pins,flag )
    16 rshift dup 
    if 1- true then ;

: last-throw! ( game,pins -- game )
    over last-throw 0= if 
        1+ 16 lshift rot or 
    else drop drop 65635 and then ;

: knock-down ( game,pins -- game )
    dup -rot
    last-throw!
    + ;

: score ( game -- score ) 
    511 and ;



