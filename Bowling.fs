: start-game ( -- bonus,last,score )
    0 -1 0 ;

: spare? ( last,pins -- flag )
    over 0 >= 
    + 10 = and ;

: add-bonus ( bonus,score,pins, -- bonus,score,pins )
    rot if 
        dup rot + swap
    then
    0 -rot ;

: next-bonus ( last,bonus,score,pins -- score,pins,bonus )
    2swap drop        \ score,pins,last
    over spare? ;     \ score,pins,bonus

: keep-last ( score,pins,bonus -- last,bonus,score,pins )
    over swap 2swap ;
    
: add-throw ( score,pins -- score )
    + ;

: knock-down ( last,bonus,score,pins -- last,bonus,score ) 
    add-bonus         \ last,bonus,score,pins
    next-bonus        \ score,pins,bonus
    keep-last         \ last,bonus,score,pins
    add-throw ;       \ last,bonus,score

: reset-score ( bonus,last,score -- bonus,last,score )
    drop drop drop 
    start-game ;

