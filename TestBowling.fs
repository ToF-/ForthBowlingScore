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
}t
t{ ." last-throw gives the number of pins knocked down on last throw and a true flag if we are in the second half of a frame " cr
    start-game
    7 knock-down
    last-throw ?true 7 ?s
}t
t{ ." after the second throw, last-throw gives a false flag " cr
    start-game
    3 knock-down
    2 knock-down
    last-throw ?false
}t
.fut-tests-result cr bye

