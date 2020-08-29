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

t{ ." when two rolls knock down 10 pins, the next roll is added as a bonus" cr
    START-GAME
    4 ADD-ROLL  6 ADD-ROLL
    3 ADD-ROLL  7 ADD-ROLL
    2 ADD-ROLL 
    SCORE 10 3 + 10 2 + 2 + + ?S
}t

t{ ." spare bonus is counted only when spare happens within a frame" cr
    START-GAME
    4 ADD-ROLL  5 ADD-ROLL
    5 ADD-ROLL  2 ADD-ROLL
    SCORE 9 7 + ?S
}t
.fut-tests-result
bye

