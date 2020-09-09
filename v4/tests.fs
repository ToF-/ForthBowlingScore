INCLUDE ffl/tst.fs
INCLUDE Bowling.fs

T{ ." frame++ increments frame count unless frame count is ten" cr
    0 FRAME# 0 ?S
    0 FRAME++ FRAME# 1 ?S
    0 FRAME++ FRAME++ FRAME++ FRAME++ FRAME++ FRAME++
      FRAME++ FRAME++ FRAME++ FRAME++ FRAME++ FRAME++
    FRAME# 10 ?S
}T

T{ ." a frame is not open initially or if its count has just been incremented " cr
    0 OPEN? ?false
    0 FRAME++ OPEN? ?false
}T

T{ ." a frame is open after saving the previous roll " cr
    7 0 ROLL>FRAME OPEN? ?true
}T

T{ ." if a frame is open it contains the previous roll " cr
    8 0 ROLL>FRAME FRAME>ROLL 8 ?S
}T

T{ ." a normal roll on a new frame gives no bonus " cr
    0 7 BONUS 0 ?s
}T

T{ ." a ten on a new frame gives a strike bonus " cr
    10 0 BONUS STRIKE ?s
}T

T{ ." a complement to ten on an open frame gives a spare bonus " cr
    6 0 ROLL>FRAME
    4 SWAP  BONUS SPARE ?s
}T

T{ ." the bonus factor from a spare is 1 " cr
    SPARE FACTOR 1 ?s
}T

T{ ." the bonus factor from a strike is 1 " cr
    STRIKE FACTOR 1 ?s
}T

T{ ." the bonus factor from a strike on a strike is 2 " cr
    STRIKE 1+ FACTOR 2 ?s
}T

T{ ." advance bonus switch the bonus to next throw " cr
    SPARE  ADVANCE-BONUS 0 ?s
    STRIKE ADVANCE-BONUS 1 ?s
    STRIKE 1+ ADVANCE-BONUS 1 ?s
}T

T{ ." a game includes frame and bonus information " cr
    7 0 ROLL>FRAME 0 FRAME>GAME 
    GAME>FRAME FRAME>ROLL 7 ?s

    STRIKE 0 BONUS>GAME 
    GAME>BONUS STRIKE ?s
}T
    
BYE

