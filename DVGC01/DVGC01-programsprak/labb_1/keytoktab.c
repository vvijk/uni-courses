
/*          Johan Brekstad Wijk                                       */


/**********************************************************************/
/* lab 1 DVG C01 - Driver OBJECT                                      */
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
#include "keytoktab.h"

/**********************************************************************/
/* OBJECT ATTRIBUTES FOR THIS OBJECT (C MODULE)                       */
/**********************************************************************/
/**********************************************************************/
/* type definitions                                                   */
/**********************************************************************/
typedef struct tab {
    char *text;
    int token;
} tab;

/**********************************************************************/
/* data objects (tables)                                              */
/**********************************************************************/
static tab tokentab[] = {
    {"id", id},
    {"number", number},
    {":=", assign},
    {"undef", undef},
    {"predef", predef},
    {"tempty", tempty},
    {"error", error},
    {"type", typ},
    {"$", '$'},
    {"(", '('},
    {")", ')'},
    {"*", '*'},
    {"+", '+'},
    {",", ','},
    {"-", '-'},
    {".", '.'},
    {"/", '/'},
    {":", ':'},
    {";", ';'},
    {"=", '='},
    {"TERROR", nfound}};

static tab keywordtab[] = {
    {"program", program},
    {"input", input},
    {"output", output},
    {"var", var},
    {"begin", begin},
    {"end", end},
    {"boolean", boolean},
    {"integer", integer},
    {"real", real},
    {"KERROR", nfound}};

/**********************************************************************/
/*  PUBLIC METHODS for this OBJECT  (EXPORTED)                        */
/**********************************************************************/
/**********************************************************************/
/* Display the tables                                                 */
/**********************************************************************/
void p_toktab() {
    printf("\nTHE PROGRAM KEYWORDS");
    for (int i = 0; i < (kend - kstart - 1); i++) {
        printf("\n%s        %i", keywordtab[i].text, keywordtab[i].token);
    }

    printf("\n\nTHE PROGRAM TOKENS");
    for (int i = 0; i < (tend - tstart + 11); i++) {
        printf("\n%s        %i", tokentab[i].text, tokentab[i].token);
    }
}

/**********************************************************************/
/* lex2tok - convert a lexeme to a token                              */
/**********************************************************************/
toktyp lex2tok(char *fplex) {
    for (int i = 0; i < sizeof(tokentab) / sizeof(tab); i++) {
        if (strcmp(fplex, tokentab[i].text) == 0) {
            return tokentab[i].token;
        }
    }
    for (int j = 0; j < sizeof(keywordtab) / sizeof(tab); j++) {
        if (strcmp(fplex, keywordtab[j].text) == 0) {
            return keywordtab[j].token;
        }
    }
    return 0;
}

/**********************************************************************/
/* key2tok - convert a keyword to a token                             */
/**********************************************************************/
toktyp key2tok(char *fplex) {
    for (int i = 0; i < sizeof(keywordtab) / sizeof(tab); i++) {
        if (strcmp(keywordtab[i].text, fplex) == 0) {
            return keywordtab[i].token;
        }
    }
    return id; //
}

/**********************************************************************/
/* tok2lex - convert a token to a lexeme                              */
/**********************************************************************/
char *tok2lex(toktyp ftok) {
    for (int i = 0; i < sizeof(tokentab) / sizeof(tab); i++) {
        if (ftok == tokentab[i].token) {
            return tokentab[i].text;
        }
    }
    for (int j = 0; j < sizeof(keywordtab) / sizeof(tab); j++) {
        if (ftok == keywordtab[j].token) {
            return keywordtab[j].text;
        }
    }
    return "errrrrrror";
}

/**********************************************************************/
/* End of code                                                        */
/**********************************************************************/
