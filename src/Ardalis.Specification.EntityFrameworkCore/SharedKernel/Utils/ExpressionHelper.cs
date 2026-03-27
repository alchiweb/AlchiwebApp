using System.Reflection;

namespace Ardalis.Specification.EntityFrameworkCore.SharedKernel.Utils;

public static class ExpressionHelper
{
    public static MemberExpression CreateMemberExpression<TEntity, TFieldEnum>(ParameterExpression entity, TFieldEnum fieldType)
        where TFieldEnum : struct, Enum
    {
        var fieldNestedName = Enum.GetName(fieldType);
        if (string.IsNullOrEmpty(fieldNestedName))
            throw new ArgumentException($"Property '{fieldNestedName}' not found on type '{typeof(TEntity).Name}'");

        MemberExpression propertyField;
        MemberExpression? tempMemberExpression = null;
        foreach (var fieldName in fieldNestedName.Split('_'))
        {
            tempMemberExpression = Expression.Property(tempMemberExpression == null ? entity : tempMemberExpression, fieldName);
        }
        if (tempMemberExpression == null)
            throw new ArgumentException($"Property '{fieldNestedName}' not found on type '{typeof(TEntity).Name}'");
        propertyField = tempMemberExpression;
        return propertyField;
    }
    public static Expression CreateGetMethodMemberExpression<TEntity, TFieldEnum>(ParameterExpression entity, TFieldEnum fieldType)
        where TFieldEnum : struct, Enum
    {
        var propertyField = CreateMemberExpression<TEntity, TFieldEnum>(entity, fieldType);
        var Member = (PropertyInfo)propertyField.Member;
        if (Member.GetMethod == null)
            return propertyField;
        return Expression.Call(entity, Member.GetMethod);
    }

    public static Expression? AddExpression(Expression? sourceExpression, Expression? newExpression, bool isOrCondition)
    {
        if (newExpression != null)
        {
            if (sourceExpression == null)
                sourceExpression = newExpression;
            else
            {
                sourceExpression = isOrCondition ?
                    Expression.Or(sourceExpression, newExpression) :
                    Expression.And(sourceExpression, newExpression);
            }
        }
        return sourceExpression;
    }
}
