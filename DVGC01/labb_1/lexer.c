
/*          Johan Brekstad Wijk                                       */


/**********************************************************************/
/* lab 1 DVG C01 - Lexer OBJECT                                       */
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
#define BUFSIZE 1024
#define LEXSIZE 30
static char buffer[BUFSIZE];
static char lexbuf[LEXSIZE];
static int pbuf = 0; /* current index program buffer  */
static int plex = 0; /* current index lexeme  buffer  */

/**********************************************************************/
/*  PRIVATE METHODS for this OBJECT  (using "static" in C)            */
/**********************************************************************/
/**********************************************************************/
/* buffer functions                                                   */
/**********************************************************************/
/**********************************************************************/
/* Read the input file into the buffer                                */
/**********************************************************************/

static void get_prog() {
    char c;
    while ((c = getc(stdin)) != EOF) {
        buffer[pbuf++] = c;
    }
    // buffer[pbuf++] = '\n';
    buffer[pbuf++] = '$';
    // buffer[pbuf++] = '\0';
    pbuf = 0;
}

/**********************************************************************/
/* Display the buffer                                                 */
/**********************************************************************/

static void pbuffer() {
    printf("\n________________________________________________________");
    printf("\nTHE PROGRAM TEXT");
    printf("\n________________________________________________________\n");
    printf("%s", buffer);
    printf("\n________________________________________________________\n");
}

/**********************************************************************/
/* Copy a character from the program buffer to the lexeme buffer      */
/**********************************************************************/

static void get_char() {
    lexbuf[plex++] = buffer[pbuf++];
}

/**********************************************************************/
/* End of buffer handling functions                                   */
/**********************************************************************/

/**********************************************************************/
/*  PUBLIC METHODS for this OBJECT  (EXPORTED)                        */
/**********************************************************************/
/**********************************************************************/
/* Return a token                                                     */
/**********************************************************************/
int get_token() {
    if (pbuf == 0) {
        get_prog();
        pbuffer();
    }
    plex = 0;
    memset(lexbuf, 0, sizeof(lexbuf)); // Reset
    while (isspace(buffer[pbuf]))
        pbuf++; // Whitespace 

    get_char();

    if (isalpha(lexbuf[0])) { // checks if A-Z
        while ((isalpha(buffer[pbuf]) || isalnum(buffer[pbuf])) && !isspace(buffer[pbuf]))
            get_char();
        return key2tok(lexbuf);

    } else if (isdigit(lexbuf[0])) { // checks if 0-9
        while (isdigit(buffer[pbuf]) && !isspace(buffer[pbuf]))
            get_char();
        return number;

    } else if (ispunct(lexbuf[0])) { // checks for special characters
        if (lexbuf[0] == ':' && (buffer[pbuf] == '=')) {
            get_char();
        }
        return lex2tok(lexbuf);
    }
    return 0;
}

/**********************************************************************/
/* Return a lexeme                                                    */
/**********************************************************************/
char *get_lexeme() {
    return lexbuf;
}

/**********************************************************************/
/* End of code                                                        */
/**********************************************************************/
