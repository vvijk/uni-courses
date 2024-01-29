;; Implement the recursive function strlen that takes as its input a list of characters and
;; returns the length of the list, i.e. strlen: L à int

(defun strlen (lista)
    (if (null lista)
        0
        (+ 1 (strlen (rest lista)))
    )
)
;; Example usage
(setq lista '(1 2 3 4 5 6 7))
(print (strlen lista)) ; Output should be 7