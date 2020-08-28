variable score
variable bonus
variable last-throw
variable half-frame

: start-game
    score off 
    bonus off 
    half-frame off ;

: collect-bonus ( pins -- ) 
    bonus @ if dup score +! then ;

: spare? ( pins -- flag )
    last-throw @ + 10 = ;

: half-frame? ( -- flag )
    half-frame @ ;

: calc-next-bonus ( pins -- )
    half-frame? 
    over spare? and
    if bonus on then ;

: advance-frame 
    half-frame? 0= half-frame ! ;

: add-throw ( pins -- ) 
    collect-bonus
    calc-next-bonus
    dup last-throw !
    score +! 
    advance-frame ;


