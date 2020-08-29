require fut/fut.fs
require Bowling.fs

t{ ." start-game sets the score to zero " cr
    start-game 
    score @ 0 ?s
}t

t{ ." add-throw increases the score " cr
    start-game
    4 add-throw 
    score @ 4 ?s
}t

t{ ." after a spare, the next throw counts as bonus " cr
    start-game
    4 add-throw
    6 add-throw
    3 add-throw
    score @ 16 ?s 
}t
t{ ." the second throw of a frame does not create a spare " cr
    start-game
    2 add-throw
    7 add-throw
    3 add-throw
    4 add-throw
    score @ 2 7 + 3 4 + + ?s 
}t

t{ ." after a strike, the next two throws count as bonus " cr
    start-game
    10 add-throw
    2  add-throw
    3  add-throw 
    score @ 10 2 2 + + 3 3 + + ?s
}t
t{ ." after two strikes, the next two throws count as bonus " cr
    start-game
    10 add-throw      \ 10
    10 add-throw      \ 10 + 10
    2  add-throw      \ 2  + 2  + 2
    3  add-throw      \ 3  + 3
    score @ 42 ?s
}t
t{ ." after ten frames, following throws count only as bonus " cr
    start-game
    10 add-throw 10 add-throw 10 add-throw
    10 add-throw 10 add-throw 10 add-throw
    10 add-throw 10 add-throw 10 add-throw
    5 add-throw 5 add-throw
    3 add-throw   
    score @ 268 ?s
}t
t{ ." after ten frames, strikes and spares don't create bonus " cr
    start-game
    10 add-throw 10 add-throw 10 add-throw
    10 add-throw 10 add-throw 10 add-throw
    10 add-throw 10 add-throw 10 add-throw
    10 add-throw 10 add-throw 10 add-throw
    score @ 300 ?s
    10 add-throw 10 add-throw 10 add-throw
    score @ 300 ?s
}t

cr .fut-tests-result
bye
