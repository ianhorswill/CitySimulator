:- public institution/1.

:- define_indexical(institution_manager, $institutionmanager.singleton).
:- define_indexical(institution_list, $institution_manager.institutionlist).

institution(X) :-
   is_class(X, $institution, $institution_list).