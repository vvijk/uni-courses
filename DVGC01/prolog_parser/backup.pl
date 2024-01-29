/******************************************************************************/
/* Prolog Lab 2 example - Grammar test bed                                    */
/******************************************************************************/

/******************************************************************************/
/* Grammar Rules in Definite Clause Grammar form                              */
/* This the set of productions, P, for this grammar                           */
/* This is a slightly modified from of the Pascal Grammar for Lab 2 Prolog    */
/******************************************************************************/

program       --> prog_head, var_part, stat_part.

/******************************************************************************/
/* Program Header                                                             */
/******************************************************************************/
prog_head     --> [program], id, ['('], [input], [','], [output], [')'], [';'].
id            --> [a]|[b]|[c]


/******************************************************************************/
/* Var_part                                                                   */
/******************************************************************************/
var_part            -->     var, var_dec_list.
var_dec_list        -->     var_dec | var_dec, var_dec_list.
var_dec             -->     id_list, [':'], type, [';'].
id_list             -->     id | id, [','], id_list.
/* type               -->    .....*/

/******************************************************************************/
/* Lexer                                                                   */
/******************************************************************************/
lexer([ ], [ ]).
lexer([H|T], [F|S]) :- match(H, F), lexer(T,S). 


lab2(File, Result) :- read_in(File, L), lexer(L, Tokens), write(Tokens).
test_lexer(File, X) :- read_in(File, L), lexer(L,X), write(X).
%######################################
match(L,T) :- L = 'program', T is 256.
match(L,T) :- L = 'input'  , T is 257.
match(L,T) :- L = 'output' , T is 258.
match(L,T) :- L = 'var'    , T is 259.
match(L,T) :- L = 'integer', T is 260.
match(L,T) :- L = 'begin'  , T is 261.
match(L,T) :- L = 'end'    , T is 262.
match(L,T) :- L = 'boolean', T is 263.
match(L,T) :- L = 'real'   , T is 264.
match(L,T) :- L = ':='     , T is 271.
match(L,T) :- L = '('      , T is 40.
match(L,T) :- L = ')'      , T is 41.
match(L,T) :- L = '*'      , T is 42.
match(L,T) :- L = '+'      , T is 43.
match(L,T) :- L = ','      , T is 44.
match(L,T) :- L = '.'      , T is 46.
match(L,T) :- L = ':'      , T is 58.
match(L,T) :- L = ';'      , T is 59.

match(L,T) :- name(L, [H|Tail]), char_type(H, digit), match_id(Tail), T is 270.

match(L,T) :- name(L, [H|Tail]), char_type(H,digit), match_num(Tail), T is 272.

match(L,T) :-  char_type(L, end_of_file), T is 275.
match(L,T) :- char_type(L, ascii)  , T is 273.

match_id([ ]).
match_id([H|T]) :- char_type(H, alnum), match_id(T).
match_num([]).
match_num([H|T]) :- char_type(H,digit), match_num(T).


/******************************************************************************/
/* Terminals = 'Facts'                                                        */
/******************************************************************************/
program --> [256].
input   --> [257].
output  --> [258].
var     --> [259].
integer --> [260].
begin   --> [261].
end     --> [262].
boolean --> [263].
real    --> [264].
id      --> [270].

lpar    -->  [40].  % ( 
rpar    -->  [41].  % )
mult    -->  [42].  % *
add     -->  [43].  % +
comma   -->  [44].
dot     -->  [46].
colon   -->  [58].
scolon  -->  [59].

assign  --> [271].
number  --> [272].

/******************************************************************************/
/* Stat part                                                                  */
/******************************************************************************/
stat_part            -->  stat_part_todo.
stat_part_todo(_,_)  :-   write('stat_part: To Be Done'), nl.

/******************************************************************************/
/* Testing the system: this may be done stepwise in Prolog                    */
/* below are some examples of a "bottom-up" approach - start with simple      */
/* tests and buid up until a whole program can be tested                      */
/******************************************************************************/
/* Stat part                                                                  */
/******************************************************************************/
/*  op(['+'], []).                                                            */
/*  op(['-'], []).                                                            */
/*  op(['*'], []).                                                            */
/*  op(['/'], []).                                                            */
/*  addop(['+'], []).                                                         */
/*  addop(['-'], []).                                                         */
/*  mulop(['*'], []).                                                         */
/*  mulop(['/'], []).                                                         */
/*  factor([a], []).                                                          */
/*  factor(['(', a, ')'], []).                                                */
/*  term([a], []).                                                            */
/*  term([a, '*', a], []).                                                    */
/*  expr([a], []).                                                            */
/*  expr([a, '*', a], []).                                                    */
/*  assign_stat([a, assign, b], []).                                          */
/*  assign_stat([a, assign, b, '*', c], []).                                  */
/*  stat([a, assign, b], []).                                                 */
/*  stat([a, assign, b, '*', c], []).                                         */
/*  stat_list([a, assign, b], []).                                            */
/*  stat_list([a, assign, b, '*', c], []).                                    */
/*  stat_list([a, assign, b, ';', a, assign, c], []).                         */
/*  stat_list([a, assign, b, '*', c, ';', a, assign, b, '*', c], []).         */
/* stat_part([begin, a, assign, b, '*', c, end, '.'], []).                    */
/******************************************************************************/
/* Var part                                                                   */
/******************************************************************************/
/* typ([integer], []).                                                        */
/* typ([real], []).                                                           */
/* typ([boolean], []).                                                        */
/* id([a], []).                                                               */
/* id([b], []).                                                               */
/* id([c], []).                                                               */
/* id_list([a], []).                                                          */
/* id_list([a, ',', b], []).                                                  */
/* id_list([a, ',', b, ',', c], []).                                          */
/* var_dec([a, ':', integer], []).                                            */
/* var_dec_list([a, ':', integer], []).                                       */
/* var_dec_list([a, ':', integer, b, ':', real], []).                         */
/* var_part([var, a, ':', integer], []).                                      */
/******************************************************************************/
/* Program header                                                             */
/******************************************************************************/
/* prog_head([program, c, '(', input, ',', output, ')', ';'], []).            */
/******************************************************************************/

/******************************************************************************/
/* Whole program                                                              */
/******************************************************************************/
/* program([program, c, '(', input, ',', output, ')', ';',                    */
/*          var, a,    ':', integer, ';',                                     */
/*               b, ',', c, ':', real,    ';',                                */
/*          begin,                                                            */
/*             a, assign, b, '*', c, ';',                                     */  
/*             a, assign, b, '+', c,                                          */
/*          end, '.'], []).                                                   */
/******************************************************************************/

/******************************************************************************/
/* Define the above tests                                                     */
/******************************************************************************/

testph :- prog_head([program, c, '(', input, ',', output, ')', ';'], []).
testpr :-   program([program, c, '(', input, ',', output, ')', ';'], []).

/******************************************************************************/
/* From Programming in Prolog (4th Ed.) Clocksin & Mellish, Springer (1994)   */
/* Chapter 5, pp 101-103 (DFR (140421) modified for input from a file)        */
/******************************************************************************/

read_in(File,[W|Ws]) :- see(File), get0(C), 
                        readword(C, W, C1), restsent(W, C1, Ws), nl, seen.

/******************************************************************************/
/* Given a word and the character after it, read in the rest of the sentence  */
/******************************************************************************/

restsent(W, _, [])         :- W = -1.                /* added EOF handling */
restsent(W, _, [])         :- lastword(W).
restsent(_, C, [W1 | Ws ]) :- readword(C, W1, C1), restsent(W1, C1, Ws).

/******************************************************************************/
/* Read in a single word, given an initial character,                         */
/* and remembering what character came after the word (NB!)                   */
/******************************************************************************/

readword(C, W, _)  :- C = -1, W = C.                    /* added EOF handling */
readword(C, W, C1) :- single_character( C ), name(W, [C]), get0(C1).
readword(C, W, C2) :-
   in_word(C, NewC ),
   get0(C1),
   restword(C1, Cs, C2),
   name(W, [NewC|Cs]).

readword(_, W, C2) :- get0(C1), readword(C1, W, C2).

restword(C, [NewC|Cs], C2) :-
   in_word(C, NewC),
   get0(C1),
   restword(C1, Cs, C2).

restword(C, [ ], C).

/******************************************************************************/
/* These characters form words on their own                                   */
/******************************************************************************/

single_character(40).                  /* ( */
single_character(41).                  /* ) */
single_character(42).                  /* + */
single_character(43).                  /* * */
single_character(44).                  /* , */
single_character(59).                  /* ; */
single_character(58).                  /* : */
single_character(61).                  /* = */
single_character(46).                  /* . */

/******************************************************************************/
/* These characters can appear within a word.                                 */
/* The second in_word clause converts character to lower case                 */
/******************************************************************************/

in_word(C, C) :- C>96, C<123.             /* a b ... z */
in_word(C, L) :- C>64, C<91, L is C+32.   /* A B ... Z */
in_word(C, C) :- C>47, C<58.              /* 1 2 ... 9 */

/******************************************************************************/
/* These words terminate a sentence                                           */
/******************************************************************************/

lastword('.').

/******************************************************************************/
/* added for demonstration purposes 140421, updated 150301                    */
/* testa  - file input (characters + Pascal program)                          */
/* testb  - file input as testa + output to file                              */
/* ttrace - file input + switch on tracing (check this carefully)             */
/******************************************************************************/

testa   :- testread(['cmreader.txt', 'testok1.pas']).
testb   :- tell('cmreader.out'), testread(['cmreader.txt', 'testok1.pas']), told.

ttrace  :- trace, testread(['cmreader.txt']), notrace, nodebug.

testread([]).
testread([H|T]) :- nl, write('Testing C&M Reader, input file: '), write(H), nl,
                   read_in(H,L), write(L), nl,
                   nl, write(' end of C&M Reader test'), nl,
                   testread(T).

/******************************************************************************/
/* end of program                                                             */
/******************************************************************************/