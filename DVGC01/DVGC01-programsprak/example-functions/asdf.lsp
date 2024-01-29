(defun init (fun lista)
  (cond
        ((null lista) nil)
        ((endp lista) nil)
        (t (cons (funcall fun (first lista))
                (init fun (rest lista))))
    )
)

(defun get_second (full-name)
    (car (last full-name))
)

(defun get_initials (full-name)
    (print (full-name))
)

(setf a '((bushwick bill) (tupac shakur) (biggie smalls)))

;(print (init #'get_second a))
;(print (init #'get_initials a))
(print (get_initials a))
