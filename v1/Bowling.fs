variable score
variable bonus
variable next-bonus
variable last-throw
variable half-frame
variable frame

: start-game
    score off 
    bonus off 
    next-bonus off
    half-frame off 
    1 frame ! ;

: collect-bonus ( pins -- ) 
    bonus @ if 
        bonus @ * score +! 
        next-bonus @ bonus !
        next-bonus off
    else
        drop 
    then ;

: half-frame? ( -- flag )
    half-frame @ ;

: spare? ( pins -- flag )
    last-throw @ + 10 = ;

: strike? ( pins -- flag )
    10 = half-frame? 0= and ;

: calc-next-bonus ( pins -- )
    strike? if
        1 bonus +!
        1 next-bonus !
        half-frame on
    else
        half-frame? 
        over spare? and
        if 1 bonus ! then 
    then ;

: advance-frame 
    half-frame? 0= half-frame ! 
    half-frame? 0= if
        1 frame +! 
    then ;

: .game 
    ." score " score @ . 
    ." last-throw " last-throw @ .
    ." bonus " bonus @ .
    ." next-bonus " next-bonus @ .
    ." half-frame " half-frame @ . cr ;

: add-pins ( pins -- )
    dup last-throw !
    score +! ;

: add-throw ( pins -- )
    dup collect-bonus
    frame @ 10 <= if
        dup calc-next-bonus
        add-pins
    else
        drop
    then
    advance-frame ;

