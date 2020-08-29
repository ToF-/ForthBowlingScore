require fut/fut.fs
require Bowling.fs

t{ ." at the start of a game, score is zero" cr
    START-GAME  SCORE 0 ?S
}t

t{ ." after adding a roll, score increases by the number of pins knocked down" cr
    START-GAME
    4 ADD-ROLL
    SCORE 4 ?S
}t

.fut-tests-result
bye

