INCLUDE ffl/tst.fs
INCLUDE Bowling.fs

T{ ."  after starting a game and adding 3 then 5 then the score is 8" CR
    START 3 ROLL+ 5 ROLL+ SCORE @ 8 ?S
}T

BYE


