: start-game ( -- game )
    0 ;

: knock-down ( game,pins -- game )
    + ;

: score ( game -- score ) 
    ;

: last-throw ( game -- pins,flag )
    false ;

