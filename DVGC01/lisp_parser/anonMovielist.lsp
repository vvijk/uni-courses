(defun anon (movies)
  (cond
    ((null movies) nil)
    ((endp movies) nil)
    (t (cons (second (first movies))
             (anon (rest movies)))))
)

(setf movielist '((('Shining) ('Kubrick) ('Nicholson))
                  (('Pulp Fiction) ('Tarantino) ('Jackson))
                  (('Robocop) ('Verhoeven) ('Weller))
                  (('Accountant) ('OConnor) ('Affleck))
                  (('Lone Wolf and Cub 1) ('Misumi) ('Wakayama))))
(anon movielist)