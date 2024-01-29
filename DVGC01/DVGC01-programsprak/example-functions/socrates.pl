 
/******************************************************************************/
/* Famous example from a first course in logic!                               */
/******************************************************************************/

/******************************************************************************/
/* Facts                                                                      */
/******************************************************************************/
man(socrates).                    /* Socrates is a man                        */

/******************************************************************************/
/* Rules                                                                      */
/******************************************************************************/
mortal(X) :- man(X).              /* all men are mortal                       */

/******************************************************************************/
/* Tests to try                                                               */
/******************************************************************************/
/*   mortal(socrates).                                                        */
/*   true  - rule + fact                                                      */
/******************************************************************************/

/******************************************************************************/
/* End of program                                                             */
/******************************************************************************/
