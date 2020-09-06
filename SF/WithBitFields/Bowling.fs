\ Bowling.fs

: BITSTRUCT  ( <name> -- offset ) 
    CREATE 0 ;
    
: BITFIELD ( <name> offset size -- offset )
    CREATE 2DUP , , + 
    DOES> ( --  ) 2@  ;

: ENDBITSTRUCT ( offset -- )
    DROP ;

: MASK ( size -- mask )
    1 SWAP LSHIFT 1- ;

: OFFSET ( value,offset -- value )
    LSHIFT ;

: %> ( value,offset,size -- bitfield )
    MASK -ROT RSHIFT AND ;

: <% ( value,bitfield,offset,size -- value )
    MASK OVER OFFSET INVERT 
    >R ROT R>  AND  -ROT OFFSET OR ;
    

