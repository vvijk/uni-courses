#19-06-07

append([], L, L).
append([H|T], L2, [H|L3])   :-  append(T, L2, L3).


# Giva a trace on append([a, b, c], [x, y, z], R).

?- call append([a, b, c], [x, y, z], R).    -
call: append([b, c], [x, y, z], L3).    -
call: append([c], [x, y, z], L3).    -
call: append([], [x, y, z], L3).
call: append([], [x, y, z], L3).    
exit: append([c], [x, y, z], [c, x, y, z]).
exit: append([b, c], [x, y, z], [b, c, x, y, z]).
exit: append([a, b, c], [x, y, z], [a, b, c, x, y, z]).

