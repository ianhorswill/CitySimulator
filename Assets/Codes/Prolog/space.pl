:- define_indexical(space_component, $space.singleton).
:- define_indexical(plots_array, $space_component.plotsarray).
:- define_indexical(streets, $space_component.streetslist).
:- define_indexical(empty_plots, $space_component.emptyplots).
:- define_indexical(occupied_plots, $space_component.occupiedplots).

:- public plot/1, empty_plot/1, occupied_plot/1.
empty_plot(X) :-
   is_class(X, $plot, $empty_plots).
occupied_plot(X) :-
   is_class(X, $plot, $occupied_plots).

plot(X) :-
   occupied_plot(X).
plot(X) :-
   empty_plot(X).
