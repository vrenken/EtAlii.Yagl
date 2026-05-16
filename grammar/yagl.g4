grammar yagl;

options {
    caseInsensitive = true;
}

yagl
    : sortedQuery EOF
    ;

sortedQuery
    : prefixAssignment sortedQuery
    | scopedClause (SORTBY sortSpec)?
    ;

sortSpec
    : sortSpec singleSpec
    | singleSpec
    ;

singleSpec
    : index modifierList?
    ;

cqlQuery
    : prefixAssignment cqlQuery
    | scopedClause
    ;

prefixAssignment
    : '>' prefix_ '=' uri
    | '>' uri
    ;

scopedClause
    : scopedClause booleanGroup searchClause
    | searchClause
    ;

booleanGroup
    : boolean_ modifierList?
    ;

boolean_
    : AND
    | OR
    | NOT
    | PROX
    ;

searchClause
    : '(' cqlQuery ')'
    | index relation searchTerm
    | searchTerm
    ;

relation
    : comparitor modifierList?
    ;

comparitor
    : comparitorSymbol
    | namedComparitor
    ;

comparitorSymbol
    : '='
    | '>'
    | '<'
    | '>='
    | '<='
    | '<>'
    | '=='
    ;

namedComparitor
    : identifier
    ;

modifierList
    : modifierList modifier
    | modifier
    ;

modifier
    : '/' modifierName (comparitorSymbol modifierValue)?
    ;

prefix_
    : term
    ;

uri
    : term
    ;

modifierName
    : term
    ;

modifierValue
    : term
    ;

searchTerm
    : term
    ;

index
    : term
    ;

term
    : identifier
    | AND
    | OR
    | NOT
    | PROX
    | SORTBY
    ;

identifier
    : CHARSTRING1
    | CHARSTRING2
    ;

AND
    : 'AND'
    ;

OR
    : 'OR'
    ;

NOT
    : 'NOT'
    ;

PROX
    : 'PROX'
    ;

SORTBY
    : 'SORTBY'
    ;

CHARSTRING1
    : [A-Z.]+
    ;

CHARSTRING2
    : '"' .*? '"'
    ;

WS
    : [ \r\n\t]+ -> skip
    ;