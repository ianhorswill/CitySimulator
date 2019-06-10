:- public institution/1.

:- define_indexical(institution_list, $institutionmanager.institutionlist).

institution(X) :-
   is_class(X, $institution, $institution_list).

owner(I, O) :-
   nonvar(I),
   institution(I),
   O is I.owner,
   O \= null.

employee(I, E) :-
   nonvar(I),
   !,
   institution(I),
   L is I.employeelist,
   L \= null,
   member(E, L).

employee(I, E) :-
   nonvar(E),
   !,
   I is E.currentinstitution,
   L is I.employeelist,
   L \= null,
   member(E, L).