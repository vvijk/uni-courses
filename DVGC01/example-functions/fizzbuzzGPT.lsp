(defun fizzbuzz (x)
  (cond
    ((zerop (mod (first x) 3)) (format t "fizz"))
    ((zerop (mod (first x) 5)) (format t "buzz"))
    (t (format t "~a" (first x)))
  )
  (when (rest x)
    (format t " ")
    (fizzbuzz (rest x))
  )
)

(fizzbuzz '(1 2 3 4 5 6))
