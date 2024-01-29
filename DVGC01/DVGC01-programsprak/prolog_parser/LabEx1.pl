/******************************************************************************/
/* Prolog Lab 2 example - Grammar test bed                                    */
/******************************************************************************/
/* JOHAN BREKSTAD WIJK */
/* JOHAN BREKSTAD WIJK */
/* JOHAN BREKSTAD WIJK */
/* JOHAN BREKSTAD WIJK */
/* JOHAN BREKSTAD WIJK */
/******************************************************************************/
/* Grammar Rules in Definite Clause Grammar form                              */
/* This the set of productions, P, for this grammar                           */
/* This is a slightly modified from of the Pascal Grammar for Lab 2 Prolog    */
/******************************************************************************/
prog       --> prog_head, var_part, stat_part.

/******************************************************************************/
/* Program Header                                                             */
/******************************************************************************/
prog_head     --> program, id, lpar, input, comma, output, rpar, scolon.

/******************************************************************************/
/* Var_part                                                                   */
/******************************************************************************/
var_part            -->     var, var_dec_list.
var_dec_list        -->     var_dec | var_dec, var_dec_list.
var_dec             -->     id_list, colon, type, scolon.
id_list             -->     id | id, comma, id_list.
type                -->     integer | boolean |real.

/******************************************************************************/
/* Stat part                                                                  */
/******************************************************************************/
stat_part		-->  begin, stat_list, end, dot.
stat_list		-->  stat | stat, scolon, stat_list.
stat		   	-->  assign_stat.
assign_stat		-->  id, assign, expr.
expr		   	-->  term | term, add, expr.
term		   	-->  factor | factor, mult, term.
factor			-->  lpar, expr, rpar | operand.
operand			-->  id | number.

/******************************************************************************/
/* Lexer                                                                   */
/******************************************************************************/
lexer([ ], [ ]).
lexer([H|T], [F|S]) :- match(H,F), lexer(T,S). 

test_lexer(File)  :- read_in(File, L), lexer(L, X), write(X).
test_file(File)   :- read_in(File, L), write(L).

match(H,T) :- H = 'program', T is 256.
match(H,T) :- H = 'input'  , T is 257.
match(H,T) :- H = 'output' , T is 258.
match(H,T) :- H = 'var'    , T is 259.
match(H,T) :- H = 'integer', T is 260.
match(H,T) :- H = 'begin'  , T is 261.
match(H,T) :- H = 'end'    , T is 262.
match(H,T) :- H = 'boolean', T is 263.
match(H,T) :- H = 'real'   , T is 264.
match(H,T) :- H = ':='     , T is 271.
match(H,T) :- H = '('      , T is 40.
match(H,T) :- H = ')'      , T is 41.
match(H,T) :- H = '*'      , T is 42.
match(H,T) :- H = '+'      , T is 43.
match(H,T) :- H = ','      , T is 44.
match(H,T) :- H = '.'      , T is 46.
match(H,T) :- H = ':'      , T is 58.
match(H,T) :- H = ';'      , T is 59.

match(L,T) :- name(L, [H|Tail]), char_type(H, alpha), match_id(Tail), T is 270.
match(L,T) :- name(L, [H|Tail]), char_type(H, digit), match_num(Tail), T is 272.

match(L,T) :- char_type(L, end_of_file), T is 275.
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
readword(C,W,C2) :- C = 58, get0(C1), readwordaux(C,W,C1,C2).
readword(C, W, C2) :- C<58, C>47, name(W, [C]), get0(C2).
readword(C, W, C1) :- single_character( C ), name(W, [C]), get0(C1).
readword(C, W, C2) :-
   in_word(C, NewC ),
   get0(C1),
   restword(C1, Cs, C2),
   name(W, [NewC|Cs]).

readword(_, W, C2) :- get0(C1), readword(C1, W, C2).

readwordaux(C,W,C1,C2) :- C1 = 61, name(W,[C,C1]), get0(C2).
readwordaux(C,W,C1,C2) :- C1 \= 61, name(W,[C]), C1 = C2.

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

testall :-  parseFiles( [ 'testfiles/testok1.pas', 'testfiles/testok2.pas', 'testfiles/testok3.pas', 'testfiles/testok4.pas', 
                          'testfiles/testok5.pas', 'testfiles/testok6.pas', 'testfiles/testok7.pas', 'testfiles/testa.pas',
                          'testfiles/testb.pas', 'testfiles/testc.pas', 'testfiles/testd.pas', 'testfiles/teste.pas', 'testfiles/testf.pas',
                          'testfiles/testg.pas', 'testfiles/testh.pas', 'testfiles/testi.pas', 'testfiles/testj.pas', 'testfiles/testk.pas',
                          'testfiles/testl.pas', 'testfiles/testm.pas', 'testfiles/testn.pas', 'testfiles/testo.pas', 'testfiles/testp.pas',
                          'testfiles/testq.pas', 'testfiles/testr.pas', 'testfiles/tests.pas', 'testfiles/testt.pas', 'testfiles/testu.pas',
                          'testfiles/testv.pas', 'testfiles/testw.pas', 'testfiles/testx.pas', 'testfiles/testy.pas', 'testfiles/testz.pas',
                          'testfiles/fun1.pas', 'testfiles/fun2.pas', 'testfiles/fun3.pas', 'testfiles/fun4.pas', 'testfiles/fun5.pas', 
                          'testfiles/sem1.pas', 'testfiles/sem2.pas', 'testfiles/sem3.pas', 'testfiles/sem4.pas', 'testfiles/sem5.pas']).
parseFiles([]). 
parseFiles([H|T]) :-  write('Testing '), write(H), nl, 
                      read_in(H,L), lexer(L, Tokens),
                      write(L), nl, write(Tokens), nl,
                      parser(Tokens, []), nl,
                      write(H), write(' end of parse'), nl, nl,
                      parseFiles(T).

parser(Tokens, Res) :- (prog(Tokens, Res), Res = [], write('Parse OK!'));  
                        write('Parse Fail!').
/******************************************************************************/
/* end of program                                                             */
/******************************************************************************/