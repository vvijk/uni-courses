# Explain how the predicate works by explaining the queries len([], R).
# Explain how the predicate works by explaining the queries len([4,2], R).
# Show the execution a la trace (3p).

len([], 0).
len([_|T], L)   :-  len(T, TL), L is 1 + TL.


#Call len([4,2], R).
#   Call len([2], R).
#       Call len([], R).
#       Exit len([], 0).
#           Call 1 + 0
#           Exit 1 + +. L = 1
#   Exit(len[2], 1)
#       Call 1 + 1
#       Exit 1 + 1. L = 2
#Exit(len[4,2], 2)
#R = [2]



# a)    # len([], R).
# call len([], R).
# exit len([], R). 
# R = []

# b)    # len([4,2], R).
# fail len([], 0).
# call len([2], L). 
# call len([], L). 
# exit len([], 0)
# call L is 1+0
# exit L is 1
# exit len([2], 1)
# call L is 1+1
# exit L is 2
# exit len([4,2], 2)
# R = 2



