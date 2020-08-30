VARIABLE _SCORE
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE LAST-ROLL
VARIABLE FRAME-COUNT
VARIABLE FRAME#

: START-GAME 
    _SCORE     OFF 
    BONUS      OFF 
    NEXT-BONUS OFF
    LAST-ROLL  OFF 
    FRAME#     OFF ;

: SCORE 
    _SCORE @ ;

: ADVANCE-BONUS 
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: COLLECT-BONUS ( pins -- )
    BONUS @ ?DUP IF
        * _SCORE +!
        ADVANCE-BONUS
    ELSE 
        DROP 
    THEN ;

: OPEN-FRAME? ( -- flag )
    FRAME# @ 10 MOD 5 = ;

: STRIKE? ( pins -- flag )
    10 = OPEN-FRAME? 0= AND ;

: SPARE? ( pins -- flag )
    LAST-ROLL @ + 10 = 
    OPEN-FRAME? AND ;

: PAST-10-FRAMES? ( -- flag )
    FRAME# @ 100 >= ;

: STRIKE-BONUS 
    1 BONUS +!  1 NEXT-BONUS ! ;

: SPARE-BONUS
    1 BONUS ! ;

: CALC-BONUS
    PAST-10-FRAMES? IF DROP ELSE
        DUP STRIKE? IF 
            DROP
            STRIKE-BONUS 
            5 FRAME# +!
        ELSE 
            SPARE? IF SPARE-BONUS THEN
        THEN
    THEN ;

: ADVANCE-FRAME
    5 FRAME# +! ;

: ADD-PINS ( pins -- )
    PAST-10-FRAMES? IF DROP ELSE _SCORE +!  THEN ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    DUP CALC-BONUS
    DUP LAST-ROLL !
    ADD-PINS
    ADVANCE-FRAME ;
