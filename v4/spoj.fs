VARIABLE SCORE
VARIABLE BONUS
VARIABLE NEXT-BONUS
VARIABLE LAST-ROLL
-1 CONSTANT NOTHING

: NEW-FRAME? ( -- flag )
    LAST-ROLL @ NOTHING = ;

VARIABLE FRAME#

: FRAME++
    FRAME# @ 1+ 10 MIN FRAME# ! 
    NOTHING LAST-ROLL ! ;

: START 
    0 SCORE !
    0 FRAME# !
    NOTHING LAST-ROLL !
    0 BONUS !
    0 NEXT-BONUS ! ;

: COLLECT-BONUS ( #pins -- )
    BONUS @ * SCORE +!
    NEXT-BONUS @ BONUS ! 
    0 NEXT-BONUS ! ;

: STRIKE!
    1 BONUS +!  1 NEXT-BONUS ! ;

: SPARE!
    1 BONUS ! 0 NEXT-BONUS ! ;

: NEW-CHECK ( #pins -- ) 
    DUP 10 = IF 
        DROP STRIKE! FRAME++ 
    ELSE
        LAST-ROLL !  
    THEN ;

: OPEN-CHECK ( #pins -- )
    LAST-ROLL @ + 10 = IF SPARE! THEN
    FRAME++ ;

: CHECK-BONUS ( #pins -- )
    NEW-FRAME? IF NEW-CHECK ELSE OPEN-CHECK THEN ;

: BEFORE-10TH-FRAME? 
    FRAME# @ 0 10 WITHIN ;

: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    BEFORE-10TH-FRAME? IF
        DUP CHECK-BONUS
        SCORE +!
    ELSE DROP THEN ;

: .SCORE
    SCORE ? ;

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
