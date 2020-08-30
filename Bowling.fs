\ Bowling.fs

\ The word `START-GAME` will reinitialize the bowling game, setting the score to 0.
\ The word `SCORE ( -- score ) ` will return the current score.
\ The word `ADD-ROLL ( pins -- ) ` will register knocked down pins and calculate the score.

VARIABLE _SCORE
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE LAST-ROLL
VARIABLE FRAME#
CREATE SHEET 80 ALLOT
VARIABLE SHEET-SIZE

: START-GAME 
    _SCORE     OFF 
    BONUS      OFF 
    NEXT-BONUS OFF
    LAST-ROLL  OFF 
    FRAME#     OFF
    SHEET-SIZE OFF ;

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

: SHEET! ( char -- )
    SHEET SHEET-SIZE @ + C!
    1 SHEET-SIZE +! ;

: MARK-STRIKE
    [CHAR] X SHEET!
    BL       SHEET! ;

: MARK-SPARE 
    -1 SHEET-SIZE +!
    [CHAR] / SHEET! ;

: MARK-ROLL ( pins -- )
    DUP 10 = IF DROP [CHAR] X ELSE
    [CHAR] 0 + THEN SHEET! ;

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
    PAST-10-FRAMES? IF MARK-ROLL ELSE
        DUP STRIKE? IF 
            DROP
            STRIKE-BONUS 
            MARK-STRIKE
            5 FRAME# +!
            
        ELSE 
            DUP SPARE? IF SPARE-BONUS MARK-SPARE DROP
            ELSE MARK-ROLL THEN
        THEN
    THEN ;

: ADVANCE-FRAME
    5 FRAME# +! ;

: ADD-PINS ( pins -- )
    PAST-10-FRAMES? IF DROP ELSE _SCORE +!  THEN ;

: .SHEET 
    SHEET SHEET-SIZE @ TYPE ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    DUP CALC-BONUS
    DUP LAST-ROLL !
    ADD-PINS
    ADVANCE-FRAME 
    BL SHEET! ;
