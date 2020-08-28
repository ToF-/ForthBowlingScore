variable score
variable bonus
variable next-bonus
variable last-throw
variable half-frame

: start-game
    score off 
    bonus off 
    next-bonus off
    half-frame off ;

: collect-bonus ( pins -- ) 
    bonus @ if 
        dup bonus @ * score +! 
        next-bonus @ bonus !
        next-bonus off
    then ;

: half-frame? ( -- flag )
    half-frame @ ;

: spare? ( pins -- flag )
    last-throw @ + 10 = ;

: strike? ( pins -- flag )
    10 = half-frame? 0= and ;



: calc-next-bonus ( pins -- )
    dup strike? if
        1 bonus +!
        1 next-bonus !
        half-frame on
    else
        half-frame? 
        over spare? and
        if 1 bonus ! then 
    then ;

: advance-frame 
    half-frame? 0= half-frame ! ;

: .game 
    ." score " score @ . 
    ." last-throw " last-throw @ .
    ." bonus " bonus @ .
    ." next-bonus " next-bonus @ .
    ." half-frame " half-frame @ . cr ;

: add-throw ( pins -- ) 
    collect-bonus
    calc-next-bonus
    dup last-throw !
    score +! 
    advance-frame ;


