:- define_indexical(residents, $town.aliveResidents).

person(X) :-
   nonvar(X),
   !,
   is_class(X,$person).
person(X) :-
   member(X, $residents).

parent(Child, Parent) :-
   nonvar(Child),
   !,
   P is Child.parents,
   member(Parent, P),
   % I'm not sure why this is needed
   % but there are Persons with two parents both of whom are null
   Parent \= null.
parent(Child, Parent) :-
   nonvar(Parent),
   !,
   C is Parent.children,
   member(Child, C).
parent(Child, Parent) :-
   person(Child),  % bind child to a person
   parent(Child, Parent).

sibling(X, Y) :-
   parent(X, P),
   parent(Y, P),
   X \= Y.

male(X) :-
   person(X),
   X.isMale.

female(X) :-
   person(X),
   X.isFemale.

mother(X, M) :-
   parent(X, M),
   female(M).

father(X, F) :-
   parent(X, F),
   male(F).

sister(X, S) :-
   sibling(X, S),
   female(S).

brother(X, B) :-
   sibling(X, B),
   male(B).

romantically_interested_in(X,Y) :-
   person(X),
   I is X.romanticallyinterestedin,
   member(Y, I).

love_triangle(X,Y,Z) :-
   romantically_interested_in(X, Z),
   romantically_interested_in(Y, Z).

significant_other(Person, SO) :-
   person(Person),
   SO is Person.sigOther,
   SO \= null.

couple(A, B) :-
   significant_other(A, B),
   significant_other(B, A).




