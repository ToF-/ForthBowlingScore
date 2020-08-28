require fut/fut.fs
require Bowling.fs

t{ ." start-game sets the score to zero " cr
    start-game 
    score @ 0 ?s
}t
cr .fut-tests-result
bye
