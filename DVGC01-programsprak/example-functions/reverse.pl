#19-01-10

reverse([], Ys, Ys).

reverse(Xs, Ys)         :-      reverse(Xs, [], Ys).
reverse([H|T], Acc, Ys) :-      reverse(T, [H| Acc], Ys).

#?- trace reverse([c,a,t], Ys).
call: reverse([c,a,t], Ys).  
call: reverse([c,a,t], [], Ys).  -
call: reverse([a,t], [c], Ys).  -
call: reverse([t], [a,c], Ys).  
call: reverse([], [t,a,c], Ys).  -

exit: reverse([], [t,a,c], [t,a,c]). 

exit: reverse([t], [a,c], [t,a,c])
exit: reverse([a,t], [c], [t,a,c])
exit: reverse([c,a,t], [], [t,a,c])

# Facit:
# ?- reverse([c,a,t], Ys).
# Call: (10) reverse([c, a, t], _10368) ? creep
# Call: (11) reverse([c, a, t], [], _10368) ? creep
# Call: (12) reverse([a, t], [c], _10368) ? creep
# Call: (13) reverse([t], [a, c], _10368) ? creep
# Call: (14) reverse([], [t, a, c], _10368) ? creep

# Exit: (14) reverse([], [t, a, c], [t, a, c]) ? creep

# Exit: (13) reverse([t], [a, c], [t, a, c]) ? creep
# Exit: (12) reverse([a, t], [c], [t, a, c]) ? creep
# Exit: (11) reverse([c, a, t], [], [t, a, c]) ? creep
# Exit: (10) reverse([c, a, t], [t, a, c]) ? creep