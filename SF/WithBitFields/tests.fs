PAGE
WARNING OFF

INCLUDE %SwiftForth/unsupported/tests/ttester.fs
INCLUDE Bowling.fs

BITSTRUCT FOO
    4 BITFIELD BAR
    3 BITFIELD QUX
ENDBITSTRUCT
: TESTS
T{ ." A bitfield yields its offset and size" cr
    BAR QUX -> 0 4 4 3
}T

T{ ." mask sets n bits to 1 " cr
    4 MASK 3 MASK -> 15 7
}T 

T{ ." offset moves a bit field value to its position " cr
    3 4 OFFSET -> 48
}T

T{ ." %> extracts a bit field value given offset and size " cr
    0013 1 3 %> -> 6
}T

T{ ." <% save a bit field value given offset and size " cr
    32 6 1 3 <% -> 44
}T
T{ ." a bitfield value can be saved and retrieved " cr
    255 6 QUX <% 
      10 BAR  <%
     DUP QUX %>
    SWAP BAR %> -> 6 10
}T
;


TESTS
BYE

