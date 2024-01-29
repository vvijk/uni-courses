(defun fizzbuzz (x)
    (cond
        ((and (zerop (mod (first x) 3)) (zerop (mod (first x) 5))) (format t "FizzBuzz "))
        ((zerop (mod (first x) 3)) (format t "Fizz"))
        ((zerop (mod (first x) 5)) (format t "Buzz"))
        (t (format t "~a" (first x)))
        ;; (t (format t (first x)))     Det här hade säkert varit godkänt på tenta
    )
    (when (rest x)
        (format t " ")
        (fizzbuzz (rest x))
    )
)

(fizzbuzz '(1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20))