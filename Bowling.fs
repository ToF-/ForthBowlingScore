variable score
variable bonus
variable last-throw

: start-game
    score off 
    bonus off ;

: add-throw ( pins -- ) 
    bonus @ if dup score +! then
    dup last-throw @ + 10 = if bonus on then
    dup last-throw !
    score +! ;


