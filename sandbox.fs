
\ multiply by bonus, then make bonus advance 

variable score
: collect ( bonus,pins -- bonus',points )
    over 3 and *
    swap 2 rshift swap ;

: open-frame? ( frame -- flag )
    256 and ;

: frame# ( frame -- frame# )
    15 and ;

: prev-roll ( frame -- pins )
    4 rshift 15 and ;

: frame#++ ( frame -- frame )
    15 and 1+ 10 min ;

: save-roll ( frame,pins -- frame )
    swap 15 and 256 or
    swap 4 lshift or ;

: strike? ( frame,pins -- flag )
    10 = swap open-frame? 0= and ;

: spare? ( frame,pins -- flag ) 
    swap dup open-frame? if
        prev-roll + 10 = 
    else 
        drop drop false 
    then ;

: claim ( bonus,frame,pins -- bonus )
    over frame# 10 < if
        2dup strike? -rot spare?
        if drop drop 1 else if 1 + 4 or then then
    else
        drop drop
    then ;

: advance ( frame,pins,bonus-- frame )
    swap -rot                     \ pins,frame,bonus
    4 and over open-frame? or if
        frame#++ swap drop 
    else
        swap save-roll 
    then ;

: add-pins ( frame,pins -- )
    swap frame# 10 < if score +! else drop then ;

: .bonus ( bonus -- )
    ." bonus: " dup 3 and . ." next:" 4 and 2 rshift . ;

: .frame# ( frame -- )
    ." frame# " 15 and . ;

: .open-frame ( frame -- )
    ." open:" 256 and if ." yes " else ." no " then ;

: .game ( frame,bonus -- )
    .bonus dup .frame# .open-frame ;

: .score ( -- )
    ." score:" score ? ;

: add-roll ( frame,bonus,pins -- frame,bonus )
    dup -rot collect score +!    \ frame,pins,bonus
    >r 2dup r> -rot claim        \ frame,pins,bonus
    >r 2dup r> -rot add-pins     \ frame,pins,bonus
    dup >r advance r> 
    2dup .game .score cr .s cr ;          \ frame,bonus

: x ( frame,bonus -- frame,bonus )
    10 add-roll ;

: / ( frame,bonus -- frame,bonus )
    over open-frame? if
        over prev-roll 10 swap - add-roll
    else ." not in open frame" then ;

: o ( frame,bonus,pins -- frame,bonus )
    add-roll ;

: start-game
    score off 
    0 0 ;

: end-game
    drop drop ;

: game
    start-game
    begin
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



