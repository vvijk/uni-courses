(defun separate (names fun)
    (cond
        ((null names)nil                    )
        ((endp names)nil                    )
        (t      (cons (funcall fun (first names  ))
                (separate (rest names) fun)))
    )
)
(setf a '((aretha franklin) (tina turner) (amy winehouse)))


(format t "~a" (separate a #'second))  ;; FÖR FRÅGA b)
;; (format t "~a" (list (separate a #'first) (separate a #'second))) ;; FÖR FRÅGA 
