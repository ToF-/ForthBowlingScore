INCLUDE ffl/tst.fs
INCLUDE Bowling.fs

T{ ." After starting a game and adding 3 then 5 then the score is 8" CR
    START 3 ROLL+ 5 ROLL+ SCORE @ 8 ?S
}T

T{ ." After adding 6 then 4 then 2 the score is 14." CR
    START 6 ROLL+ 4 ROLL+ 2 ROLL+ SCORE @ 14 ?S
}T

T{ ." After adding 3 and 3 then 5 and 5 then 2 the score is 20 " CR
    START 3 ROLL+ 3 ROLL+ 5 ROLL+ 5 ROLL+ 2 ROLL+ SCORE @ 20 ?S
}T
BYE


