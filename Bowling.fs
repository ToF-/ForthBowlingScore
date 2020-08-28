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

: calc-next-bonus ( pins -- )
    half-frame @ if
        dup last-throw @ + 10 = if bonus on then 
    then ;


: add-throw ( pins -- ) 
    collect-bonus
    calc-next-bonus
    dup last-throw !
    score +! 
    half-frame @ 0= half-frame ! ;


