#FACTS
rec_sum([], 0).
#RULES
rec_sum([H|T], Sum) :- rec_sum(T, TailSum), Sum is H + TailSum.

#?-rec_sum([1,2,3], R).
# Call: rec_sum([1, 2, 3], Tailsum).
# Call: rec_sum([2, 3], TailSum).
# Call: rec_sum([3], TailSum).

# Call: rec_sum([], TailSum).
# Exit: rec_sum([], 0).

# Call: 1 + 0
# Exit: rec_sum([1], 1).
# Call: 2 + 1
# Exit: rec_sum([1, 2], 3).
# Call: 3 + 5
# Exit: rec_sum([1, 2, 3], 8).

####   FACIT   ###
# Call: rec_sum([1, 2, 3], _35160) ? creep
# Call: rec_sum([2, 3], _36500) ? creep
# Call: rec_sum([3], _37312) ? creep

# Call: rec_sum([], _38124) ? creep
# Exit: rec_sum([], 0) ? creep

# Call: _37312 is 3+0 ? creep
# Exit: 3 is 3+0 ? creep
# Exit: rec_sum([3], 3) ? creep
# Call: _36500 is 2+3 ? creep
# Exit: 5 is 2+3 ? creep
# Exit: rec_sum([2, 3], 5) ? creep
# Call: _35160 is 1+5 ? creep
# Exit: 6 is 1+5 ? creep
# Exit: rec_sum([1, 2, 3], 6) ? creep
