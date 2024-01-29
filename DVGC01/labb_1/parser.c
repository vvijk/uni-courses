
/*          Johan Brekstad Wijk                                       */


/**********************************************************************/
/* lab 1 DVG C01 - Parser OBJECT                                      */
/**********************************************************************/
/**********************************************************************/
/* Include files                                                      */
/**********************************************************************/
#include <ctype.h>
#include <stdio.h>
#include <string.h>

/**********************************************************************/
/* Other OBJECT's METHODS (IMPORTED)                                  */
/**********************************************************************/
#include "keytoktab.h" /* when the keytoktab is added   */
#include "lexer.h"     /* when the lexer     is added   */
#include "optab.h"     /* when the optab     is added   */
#include "symtab.h"    /* when the symtab    is added   */

/**********************************************************************/
/* OBJECT ATTRIBUTES FOR THIS OBJECT (C MODULE)                       */
/**********************************************************************/
#define DEBUG 0
static int lookahead = 0;
static int is_parse_ok = 1;

/**********************************************************************/
/* RAPID PROTOTYPING - simulate the token stream & lexer (get_token)  */
/**********************************************************************/
/* define tokens + keywords NB: remove this when keytoktab.h is added */
/**********************************************************************/
// enum tvalues { program = 257,
//                id,
//                input,
//                output,
//                var = 261,
//                integer = 262,
//                begin = 263,
//                end = 266,
//                predef = 264,
//                number = 265 };
/**********************************************************************/
/* Simulate the token stream for a given program                      */
/**********************************************************************/
/*
static int tokens[] = {program, id, '(', input, ',', output, ')', ';',
                       var, id, ',', id, ',', id, ':', integer, ';',
                       begin, id, assign, id, '+', id, '*', number,
                       end, '.', '$'};
*/
/**********************************************************************/
/*  Simulate the lexer -- get the next token from the buffer          */
/**********************************************************************/
/*
static int pget_token() {
    static int i = 0;
    if (tokens[i] != '$')
        return tokens[i++];
    else
      return '$';
}
*/
/**********************************************************************/
/*  PRIVATE METHODS for this OBJECT  (using "static" in C)            */
/**********************************************************************/

static toktyp expr();

static void printInOut(char *text, int IN) {
    if (DEBUG) {
        if (IN) {
            printf("\n*** In %s", text);
        } else {
            printf("\n*** Out %s", text);
        }
    }
}

/**********************************************************************/
/* The Parser functions                                               */
/**********************************************************************/
static void match(int t) {
    if (DEBUG)
        printf("\n --------In match expected: %4s, found: %4s",
               tok2lex(t), tok2lex(lookahead));
    if (lookahead == t) {
        lookahead = get_token();
    } else {
        is_parse_ok = 0;
        printf("SYNTAX: Symbol expected: %s found: %s\n",
               tok2lex(t), get_lexeme(lookahead));
    }
}

/**********************************************************************/
/* The grammar functions                                              */
/**********************************************************************/
static void program_header() {
    printInOut("program_header", 1);
    match(program);
    if (lookahead == id) {
        addp_name(get_lexeme());
        match(id);
    } else {
        printf("SYNTAX: ID expected found %s\n", get_lexeme());
        addp_name("???");
        is_parse_ok = 0;
    }
    match('(');
    match(input);
    match(',');
    match(output);
    match(')');
    match(';');

    printInOut("program_header", 0);
}
/**********************************************************************/
/* The var part                                                       */
/**********************************************************************/
static void type() {
    /* Check if Integer | real | boolean */
    printInOut("type", 1);
    switch (lookahead) {
    case integer:
        match(integer);
        setv_type(integer);
        break;
    case real:
        match(real);
        setv_type(real);
        break;
    case boolean:
        match(boolean);
        setv_type(boolean);
        break;
    default:
        setv_type(error);
        printf("SYNTAX: Type name expected found %s\n", get_lexeme());
        is_parse_ok = 0;
        break;
    }
    printInOut("type", 0);
}
static void id_list() {
    printInOut("id_list", 1);
    if (lookahead == id) {
        char *name = get_lexeme();
        if (!find_name(name)) {
            addv_name(name);
        } else {
            printf("SEMANTIC: ID already declared: %s\n", get_lexeme());
            is_parse_ok = 0;
        }
    }
    match(id);
    if (lookahead == ',') {
        match(',');
        id_list();
    }
    printInOut("id_list", 0);
}
static void var_dec() {
    printInOut("var_dec", 1);
    id_list();
    match(':');
    type();
    match(';');
    printInOut("var_dec", 0);
}
static void var_dec_list() {
    printInOut("var_dec_list", 1);
    var_dec();
    if (lookahead == id) {
        var_dec_list();
    }
    printInOut("var_dec_list", 0);
}
static void var_part() {
    printInOut("var_part", 1);
    match(var);
    var_dec_list();
    printInOut("var_part", 0);
}
/**********************************************************************/
/* The stat part                                                      */
/**********************************************************************/
static toktyp operand() {
    printInOut("operand", 1);
    toktyp variableType = error;
    if (lookahead == id) {
        char* variableName;
        variableName = get_lexeme();
        if(find_name(variableName)){
            variableType = get_ntype(variableName);
            match(id);
        }else {
            printf("SEMANTIC: ID NOT declared: %s\n", get_lexeme());
            match(id);
        }
    } else if (lookahead == number) {
        match(number);
        variableType = integer;
    }else{
        is_parse_ok = 0;
        printf("SYNTAX: Operand expected\n");
        variableType = error;
    }
    printInOut("operand", 0);
    return variableType;
}
static toktyp factor() {
    printInOut("factor", 1);
    toktyp typ;
    if (lookahead == '(') {
        match('(');
        typ = expr();
        match(')');
    } else {
        typ = operand();
    }
    printInOut("factor", 0);
    return typ;
}
static toktyp term() {
    printInOut("term", 1);
    toktyp l_side = factor();
    if(lookahead == '*'){
        match('*');
        l_side = get_otype('*', l_side, term());
    }
    printInOut("term", 0);
    return l_side;
}
static toktyp expr() {
    /*
    [expr] ::= [term] [expr-tail]
    [expr-tail] ::= empty | + [term] [expr-tail]
    */
    printInOut("expr", 1);
    toktyp l_side = term();
    if (lookahead == '+') {
        match('+');
        l_side = get_otype('+', l_side, expr());
    }
    printInOut("expr", 0);
    return l_side;
}

static void assaign_stat() {
    printInOut("assaign_stat", 1);
    char* lex = get_lexeme();
    toktyp left = error;
    toktyp right = error;
    if(lookahead == id){
        if(find_name(lex)){
            left = get_ntype(lex);
        }else{
            printf("SYNTAX: ID NOT declared:%s\n", lex);
        }
        match(id);
    }else{
        printf("SYNTAX: ID expected found:%s\n", get_lexeme());
        is_parse_ok = 0;
    }
    match(assign);
    right = expr();
    if(left != right){
        printf("SEMANTIC: Assign types: %s := %s\n", tok2lex(left), tok2lex(right));
        is_parse_ok = 0;
    }
    printInOut("assaign_stat", 0);
}
static void stat() {
    printInOut("stat", 1);
    assaign_stat();
    printInOut("stat", 0);
}
static void stat_list() {
    printInOut("stat_list", 1);
    stat();
    if (lookahead == ';') {
        match(';');
        stat_list();
    }
    printInOut("stat_list", 0);
}
static void stat_part() {
    printInOut("stat_part", 1);
    match(begin);
    stat_list();
    match(end);
    match('.');
    printInOut("stat_part", 0);
}
/**********************************************************************/
/*  PUBLIC METHODS for this OBJECT  (EXPORTED)                        */
/**********************************************************************/

int parser() {
    printInOut("parser", 1);
    lookahead = get_token();

    if (lookahead != '$') {
        program_header();
        var_part();
        stat_part();
        if (lookahead != '$') {
            printf("SYNTAX: Extra symbols after end of parse!\n");
            while(lookahead != '$'){
                printf("%s, ", get_lexeme());
                lookahead = get_token();
            }
        }
    } else {
        printf("SYNTAX: Input file is empty\n");
        is_parse_ok = 0;
    }
    if (is_parse_ok) {
        printInOut("parser", 0);
    }
    p_symtab();
    return is_parse_ok;
}

/**********************************************************************/
/* End of code                                                        */
/**********************************************************************/