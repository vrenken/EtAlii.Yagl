grammar yagl;

query
    : element* EOF
    ;

element
    : entity
    ;

entity
    : identifier (':' parameters)? body?
    ;

parameters
    : parameter (',' parameter)*
    ;

parameter
    : identifier '=' value
    ;

value
    : number
    | STRING_LITERAL
    | identifier
    ;

body
    : property_relation+
    ;

property_relation
    : '|' (wildcard | template | entity_ref) body?
    ;

entity_ref
    : identifier (':' parameters)?
    ;

template
    : '#' identifier
    ;

wildcard
    : identifier? '*' identifier?
    ;

identifier
    : IDENTIFIER
    ;

number
    : NUMBER
    ;

IDENTIFIER
    : [a-zA-Z_] [a-zA-Z0-9_]*
    ;

NUMBER
    : '-'? [0-9]+
    ;

STRING_LITERAL
    : '"' ( ~["\r\n] | '""' )* '"'
    ;

COMMENT
    : '//' ~[\r\n]* -> skip
    ;

WS
    : [ \t\r\n]+ -> skip
    ;
