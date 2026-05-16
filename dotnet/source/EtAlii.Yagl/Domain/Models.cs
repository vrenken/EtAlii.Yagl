namespace EtAlii.Yagl;

public record Query(List<Entity> Elements);

public record Entity(string Identifier, List<Parameter> Parameters, Body? Body);

public record Parameter(string Identifier, object Value);

public record Body(List<PropertyRelation> Relations);

public record PropertyRelation(IPropertyRelationContent Content, Body? Body);

public interface IPropertyRelationContent;

public record Wildcard(string? Prefix, string? Suffix) : IPropertyRelationContent;

public record Template(string Identifier) : IPropertyRelationContent;

public record EntityRef(string Identifier, List<Parameter> Parameters) : IPropertyRelationContent;
