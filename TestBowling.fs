require fut/fut.fs
require Bowling.fs

t{ ." start-game sets the score to zero " cr
    start-game 
    score 0 ?s
}t
t{ ." knock-down increases the score by the number of pins knocked down " cr
    start-game 
    4 knock-down
    score 4 ?s
}t
t{ ." last-throw gives a false flag if we are at the beginning of a new frame " cr
    start-game 
    last-throw ?false 
    clearstack
}t
.fut-tests-result cr bye

