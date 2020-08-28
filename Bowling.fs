hex
F000 constant last-throw-mask
0FFF constant score-mask
decimal
12   constant last-throw-offset
: start-game ( -- game )
    0 ;

: reset ( value -- value )
    -1 xor and ;

: last-throw ( game -- pins,flag )
    last-throw-offset rshift dup 
    if 1- true then ;

: set-last-throw ( game,pins -- game )
    1+ last-throw-offset lshift or ;

: reset-last-throw ( game -- game )
    last-throw-mask reset ;

: last-throw! ( game,pins -- game )
    over last-throw 0= if 
        set-last-throw 
    else 
        2drop
        reset-last-throw
    then ;

: add-to-score! ( game,pins -- game )
    + ;

: knock-down ( game,pins -- game )
    tuck
    last-throw!
    swap
    add-to-score! ;

: score ( game -- score ) 
    score-mask and ;



