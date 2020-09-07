PAGE

INCLUDE ffl/tst.fs
INCLUDE Bowling.fs

BITSTRUCT FOO
    4 BITFIELD BAR
    3 BITFIELD QUX
ENDBITSTRUCT
: TESTS
T{ ." A bitfield yields its offset and size" cr
    BAR 4 ?s 0 ?s 
    QUX 3 ?s 4 ?s
}T

T{ ." mask sets n bits to 1 " cr
    4 MASK 15 ?s
    3 MASK  7 ?s
}T 

T{ ." offset moves a bit field value to its position " cr
    3 4 OFFSET 48 ?s
}T

T{ ." %> extracts a bit field value given offset and size " cr
    0013 1 3 %> 6 ?s
}T

T{ ." <% save a bit field value given offset and size " cr
    32 6 1 3 <% 44 ?s
}T
T{ ." a bitfield value can be saved and retrieved " cr
    255 6 QUX <%  
      10 BAR  <% 
     DUP QUX %> 6 ?s 
      BAR %> 10 ?S
}T
;


TESTS
BYE

