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
cr .fut-tests-result
bye
