is_it(X, R) :- string_chars(R, Z), member(X, Z).

spl([H|T], [H|R], X, S)     :- is_it(H, S), spl(T, R, X, S).
spl([H|T], X, [H|R], S)     :- spl(T, X, R, S).
spl([], [], [], _).

split(S, V, K, Q)           :- string_chars(S, T), spl(T, W, L, Q), string_chars(V, W), string_chars(K, L).

#call split("prolog", A, B, "aeiouy").
#    call string_chars("prolog", T)
#    exit string_chars("prolog", ['p', 'r', 'o', 'l', 'o', 'g']).
#    call spl(['p', 'r', 'o', 'l', 'o', 'g'], W, L, "aeiouy").
#        call is_it(['p'], "aeiouy")
#            call string_chars("aeiouy", Z)
#            exit string_chars("aeiouy", ['a', 'e', 'i', 'o', 'u', 'y'])
#            call member('p', ['a', 'e', 'i', 'o', 'u', 'y'])
#            fail member('p', ['a', 'e', 'i', 'o', 'u', 'y'])
#        fail is_it(['p'], "aeiouy")
#    call spl(['r', 'o', 'l', 'o', 'g'], X, [], "aeiouy").
