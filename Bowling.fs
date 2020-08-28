variable score
variable bonus
variable last-throw

: start-game
    score off 
    bonus off ;

: collect-bonus ( pins -- ) 
    bonus @ if dup score +! then ;

: calc-next-bonus ( pins -- )
    dup last-throw @ + 10 = if bonus on then ;
: add-throw ( pins -- ) 
    collect-bonus
    calc-next-bonus
    dup last-throw !
    score +! ;


