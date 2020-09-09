VARIABLE SCORE
VARIABLE BONUS
VARIABLE FRAME-STATE
VARIABLE FRAME#

: START
    0 SCORE !
    0 BONUS !
    0 FRAME-STATE !
    0 FRAME# ! ;

: SPARE! 1 BONUS ! ;
: STRIKE! BONUS @ 1+ 4 OR BONUS ! ;

: BONUS> ( -- factor )
    BONUS @ DUP 3 AND 
    SWAP 2 RSHIFT BONUS ! ;

: COLLECT-BONUS ( #pins -- )
    BONUS> * SCORE +! ;

: CLOSE-FRAME!
    0 FRAME-STATE ! ;

: OPEN-FRAME! ( #pins -- )
    1+ FRAME-STATE ! ;

: OPEN-FRAME? ( -- flag )
    FRAME-STATE @ ;

: NEW-FRAME? ( -- flag )
    OPEN-FRAME? 0= ;

: LAST-ROLL ( -- #pins )
    FRAME-STATE @ 1 - ;

: FRAME>
    NEW-FRAME? IF FRAME# @ 1+ 10 MIN FRAME# ! THEN ;

: CHECK-STRIKE ( #pins -- )
    DUP 10 = IF STRIKE! CLOSE-FRAME! ELSE OPEN-FRAME! THEN ;

: CHECK-SPARE ( #pins -- )
    LAST-ROLL + 10 = IF SPARE! THEN CLOSE-FRAME! ;

: CHECK-BONUS ( #pins -- )
    NEW-FRAME? IF CHECK-STRIKE ELSE CHECK-SPARE THEN ;

: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    FRAME# @ 0 10 WITHIN IF
        DUP CHECK-BONUS
        SCORE +!
        FRAME>
    THEN ;

: .SCORE SCORE ? ;

: TO-DIGIT ( char -- n )
    [CHAR] 0 - ;

: IS-DIGIT? ( char -- flag )
    TO-DIGIT 0 10 WITHIN ;     

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
    GET-NUMBER ?DUP 0 DO
        START
        GET-NUMBER ?DUP 0 DO 
            GET-NUMBER ROLL+ 
        LOOP
        .SCORE CR
    LOOP ;


MAIN BYE
