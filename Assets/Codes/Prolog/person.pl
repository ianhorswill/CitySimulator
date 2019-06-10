:- public person/1, parent/2, grandparent/2, sibling/2, male/1, female/1.
:- public mother/2, father/2, grandmother/2, grandfather/2.
:- public aunt/2, uncle/2, brother/2, sister/2, cousin/2.
:- public romantically_interested_in/2, significant_other/2, couple/2.
:- public love_triangle/3.
:- public show_person/1, person_named/2, show_random_person/0, random_person/1.
:- public r/3.

:- define_indexical(residents, $town.aliveResidents).

% person(X) :-
%    nonvar(X),
%    !,
%    is_class(X,$person).
% person(X) :-
%    member(X, $residents).

% Relationships between persons
relationship(P1, P2, parent) :-
   parent(P1, P2).
relationship(P1, P2, sibling) :-
   sibling(P1, P2).
relationship(P1, P2, significant_other) :-
   significant_other(P1, P2).
relationship(P1, P2, love_interest) :-
   romantically_interested_in(P1, P2).
relationship(P1, I, affiliation) :-
   affiliation(P1, I).
relationship(I, O, owner) :-
   owner(I, O).
relationship(I, E, employee) :-
   employee(I, E).
relationship(P1, P2, coworker) :-
   coworker(P1, P2).
relationship(P1, P2, boss) :-
   boss(P1, P2).

r(P1, P2, R) :-
   personify(P1, P1P),
   personify(P2, P2P),
   relationship(P1P, P2P, R).

relationships(Start, Start, P2, R, 0) :-
   relationship(Start, P2, R).
relationships(Start, P1, P2, R, N) :-
   N > 0,
   Decrement is N-1,
   relationship(Start, I, _),
   relationships(I, P1, P2, R, Decrement).

% The orbit of a person is the set of people they have relationships to.
orbit(PersonOrName, [Person | Relatees]) :-
   personify(PersonOrName, Person),
   all(Relatee, relationship(Person, Relatee, _), Relatees).

show_person(Person) :-
   step_limit(10000000),
   write("Displaying orbit of"),
   writeln(Person),
   $graph.create(),
   add_orbit_edges(Person).

add_orbit_edges(P) :-
   orbit(P, O),
   writeln(O),
   add_orbit_edges_aux(O).

add_orbit_edges_aux(O) :-
   member(A, O),
   member(B, O),
   A \= B,
   relationship(A, B, R),
   once(edge_color(R, C)),
   show_edge(A, B, R, C),
   fail.
add_orbit_edges(_).

edge_color(significant_other, "magenta").
edge_color(parent, "cyan").
edge_color(sibling, "gray").
edge_color(love_interest, "red").
edge_color(affiliation, "blue").
edge_color(owner, "blue").
edge_color(employee, "blue").
edge_color(coworker, "gray").
edge_color(boss, "blue").
edge_color(_, "white").

show_random_person :-
   random_person(P),
   parent(P, _),
   show_person(P).

person_named(X, Name) :-
   person(X),
   Name is X.name.

personify(Name, Person) :-
   string(Name),
   !,
   person_named(Person, Name).
personify(Person, Person) :-
   person(Person).

person(X) :-
   is_class(X, $person, $residents).

random_person(X) :-
   X is $town.randomperson().

male(X) :-
   person(X),
   X.isMale.

female(X) :-
   person(X),
   X.isFemale.

parent(Child, Parent) :-
   nonvar(Child),
   !,
   person(Child),
   P is Child.parents,
   P \= null,
   member(Parent, P),
   % I'm not sure why this is needed
   % but there are Persons with two parents both of whom are null
   Parent \= null.
parent(Child, Parent) :-
   nonvar(Parent),
   !,
   person(Parent),
   C is Parent.children,
   C \= null,
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
   person(X),
   S is X.siblings,
   S \= null,
   member(Y, S).
sibling(Y, X) :-
   nonvar(X),
   !,
   person(X),
   S is X.siblings,
   S \= null,
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
   romantically_interested_in(Y, Z),
   X \= Y.

significant_other(Person, SO) :-
   person(Person),
   SO is Person.sigOther,
   SO \= null.

couple(A, B) :-
   significant_other(A, B),
   significant_other(B, A).

affiliation(Person, Institution) :-
   person(Person),
   Institution is Person.currentinstitution.

coworker(A, B) :-
   affiliation(A, I),
   (employee(I, B)),
   A \= B.

boss(E, B) :-
   affiliation(E, I),
   owner(I, B),
   E\= B.

