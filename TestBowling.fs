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

t{ ." when 10 pins are knocked down at first roll, the next two rolls are added as a bonus" cr
    START-GAME
    10 ADD-ROLL
    3  ADD-ROLL
    4  ADD-ROLL
    SCORE 10 3 + 4 + 3 + 4 + ?S
}t

t{ ." when several strikes are rolled, the bonus for the next roll accumulates" cr
    START-GAME
    10 ADD-ROLL
    10 ADD-ROLL
    8  ADD-ROLL
    1  ADD-ROLL
    SCORE 10 10 + 8 + 10 + 8 + 1 + 8 + 1 + ?S
}t

t{ ." knocking down 10 pins on second roll does not make a strike " cr
    START-GAME 
    0  ADD-ROLL
    10 ADD-ROLL
    3  ADD-ROLL
    6  ADD-ROLL 
    SCORE 10 3 + 3 + 6 + ?S
}t

: NINE-ZERO-FRAMES
     18 0 DO 0 ADD-ROLL LOOP ;
 
 t{ ." after 10 frames are played, rolls are added to score only as bonus" cr
     START-GAME 
     NINE-ZERO-FRAMES
     4 ADD-ROLL 6 ADD-ROLL
     2 ADD-ROLL 
     SCORE 10 2 + ?S
 }t

: TWELVE-STRIKES
    12 0 DO 10 ADD-ROLL LOOP ;

 t{ ." after 10 frames are played, a strike does not create a bonus" cr
     START-GAME 
     TWELVE-STRIKES
     SCORE 300 ?S
 }t
    
.fut-tests-result
bye

