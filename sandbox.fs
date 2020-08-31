
\ multiply by bonus, then make bonus advance 

variable score

2 base !
000000000000000001111 constant frame%
000000000000000110000 constant bonus%
000000000000001000000 constant nextb%
000000000000010000000 constant open?%
000000000111100000000 constant proll%
111111111000000000000 constant score%
decimal
 0 constant [frame]
 4 constant [bonus]
 6 constant [nextb]
 7 constant [open?]
 8 constant [proll]
12 constant [score]

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

: nextb@ ( game -- nextb )
    [nextb] nextb% bits@ ;

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

: nextb! ( game,nextb -- game )
    [nextb] nextb% bits! ;

: open?! ( game,open? -- game )
    [open?] open?% bits! ;

: proll! ( game,proll -- game )
    [proll] proll% bits! ;

: score! ( game,score -- game )
    [score] score% bits! ;

: add-to-score ( game,pins -- game )
    over score@ + score! ;

: collect-bonus ( game,pins -- game )
    over bonus@ * 
    add-to-score
    dup nextb@ bonus! 
    0 nextb! ;

: frame++ ( game -- game )
    dup frame@ 1+ 10 min frame! ;

: claim-strike ( game -- game )
    dup bonus@ 1+ bonus!
    1 nextb! 
    1 open?! ;

: claim-spare ( game -- game )
    1 bonus!
    0 nextb! ;

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

: add-roll ( game,pins -- game )
    tuck collect-bonus

    over claim-bonus
    over add-pins-to-score
    swap advance-frame dup frame@ . dup score@ . cr ;

: x ( game -- game )
    10 add-roll ;

: / ( game -- game )
    dup open?@ if
        dup proll@ 10 swap - add-roll
    else ." not in open frame" then ;

: o ( frame,bonus,pins -- frame,bonus )
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

start-game
cr 
1 add-roll 4 add-roll 4 add-roll 5 add-roll 6 add-roll 4 add-roll 5 add-roll 5 add-roll 10 add-roll 0 add-roll 1 add-roll 7 add-roll 3 add-roll 6 add-roll 4 add-roll 10 add-roll 2 add-roll 8 add-roll 6 add-roll score@ . cr
