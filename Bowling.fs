: start-game ( -- game )
    0 ;

: knock-down ( game,pins -- game )
    dup 1+ 16 lshift 
    rot or + ;

: score ( game -- score ) 
    511 and ;

: last-throw ( game -- pins,flag )
    16 rshift dup 
    if 1- true then ;

    

