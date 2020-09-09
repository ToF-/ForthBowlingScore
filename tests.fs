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

T{ ." After adding 4 and 4 and 6 and 2 the score is 16" CR
    START 4 ROLL+ 4 ROLL+ 6 ROLL+ 2 ROLL+ SCORE @ 16 ?S
}T

T{ ." After 10 and 4 and 2 the score is 22" CR
    START 10 ROLL+ 4 ROLL+ 2 ROLL+ SCORE @ 22 ?S
}T

T{ ." After 10 and 10 and 4 and 2 and 1 the score is 47" CR
    START 10 ROLL+ 10 ROLL+ 4 ROLL+ 2 ROLL+ 1 ROLL+ SCORE @ 47 ?S
}T

T{ ." After 10 frames and no bonus, adding throws does not increase score" CR
    START 
    8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 
    8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 
    8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 
    8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 8 ROLL+ 1 ROLL+ 
    SCORE @ 90 ?S
}T
BYE



