using Antlr4.Runtime.Misc;

namespace EtAlii.Yagl;

public class YaglVisitor : yaglBaseVisitor<object>
{
    public override object VisitQuery([NotNull] yaglParser.QueryContext context)
    {
        var elements = context.element()
            .Select(e => (Entity)VisitElement(e))
            .ToList();
        return new Query(elements);
    }

    public override object VisitElement([NotNull] yaglParser.ElementContext context)
    {
        return VisitEntity(context.entity());
    }

    public override object VisitEntity([NotNull] yaglParser.EntityContext context)
    {
        var id = context.identifier().GetText();
        var parameters = context.parameters() != null
            ? (List<Parameter>)VisitParameters(context.parameters())
            : [];
        var body = context.body() != null
            ? (Body)VisitBody(context.body())
            : null;

        return new Entity(id, parameters, body);
    }

    public override object VisitParameters([NotNull] yaglParser.ParametersContext context)
    {
        return context.parameter()
            .Select(p => (Parameter)VisitParameter(p))
            .ToList();
    }

    public override object VisitParameter([NotNull] yaglParser.ParameterContext context)
    {
        var id = context.identifier().GetText();
        var value = VisitValue(context.value());
        return new Parameter(id, value);
    }

    public override object VisitValue([NotNull] yaglParser.ValueContext context)
    {
        if (context.number() != null)
        {
            return long.Parse(context.number().GetText());
        }
        if (context.STRING_LITERAL() != null)
        {
            return context.STRING_LITERAL().GetText().Trim('"');
        }
        if (context.identifier() != null)
        {
            return context.identifier().GetText();
        }
        throw new InvalidOperationException("Unknown value type");
    }

    public override object VisitBody([NotNull] yaglParser.BodyContext context)
    {
        var relations = context.property_relation()
            .Select(r => (PropertyRelation)VisitProperty_relation(r))
            .ToList();
        return new Body(relations);
    }

    public override object VisitProperty_relation([NotNull] yaglParser.Property_relationContext context)
    {
        IPropertyRelationContent content = null;
        if (context.wildcard() != null)
        {
            content = (IPropertyRelationContent)VisitWildcard(context.wildcard());
        }
        else if (context.template() != null)
        {
            content = (IPropertyRelationContent)VisitTemplate(context.template());
        }
        else if (context.entity_ref() != null)
        {
            content = (IPropertyRelationContent)VisitEntity_ref(context.entity_ref());
        }

        var body = context.body() != null ? (Body)VisitBody(context.body()) : null;
        return new PropertyRelation(content, body);
    }

    public override object VisitEntity_ref([NotNull] yaglParser.Entity_refContext context)
    {
        var id = context.identifier().GetText();
        var parameters = context.parameters() != null
            ? (List<Parameter>)VisitParameters(context.parameters())
            : [];
        return new EntityRef(id, parameters);
    }

    public override object VisitTemplate([NotNull] yaglParser.TemplateContext context)
    {
        return new Template(context.identifier().GetText());
    }

    public override object VisitWildcard([NotNull] yaglParser.WildcardContext context)
    {
        var text = context.GetText();
        // This is a simple implementation, we might need a better parsing if needed.
        // For now, let's treat it as a string
        return new Wildcard(text, null);
    }
}
