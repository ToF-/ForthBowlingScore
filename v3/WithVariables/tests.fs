PAGE

INCLUDE ffl/tst.fs
INCLUDE Bowling.fs

: TESTS
T{
   ." when starting a game, the score is zero " cr
    START-GAME SCORE @ 0 ?S 
}T

T{ ." after adding a roll, the score increases " cr
    START-GAME 7 ADD-ROLL SCORE @ 7 ?S
}T

T{ ." after a spare the next roll is counted as a bonus " cr
    START-GAME 6 ADD-ROLL 4 ADD-ROLL 2 ADD-ROLL SCORE @ 14 ?S
}T

T{ ." two rolls in different frames cannot make spare " cr
    START-GAME 2 ADD-ROLL 4 ADD-ROLL 6 ADD-ROLL 3 ADD-ROLL SCORE @ 15 ?S
}T

T{ ." a strike at first rolls creates bonus for the next two rolls " cr
    START-GAME 10 ADD-ROLL 4 ADD-ROLL 6 ADD-ROLL SCORE @ 30 ?S 
}T

T{ ." after ten frames, throws are not counted anymore " cr
    START-GAME 
    3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL
    3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL
    3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL
    3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL 3 ADD-ROLL 5 ADD-ROLL
    SCORE @ 80 ?S
}T

T{ ." after ten frames, throws don't generate bonus anymore "
    START-GAME
    10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 
    10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 
    10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 10 ADD-ROLL 
    SCORE @ 300 ?S
}T
;

TESTS
BYE
