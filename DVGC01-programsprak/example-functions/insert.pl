#19-01-10

insert([], It, [It]).
insert([H|T], It, [It, H|T]) :- H @> It.
insert([H|T], It, [H|NewT]) :- H @< It, insert(T, It, NewT).



#?- TRACE insert([1,2,3], 4, X).
# Call: insert([1,2,3], 4, X). 
# Fail: 1 > 4
# Redo: insert([1,2,3], 4, NewT). - 
# Call: 1 < 4 
# exit: H < 4
# call: insert([2,3], 4, NewT).  
# call: 2 < 4
# Redo: insert([2,3], 4, NewT).  -
# call: 2 < 4
# exit: 2 < 4
# call: insert([3], 4, NewT).  
# call: 3 < 4
# Redo: insert([3], 4, NewT).  -
# call: 3 < 4
# exit: 3 < 4

# call: insert([], 4, NewT).  

# exit: insert([], 4, [4]).  
# exit: insert([3], 4, [3, 4]).
# exit: insert([2, 3], 4, [2, 3, 4]).
# exit: insert([1, 2, 3], 4, [1, 2, 3, 4]).

## FACIT:
# [trace]  ?- insert([1,2,3], 4, X).
#    Call: (10) insert([1, 2, 3], 4, _13270) ? creep
#    Call: (11) 1@>4 ? creep
#    Fail: (11) 1@>4 ? creep
#    Redo: (10) insert([1, 2, 3], 4, _13270) ? creep
#    Call: (11) 1@<4 ? creep
#    Exit: (11) 1@<4 ? creep
#    Call: (11) insert([2, 3], 4, _17064) ? creep
#    Call: (12) 2@>4 ? creep
#    Fail: (12) 2@>4 ? creep
#    Redo: (11) insert([2, 3], 4, _17064) ? creep
#    Call: (12) 2@<4 ? creep
#    Exit: (12) 2@<4 ? creep
#    Call: (12) insert([3], 4, _21950) ? creep
#    Call: (13) 3@>4 ? creep
#    Fail: (13) 3@>4 ? creep
#    Redo: (12) insert([3], 4, _21950) ? creep
#    Call: (13) 3@<4 ? creep
#    Exit: (13) 3@<4 ? creep
#    Call: (13) insert([], 4, _26836) ? creep
#    Exit: (13) insert([], 4, [4]) ? creep
#    Exit: (12) insert([3], 4, [3, 4]) ? creep
#    Exit: (11) insert([2, 3], 4, [2, 3, 4]) ? creep
#    Exit: (10) insert([1, 2, 3], 4, [1, 2, 3, 4]) ? creep
# X = [1, 2, 3, 4].
