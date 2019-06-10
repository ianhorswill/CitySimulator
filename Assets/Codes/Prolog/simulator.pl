:- public define_indexical/2.
:- public show_graph/1.

define_indexical(Name, ValueExpression) :-
   Value is ValueExpression,
   call((indexical Name=Value)).

:- $simulator.initialize().
:- step_limit(10000000).

show_graph(Edges) :-
   $graph.create(),
   forall(member(edge(From, To, Label, Color), Edges),
	  show_edge(From, To, Label, Color)).

show_edge(From,To, Label, Color) :-
   textify(From, FromRep),
   textify(To, ToRep),
   textify(Label, LabelRep),
   textify(Color, ColorRep),
   $graph.addedge(FromRep, ToRep, LabelRep, ColorRep),
   ignore(fix_color(From, FromRep)),
   ignore(fix_color(To, ToRep)).


fix_color(NodeValue, Node) :-
   institution(NodeValue),
   $graph.setcolor(Node, "blue"),
   true.

textify(Object, Text) :-
   Text is Object.tostring().
