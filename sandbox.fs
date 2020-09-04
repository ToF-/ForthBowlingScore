
\ multiply by bonus, then make bonus advance 

variable score

2 base !
00000000000000001111 constant frame%
00000000000000110000 constant bonus%
00000000000001000000 constant open?%
00000000011110000000 constant proll%
11111111100000000000 constant score%
decimal
 0 constant [frame]
 4 constant [bonus]
 6 constant [open?]
 7 constant [proll]
11 constant [score]

: reset ( mask -- value )
    -1 xor and ;

: bits@ ( struct,offset,mask -- value )
    rot swap and swap rshift ;

: bits! ( struct,value,offset,mask -- struct )
    -rot                      \ struct,mask,value,offset
    lshift -rot               \ value',struct,mask
    reset or ;

: frame@ ( game -- frame )
    [frame] frame% bits@ ;

: bonus@ ( game -- bonus )
    [bonus] bonus% bits@ ;

: open?@ ( game -- open? )
    [open?] open?% bits@ ;

: proll@ ( game -- proll )
    [proll] proll% bits@ ;

: score@ ( game -- score )
    [score] score% bits@ ;

: frame! ( game,frame -- frame )
    [frame] frame% bits! ;

: bonus! ( game,bonus -- game )
    [bonus] bonus% bits! ;

: open?! ( game,open? -- game )
    [open?] open?% bits! ;

: proll! ( game,proll -- game )
    [proll] proll% bits! ;

: score! ( game,score -- game )
    [score] score% bits! ;

: add-to-score ( game,pins -- game )
    over score@ + score! ;

: points ( pin,bonus -- points )
    dup 1 and swap 2/ + * ;

: collect-bonus ( game,pins -- game )
    over bonus@ points
    add-to-score
    dup bonus@ 2/ bonus! ;

: frame++ ( game -- game )
    dup frame@ 1+ 10 min frame! ;

: claim-strike ( game -- game )
    dup bonus@ 2 + bonus! 
    1 open?! ;

: claim-spare ( game -- game )
    1 bonus! ;

: claim-open ( game,pins -- game )
    over proll@ + 10 = if claim-spare then ;

: claim-closed ( game,pins -- game )
    10 = if claim-strike then ;

: claim-bonus ( game,pins -- game )
    over frame@ 10 < if
        over open?@ if 
            claim-open 
        else 
            claim-closed 
        then
    else
        drop
    then ;

: next-frame ( game -- game )
    0 proll! 
    0 open?!
    frame++ ;

: open-frame ( game,pins -- game )
    proll! 1 open?! ;

: advance-frame ( game,pins -- game )
    over open?@ if 
        drop next-frame 
    else 
        open-frame 
    then ;

: add-pins-to-score ( game,pins -- game )
    over frame@ 10 < if
        add-to-score 
    else
        drop
    then ;

: .game ( game -- )
    ." frame :" dup frame@ .
    ." open  :" dup open?@ if ." yes " else ." no " then
    ." bonus :" dup bonus@ .
    ." prev  :" dup proll@ .
    ." score :" score@ . ;


: add-roll ( game,pins -- game )
    tuck collect-bonus
    over claim-bonus
    over add-pins-to-score
    swap advance-frame 
    ;

: x ( game -- game )
    10 add-roll ;

: / ( game -- game )
    dup open?@ if
        dup proll@ 10 swap - add-roll
    else ." not in open frame" then ;

: o ( game,pins -- game )
    add-roll ;

: start-game
    0 ;

: end-game
    drop ;

: game
    start-game
    begin
        dup score@ . cr
        key >r 
        i [char] x = if 
                ." strike! " cr x
        else i [char] / = if 
            ." spare!" cr /
            else 
                i dup [char] 0 >= swap [char] 9 <= and if 
                    i dup emit cr
                    [char] 0 - o 
                then
            then
        then
        r> [char] q = 
    until 
    end-game ;
