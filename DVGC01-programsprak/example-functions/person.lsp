(defstruct person
    (name)
    (phone)
    (address)
)

;; Create an instance of the struct above for an
;; abritrary person and let a variable refrence it (2p)
(setf enPerson (make-person     :name "Juan"
                                :phone "1337"
                                :address "Sommargatan")
)

;; Write a function that prints the name and anddress for a person (3p)
(defun printAperson (enPerson)
        (progn
            (format t "~a" (person-address enPerson)
            (format t "~a" (person-name enPerson))
        )
))
(printAperson enPerson)
