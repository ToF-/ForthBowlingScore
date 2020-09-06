\ Bowling.fs

VARIABLE SCORE
VARIABLE PREV-ROLL
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE OPEN-FRAME
VARIABLE FRAME#

: START-GAME 
    0 SCORE ! 0 PREV-ROLL ! 0 BONUS ! 
    0 NEXT-BONUS ! FALSE OPEN-FRAME ! 
    0 FRAME# ! ;

: COLLECT-BONUS ( pins -- )
    BONUS @ ?DUP IF * SCORE +! ELSE DROP THEN 
    NEXT-BONUS @ BONUS !
    NEXT-BONUS OFF ;

: STRIKE? ( pins -- flag )
    10 = OPEN-FRAME @ 0= AND ;

: SPARE? ( pins -- flag )
    PREV-ROLL @ + 10 =
    OPEN-FRAME @ AND ;

: MARK-STRIKE 
    1 BONUS +!
    1 NEXT-BONUS !
    TRUE OPEN-FRAME ! ;

: MARK-SPARE 
    1 BONUS ! ;

: CALC-BONUS ( pins -- )
    DUP STRIKE? IF
        DROP MARK-STRIKE
    ELSE SPARE? IF 
        MARK-SPARE THEN 
    THEN ;

: ADD-PINS
    SCORE +! ;

: NEXT-FRAME 
    FRAME# @ 1+ 
    10 MIN FRAME# ! ;

: ADVANCE-FRAME ( pins -- )
    PREV-ROLL !
    OPEN-FRAME @ 0= OPEN-FRAME ! 
    OPEN-FRAME @ 0= IF 
        FRAME# @ 1+ 10 MIN 
        FRAME# !
    THEN ;

: ADD-ROLL ( pins -- )
    DUP COLLECT-BONUS
    FRAME# @ 10 < IF 
        DUP CALC-BONUS 
        DUP ADD-PINS 
    THEN
    ADVANCE-FRAME ;

: TO-DIGIT ( char -- n )
    [CHAR] 0 - ;

: IS-DIGIT? ( char -- flag )
    TO-DIGIT DUP 0 >= SWAP 9 <= AND ;     

: SKIP-NON-DIGIT ( -- char )
    BEGIN KEY DUP IS-DIGIT? 0= WHILE DROP REPEAT ;

: GET-NUMBER ( -- n )
    SKIP-NON-DIGIT  
    0 SWAP          \ accumulator
    BEGIN
        TO-DIGIT SWAP 10 * + 
        KEY DUP IS-DIGIT? 
    0= UNTIL DROP ;

: MAIN
    GET-NUMBER 0 DO
        GET-NUMBER 
        START-GAME
        0 DO GET-NUMBER ADD-ROLL LOOP
        SCORE ? CR
    LOOP ;
