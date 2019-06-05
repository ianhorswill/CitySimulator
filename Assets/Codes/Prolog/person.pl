:- public person/1, parent/2, grandparent/2, sibling/2, male/1, female/1.
:- public mother/2, father/2, grandmother/2, grandfather/2.
:- public aunt/2, uncle/2, brother/2, sister/2, cousin/2.
:- public romantically_interested_in/2, significant_other/2, couple/2.
:- public love_triangle/3.

:- define_indexical(residents, $town.aliveResidents).

% person(X) :-
%    nonvar(X),
%    !,
%    is_class(X,$person).
% person(X) :-
%    member(X, $residents).

person(X) :-
   is_class(X, $person, $residents).

male(X) :-
   person(X),
   X.isMale.

female(X) :-
   person(X),
   X.isFemale.

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

mother(X, M) :-
   parent(X, M),
   female(M).

father(X, F) :-
   parent(X, F),
   male(F).

grandparent(C, G) :-
   parent(C, P),
   parent(P, G).

grandmother(X, M) :-
   grandparent(X, M),
   female(M).

grandfather(X, F) :-
   grandparent(X, F),
   male(F).

sibling(X, Y) :-
   nonvar(X),
   !,
   S is X.siblings,
   member(Y, S).
sibling(Y, X) :-
   nonvar(X),
   !,
   S is X.siblings,
   member(Y, S).
sibling(X, Y) :-
   person(X),
   sibling(X, Y).

sister(X, S) :-
   sibling(X, S),
   female(S).

brother(X, B) :-
   sibling(X, B),
   male(B).

uncle(X, U) :-
   parent(X, P),
   sibling(P, U),
   male(U).

aunt(X, A) :-
   parent(X, P),
   sibling(P, A),
   female(A).

cousin(X, C) :-
   parent(X, P),
   sibling(P, S),
   parent(C, S).

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




