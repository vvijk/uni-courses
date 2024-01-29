;;=====================================================================
;; LISP READER & LEXER - new version 161202
;;=====================================================================

;;=====================================================================
;; Help functions
;;=====================================================================
;; ctos         convert a character to a string
;; str_con      concatenate 2 strings str, c
;; whitespace   is c whitespace?
;;=====================================================================

(defun ctos (c)        (make-string 1 :initial-element c))
(defun str-con (str c) (concatenate 'string str (ctos c)))
(defun whitespace (c)  (member c '(#\Space #\Tab #\Newline)))

;;=====================================================================
;; get-wspace   remove whitespace
;;=====================================================================

(defun get-wspace (ip)
   (setf c (read-char ip nil 'EOF))
   (cond   
           ((whitespace c)  (get-wspace ip))
           (t                             c)
   )
)

;;=====================================================================
;; Read an Identifier         Compare this with C's do-while construct
;;=====================================================================

(defun get-name (ip lexeme c)
   (setf lexeme (str-con lexeme c))
   (setf c      (read-char ip nil 'EOF))
   (cond        
                ((alphanumericp c)  (get-name ip lexeme c))
                (t                  (list        c lexeme))
   )
)

;;=====================================================================
;; Read a Number              Compare this with C's do-while construct
;;=====================================================================

(defun get-number (ip lexeme c)
   (setf lexeme (str-con lexeme c))
   (setf c      (read-char ip nil 'EOF))
   (cond
         ((not (null (digit-char-p c)))  (get-number ip lexeme c))
         (t                              (list          c lexeme))
   )
  )

;;=====================================================================
;; Read a single character or ":="
;;=====================================================================

(defun get-symbol (ip lexeme c)
   (setf lexeme (str-con lexeme c))
   (setf c1 c)
   (setf c (read-char ip nil 'EOF))
   (cond
         ((and (char= c1 #\:) (char= c #\=))  (get-symbol ip lexeme c))
         (t                                   (list          c lexeme))
   )
)

;;=====================================================================
;; Read a Lexeme                       lexeme is an "accumulator"
;;                                     Compare this with the C version
;;=====================================================================

(defun get-lex (state)
   (setf lexeme "")
   (setf ip (pstate-stream   state))
   (setf c  (pstate-nextchar state))
   (if (whitespace c) (setf c (get-wspace ip)))
   (cond
         ((eq c 'EOF)                     (list 'EOF ""))
         ((alpha-char-p c)                (get-name   ip lexeme c))
         ((not (null (digit-char-p c)))   (get-number ip lexeme c))
         (t                               (get-symbol ip lexeme c))
   )
)

;;=====================================================================
; map-lexeme(lexeme) returns a list: (token, lexeme)
;;=====================================================================

(defun map-lexeme (lexeme)
(format t "Symbol: ~S ~%" lexeme)
   (list (cond
         ((string=   lexeme   "program")  'PROGRAM )
         ((string=   lexeme   "var"    )  'VAR     )
         ((string=   lexeme   ","      )  'COMMA   )
         ((string=   lexeme   "input"  )  'INPUT   )
         ((string=   lexeme   "output" )  'OUTPUT  )
         ((string=   lexeme   "begin"  )  'BEGIN   )
         ((string=   lexeme   "end"    )  'END     )
         ((string=   lexeme   "boolean")  'BOOLEAN )
         ((string=   lexeme   "integer")  'INTEGER )
         ((string=   lexeme   "real"   )  'REAL    )
         ((string=   lexeme   ":="     )  'ASSIGN  )
         ((string=   lexeme   "."      )  'FSTOP   )
         ((string=   lexeme   ":"      )  'COLON   )
         ((string=   lexeme   ";"      )  'SCOLON  )
         ((string=   lexeme   "("      )	'LP      )
         ((string=   lexeme   ")"      )	'RP      )
         ((string=   lexeme   "+"      )  'PLUS    )
         ((string=   lexeme   "*"      )  'MULTI   )

         ((string=   lexeme ""         )	'EOF     )
         ((is-id     lexeme            )  'ID      )
         ((is-number lexeme            )  'NUM     )
         (t                             'UNKNOWN )
         )
    lexeme)
)

;;=====================================================================
; ID is [A-Z,a-z][A-Z,a-z,0-9]*          number is [0-9][0-9]*
;;=====================================================================

(defun is-id (str)
   (some #'alpha-char-p str)
)

(defun is-number (str)
   (every #'digit-char-p str)
)

;;=====================================================================
; THIS IS THE PARSER PART
;;=====================================================================

;;=====================================================================
; Create a stucture - parse state descriptor
;;=====================================================================
; lookahead is the list (token, lexeme)
; stream    is the input filestream
; nextchar  is the char following the last lexeme read
; status    is the parse status (OK, NOTOK)
; symtab    is the symbol table
;;=====================================================================

(defstruct pstate
    (lookahead) ; Innehåller token och motsvarande lexeme
    (stream)   ;Själva filen vi läser ifrån
    (nextchar) ; tecknet efter
    (status)   ;is-parse-ok; OK/NOTOK
    (symtab)
)

;;=====================================================================
; Constructor for the structure pstate - initialise
; stream to the input file stream (ip)
;;=====================================================================

(defun create-parser-state (ip)
   (make-pstate
      :stream        ip
      :lookahead     ()
      :nextchar      #\Space
      :status        'OK
      :symtab        ()
    )
)

;;=====================================================================
; SYMBOL TABLE MANIPULATION
;;=====================================================================

;;=====================================================================
; token  - returns the token  from (token lexeme)(reader)
; lexeme - returns the lexeme from (token lexeme)(reader)
;;=====================================================================

(defun token  (state)
   (first (pstate-lookahead state))
)
(defun lexeme (state)
   (second (pstate-lookahead state))
)

;;=====================================================================
; symbol table manipulation: add + lookup + display
;;=====================================================================

(defun symtab-add (state id)
   ;(pushnew id (pstate-symtab state) :test #'string-equal)
   ;adds an element to a list if it's not already a member.
   (if (null (pstate-symtab state ))
      (setf (pstate-symtab state) (list id))
      (setf (pstate-symtab state) (append (pstate-symtab state) (list id)))
   )
)

(defun symtab-member (state id)
   (member id (pstate-symtab state) :test #'string-equal)
)

(defun symtab-display (state)
   (format t "------------------------------------------------------~%")
   (format t "Symbol Table is: ~S ~%" (pstate-symtab state))
   (format t "------------------------------------------------------~%")
)

;;=====================================================================
; Error functions: Syntax & Semantic
;;=====================================================================

(defun synerr1 (state symbol)
    (format t "*** Syntax error:   Expected ~8S found ~8S ~%"
           symbol (lexeme state))
    (setf (pstate-status state) 'NOTOK))

(defun synerr2 (state)
    (format t "*** Syntax error:   Expected TYPE     found ~S ~%"
           (lexeme state))
    (setf (pstate-status state) 'NOTOK))

(defun synerr3 (state)
    (format t "*** Syntax error:   Expected OPERAND  found ~S ~%"
           (lexeme state))
    (setf (pstate-status state) 'NOTOK))

(defun semerr1 (state)
    (format t "*** Semantic error: ~S already declared.~%"
                (lexeme state))
    (setf (pstate-status state) 'NOTOK))
(defun semerr2 (state)
    (format t "*** Semantic error: ~S not declared.~%"
          (lexeme state))
    (setf (pstate-status state) 'NOTOK))
(defun semerr3 (state)
    (format t "*** Semantic error: found ~8S expected EOF.~%"
          (lexeme state))
    (setf (pstate-status state) 'NOTOK)
    ;; *** TO BE DONE - completed! ***
)

;;=====================================================================
; The return value from get-token is always a list. (token lexeme)
;;=====================================================================

(defun get-token (state)
  (let    ((result (get-lex state)))
    (setf (pstate-nextchar  state) (first result))
    (setf (pstate-lookahead state) (map-lexeme (second result)))
  )
 )

;;=====================================================================
; match compares lookahead with symbol (the expected token)
; if symbol == lookahead token ==> get next token else Syntax error
;;=====================================================================

(defun match (state symbol)
   (if (eq symbol (token state))
       (get-token  state)
       (synerr1    state symbol)
       )
)

;;=====================================================================
; THE GRAMMAR RULES
;;=====================================================================

;;=====================================================================
; <stat-part>     --> begin <stat-list> end .
; <stat-list>     --> <stat> | <stat> ; <stat-list>
; <stat>          --> <assign-stat>
; <assign-stat>   --> id := <expr>
; <expr>          --> <term>     | <term> + <expr>
; <term>          --> <factor>   | <factor> * <term>
; <factor>        --> ( <expr> ) | <operand>
; <operand>       --> id | number
;;=====================================================================
(defun operand (state)
   (cond
      ((and (eq (token state) 'ID) (symtab-member state (lexeme state))) 
         (progn 
            (match state 'ID)))
      ((eq (token state) 'ID)
         (progn 
            (semerr2 state)
            (match state 'ID)))
      ((eq (token state) 'NUM) 
         (progn 
            (match state 'NUM)))
      (t (synerr3 state)))
)
(defun factor (state)
   (if (eq (token state) 'LP)
      (progn
         (match state 'LP)
         (expr state)     
         (match state 'RP)
      )
   (operand state))
)
(defun term (state)
   (factor state)
   (if (eq (token state) 'MULTI)
      (progn
      (match state 'MULTI)
      (term state)
      )
   )
)
(defun expr (state)
   (term state)
   (if (eq (token state) 'PLUS)
      (progn
         (match state 'PLUS)
         (expr state)
      )
   )
)
(defun assaign-stat (state)
   (if (eq (token state) 'ID)
      (progn
         (if (not (symtab-member state (lexeme state)))
            (semerr2 state))
         (match state 'ID)
      )
      (synerr1 state 'ID);;
   )

   (match state 'ASSIGN)
   (expr state)
)
(defun stat-stat (state)
   (assaign-stat state)
)
(defun stat-list (state)
   (stat-stat state)
   (if (EQ(token state) 'SCOLON)
      (progn
      (match state 'SCOLON)
      (stat-list state)
      )
   )
)
(defun stat-part (state)
   (match state 'BEGIN)
   (stat-list state)
   (match state 'END)
   (match state 'FSTOP)
)

;;=====================================================================
; <var-part>     --> var <var-dec-list>
; <var-dec-list> --> <var-dec> | <var-dec><var-dec-list>
; <var-dec>      --> <id-list> : <type> ;
; <id-list>      --> id | id , <id-list>
; <type>         --> integer | real | boolean
;;=====================================================================
(defun var-type (state)
   (cond
      ((EQ(token state) 'INTEGER)   (match state 'INTEGER))
      ((EQ(token state) 'REAL)      (match state 'REAL))
      ((EQ(token state) 'BOOLEAN)   (match state 'BOOLEAN))
      (t (synerr2 state))
   )
)
(defun id-list (state)
   (if (eq (token state) 'ID)
       (if (not (symtab-member state (lexeme state)))
           (symtab-add state (lexeme state))
           (semerr1 state)))

   (match state 'id)

   (if (eq (token state) 'COMMA)
      (progn
      (match state 'COMMA)
      (id-list state))
   )
)
(defun var-dec (state)
   (id-list state)
   (match state 'COLON)
   (var-type state)
   (match state 'SCOLON)
)
(defun var-dec-list (state)
   (var-dec state)
   (if (EQ(token state) 'ID)(var-dec-list state))
)
(defun var-part (state)
   (match state 'VAR)
   (var-dec-list state)
)
;;=====================================================================
; <program-header>
;;=====================================================================
(defun program-header (state)
  (match state 'PROGRAM)
  (match state 'ID)
  (match state 'LP)
  (match state 'INPUT)
  (match state 'COMMA)
  (match state 'OUTPUT)
  (match state 'RP)
  (match state 'SCOLON)
)
;;=====================================================================
; <program> --> <program-header><var-part><stat-part>
;;=====================================================================
(defun program (state)
   (program-header state)
   (var-part       state)
   (stat-part      state)
)
;;=====================================================================
; THE PARSER - parse a file
;;=====================================================================
(defun check-end-end (state)
   (semerr3 state)
   (get-token state)
   (check-end state)
)
(defun check-end (state)
   (if (not (eq (token state) 'EOF))(check-end-end state))
)

;;=====================================================================
; Test parser for file name input
;;=====================================================================

(defun parse (filename)
   (format t "~%------------------------------------------------------")
   (format t "~%--- Parsing program: ~S " filename)
   (format t "~%------------------------------------------------------~%")
   (with-open-file (ip (open filename) :direction :input) ;; ip = filpekare
      (setf state (create-parser-state ip))
      (setf (pstate-nextchar state) (read-char ip nil 'EOF))
      (get-token      state)
      (program        state)
      (check-end      state)
      (symtab-display state)
      )
   (if (eq (pstate-status state) 'OK)
      (format t "Parse Successful. ~%")
      (format t "Parse Fail. ~%")
      )
   (format t "------------------------------------------------------~%")
)
;;=====================================================================
; THE PARSER - parse all the test files
;;=====================================================================

(defun parse-all ()
   (mapcar #'parse '("testfiles/testa.pas" 
                     "testfiles/testb.pas" 
                     "testfiles/testc.pas"
                     "testfiles/testd.pas"
                     "testfiles/teste.pas"
                     "testfiles/testf.pas"
                     "testfiles/testg.pas"
                     "testfiles/testh.pas"
                     "testfiles/testi.pas"
                     "testfiles/testj.pas"
                     "testfiles/testk.pas"
                     "testfiles/testl.pas"
                     "testfiles/testm.pas"
                     "testfiles/testn.pas"
                     "testfiles/testo.pas"
                     "testfiles/testp.pas"
                     "testfiles/testq.pas"
                     "testfiles/testr.pas"
                     "testfiles/tests.pas"
                     "testfiles/testt.pas"
                     "testfiles/testu.pas"
                     "testfiles/testv.pas"
                     "testfiles/testw.pas"
                     "testfiles/testx.pas"
                     "testfiles/testy.pas"
                     "testfiles/testz.pas"
                     "testfiles/testok1.pas"
                     "testfiles/testok2.pas"
                     "testfiles/testok3.pas"
                     "testfiles/testok4.pas"
                     "testfiles/testok5.pas"
                     "testfiles/testok6.pas"
                     "testfiles/testok7.pas"
                     "testfiles/fun1.pas"
                     "testfiles/fun2.pas"
                     "testfiles/fun3.pas"
                     "testfiles/fun4.pas"
                     "testfiles/fun5.pas"
                     "testfiles/sem1.pas"
                     "testfiles/sem2.pas"
                     "testfiles/sem3.pas"
                     "testfiles/sem4.pas"
                     "testfiles/sem5.pas"
                  )
   )
)

;;=====================================================================
; THE PARSER - test all files
;;=====================================================================
(parse-all)
;;=====================================================================
; THE PARSER - test a single file
;;=====================================================================
;;=====================================================================
; THE PARSER - end of code
;;=====================================================================

