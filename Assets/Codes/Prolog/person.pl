:- define_indexical(residents, $town.aliveResidents).

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

siblings(X, Y) :-
   parent(X, P),
   parent(Y, P),
   X \= Y.

sex(Who, What) :-
   person(Who),
   What is Who.biologicalsex.

romantically_interested_in(X,Y) :-
   person(X),
   I is X.romanticallyinterestedin,
   member(Y, I).

love_triangle(X,Y,Z) :-
   romantically_interested_in(X, Z),
   romantically_interested_in(Y, Z).

