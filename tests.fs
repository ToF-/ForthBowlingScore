INCLUDE ffl/tst.fs
INCLUDE Bowling.fs

T{ ." next-frame increments frame count unless frame count is ten" cr
    0 FRAME# 0 ?S
    0 NEXT-FRAME FRAME# 1 ?S
    0 NEXT-FRAME NEXT-FRAME NEXT-FRAME NEXT-FRAME NEXT-FRAME NEXT-FRAME
      NEXT-FRAME NEXT-FRAME NEXT-FRAME NEXT-FRAME NEXT-FRAME NEXT-FRAME
    FRAME# 10 ?S
}T

T{ ." a frame is not open initially or if its count has just been incremented " cr
    0 OPEN-FRAME? ?false
    0 NEXT-FRAME OPEN-FRAME? ?false
}T

T{ ." a frame is open after saving the previous roll " cr
    7 0 ROLL>FRAME OPEN-FRAME? ?true
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
BYE

